using System.IO.Compression;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Collections.Immutable;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.Implementation;

internal sealed partial class TwitchConnector : ITwitchConnector {
    private readonly IAuthorizer m_authorizer;
    private readonly IWebSocketConnection m_webSocketConnection;
    private readonly DateTime m_startTimestamp = DateTime.Now;
    private Action<PrivMessage> m_privCallback;
    private Action<ClearMessage> m_clearCallback;

    public TwitchConnector(
        IAuthorizer authorizer,
        IWebSocketConnection webSocketConnection
    ) {
        m_authorizer = authorizer;
        m_webSocketConnection = webSocketConnection;
        m_privCallback = delegate(
            PrivMessage message
        ) {
            Console.WriteLine( message.Text );
        };
        
        m_clearCallback = delegate(
            ClearMessage message
        ) {
            Console.WriteLine( message.UserId );
        };
    }
    
    async Task<bool> ITwitchConnector.TryConnect( 
        TwitchConnectionOptions options, 
        Action<PrivMessage> privCallback,
        Action<ClearMessage> clearCallback
    ) {
        m_privCallback = privCallback;
        m_clearCallback = clearCallback;
        
        string token = await m_authorizer.GetToken();
        
        await ConnectToWebsocket( m_webSocketConnection, token, options );
        return true;
    }

    private async Task ConnectToWebsocket(
        IWebSocketConnection ws,
        string token,
        TwitchConnectionOptions options
    ) {
        await ws.ConnectAsync(
            uri: new Uri(uriString: "ws://irc-ws.chat.twitch.tv:80"), 
            cancellationToken: CancellationToken.None
        );
        byte[] buffer = new byte[4096];

        await SendMessage( 
            socketConnection: ws, 
            message: "CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands" 
        );
        Thread.Sleep(millisecondsTimeout: 1000);
        await SendMessage( socketConnection: ws, message: $"PASS oauth:{token}" );
        await SendMessage( socketConnection: ws, message: "NICK chatreaderbot" );
        await SendMessage( socketConnection: ws, message: $"JOIN #{options.Channel}" );
        
        while (ws.State == WebSocketState.Open) {
            await ReceiveWebSocketResponse( ws: ws, buffer: buffer, options: options );
        }
    }
    private async Task ReceiveWebSocketResponse(
        IWebSocketConnection ws,
        byte[] buffer,
        TwitchConnectionOptions options
    ) {
        WebSocketReceiveResult result = await ws.ReceiveAsync(
            buffer: buffer, 
            cancellationToken: CancellationToken.None
        );
            
        if ( result.MessageType == WebSocketMessageType.Close ) {
            await ws.CloseAsync(
                closeStatus: WebSocketCloseStatus.NormalClosure, 
                statusDescription: null, 
                cancellationToken: CancellationToken.None
            );
        } else {
            string response = Encoding.ASCII.GetString( 
                bytes: buffer, 
                index: 0, 
                count: result.Count 
            );

            // keep alive, let twitch know we are still listening
            if( response.Contains("PING :tmi.twitch.tv") ) {
                await SendMessage( ws, "PONG :tmi.twitch.tv" );
            }

            if( response.Contains( $" PRIVMSG #{options.Channel}" ) ) {
                PrivMessage privMsg = GetUserMessage( response, options.Channel );
                m_privCallback( privMsg );
            }
            
            if( response.Contains( $" CLEARCHAT #{options.Channel}" ) ) {
                ClearMessage message = GetClearChatMessage( response, options.Channel );
                m_clearCallback( message );
            }
        }
    }

    private ClearMessage GetClearChatMessage(
        string response,
        string channel
    ) {
        string[] segments = response.Split( $"CLEARCHAT #{channel} :" );

        string duration = DurationPattern
            .Match( response )
            .Value
            .Split( "=" )[1]
            .TrimEnd( ';' );

        long rawTimestamp = Convert.ToInt64(
            MessageTimestampPattern.Match( response ).Value
                .Split( "=" )[1]
        );

        DateTime messageTimestamp = DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;
        string message = segments[1].Trim( '\r', '\n' );

        return new ClearMessage(
            Duration: Int64.Parse( duration ),
            RoomId: channel,
            UserId: String.IsNullOrEmpty(message) ? null : message,
            Timestamp: messageTimestamp
        );
    }

    private PrivMessage GetUserMessage( string response, string channel ) {
        string[] segments = response.Split( $"PRIVMSG #{channel} :" );
 
        string displayName = DisplayNamePattern
            .Match( response )
            .Value
            .Split( "=" )[1]
            .TrimEnd( ';' );
        
        string badges = BadgesPattern
            .Match( response )
            .Value
            .Split( "=" )[1]
            .TrimEnd( ';' );

        IImmutableList<Badge> parsedBadges = GetBadges( badges );

        long rawTimestamp = Convert.ToInt64(
            MessageTimestampPattern.Match( response ).Value
            .Split( "=" )[1]
            .TrimEnd( ';' )
        );

        DateTime messageTimestamp = DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;
        TimeSpan timeSinceStartOfStream = messageTimestamp - m_startTimestamp;
        
        string message = segments[1].Trim( '\r', '\n' );
        
        if( segments.Length <= 1 ) {
            throw new UnexpectedUserMessageException();
        }

        return new PrivMessage(
            Text: message,
            Timestamp: messageTimestamp,
            SinceStartOfStream: timeSinceStartOfStream,
            DisplayName: displayName,
            Badges: parsedBadges,
            MessageType: MessageType.PrivMsg
        );
    }

    private IImmutableList<Badge> GetBadges(
        string badges
    ) {
        string[] badgeList = badges.Split( ',' );

        if( string.IsNullOrEmpty( badges ) ) {
            return Array.Empty<Badge>().ToImmutableList();
        }

        List<Badge> parsedBadges = new();

        for( int index = 0; index < badgeList.Length; index++ ) {
            string badge = badgeList[index];
            string[] badgeInfo = badge.Split( '/' );

            if( badgeInfo.Length == 2 ) {
                parsedBadges.Add( new Badge( Name: badgeInfo[0], Version: badgeInfo[1] ) );
            }
        }

        return parsedBadges.ToImmutableList();
    }

    private async Task SendMessage(
        IWebSocketConnection socketConnection,
        string message
    ) {
        Console.WriteLine( "WRITING: " + message );
        await socketConnection.SendAsync(
            buffer: Encoding.ASCII.GetBytes(message), 
            messageType: WebSocketMessageType.Text, 
            endOfMessage: true, 
            cancellationToken: CancellationToken.None
        );
    }

    private readonly static Regex DisplayNamePattern = new("display-name([^;]*);");
    private readonly static Regex MessageTimestampPattern = new Regex("tmi-sent-ts=([0-9]+)");
    private readonly static Regex BadgesPattern = new("badges([^;]*);");
    private readonly static Regex DurationPattern = new("duration([^;]*);");
}

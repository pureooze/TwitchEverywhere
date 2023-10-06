using System.IO.Compression;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.Collections;

namespace TwitchEverywhere.Implementation;

internal sealed partial class TwitchConnector : ITwitchConnector {
    private readonly IAuthorizer m_authorizer;
    private readonly IWebSocketConnection m_webSocketConnection;
    private DateTime m_startTimestamp = DateTime.Now;
    private Action<string> m_callback;

    public TwitchConnector(
        IAuthorizer authorizer,
        IWebSocketConnection webSocketConnection
    ) {
        m_authorizer = authorizer;
        m_webSocketConnection = webSocketConnection;
        m_callback = delegate(
            string s
        ) {
            Console.WriteLine( s );
        };
    }
    
    async Task<bool> ITwitchConnector.TryConnect( 
        TwitchConnectionOptions options, 
        Action<string> messageCallback
    ) {
        m_callback = messageCallback;
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
                string message = GetUserMessage( response, options.Channel );
                m_callback( $"MESSAGE {message}" );
            }
            
            if( response.Contains( $" CLEARCHAT #{options.Channel}" ) ) {
                string message = GetClearUserMessage( response, options.Channel );
                m_callback( $"CLEARCHAT {message}" );
            }
        }
    }

    private string GetClearUserMessage(
        string response,
        string channel
    ) {
        string[] segments = response.Split( $"CLEARCHAT #{channel} :" );
        string message = segments[1].Trim( '\r', '\n' );

        return message;
    }

    private string GetUserMessage( string response, string channel ) {
        string[] segments = response.Split( $"PRIVMSG #{channel} :" );
 
        string displayName = DisplayNamePattern()
            .Match( response )
            .Value
            .Split( "=" )[1]
            .TrimEnd( ';' );
        
        string badges = BadgesPattern()
            .Match( response )
            .Value
            .Split( "=" )[1]
            .TrimEnd( ';' );

        string parsedBadges = GetBadges( badges );

        long rawTimestamp = Convert.ToInt64(
            MessageTimestampPattern().Match( response ).Value
            .Split( "=" )[1]
            .TrimEnd( ';' )
        );

        DateTime messageTimestamp = DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;
        TimeSpan timeSinceStartOfStream = messageTimestamp - m_startTimestamp;
        
        string message = segments[1].Trim( '\r', '\n' );
        
        if( segments.Length <= 1 ) {
            throw new UnexpectedUserMessageException();
        }

        return $"{{ timestamp: \"{messageTimestamp.ToString("o")}\", sinceStartOfStream: \"{timeSinceStartOfStream.Ticks}\", displayName: \"{displayName}\", message: \"{message}\", badges: {parsedBadges} }}";
    }

    private string GetBadges(
        string badges
    ) {
        string parsedBadges = "";
        string[] badgeList = badges.Split( ',' );

        if( string.IsNullOrEmpty( badges ) ) {
            return "[]";
        }


        parsedBadges += "[";
        for( int index = 0; index < badgeList.Length; index++ ) {
            string badge = badgeList[index];
            string[] badgeInfo = badge.Split( '/' );

            if( badgeInfo.Length == 2 ) {
                parsedBadges += $"{{ \"name\": \"{badgeInfo[0]}\", \"version\": \"{badgeInfo[1]}\"}}";
            }
            

            if( index < badgeList.Length - 1 ) {
                parsedBadges += ",";
            }
        }

        parsedBadges += "]";
        return parsedBadges;
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

    [GeneratedRegex("display-name([^;]*);")]
    private static partial Regex DisplayNamePattern();
    
    [GeneratedRegex("tmi-sent-ts([^;]*);")]
    private static partial Regex MessageTimestampPattern();
    
    [GeneratedRegex("id([^;]*);")]
    private static partial Regex MessageIdPattern();
    
    [GeneratedRegex("badges([^;]*);")]
    private static partial Regex BadgesPattern();
}
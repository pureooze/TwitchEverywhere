using System.IO.Compression;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Collections.Immutable;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.Implementation;

internal sealed class TwitchConnector : ITwitchConnector {
    private readonly IAuthorizer m_authorizer;
    private readonly IWebSocketConnection m_webSocketConnection;
    private readonly DateTime m_startTimestamp;
    private Action<Message> m_messageCallback;

    public TwitchConnector(
        IAuthorizer authorizer,
        IWebSocketConnection webSocketConnection,
        IDateTimeService dateTimeService
    ) {
        m_authorizer = authorizer;
        m_webSocketConnection = webSocketConnection;
        m_startTimestamp = dateTimeService.GetStartTime();
        
        m_messageCallback = delegate(
            Message message
        ) {
            if( message.MessageType == MessageType.PrivMsg ) {
                PrivMsg privMsg = (PrivMsg) message;
                Console.WriteLine( $"PrivMsg: {privMsg.Text}" );
            }
        };
    }
    
    async Task<bool> ITwitchConnector.TryConnect( 
        TwitchConnectionOptions options, 
        Action<Message> messageCallback
    ) {
        m_messageCallback = messageCallback;
        
        string token = await m_authorizer.GetToken();
        
        bool result = await ConnectToWebsocket( m_webSocketConnection, token, options );
        return result;
    }

    private async Task<bool> ConnectToWebsocket(
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
        await SendMessage( socketConnection: ws, message: $"PASS oauth:{token}" );
        await SendMessage( socketConnection: ws, message: $"NICK {options.ClientName}" );
        await SendMessage( socketConnection: ws, message: $"JOIN #{options.Channel}" );
        
        while (ws.State == WebSocketState.Open) {
            await ReceiveWebSocketResponse( ws: ws, buffer: buffer, options: options );
        }

        return true;
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
            } else if( response.Contains( $" PRIVMSG #{options.Channel}" ) ) {
                Message privMsg = GetUserMessage( response, options.Channel );
                m_messageCallback( privMsg );
            } else if( response.Contains( $" CLEARCHAT #{options.Channel}" ) ) {
                Message chat = GetClearChatMessage( response, options.Channel );
                m_messageCallback( chat );
            } else if( response.Contains( $" CLEARMSG #{options.Channel}" ) ) {
                Message chat = GetClearMsgMessage( response, options.Channel );
                m_messageCallback( chat );
            }
        }
    }
    private Message GetClearMsgMessage(
        string response,
        string channel
    ) {
        string login = LoginPattern
            .Match( response )
            .Value
            .Split( "=" )[1]
            .TrimEnd( ';' );
        
        string targetMessageId = TargetMessageIdPattern
            .Match( response )
            .Value
            .Split( "=" )[1]
            .TrimEnd( ';' );
        
        long rawTimestamp = Convert.ToInt64(
            MessageTimestampPattern.Match( response ).Value
                .Split( "=" )[1]
        );
        
        string roomId = GetValueFromResponse( response, RoomIdPattern );

        DateTime messageTimestamp = DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;
        
        return new ClearMsg(
            Login: login,
            RoomId: roomId,
            TargetMessageId: targetMessageId,
            Timestamp: messageTimestamp
        );
    }

    private Message GetClearChatMessage(
        string response,
        string channel
    ) {
        string[] segments = response.Split( $"CLEARCHAT #{channel}" );

        string duration = GetValueFromResponse( response, BanDurationPattern );
        string roomId = GetValueFromResponse( response, RoomIdPattern );
        string targetUserId = GetValueFromResponse( response, TargetUserIdPattern );
        
        long rawTimestamp = Convert.ToInt64(
            MessageTimestampPattern.Match( response ).Value
                .Split( "=" )[1]
        );

        DateTime messageTimestamp = DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;

        return new ClearChat(
            Duration: string.IsNullOrEmpty( duration ) ? null : long.Parse( duration ),
            RoomId: roomId,
            UserId: targetUserId,
            Timestamp: messageTimestamp,
            Text: segments[1]
        );
    }

    private Message GetUserMessage( string response, string channel ) {
        string[] segments = response.Split( $"PRIVMSG #{channel} :" );
        
        string displayName = GetValueFromResponse( response, DisplayNamePattern );
        string badges = GetValueFromResponse( response, BadgesPattern );

        string emotesText = GetValueFromResponse( response, EmotesPattern );
        IImmutableList<Emote>? emotes = GetEmotesFromText( emotesText );
        
        string id = GetValueFromResponse( response, IdPattern );
        string pinnedChatPaidAmount = GetValueFromResponse( response, PinnedChatPaidAmountPattern );
        string pinnedChatPaidCurrency = GetValueFromResponse( response, PinnedChatPaidCurrencyPattern );
        string pinnedChatPaidExponent = GetValueFromResponse( response, PinnedChatPaidExponentPattern );

        string pinnedChatPaidLevelText = GetValueFromResponse( response, PinnedChatPaidLevelPattern );
        PinnedChatPaidLevel? pinnedChatPaidLevel = GetPinnedChatPaidLevelType( pinnedChatPaidLevelText );
        
        string pinnedChatPaidIsSystemMessage = GetValueFromResponse( response, PinnedChatPaidIsSystemMessagePattern );
        string replyParentMsgId = GetValueFromResponse( response, ReplyParentMsgIdPattern );
        string replyParentUserId = GetValueFromResponse( response, ReplyParentUserIdPattern );
        string replyParentUserLogin = GetValueFromResponse( response, ReplyParentUserLoginPattern );        
        string replyParentDisplayName = GetValueFromResponse( response, ReplyParentDisplayNamePattern );
        string replyThreadParentMsg = GetValueFromResponse( response, ReplyThreadParentMsgPattern );
        string roomId = GetValueFromResponse( response, RoomIdPattern );
        string subscriber = GetValueFromResponse( response, SubscriberPattern );
        string turbo = GetValueFromResponse( response, TurboPattern );
        string userId = GetValueFromResponse( response, UserIdPattern );
        
        string userTypeText = GetValueFromResponse( response, UserTypePattern );
        UserType userType = GetUserType( userTypeText );
        
        string vip = GetValueFromResponse( response, VipPattern );
        string isMod = GetValueFromResponse( response, ModPattern );
        string color = GetValueFromResponse( response, ColorPattern );
        
        string[] bitsArray = BitsPattern
            .Match( response )
            .Value
            .Split( "=" );

        string bits = string.Empty;
        if( bits.Length > 1 ) {
            bits = bitsArray.ElementAt( 1 ).TrimEnd( ';' );
            Console.WriteLine($"BITS: {bits}");
        }
        
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

        return new PrivMsg(
            Badges: parsedBadges,
            Bits: bits,
            Color: color,
            DisplayName: displayName,
            Emotes: emotes,
            Id: id,
            Mod: int.Parse( isMod ) == 1,
            PinnedChatPaidAmount: string.IsNullOrEmpty( pinnedChatPaidAmount ) ? null : long.Parse( pinnedChatPaidAmount ),
            PinnedChatPaidCurrency: pinnedChatPaidCurrency,
            PinnedChatPaidExponent: string.IsNullOrEmpty( pinnedChatPaidExponent ) ? null : int.Parse( pinnedChatPaidExponent ),
            PinnedChatPaidLevel: pinnedChatPaidLevel,
            PinnedChatPaidIsSystemMessage: !string.IsNullOrEmpty( pinnedChatPaidIsSystemMessage ),
            ReplyParentMsgId: replyParentMsgId,
            ReplyParentUserId: replyParentUserId,
            ReplyParentUserLogin: replyParentUserLogin,
            ReplyParentDisplayName: replyParentDisplayName,
            ReplyThreadParentMsg: replyThreadParentMsg,
            RoomId: roomId,
            Subscriber: int.Parse( subscriber ) == 1,
            Timestamp: messageTimestamp,
            Turbo: int.Parse( turbo ) == 1,
            UserId: userId,
            UserType: userType,
            Vip: !string.IsNullOrEmpty( vip ),
            SinceStartOfStream: timeSinceStartOfStream,
            Text: message
        );
    }
    private IImmutableList<Emote>? GetEmotesFromText(
        string emotesText
    ) {

        if( string.IsNullOrEmpty( emotesText ) ) {
            return null;
        }
// 25 : 0-4 , 12-16 / 1902 : 6-10
        List<Emote> emotes = new();
        string[] separatedRawEmotes = emotesText.Split( "/" );

        foreach (string rawEmote in separatedRawEmotes) {
            string[] separatedEmote = rawEmote.Split( ":" );
            string[] separatedEmoteLocationGroup = separatedEmote[1].Split( "," );

            foreach (string locationGroup in separatedEmoteLocationGroup) {
                string[] separatedEmoteLocation = locationGroup.Split( "-" );
                
                emotes.Add(
                    new Emote( 
                        separatedEmote[0], 
                        int.Parse( separatedEmoteLocation[0] ), 
                        int.Parse( separatedEmoteLocation[1] )
                    )
                );
            }
        }
        
        
        return emotes.ToImmutableList();
    }
    private static PinnedChatPaidLevel? GetPinnedChatPaidLevelType(
        string pinnedChatPaidLevelText
    ) {
        return pinnedChatPaidLevelText switch {
            "ONE" => PinnedChatPaidLevel.One,
            "TWO" => PinnedChatPaidLevel.Two,
            "THREE" => PinnedChatPaidLevel.Three,
            "FOUR" => PinnedChatPaidLevel.Four,
            "FIVE" => PinnedChatPaidLevel.Five,
            "SIX" => PinnedChatPaidLevel.Six,
            "SEVEN" => PinnedChatPaidLevel.Seven,
            "EIGHT" => PinnedChatPaidLevel.Eight,
            "NINE" => PinnedChatPaidLevel.Nine,
            "TEN" => PinnedChatPaidLevel.Ten,
            _ => null
        };
    }
    private static UserType GetUserType(
        string userTypeText
    ) {
        return userTypeText switch {
            "mod" => UserType.Mod,
            "admin" => UserType.Admin,
            "global_mod" => UserType.GlobalMod,
            "staff" => UserType.Staff,
            _ => UserType.Normal
        };
    }

    private static string GetValueFromResponse(
        string response,
        Regex pattern
    ) {
        Match match = pattern
            .Match( response );

        string result = string.Empty;
        if( match.Success ) {
            result = match.Value.Split( "=" )[1].TrimEnd( ';' );
        }

        return result;
    }

    private static IImmutableList<Badge> GetBadges(
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

    private async static Task SendMessage(
        IWebSocketConnection socketConnection,
        string message
    ) {
        await socketConnection.SendAsync(
            buffer: Encoding.ASCII.GetBytes(message), 
            messageType: WebSocketMessageType.Text, 
            endOfMessage: true, 
            cancellationToken: CancellationToken.None
        );
    }

    private readonly static Regex DisplayNamePattern = new("display-name([^;]*);");
    private readonly static Regex LoginPattern = new("login([^;]*)");
    private readonly static Regex TargetMessageIdPattern = new("target-msg-id([^;]*)");
    private readonly static Regex BadgesPattern = new("badges([^;]*);");
    private readonly static Regex DurationPattern = new("duration([^;]*);");
    private readonly static Regex BitsPattern = new("bits=([^;]*);");
    private readonly static Regex ColorPattern = new("color=([^;]*);");
    private readonly static Regex EmotesPattern = new("emotes=([^;]*);");
    private readonly static Regex IdPattern = new(";id=([^;]*);");
    private readonly static Regex ModPattern = new("mod=([^;]*);");
    private readonly static Regex PinnedChatPaidAmountPattern = new("pinned-chat-paid-amount=([^;]*);");
    private readonly static Regex PinnedChatPaidCurrencyPattern = new("pinned-chat-paid-currency=([^;]*);");
    private readonly static Regex PinnedChatPaidExponentPattern = new("pinned-chat-paid-exponent=([^;]*);");
    private readonly static Regex PinnedChatPaidLevelPattern = new("pinned-chat-paid-level=([^;]*);");
    private readonly static Regex PinnedChatPaidIsSystemMessagePattern = new("pinned-chat-paid-is-system-message=([^;]*);");
    private readonly static Regex ReplyParentMsgIdPattern = new("reply-parent-msg-id=([^;]*);");
    private readonly static Regex ReplyParentUserIdPattern = new("reply-parent-user-id=([^;]*);");
    private readonly static Regex ReplyParentUserLoginPattern = new("reply-parent-user-login=([^;]*);");
    private readonly static Regex ReplyParentDisplayNamePattern = new("reply-parent-display-name=([^;]*);");
    private readonly static Regex ReplyThreadParentMsgPattern = new("reply-thread-parent-msg-id=([^;]*);");
    private readonly static Regex RoomIdPattern = new("room-id=([^;]*);");
    private readonly static Regex SubscriberPattern = new("subscriber=([^;]*);");
    private readonly static Regex MessageTimestampPattern = new("tmi-sent-ts=([0-9]+)");
    private readonly static Regex TurboPattern = new("turbo=([^;]*);");
    private readonly static Regex UserIdPattern = new("user-id=([^;]*);");
    private readonly static Regex UserTypePattern = new("user-type=([^; ]+)");
    private readonly static Regex VipPattern = new("vip=([^;]*)");
    private readonly static Regex BanDurationPattern = new("ban-duration=([^;]*)");
    private readonly static Regex TargetUserIdPattern = new("target-user-id=([^;]*)");
}

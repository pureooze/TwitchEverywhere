using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
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
            Console.WriteLine( $"Msg: {message.ToString()}" );
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
                Message chat = GetClearMsgMessage( response );
                m_messageCallback( chat );
            } else if( response.Contains( $" NOTICE #{options.Channel}" ) ) {
                Message chat = GetNoticeMsgMessage( response );
                m_messageCallback( chat );
            }
        }
    }
    private Message GetNoticeMsgMessage(
        string response
    ) {
        string targetMessageId = GetValueFromResponse( response, MsgIdPattern );
        string targetUserId = GetValueFromResponse( response, TargetUserIdPattern );

        NoticeMsgIdType targetMsgIdType = GetNoticeMsgIdType( targetMessageId );
        
        return new NoticeMsg(
            MsgId: targetMsgIdType, 
            TargetUserId: targetUserId 
        );
    }
    private NoticeMsgIdType GetNoticeMsgIdType(
        string targetMessageId
    ) {
        return targetMessageId switch {
            "already_banned" => NoticeMsgIdType.AlreadyBanned,
            "already_emote_only_off" => NoticeMsgIdType.AlreadyEmoteOnlyOff,
            "already_emote_only_on" => NoticeMsgIdType.AlreadyEmoteOnlyOn,
            "already_followers_off" => NoticeMsgIdType.AlreadyFollowersOff,
            "already_followers_on" => NoticeMsgIdType.AlreadyFollowersOn,
            "already_r9k_off" => NoticeMsgIdType.AlreadyR9KOff,
            "already_r9k_on" => NoticeMsgIdType.AlreadyR9KOn,
            "already_slow_off" => NoticeMsgIdType.AlreadySlowOff,
            "already_slow_on" => NoticeMsgIdType.AlreadySlowOn,
            "already_subs_off" => NoticeMsgIdType.AlreadySubsOff,
            "already_subs_on" => NoticeMsgIdType.AlreadySubsOn,
            "autohost_receive" => NoticeMsgIdType.AutohostReceive,
            "bad_ban_admin" => NoticeMsgIdType.BadBanAdmin,
            "bad_ban_anon" => NoticeMsgIdType.BadBanAnon,
            "bad_ban_broadcaster" => NoticeMsgIdType.BadBanBroadcaster,
            "bad_ban_mod" => NoticeMsgIdType.BadBanMod,
            "bad_ban_self" => NoticeMsgIdType.BadBanSelf,
            "bad_ban_staff" => NoticeMsgIdType.BadBanStaff,
            "bad_commercial_error" => NoticeMsgIdType.BadCommercialError,
            "bad_delete_message_broadcaster" => NoticeMsgIdType.BadDeleteMessageBroadcaster,
            "bad_delete_message_mod" => NoticeMsgIdType.BadDeleteMessageMod,
            "bad_host_error" => NoticeMsgIdType.BadHostError,
            "bad_host_hosting" => NoticeMsgIdType.BadHostHosting,
            "bad_host_rate_exceeded" => NoticeMsgIdType.BadHostRateExceeded,
            "bad_host_rejected" => NoticeMsgIdType.BadHostRejected,
            "bad_host_self" => NoticeMsgIdType.BadHostSelf,
            "bad_mod_banned" => NoticeMsgIdType.BadModBanned,
            "bad_mod_mod" => NoticeMsgIdType.BadModMod,
            "bad_slow_duration" => NoticeMsgIdType.BadSlowDuration,
            "bad_timeout_admin" => NoticeMsgIdType.BadTimeoutAdmin,
            "bad_timeout_anon" => NoticeMsgIdType.BadTimeoutAnon,
            "bad_timeout_broadcaster" => NoticeMsgIdType.BadTimeoutBroadcaster,
            "bad_timeout_duration" => NoticeMsgIdType.BadTimeoutDuration,
            "bad_timeout_mod" => NoticeMsgIdType.BadTimeoutMod,
            "bad_timeout_self" => NoticeMsgIdType.BadTimeoutSelf,
            "bad_timeout_staff" => NoticeMsgIdType.BadTimeoutStaff,
            "bad_unban_no_ban" => NoticeMsgIdType.BadUnbanNoBan,
            "bad_unhost_error" => NoticeMsgIdType.BadUnhostError,
            "bad_unmod_mod" => NoticeMsgIdType.BadUnmodMod,
            "bad_vip_grantee_banned" => NoticeMsgIdType.BadVipGranteeBanned,
            "bad_vip_grantee_already_vip" => NoticeMsgIdType.BadVipGranteeAlreadyVip,
            "bad_vip_max_vips_reached" => NoticeMsgIdType.BadVipMaxVipsReached,
            "bad_vip_achievement_incomplete" => NoticeMsgIdType.BadVipAchievementIncomplete,
            "bad_unvip_grantee_not_vip" => NoticeMsgIdType.BadUnvipGranteeNotVip,
            "ban_success" => NoticeMsgIdType.BanSuccess,
            "cmds_available" => NoticeMsgIdType.CmdsAvailable,
            "color_changed" => NoticeMsgIdType.ColorChanged,
            "commercial_success" => NoticeMsgIdType.CommercialSuccess,
            "delete_message_success" => NoticeMsgIdType.DeleteMessageSuccess,
            "delete_staff_message_success" => NoticeMsgIdType.DeleteStaffMessageSuccess,
            "emote_only_off" => NoticeMsgIdType.EmoteOnlyOff,
            "emote_only_on" => NoticeMsgIdType.EmoteOnlyOn,
            "followers_off" => NoticeMsgIdType.FollowersOff,
            "followers_on" => NoticeMsgIdType.FollowersOn,
            "followers_on_zero" => NoticeMsgIdType.FollowersOnZero,
            "host_off" => NoticeMsgIdType.HostOff,
            "host_on" => NoticeMsgIdType.HostOn,
            "host_receive" => NoticeMsgIdType.HostReceive,
            "host_receive_no_count" => NoticeMsgIdType.HostReceiveNoCount,
            "host_target_went_offline" => NoticeMsgIdType.HostTargetWentOffline,
            "hosts_remaining" => NoticeMsgIdType.HostsRemaining,
            "invalid_user" => NoticeMsgIdType.InvalidUser,
            "mod_success" => NoticeMsgIdType.ModSuccess,
            "msg_banned" => NoticeMsgIdType.MsgBanned,
            "msg_bad_characters" => NoticeMsgIdType.MsgBadCharacters,
            "msg_channel_blocked" => NoticeMsgIdType.MsgChannelBlocked,
            "msg_channel_suspended" => NoticeMsgIdType.MsgChannelSuspended,
            "msg_duplicate" => NoticeMsgIdType.MsgDuplicate,
            "msg_emoteonly" => NoticeMsgIdType.MsgEmoteOnly,
            "msg_followersonly" => NoticeMsgIdType.MsgFollowersOnly,
            "msg_followersonly_followed" => NoticeMsgIdType.MsgFollowersOnlyFollowed,
            "msg_followersonly_zero" => NoticeMsgIdType.MsgFollowersOnlyZero,
            "msg_r9k" => NoticeMsgIdType.MsgR9K,
            "msg_ratelimit" => NoticeMsgIdType.MsgRateLimit,
            "msg_rejected" => NoticeMsgIdType.MsgRejected,
            "msg_rejected_mandatory" => NoticeMsgIdType.MsgRejectedMandatory,
            "msg_requires_verified_phone_number" => NoticeMsgIdType.MsgRequiresVerifiedPhoneNumber,
            "msg_slowmode" => NoticeMsgIdType.MsgSlowMode,
            "msg_subsonly" => NoticeMsgIdType.MsgSubsOnly,
            "msg_suspended" => NoticeMsgIdType.MsgSuspended,
            "msg_timedout" => NoticeMsgIdType.MsgTimedOut,
            "msg_verified_email" => NoticeMsgIdType.MsgVerifiedEmail,
            "no_help" => NoticeMsgIdType.NoHelp,
            "no_mods" => NoticeMsgIdType.NoMods,
            "no_vips" => NoticeMsgIdType.NoVips,
            "not_hosting" => NoticeMsgIdType.NotHosting,
            "no_permission" => NoticeMsgIdType.NoPermission,
            "r9k_off" => NoticeMsgIdType.R9KOff,
            "r9k_on" => NoticeMsgIdType.R9KOn,
            "raid_error_already_raiding" => NoticeMsgIdType.RaidErrorAlreadyRaiding,
            "raid_error_forbidden" => NoticeMsgIdType.RaidErrorForbidden,
            "raid_error_self" => NoticeMsgIdType.RaidErrorSelf,
            "raid_error_too_many_viewers" => NoticeMsgIdType.RaidErrorTooManyViewers,
            "raid_error_unexpected" => NoticeMsgIdType.RaidErrorUnexpected,
            "raid_notice_mature" => NoticeMsgIdType.RaidNoticeMature,
            "raid_notice_restricted_chat" => NoticeMsgIdType.RaidNoticeRestrictedChat,
            "room_mods" => NoticeMsgIdType.RoomMods,
            "slow_off" => NoticeMsgIdType.SlowOff,
            "slow_on" => NoticeMsgIdType.SlowOn,
            "subs_off" => NoticeMsgIdType.SubsOff,
            "subs_on" => NoticeMsgIdType.SubsOn,
            "timeout_no_timeout" => NoticeMsgIdType.TimeoutNoTimeout,
            "timeout_success" => NoticeMsgIdType.TimeoutSuccess,
            "tos_ban" => NoticeMsgIdType.TosBan,
            "turbo_only_color" => NoticeMsgIdType.TurboOnlyColor,
            "unavailable_command" => NoticeMsgIdType.UnavailableCommand,
            "unban_success" => NoticeMsgIdType.UnbanSuccess,
            "unmod_success" => NoticeMsgIdType.UnmodSuccess,
            "unraid_error_no_active_raid" => NoticeMsgIdType.UnraidErrorNoActiveRaid,
            "unraid_error_unexpected" => NoticeMsgIdType.UnraidErrorUnexpected,
            "unraid_success" => NoticeMsgIdType.UnraidSuccess,
            "unrecognized_cmd" => NoticeMsgIdType.UnrecognizedCmd,
            "untimeout_banned" => NoticeMsgIdType.UntimeoutBanned,
            "untimeout_success" => NoticeMsgIdType.UntimeoutSuccess,
            "unvip_success" => NoticeMsgIdType.UnvipSuccess,
            "usage_ban" => NoticeMsgIdType.UsageBan,
            "usage_clear" => NoticeMsgIdType.UsageClear,
            "usage_color" => NoticeMsgIdType.UsageColor,
            "usage_commercial" => NoticeMsgIdType.UsageCommercial,
            "usage_disconnect" => NoticeMsgIdType.UsageDisconnect,
            "usage_delete" => NoticeMsgIdType.UsageDelete,
            "usage_emote_only_off" => NoticeMsgIdType.UsageEmoteOnlyOff,
            "usage_emote_only_on" => NoticeMsgIdType.UsageEmoteOnlyOn,
            "usage_followers_off" => NoticeMsgIdType.UsageFollowersOff,
            "usage_followers_on" => NoticeMsgIdType.UsageFollowersOn,
            "usage_help" => NoticeMsgIdType.UsageHelp,
            "usage_host" => NoticeMsgIdType.UsageHost,
            "usage_marker" => NoticeMsgIdType.UsageMarker,
            "usage_me" => NoticeMsgIdType.UsageMe,
            "usage_mod" => NoticeMsgIdType.UsageMod,
            "usage_mods" => NoticeMsgIdType.UsageMods,
            "usage_r9k_off" => NoticeMsgIdType.UsageR9KOff,
            "usage_r9k_on" => NoticeMsgIdType.UsageR9KOn,
            "usage_raid" => NoticeMsgIdType.UsageRaid,
            "usage_slow_off" => NoticeMsgIdType.UsageSlowOff,
            "usage_slow_on" => NoticeMsgIdType.UsageSlowOn,
            "usage_subs_off" => NoticeMsgIdType.UsageSubsOff,
            "usage_subs_on" => NoticeMsgIdType.UsageSubsOn,
            "usage_timeout" => NoticeMsgIdType.UsageTimeout,
            "usage_unban" => NoticeMsgIdType.UsageUnban,
            "usage_unhost" => NoticeMsgIdType.UsageUnhost,
            "usage_unmod" => NoticeMsgIdType.UsageUnmod,
            "usage_unraid" => NoticeMsgIdType.UsageUnraid,
            "usage_untimeout" => NoticeMsgIdType.UsageUntimeout,
            "usage_unvip" => NoticeMsgIdType.UsageUnvip,
            "usage_user" => NoticeMsgIdType.UsageUser,
            "usage_vip" => NoticeMsgIdType.UsageVip,
            "usage_vips" => NoticeMsgIdType.UsageVips,
            "usage_whisper" => NoticeMsgIdType.UsageWhisper,
            "vip_success" => NoticeMsgIdType.VipSuccess,
            "vips_success" => NoticeMsgIdType.VipsSuccess,
            "whisper_banned" => NoticeMsgIdType.WhisperBanned,
            "whisper_banned_recipient" => NoticeMsgIdType.WhisperBannedRecipient,
            "whisper_invalid_login" => NoticeMsgIdType.WhisperInvalidLogin,
            "whisper_invalid_self" => NoticeMsgIdType.WhisperInvalidSelf,
            "whisper_limit_per_min" => NoticeMsgIdType.WhisperLimitPerMin,
            "whisper_limit_per_sec" => NoticeMsgIdType.WhisperLimitPerSec,
            "whisper_restricted" => NoticeMsgIdType.WhisperRestricted,
            "whisper_restricted_recipient" => NoticeMsgIdType.WhisperRestrictedRecipient,
            _ => NoticeMsgIdType.Undefined
        };
    }

    private Message GetClearMsgMessage(
        string response
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

    private Message GetUserMessage( 
        string response, 
        string channel 
    ) {
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
    private readonly static Regex TargetUserIdPattern = new("target-user-id=([^ ;]*)");
    private readonly static Regex MsgIdPattern = new("msg-id=([^ ;]*)");
}

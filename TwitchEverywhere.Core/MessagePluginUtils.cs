using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;

namespace TwitchEverywhere.Core;

public static partial class MessagePluginUtils {

    [GeneratedRegex("badge-info=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex BadgeInfoPattern();
    
    [GeneratedRegex("badges=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex BadgesPattern();

    [GeneratedRegex("ban-duration=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex BanDurationPattern();

    [GeneratedRegex("bits=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex BitsPattern();

    [GeneratedRegex("color=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex ColorPattern();

    [GeneratedRegex("display-name=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex DisplayNamePattern();

    [GeneratedRegex("emote-only=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex EmoteOnlyPattern();

    [GeneratedRegex("emote-sets=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex EmoteSetsPattern();

    [GeneratedRegex("emotes=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex EmotesPattern();

    [GeneratedRegex("followers-only=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex FollowersOnlyPattern();

    [GeneratedRegex(@" :(.+?)!", RegexOptions.NonBacktracking)]
    public static partial Regex FromUserPattern();

    [GeneratedRegex(@":([a-zA-Z-]+)(?=\s\d+)")]
    public static partial Regex HostTargetPattern();

    [GeneratedRegex(@"\d+$", RegexOptions.NonBacktracking)]
    public static partial Regex HostViewerCountPattern();

    [GeneratedRegex(";id=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex IdPattern();

    [GeneratedRegex("login=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex LoginPattern();

    [GeneratedRegex("message-id=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MessageIdPattern();

    [GeneratedRegex("tmi-sent-ts=([0-9]+)", RegexOptions.NonBacktracking)]
    public static partial Regex MessageTimestampPattern();

    [GeneratedRegex("mod=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex ModPattern();

    [GeneratedRegex("msg-id=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgIdPattern();

    [GeneratedRegex(@"msg-param-cumulative-months=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamCumulativeMonthsPattern();

    [GeneratedRegex(@"msg-param-displayName=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamDisplayNamePattern();

    [GeneratedRegex(@"msg-param-gift-months=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamGiftMonthsPattern();

    [GeneratedRegex(@"msg-param-login=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamLoginPattern();

    [GeneratedRegex(@"msg-param-months=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamMonthsPattern();

    [GeneratedRegex(@"msg-param-promo-gift-total=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamPromoGiftTotalPattern();

    [GeneratedRegex(@"msg-param-promo-name=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamPromoNamePattern();

    [GeneratedRegex(@"msg-param-recipient-display-name=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamRecipientDisplayNamePattern();

    [GeneratedRegex(@"msg-param-recipient-id=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamRecipientIdPattern();

    [GeneratedRegex(@"msg-param-recipient-user-name=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamRecipientUserNamePattern();

    [GeneratedRegex(@"msg-param-ritual-name=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamRitualNamePattern();

    [GeneratedRegex(@"msg-param-sender-login=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamSenderLoginPattern();

    [GeneratedRegex(@"msg-param-sender-name=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamSenderNamePattern();

    [GeneratedRegex(@"msg-param-should-share-streak=([^;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamShouldShareStreakPattern();

    [GeneratedRegex(@"msg-param-streak-months=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamStreakMonthsPattern();

    [GeneratedRegex(@"msg-param-sub-plan-name=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamSubPlanNamePattern();

    [GeneratedRegex(@"msg-param-sub-plan=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamSubPlanPattern();

    [GeneratedRegex(@"msg-param-threshold=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamThresholdPattern();

    [GeneratedRegex(@"msg-param-viewerCount=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex MsgParamViewerCountPattern();

    [GeneratedRegex(@".+:(.+ ?).*", RegexOptions.NonBacktracking)]
    public static partial Regex MsgTextPattern();

    [GeneratedRegex("pinned-chat-paid-amount=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex PinnedChatPaidAmountPattern();

    [GeneratedRegex("pinned-chat-paid-currency=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex PinnedChatPaidCurrencyPattern();

    [GeneratedRegex("pinned-chat-paid-exponent=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex PinnedChatPaidExponentPattern();

    [GeneratedRegex("pinned-chat-paid-is-system-message=([^;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex PinnedChatPaidIsSystemMessagePattern();

    [GeneratedRegex("pinned-chat-paid-level=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex PinnedChatPaidLevelPattern();

    [GeneratedRegex("r9k=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex R9KPattern();

    [GeneratedRegex("reply-parent-display-name=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex ReplyParentDisplayNamePattern();

    [GeneratedRegex("reply-parent-msg-id=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex ReplyParentMsgIdPattern();

    [GeneratedRegex("reply-parent-user-id=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex ReplyParentUserIdPattern();

    [GeneratedRegex("reply-parent-user-login=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex ReplyParentUserLoginPattern();

    [GeneratedRegex("reply-thread-parent-msg-id=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex ReplyThreadParentMsgPattern();

    [GeneratedRegex("room-id=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex RoomIdPattern();

    [GeneratedRegex("slow=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex SlowPattern();

    [GeneratedRegex("subs-only=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex SubsOnlyPattern();

    [GeneratedRegex("subscriber=([^;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex SubscriberPattern();

    [GeneratedRegex("system-msg=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex SystemMessagePattern();

    [GeneratedRegex("target-msg-id=([^;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex TargetMessageIdPattern();

    [GeneratedRegex("target-user-id=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex TargetUserIdPattern();

    [GeneratedRegex("thread-id=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex ThreadIdPattern();

    [GeneratedRegex(@"WHISPER\s(.*?) :[^:]*$", RegexOptions.NonBacktracking)]
    public static partial Regex ToUserPattern();

    [GeneratedRegex("turbo=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex TurboPattern();

    [GeneratedRegex("user-id=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex UserIdPattern();

    [GeneratedRegex(@":([^!]+)![^@]+@[^ ]+", RegexOptions.NonBacktracking)]
    public static partial Regex UserJoinPattern();

    [GeneratedRegex("user-type=([^; ]+)", RegexOptions.NonBacktracking)]
    public static partial Regex UserTypePattern();

    [GeneratedRegex("vip=([^ ;]*)", RegexOptions.NonBacktracking)]
    public static partial Regex VipPattern();

    public static string GetPropertyAsString(
        Properties property
    ) {
        if( !Patterns.ContainsKey( property ) ) {
            throw new ArgumentException( $"Property '{property}' is not defined." );
        }

        return Patterns[property];
    }

    public static string GetValueFromResponse(
        string message,
        Regex pattern
    ) {
        return pattern.Match( message ).Groups[1].Value;
    }

    public static int GetIntValueFromResponse(
        string message,
        Regex pattern
    ) {
        string value = GetValueFromResponse( message, pattern );

        return string.IsNullOrEmpty( value ) ? 0 : int.Parse( value );

    }

    public static bool GetValueIsPresentOrBoolean(
        string message,
        Regex pattern
    ) {
        string value = GetValueFromResponse( message, pattern );

        if( string.IsNullOrEmpty( value ) ) {
            return false;
        }

        return int.Parse( value ) == 1;
    }

    public static string GetLastSplitValuesFromResponse(
        string message,
        Regex pattern
    ) {
        string[] splitResult = pattern.Split( message );
        return splitResult.Length > 1 ? splitResult[1] : "";
    }

    public static IImmutableList<Emote>? GetEmotesFromText(
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

    public static IImmutableList<Badge> GetBadges(
        string badges
    ) {
        string[] badgeList = badges.Split( ',' );

        if( string.IsNullOrEmpty( badges ) ) {
            return Array.Empty<Badge>().ToImmutableList();
        }

        List<Badge> parsedBadges = new();

        foreach (string badge in badgeList) {
            string[] badgeInfo = badge.Split( '/' );

            if( badgeInfo.Length == 2 ) {
                parsedBadges.Add( new Badge( Name: badgeInfo[0], Version: badgeInfo[1] ) );
            }
        }

        return parsedBadges.ToImmutableList();
    }

    public static PinnedChatPaidLevel? GetPinnedChatPaidLevelType(
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

    public static UserType GetUserType(
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

    public static string GetUserTypeText(
        UserType userType
    ) {
        return userType switch {
            UserType.Mod => "mod",
            UserType.Admin => "admin",
            UserType.GlobalMod => "global_mod",
            UserType.Staff => "staff",
            _ => "normal",
        };
    }

    public enum Properties {
        BadgeInfo,
        Badges,
        BanDuration,
        Bits,
        Color,
        DisplayName,
        EmoteOnly,
        EmoteSets,
        Emotes,
        FollowersOnly,
        FromUser,
        HostTarget,
        HostViewerCount,
        Id,
        Login,
        MessageId,
        MessageTimestamp,
        Mod,
        MsgId,
        MsgParamCumulativeMonths,
        MsgParamDisplayName,
        MsgParamGiftMonths,
        MsgParamLogin,
        MsgParamMonths,
        MsgParamPromoGiftTotal,
        MsgParamPromoName,
        MsgParamRecipientDisplayName,
        MsgParamRecipientId,
        MsgParamRecipientUserName,
        MsgParamRitualName,
        MsgParamSenderLogin,
        MsgParamSenderName,
        MsgParamShouldShareStreak,
        MsgParamStreakMonths,
        MsgParamSubPlanName,
        MsgParamSubPlan,
        MsgParamThreshold,
        MsgParamViewerCount,
        MsgText,
        PinnedChatPaidAmount,
        PinnedChatPaidCanonicalAmount,
        PinnedChatPaidCurrency,
        PinnedChatPaidExponent,
        PinnedChatPaidIsSystemMessage,
        PinnedChatPaidLevel,
        R9K,
        ReplyParentDisplayName,
        ReplyParentMsgId,
        ReplyParentUserId,
        ReplyParentUserLogin,
        ReplyThreadParentMsg,
        RoomId,
        Slow,
        SubsOnly,
        Subscriber,
        SystemMessage,
        TargetMessageId,
        TargetUserId,
        ThreadId,
        ToUser,
        Turbo,
        UserId,
        UserJoin,
        UserType,
        Vip
    }

    private readonly static Dictionary<Properties, string> Patterns = new() {
        { Properties.BadgeInfo, "badge-info={0};" },
        { Properties.Badges, "badges={0};" },
        { Properties.BanDuration, "ban-duration={0};" },
        { Properties.Bits, "bits={0};" },
        { Properties.Color, "color={0};" },
        { Properties.DisplayName, "display-name={0};" },
        { Properties.EmoteOnly, "emote-only={0};" },
        { Properties.EmoteSets, "emote-sets={0};" },
        { Properties.Emotes, "emotes={0};" },
        { Properties.FollowersOnly, "followers-only={0};" },
        { Properties.FromUser, "from-user={0};" },
        { Properties.HostTarget, "host-target={0};" },
        { Properties.HostViewerCount, "host-viewer-count={0};" },
        { Properties.Id, "id={0};" },
        { Properties.Login, "login={0};" },
        { Properties.MessageId, "msg-id={0};" },
        { Properties.MessageTimestamp, "tmi-sent-ts={0};" },
        { Properties.Mod, "mod={0};" },
        { Properties.MsgId, "msg-id={0};" },
        { Properties.MsgParamCumulativeMonths, "msg-param-cumulative-months={0};" },
        { Properties.MsgParamDisplayName, "msg-param-display-name={0};" },
        { Properties.MsgParamGiftMonths, "msg-param-gift-months={0};" },
        { Properties.MsgParamLogin, "msg-param-login={0};" },
        { Properties.MsgParamMonths, "msg-param-months={0};" },
        { Properties.MsgParamPromoGiftTotal, "msg-param-promo-gift-total={0};" },
        { Properties.MsgParamPromoName, "msg-param-promo-name={0};" },
        { Properties.MsgParamRecipientDisplayName, "msg-param-recipient-display-name={0};" },
        { Properties.MsgParamRecipientId, "msg-param-recipient-id={0};" },
        { Properties.MsgParamRecipientUserName, "msg-param-recipient-user-name={0};" },
        { Properties.MsgParamRitualName, "msg-param-ritual-name={0};" },
        { Properties.MsgParamSenderLogin, "msg-param-sender-login={0};" },
        { Properties.MsgParamSenderName, "msg-param-sender-name={0};" },
        { Properties.MsgParamShouldShareStreak, "msg-param-should-share-streak={0};" },
        { Properties.MsgParamStreakMonths, "msg-param-streak-months={0};" },
        { Properties.MsgParamSubPlanName, "msg-param-sub-plan-name={0};" },
        { Properties.MsgParamSubPlan, "msg-param-sub-plan={0};" },
        { Properties.MsgParamThreshold, "msg-param-threshold={0};" },
        { Properties.MsgParamViewerCount, "msg-param-viewer-count={0};" },
        { Properties.MsgText, "msg-text={0};" },
        { Properties.PinnedChatPaidAmount, "pinned-chat-paid-amount={0};" },
        { Properties.PinnedChatPaidCanonicalAmount, "pinned-chat-paid-canonical-amount={0};" },
        { Properties.PinnedChatPaidCurrency, "pinned-chat-paid-currency={0};" },
        { Properties.PinnedChatPaidExponent, "pinned-chat-paid-exponent={0};" },
        { Properties.PinnedChatPaidIsSystemMessage, "pinned-chat-paid-is-system-message={0};" },
        { Properties.PinnedChatPaidLevel, "pinned-chat-paid-level={0};" },
        { Properties.R9K, "r9k={0};" },
        { Properties.ReplyParentDisplayName, "reply-parent-display-name={0};" },
        { Properties.ReplyParentMsgId, "reply-parent-msg-id={0};" },
        { Properties.ReplyParentUserId, "reply-parent-user-id={0};" },
        { Properties.ReplyParentUserLogin, "reply-parent-user-login={0};" },
        { Properties.ReplyThreadParentMsg, "reply-thread-parent-msg={0};" },
        { Properties.RoomId, "room-id={0};" },
        { Properties.Slow, "slow={0};" },
        { Properties.SubsOnly, "subs-only={0};" },
        { Properties.Subscriber, "subscriber={0};" },
        { Properties.SystemMessage, "system-message={0};" },
        { Properties.TargetMessageId, "target-msg-id={0};" },
        { Properties.TargetUserId, "target-user-id={0};" },
        { Properties.ThreadId, "thread-id={0};" },
        { Properties.ToUser, "to-user={0};" },
        { Properties.Turbo, "turbo={0};" },
        { Properties.UserJoin, "user-join={0};" },
        { Properties.UserId, "user-id={0};" },
        { Properties.UserType, "user-type={0};" },
        { Properties.Vip, "vip={0};" }
    };
    
    public static string GetTagsFromMessage( RawMessage message ) {

        if( !message.TagsRange.HasValue ) {
            return string.Empty;
        }

        return Encoding.UTF8.GetString(
            message.Data.Span[
                message.TagsRange.Value.Start
                    ..message.TagsRange.Value.End
            ]
        );
    }
}
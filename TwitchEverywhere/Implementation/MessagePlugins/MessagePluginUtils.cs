using System.Collections.Immutable;
using System.Text.RegularExpressions;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.Implementation.MessagePlugins; 

internal static class MessagePluginUtils {
    
    // When we drop .NET 6 these can use the GeneratedRegex source generator
    // https://devblogs.microsoft.com/dotnet/regular-expression-improvements-in-dotnet-7/
    
    
    public readonly static Regex BadgeInfoPattern = new("badge-info=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex BadgesPattern = new("badges=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex BanDurationPattern = new("ban-duration=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex BitsPattern = new("bits=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex ColorPattern = new("color=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex DisplayNamePattern = new("display-name=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex EmoteOnlyPattern = new("emote-only=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex EmoteSetsPattern = new("emote-sets=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex EmotesPattern = new("emotes=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex FollowersOnlyPattern = new("followers-only=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex IdPattern = new(";id=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex LoginPattern = new("login=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MessageIdPattern = new("message-id=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MessageTimestampPattern = new("tmi-sent-ts=([0-9]+)", RegexOptions.Compiled);
    public readonly static Regex ModPattern = new("mod=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgIdPattern = new("msg-id=([^ ;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamCumulativeMonthsPattern = new(@"msg-param-cumulative-months=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamDisplayNamePattern = new(@"msg-param-displayName=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamGiftMonthsPattern = new(@"msg-param-gift-months=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamLoginPattern = new(@"msg-param-login=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamMonthsPattern = new(@"msg-param-months=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamPromoGiftTotalPattern = new(@"msg-param-promo-gift-total=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamPromoNamePattern = new(@"msg-param-promo-name=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamRecipientDisplayNamePattern = new(@"msg-param-recipient-display-name=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamRecipientIdPattern = new(@"msg-param-recipient-id=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamRecipientUserNamePattern = new(@"msg-param-recipient-user-name=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamRitualNamePattern = new(@"msg-param-ritual-name=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamSenderLoginPattern = new(@"msg-param-sender-login=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamSenderNamePattern = new(@"msg-param-sender-name=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamShouldShareStreakPattern = new(@"msg-param-should-share-streak=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamStreakMonthsPattern = new(@"msg-param-streak-months=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamSubPlanNamePattern = new(@"msg-param-sub-plan-name=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamSubPlanPattern = new(@"msg-param-sub-plan=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamThresholdPattern = new(@"msg-param-threshold=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex MsgParamViewerCountPattern = new(@"msg-param-viewerCount=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex PinnedChatPaidAmountPattern = new("pinned-chat-paid-amount=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex PinnedChatPaidCurrencyPattern = new("pinned-chat-paid-currency=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex PinnedChatPaidExponentPattern = new("pinned-chat-paid-exponent=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex PinnedChatPaidIsSystemMessagePattern = new("pinned-chat-paid-is-system-message=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex PinnedChatPaidLevelPattern = new("pinned-chat-paid-level=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex R9KPattern = new("r9k=([^ ;]*)", RegexOptions.Compiled);
    public readonly static Regex ReplyParentDisplayNamePattern = new("reply-parent-display-name=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex ReplyParentMsgIdPattern = new("reply-parent-msg-id=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex ReplyParentUserIdPattern = new("reply-parent-user-id=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex ReplyParentUserLoginPattern = new("reply-parent-user-login=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex ReplyThreadParentMsgPattern = new("reply-thread-parent-msg-id=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex RoomIdPattern = new("room-id=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex SlowPattern = new("slow=([^ ;]*)", RegexOptions.Compiled);
    public readonly static Regex SubsOnlyPattern = new("subs-only=([^ ;]*)", RegexOptions.Compiled);
    public readonly static Regex SubscriberPattern = new("subscriber=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex SystemMessagePattern = new("system-msg=([^ ;]*)", RegexOptions.Compiled);
    public readonly static Regex TargetMessageIdPattern = new("target-msg-id=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex TargetUserIdPattern = new("target-user-id=([^ ;]*)", RegexOptions.Compiled);
    public readonly static Regex ThreadIdPattern = new("thread-id=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex TurboPattern = new("turbo=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex UserIdPattern = new("user-id=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex UserTypePattern = new("user-type=([^; ]+)", RegexOptions.Compiled);
    public readonly static Regex VipPattern = new("vip=([^;]*)", RegexOptions.Compiled);
    public readonly static Regex UserJoinPattern = new(@"(?<=:)\w+(?=!)", RegexOptions.Compiled);
    public readonly static Regex HostViewerCountPattern = new(@"\d+$", RegexOptions.Compiled);
    public readonly static Regex HostTargetPattern = new(@":([a-zA-Z-]+)(?=\s\d+)", RegexOptions.Compiled);
    public readonly static Regex FromUserPattern = new(@" :(.+?)!", RegexOptions.Compiled);
    public readonly static Regex ToUserPattern = new(@"WHISPER\s(.*?) :[^:]*$", RegexOptions.Compiled);
    public readonly static Regex MsgTextPattern = new(@".+:(.+ ?).*", RegexOptions.Compiled);
    
    public static string GetValueFromResponse( 
        string message,
        Regex pattern
    ) {
        return pattern.Match( message ).Groups[1].Value;
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
}
using System.Text.RegularExpressions;

namespace TwitchEverywhere.Implementation.MessagePlugins; 

internal static class MessagePluginUtils {
    
    // When we drop .NET 6 these can use the GeneratedRegex source generator
    // https://devblogs.microsoft.com/dotnet/regular-expression-improvements-in-dotnet-7/
    
    public readonly static Regex DisplayNamePattern = new("display-name([^;]*);");
    public readonly static Regex BadgesPattern = new("badges([^;]*);");
    public readonly static Regex BadgeInfoPattern = new("badge-info([^;]*);");
    public readonly static Regex BitsPattern = new("bits=([^;]*);");
    public readonly static Regex ColorPattern = new("color=([^;]*);");
    public readonly static Regex EmotesPattern = new("emotes=([^;]*);");
    public readonly static Regex EmoteOnlyPattern = new("emote-only=([^;]*);");
    public readonly static Regex EmoteSetsPattern = new("emote-sets=([^;]*);");
    public readonly static Regex FollowersOnlyPattern = new("followers-only=([^;]*);");
    public readonly static Regex IdPattern = new(";id=([^;]*);");
    public readonly static Regex MessageIdPattern = new("message-id=([^;]*);");
    public readonly static Regex ModPattern = new("mod=([^;]*);");
    public readonly static Regex MessageTimestampPattern = new("tmi-sent-ts=([0-9]+)");
    public readonly static Regex PinnedChatPaidAmountPattern = new("pinned-chat-paid-amount=([^;]*);");
    public readonly static Regex PinnedChatPaidCurrencyPattern = new("pinned-chat-paid-currency=([^;]*);");
    public readonly static Regex PinnedChatPaidExponentPattern = new("pinned-chat-paid-exponent=([^;]*);");
    public readonly static Regex PinnedChatPaidLevelPattern = new("pinned-chat-paid-level=([^;]*);");
    public readonly static Regex PinnedChatPaidIsSystemMessagePattern = new("pinned-chat-paid-is-system-message=([^;]*);");
    public readonly static Regex RoomIdPattern = new("room-id=([^;]*);");
    public readonly static Regex ReplyParentMsgIdPattern = new("reply-parent-msg-id=([^;]*);");
    public readonly static Regex ReplyParentUserIdPattern = new("reply-parent-user-id=([^;]*);");
    public readonly static Regex ReplyParentUserLoginPattern = new("reply-parent-user-login=([^;]*);");
    public readonly static Regex ReplyParentDisplayNamePattern = new("reply-parent-display-name=([^;]*);");
    public readonly static Regex ReplyThreadParentMsgPattern = new("reply-thread-parent-msg-id=([^;]*);");
    public readonly static Regex SubscriberPattern = new("subscriber=([^;]*);");
    public readonly static Regex TurboPattern = new("turbo=([^;]*);");
    public readonly static Regex UserIdPattern = new("user-id=([^;]*);");
    public readonly static Regex UserTypePattern = new("user-type=([^; ]+)");
    public readonly static Regex VipPattern = new("vip=([^;]*)");
    public readonly static Regex BanDurationPattern = new("ban-duration=([^;]*)");
    public readonly static Regex TargetUserIdPattern = new("target-user-id=([^ ;]*)");
    public readonly static Regex ThreadIdPattern = new("thread-id=([^;]*);");
    public readonly static Regex MsgIdPattern = new("msg-id=([^ ;]*)");
    public readonly static Regex LoginPattern = new("login([^;]*)");
    public readonly static Regex TargetMessageIdPattern = new("target-msg-id([^;]*)");
    public readonly static Regex R9KPattern = new("r9k([^ ;]*)");
    public readonly static Regex SlowPattern = new("slow([^ ;]*)");
    public readonly static Regex SubsOnlyPattern = new("subs-only([^ ;]*)");
    
    public static string GetValueFromResponse(
        string message,
        Regex pattern
    ) {
        Match match = pattern
            .Match( message );

        string result = string.Empty;
        if( match.Success ) {
            result = match.Value.Split( "=" )[1].TrimEnd( ';' );
        }

        return result;
    }
}
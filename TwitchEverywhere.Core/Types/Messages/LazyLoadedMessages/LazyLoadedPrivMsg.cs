using System.Collections.Immutable;
using System.Text.RegularExpressions;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedPrivMsg(
    string channel,
    string message,
    TimeSpan? sinceStartOfStream = null
) : IPrivMsg {

    public MessageType MessageType => MessageType.PrivMsg;

    public string RawMessage => message;
    
    public string Channel { get; } = channel;

    IImmutableList<Badge> IPrivMsg.Badges {
        get {
            string badges = MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.BadgesPattern() );
            return MessagePluginUtils.GetBadges( badges );
        }
    }
    
    IImmutableList<Badge> IPrivMsg.BadgeInfo {
        get {
            string badges = MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.BadgeInfoPattern() );
            return MessagePluginUtils.GetBadges( badges );
        }
    }

    string IPrivMsg.Bits => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.BitsPattern() );

    string IPrivMsg.Color => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.ColorPattern() );

    string IPrivMsg.DisplayName => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.DisplayNamePattern() );

    IImmutableList<Emote>? IPrivMsg.Emotes {
        get {
            string emotesText = MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.EmotesPattern() );
            return MessagePluginUtils.GetEmotesFromText( emotesText );
        }
    }

    string IPrivMsg.Id => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.IdPattern() );

    bool IPrivMsg.Mod => MessagePluginUtils.GetValueIsPresentOrBoolean( message, MessagePluginUtils.ModPattern() );

    long? IPrivMsg.PinnedChatPaidAmount {
        get {
            string pinnedChatPaidAmount = MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.PinnedChatPaidAmountPattern() );
            return string.IsNullOrEmpty( pinnedChatPaidAmount ) ? null : long.Parse( pinnedChatPaidAmount );
        }
    }

    string IPrivMsg.PinnedChatPaidCurrency => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.PinnedChatPaidCurrencyPattern() );

    long? IPrivMsg.PinnedChatPaidExponent {
        get {
            string pinnedChatPaidExponent = MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.PinnedChatPaidExponentPattern() );
            return string.IsNullOrEmpty( pinnedChatPaidExponent ) ? null : int.Parse( pinnedChatPaidExponent );
        }
    }

    PinnedChatPaidLevel? IPrivMsg.PinnedChatPaidLevel {
        get {
            string pinnedChatPaidLevelText = MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.PinnedChatPaidLevelPattern() );
            return MessagePluginUtils.GetPinnedChatPaidLevelType( pinnedChatPaidLevelText );
        }
    }

    bool IPrivMsg.PinnedChatPaidIsSystemMessage => !string.IsNullOrEmpty( MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.PinnedChatPaidIsSystemMessagePattern() ) );

    string IPrivMsg.ReplyParentMsgId => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.ReplyParentMsgIdPattern() );

    string IPrivMsg.ReplyParentUserId => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.ReplyParentUserIdPattern() );

    string IPrivMsg.ReplyParentUserLogin => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.ReplyParentUserLoginPattern() );

    string IPrivMsg.ReplyParentDisplayName => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.ReplyParentDisplayNamePattern() );

    string IPrivMsg.ReplyThreadParentMsg => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.ReplyThreadParentMsgPattern() );

    string IPrivMsg.RoomId => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.RoomIdPattern() );

    bool IPrivMsg.Subscriber => MessagePluginUtils.GetValueIsPresentOrBoolean( message, MessagePluginUtils.SubscriberPattern() );

    DateTime? IPrivMsg.Timestamp {
        get {
            long rawTimestamp = Convert.ToInt64(
                MessagePluginUtils.MessageTimestampPattern().Match( message ).Value
                    .Split( "=" )[1]
                    .TrimEnd( ';' )
            );

            return DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;
        }
    }

    bool IPrivMsg.Turbo => MessagePluginUtils.GetValueIsPresentOrBoolean( message, MessagePluginUtils.TurboPattern() );

    string IPrivMsg.UserId => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.UserIdPattern() );

    UserType IPrivMsg.UserType => MessagePluginUtils.GetUserType( MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.UserTypePattern() ) );

    bool IPrivMsg.Vip => !string.IsNullOrEmpty( MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.VipPattern() ) );

    TimeSpan SinceStartOfStream { get; } = sinceStartOfStream ?? TimeSpan.Zero;

    string IPrivMsg.Text => MessagePluginUtils.GetLastSplitValuesFromResponse( message, new Regex($"PRIVMSG #{Channel} :") ).Trim('\n');
}
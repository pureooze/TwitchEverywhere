using System.Collections.Immutable;
using System.Text.RegularExpressions;
using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedPrivMsg : Message, IPrivMsg {
    private readonly string m_channel;
    private readonly string m_message;
    
    public LazyLoadedPrivMsg(
        string channel,
        string message,
        TimeSpan? sinceStartOfStream = null
    ) {
        m_channel = channel;
        m_message = message;
        SinceStartOfStream = sinceStartOfStream ?? TimeSpan.Zero;
    }

    public override MessageType MessageType => MessageType.PrivMsg;

    public override string RawMessage => m_message;

    IImmutableList<Badge> IPrivMsg.Badges {
        get {
            string badges = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.BadgesPattern );
            return MessagePluginUtils.GetBadges( badges );
        }
    }
    
    IImmutableList<Badge> IPrivMsg.BadgeInfo {
        get {
            string badges = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.BadgeInfoPattern );
            return MessagePluginUtils.GetBadges( badges );
        }
    }

    string IPrivMsg.Bits => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.BitsPattern );

    string IPrivMsg.Color => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ColorPattern );

    string IPrivMsg.DisplayName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.DisplayNamePattern );

    IImmutableList<Emote>? IPrivMsg.Emotes {
        get {
            string emotesText = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.EmotesPattern );
            return MessagePluginUtils.GetEmotesFromText( emotesText );
        }
    }

    string IPrivMsg.Id => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.IdPattern );

    bool IPrivMsg.Mod => MessagePluginUtils.GetValueIsPresentOrBoolean( m_message, MessagePluginUtils.ModPattern );

    long? IPrivMsg.PinnedChatPaidAmount {
        get {
            string pinnedChatPaidAmount = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.PinnedChatPaidAmountPattern );
            return string.IsNullOrEmpty( pinnedChatPaidAmount ) ? null : long.Parse( pinnedChatPaidAmount );
        }
    }

    string IPrivMsg.PinnedChatPaidCurrency => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.PinnedChatPaidCurrencyPattern );

    long? IPrivMsg.PinnedChatPaidExponent {
        get {
            string pinnedChatPaidExponent = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.PinnedChatPaidExponentPattern );
            return string.IsNullOrEmpty( pinnedChatPaidExponent ) ? null : int.Parse( pinnedChatPaidExponent );
        }
    }

    PinnedChatPaidLevel? IPrivMsg.PinnedChatPaidLevel {
        get {
            string pinnedChatPaidLevelText = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.PinnedChatPaidLevelPattern );
            return MessagePluginUtils.GetPinnedChatPaidLevelType( pinnedChatPaidLevelText );
        }
    }

    bool IPrivMsg.PinnedChatPaidIsSystemMessage => !string.IsNullOrEmpty( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.PinnedChatPaidIsSystemMessagePattern ) );

    string IPrivMsg.ReplyParentMsgId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ReplyParentMsgIdPattern );

    string IPrivMsg.ReplyParentUserId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ReplyParentUserIdPattern );

    string IPrivMsg.ReplyParentUserLogin => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ReplyParentUserLoginPattern );

    string IPrivMsg.ReplyParentDisplayName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ReplyParentDisplayNamePattern );

    string IPrivMsg.ReplyThreadParentMsg => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ReplyThreadParentMsgPattern );

    string IPrivMsg.RoomId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.RoomIdPattern );

    bool IPrivMsg.Subscriber => MessagePluginUtils.GetValueIsPresentOrBoolean( m_message, MessagePluginUtils.SubscriberPattern );

    DateTime IPrivMsg.Timestamp {
        get {
            long rawTimestamp = Convert.ToInt64(
                MessagePluginUtils.MessageTimestampPattern.Match( m_message ).Value
                    .Split( "=" )[1]
                    .TrimEnd( ';' )
            );

            return DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;
        }
    }

    bool IPrivMsg.Turbo => MessagePluginUtils.GetValueIsPresentOrBoolean( m_message, MessagePluginUtils.TurboPattern );

    string IPrivMsg.UserId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.UserIdPattern );

    UserType IPrivMsg.UserType => MessagePluginUtils.GetUserType( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.UserTypePattern ) );

    bool IPrivMsg.Vip => !string.IsNullOrEmpty( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.VipPattern ) );

    TimeSpan SinceStartOfStream { get; }

    string IPrivMsg.Text => MessagePluginUtils.GetLastSplitValuesFromResponse( m_message, new Regex($"PRIVMSG #{m_channel} :") ).Trim('\n');
}
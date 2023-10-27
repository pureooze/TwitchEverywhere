using System.Collections.Immutable;
using System.Text.RegularExpressions;
using TwitchEverywhere.Implementation.MessagePlugins;

namespace TwitchEverywhere.Types.Messages; 

public class PrivMsg : Message {
    private readonly string m_channel;
    private readonly string m_message;
    
    public PrivMsg(
        string channel,
        string message,
        TimeSpan sinceStartOfStream
    ) {
        m_channel = channel;
        m_message = message;
        SinceStartOfStream = sinceStartOfStream;
    }

    public override MessageType MessageType => MessageType.PrivMsg;

    public IImmutableList<Badge> Badges {
        get {
            string badges = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.BadgesPattern );
            return MessagePluginUtils.GetBadges( badges );
        }
    }

    public string Bits => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.BitsPattern );

    public string? Color => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ColorPattern );

    public string DisplayName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.DisplayNamePattern );

    public IImmutableList<Emote>? Emotes {
        get {
            string emotesText = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.EmotesPattern );
            return MessagePluginUtils.GetEmotesFromText( emotesText );
        }
    }

    public string Id => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.IdPattern );

    public bool Mod => int.Parse( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ModPattern ) ) == 1;

    public long? PinnedChatPaidAmount {
        get {
            string pinnedChatPaidAmount = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.PinnedChatPaidAmountPattern );
            return string.IsNullOrEmpty( pinnedChatPaidAmount ) ? null : long.Parse( pinnedChatPaidAmount );
        }
    }

    public string PinnedChatPaidCurrency => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.PinnedChatPaidCurrencyPattern );

    public long? PinnedChatPaidExponent {
        get {
            string pinnedChatPaidExponent = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.PinnedChatPaidExponentPattern );
            return string.IsNullOrEmpty( pinnedChatPaidExponent ) ? null : int.Parse( pinnedChatPaidExponent );
        }
    }

    public PinnedChatPaidLevel? PinnedChatPaidLevel {
        get {
            string pinnedChatPaidLevelText = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.PinnedChatPaidLevelPattern );
            return MessagePluginUtils.GetPinnedChatPaidLevelType( pinnedChatPaidLevelText );
        }
    }

    public bool PinnedChatPaidIsSystemMessage => !string.IsNullOrEmpty( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.PinnedChatPaidIsSystemMessagePattern ) );

    public string ReplyParentMsgId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ReplyParentMsgIdPattern );

    public string ReplyParentUserId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ReplyParentUserIdPattern );

    public string ReplyParentUserLogin => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ReplyParentUserLoginPattern );

    public string ReplyParentDisplayName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ReplyParentDisplayNamePattern );

    public string ReplyThreadParentMsg => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ReplyThreadParentMsgPattern );

    public string RoomId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.RoomIdPattern );

    public bool Subscriber => int.Parse( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.SubscriberPattern ) ) == 1;

    public DateTime Timestamp {
        get {
            long rawTimestamp = Convert.ToInt64(
                MessagePluginUtils.MessageTimestampPattern.Match( m_message ).Value
                    .Split( "=" )[1]
                    .TrimEnd( ';' )
            );

            return DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;
        }
    }

    public bool Turbo => int.Parse( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.TurboPattern ) ) == 1;

    public string UserId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.UserIdPattern );

    public UserType UserType => MessagePluginUtils.GetUserType( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.UserTypePattern ) );

    public bool Vip => !string.IsNullOrEmpty( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.VipPattern ) );

    public TimeSpan SinceStartOfStream { get; }

    public string Text => MessagePluginUtils.GetLastSplitValuesFromResponse( m_message, new Regex($"PRIVMSG #{m_channel} :") );

}
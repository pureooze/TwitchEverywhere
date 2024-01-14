using System.Collections.Immutable;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class PrivMsg : IPrivMsg {
    private readonly IPrivMsg m_inner;
    
    private string m_rawMessage;
    private string m_channel;
    private IImmutableList<Badge>? m_badges;
    private IImmutableList<Badge>? m_badgeInfo;
    private string m_bits;
    private string m_color;
    private string m_displayName;
    private IImmutableList<Emote>? m_emotes;
    private string m_id;
    private bool? m_mod;
    private long? m_pinnedChatPaidAmount;
    private string m_pinnedChatPaidCurrency;
    private long? m_pinnedChatPaidExponent;
    private PinnedChatPaidLevel? m_pinnedChatPaidLevel;
    private bool? m_pinnedChatPaidIsSystemMessage;
    private string m_replyParentMsgId;
    private string m_replyParentUserId;
    private string m_replyParentUserLogin;
    private string m_replyParentDisplayName;
    private string m_replyThreadParentMsg;
    private string m_roomId;
    private bool? m_subscriber;
    private DateTime? m_timestamp;
    private bool? m_turbo;
    private string m_userId;
    private UserType? m_userType;
    private bool? m_vip;
    private string m_text = string.Empty;

    public PrivMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedPrivMsg( message );
    }
    
    public PrivMsg(
        string channel,
        IImmutableList<Badge>? badges = null,
        IImmutableList<Badge>? badgeInfo = null,
        string? bits = null,
        string? color = null,
        string? displayName = null,
        IImmutableList<Emote>? emotes = null,
        string? id = null,
        bool mod = default,
        long? pinnedChatPaidAmount = default,
        string? pinnedChatPaidCurrency = null,
        long? pinnedChatPaidExponent = default,
        PinnedChatPaidLevel? pinnedChatPaidLevel = default,
        bool pinnedChatPaidIsSystemMessage = default,
        string? replyParentMsgId = null,
        string? replyParentUserId = null,
        string? replyParentUserLogin = null,
        string? replyParentDisplayName = null,
        string? replyThreadParentMsg = null,
        string? roomId = null,
        bool subscriber = default,
        DateTime? timestamp = null,
        bool turbo = default,
        string? userId = null,
        UserType userType = default,
        bool vip = default,
        string? text = null
    ) {
        m_badges = badges ?? ImmutableArray<Badge>.Empty;
        m_badgeInfo = badgeInfo ?? ImmutableArray<Badge>.Empty;
        m_bits = bits ?? string.Empty;
        m_color = color ?? string.Empty;
        m_displayName = displayName ?? string.Empty;
        m_channel = channel;
        m_emotes = emotes ?? ImmutableArray<Emote>.Empty;
        m_id = id ?? string.Empty;
        m_mod = mod;
        m_pinnedChatPaidAmount = pinnedChatPaidAmount;
        m_pinnedChatPaidCurrency = pinnedChatPaidCurrency ?? string.Empty;
        m_pinnedChatPaidExponent = pinnedChatPaidExponent;
        m_pinnedChatPaidLevel = pinnedChatPaidLevel;
        m_pinnedChatPaidIsSystemMessage = pinnedChatPaidIsSystemMessage;
        m_replyParentMsgId = replyParentMsgId ?? string.Empty;
        m_replyParentUserId = replyParentUserId ?? string.Empty;
        m_replyParentUserLogin = replyParentUserLogin ?? string.Empty;
        m_replyParentDisplayName = replyParentDisplayName ?? string.Empty;
        m_replyThreadParentMsg = replyThreadParentMsg ?? string.Empty;
        m_roomId = roomId ?? string.Empty;
        m_subscriber = subscriber;
        m_timestamp = timestamp ?? null;
        m_turbo = turbo;
        m_userId = userId ?? string.Empty;
        m_userType = userType;
        m_vip = vip;
        m_text = text ?? string.Empty;
    }

    MessageType IMessage.MessageType => MessageType.PrivMsg;

    string IMessage.RawMessage => m_rawMessage;

    string IMessage.Channel => m_channel;

    IImmutableList<Badge> IPrivMsg.Badges {
        get {
            m_badges ??= m_inner.Badges;

            return m_badges;
        }
    }

    IImmutableList<Badge> IPrivMsg.BadgeInfo {
        get {
            m_badgeInfo ??= m_inner.BadgeInfo;

            return m_badgeInfo;
        }
    }

    string IPrivMsg.Bits {
        get {
            if ( string.IsNullOrEmpty( m_bits ) ) {
                m_bits = m_inner.Bits;
            }

            return m_bits;
        }
    }

    string IPrivMsg.Color {
        get {
            if ( string.IsNullOrEmpty( m_color ) ) {
                m_color = m_inner.Color;
            }

            return m_color;
        }
    }

    string IPrivMsg.DisplayName {
        get {
            if ( string.IsNullOrEmpty( m_displayName ) ) {
                m_displayName = m_inner.DisplayName;
            }

            return m_displayName;
        }
    }

    IImmutableList<Emote>? IPrivMsg.Emotes {
        get {
            m_emotes ??= m_inner.Emotes;

            return m_emotes;
        }
    }

    string IPrivMsg.Id {
        get {
            if ( string.IsNullOrEmpty( m_id ) ) {
                m_id = m_inner.Id;
            }

            return m_id;
        }
    }

    bool IPrivMsg.Mod {
        get {
            m_mod ??= m_inner.Mod;

            return m_mod.Value;
        }
    }

    long? IPrivMsg.PinnedChatPaidAmount {
        get {
            m_pinnedChatPaidAmount ??= m_inner.PinnedChatPaidAmount;

            return m_pinnedChatPaidAmount;
        }
    }

    string IPrivMsg.PinnedChatPaidCurrency {
        get {
            if ( string.IsNullOrEmpty( m_pinnedChatPaidCurrency ) ) {
                m_pinnedChatPaidCurrency = m_inner.PinnedChatPaidCurrency;
            }

            return m_pinnedChatPaidCurrency;
        }
    }

    long? IPrivMsg.PinnedChatPaidExponent {
        get {
            m_pinnedChatPaidExponent ??= m_inner.PinnedChatPaidExponent;

            return m_pinnedChatPaidExponent;
        }
    }

    PinnedChatPaidLevel? IPrivMsg.PinnedChatPaidLevel {
        get {
            m_pinnedChatPaidLevel ??= m_inner.PinnedChatPaidLevel;

            return m_pinnedChatPaidLevel;
        }
    }

    bool IPrivMsg.PinnedChatPaidIsSystemMessage {
        get {
            m_pinnedChatPaidIsSystemMessage ??= m_inner.PinnedChatPaidIsSystemMessage;

            return m_pinnedChatPaidIsSystemMessage.Value;
        }
    }

    string IPrivMsg.ReplyParentMsgId {
        get {
            if ( string.IsNullOrEmpty( m_replyParentMsgId ) ) {
                m_replyParentMsgId = m_inner.ReplyParentMsgId;
            }

            return m_replyParentMsgId;
        }
    }

    string IPrivMsg.ReplyParentUserId {
        get {
            if ( string.IsNullOrEmpty( m_replyParentUserId ) ) {
                m_replyParentUserId = m_inner.ReplyParentUserId;
            }

            return m_replyParentUserId;
        }
    }

    string IPrivMsg.ReplyParentUserLogin {
        get {
            if ( string.IsNullOrEmpty( m_replyParentUserLogin ) ) {
                m_replyParentUserLogin = m_inner.ReplyParentUserLogin;
            }

            return m_replyParentUserLogin;
        }
    }

    string IPrivMsg.ReplyParentDisplayName {
        get {
            if ( string.IsNullOrEmpty( m_replyParentDisplayName ) ) {
                m_replyParentDisplayName = m_inner.ReplyParentDisplayName;
            }

            return m_replyParentDisplayName;
        }
    }

    string IPrivMsg.ReplyThreadParentMsg {
        get {
            if ( string.IsNullOrEmpty( m_replyThreadParentMsg ) ) {
                m_replyThreadParentMsg = m_inner.ReplyThreadParentMsg;
            }

            return m_replyThreadParentMsg;
        }
    }

    string IPrivMsg.RoomId {
        get {
            if ( string.IsNullOrEmpty( m_roomId ) ) {
                m_roomId = m_inner.RoomId;
            }

            return m_roomId;
        }
    }

    bool IPrivMsg.Subscriber {
        get {
            m_subscriber ??= m_inner.Subscriber;

            return m_subscriber.Value;
        }
    }

    DateTime? IPrivMsg.Timestamp {
        get {
            m_timestamp ??= m_inner.Timestamp;

            return m_timestamp;
        }
    }

    bool IPrivMsg.Turbo {
        get {
            m_turbo ??= m_inner.Turbo;

            return m_turbo.Value;
        }
    }

    string IPrivMsg.UserId {
        get {
            if ( string.IsNullOrEmpty( m_userId ) ) {
                m_userId = m_inner.UserId;
            }

            return m_userId;
        }
    }

    UserType IPrivMsg.UserType {
        get {
            m_userType ??= m_inner.UserType;

            return m_userType.Value;
        }
    }

    bool IPrivMsg.Vip {
        get {
            m_vip ??= m_inner.Vip;

            return m_vip.Value;
        }
    }

    string IPrivMsg.Text {
        get {
            if ( string.IsNullOrEmpty( m_text ) ) {
                m_text = m_inner.Text;
            }

            return m_text;
        }
    }
}
using System.Collections.Immutable;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class GlobalUserStateMsg : IGlobalUserStateMsg {
    private MessageType m_messageType;
    private string m_rawMessage;
    private string m_channel;
    private IImmutableList<Badge>? m_badges;
    private IImmutableList<Badge>? m_badgeInfo;
    private string m_color;
    private string m_displayName;
    private IImmutableList<string>? m_emoteSets;
    private bool? m_turbo;
    private string m_userId;
    private UserType? m_userType;
    private readonly IGlobalUserStateMsg m_inner;

    public GlobalUserStateMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedGlobalUserStateMsg( message );
    }
    
    MessageType IMessage.MessageType => MessageType.GlobalUserState;

    string IMessage.RawMessage {
        get {
            if (string.IsNullOrEmpty(m_rawMessage)) {
                m_rawMessage = m_inner.RawMessage;
            }
            return m_rawMessage;
        }
    }

    string IMessage.Channel {
        get {
            if (string.IsNullOrEmpty(m_channel)) {
                m_channel = m_inner.Channel;
            }
            return m_channel;
        }
    }

    IImmutableList<Badge> IGlobalUserStateMsg.Badges {
        get {
            m_badges ??= m_inner.Badges;
            return m_badges;
        }
    }

    IImmutableList<Badge> IGlobalUserStateMsg.BadgeInfo {
        get {
            m_badgeInfo ??= m_inner.BadgeInfo;
            return m_badgeInfo;
        }
    }

    string IGlobalUserStateMsg.Color {
        get {
            if (string.IsNullOrEmpty(m_color)) {
                m_color = m_inner.Color;
            }
            return m_color;
        }
    }

    string IGlobalUserStateMsg.DisplayName {
        get {
            if (string.IsNullOrEmpty(m_displayName)) {
                m_displayName = m_inner.DisplayName;
            }
            return m_displayName;
        }
    }

    IImmutableList<string> IGlobalUserStateMsg.EmoteSets {
        get {
            m_emoteSets ??= m_inner.EmoteSets;
            return m_emoteSets;
        }
    }

    bool IGlobalUserStateMsg.Turbo {
        get {
            m_turbo ??= m_inner.Turbo;
            return m_turbo.Value;
        }
    }

    string IGlobalUserStateMsg.UserId {
        get {
            if (string.IsNullOrEmpty(m_userId)) {
                m_userId = m_inner.UserId;
            }
            return m_userId;
        }
    }

    UserType IGlobalUserStateMsg.UserType {
        get {
            m_userType ??= m_inner.UserType;
            return m_userType.Value;
        }
    }
}
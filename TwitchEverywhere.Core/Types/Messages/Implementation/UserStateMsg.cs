using System.Collections.Immutable;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class UserStateMsg : IUserStateMsg {
    private string m_rawMessage;
    private string m_channel;
    private IImmutableList<Badge>? m_badgeInfo;
    private IImmutableList<Badge>? m_badges;
    private string m_color;
    private string m_displayName;
    private IImmutableList<string>? m_emoteSets;
    private string m_id;
    private bool? m_mod;
    private bool? m_subscriber;
    private bool? m_turbo;
    private UserType? m_userType;
    private readonly IUserStateMsg m_inner;

    public UserStateMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedUserStateMsg( message );
    }
    
    MessageType IMessage.MessageType => MessageType.UserState;

    string IMessage.RawMessage => m_rawMessage;

    string IMessage.Channel => m_channel;

    IImmutableList<Badge> IUserStateMsg.BadgeInfo {
        get {
            m_badgeInfo ??= m_inner.BadgeInfo;
            return m_badgeInfo;
        }
    }

    IImmutableList<Badge> IUserStateMsg.Badges {
        get {
            m_badges ??= m_inner.Badges;
            return m_badges;
        }
    }

    string IUserStateMsg.Color {
        get {
            if (string.IsNullOrEmpty(m_color)) {
                m_color = m_inner.Color;
            }
            return m_color;
        }
    }

    string IUserStateMsg.DisplayName {
        get {
            if (string.IsNullOrEmpty(m_displayName)) {
                m_displayName = m_inner.DisplayName;
            }
            return m_displayName;
        }
    }

    IImmutableList<string> IUserStateMsg.EmoteSets {
        get {
            m_emoteSets ??= m_inner.EmoteSets;
            return m_emoteSets;
        }
    }

    string IUserStateMsg.Id {
        get {
            if (string.IsNullOrEmpty(m_id)) {
                m_id = m_inner.Id;
            }
            return m_id;
        }
    }

    bool IUserStateMsg.Mod {
        get {
            m_mod ??= m_inner.Mod;
            return m_mod.Value;
        }
    }

    bool IUserStateMsg.Subscriber {
        get {
            m_subscriber ??= m_inner.Subscriber;
            return m_subscriber.Value;
        }
    }

    bool IUserStateMsg.Turbo {
        get {
            m_turbo ??= m_inner.Turbo;
            return m_turbo.Value;
        }
    }

    UserType IUserStateMsg.UserType {
        get {
            m_userType ??= m_inner.UserType;
            return m_userType.Value;
        }
    }
}
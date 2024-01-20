using System.Collections.Immutable;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class WhisperMsg : IWhisperMsg {
    private string m_rawMessage;
    private string m_channel;
    private IImmutableList<Badge>? m_badges;
    private string m_color;
    private string m_displayName;
    private IImmutableList<Emote>? m_emotes;
    private string m_msgId;
    private string m_threadId;
    private bool? m_turbo;
    private string m_userId;
    private UserType? m_userType;
    private string m_fromUser;
    private string m_toUser;
    private string m_text;
    private readonly IWhisperMsg m_inner;

    public WhisperMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedWhisperMsg( message );
    }
    
    MessageType IMessage.MessageType => MessageType.Whisper;

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

    IImmutableList<Badge> IWhisperMsg.Badges {
        get {
            m_badges ??= m_inner.Badges;
            return m_badges;
        }
    }

    string IWhisperMsg.Color {
        get {
            if (string.IsNullOrEmpty(m_color)) {
                m_color = m_inner.Color;
            }
            return m_color;
        }
    }

    string IWhisperMsg.DisplayName {
        get {
            if (string.IsNullOrEmpty(m_displayName)) {
                m_displayName = m_inner.DisplayName;
            }
            return m_displayName;
        }
    }

    IImmutableList<Emote>? IWhisperMsg.Emotes {
        get {
            m_emotes ??= m_inner.Emotes;
            return m_emotes;
        }
    }

    string IWhisperMsg.MsgId {
        get {
            if (string.IsNullOrEmpty(m_msgId)) {
                m_msgId = m_inner.MsgId;
            }
            return m_msgId;
        }
    }

    string IWhisperMsg.ThreadId {
        get {
            if (string.IsNullOrEmpty(m_threadId)) {
                m_threadId = m_inner.ThreadId;
            }
            return m_threadId;
        }
    }

    bool IWhisperMsg.Turbo {
        get {
            m_turbo ??= m_inner.Turbo;
            return m_turbo.Value;
        }
    }

    string IWhisperMsg.UserId {
        get {
            if (string.IsNullOrEmpty(m_userId)) {
                m_userId = m_inner.UserId;
            }
            return m_userId;
        }
    }

    UserType IWhisperMsg.UserType {
        get {
            m_userType ??= m_inner.UserType;
            return m_userType.Value;
        }
    }

    string IWhisperMsg.FromUser {
        get {
            if (string.IsNullOrEmpty(m_fromUser)) {
                m_fromUser = m_inner.FromUser;
            }
            return m_fromUser;
        }
    }
    
    string IWhisperMsg.ToUser {
        get {
            if (string.IsNullOrEmpty(m_toUser)) {
                m_toUser = m_inner.ToUser;
            }
            return m_toUser;
        }
    }

    string IWhisperMsg.Text {
        get {
            if (string.IsNullOrEmpty(m_text)) {
                m_text = m_inner.Text;
            }
            return m_text;
        }
    }
}
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class ClearMsg : IClearMsg {
    private string m_rawMessage;
    private string m_channel;
    private string m_login;
    private string m_roomId;
    private string m_targetMessageId;
    private DateTime? m_timestamp;
    private string m_text;
    private readonly IClearMsg m_inner;

    public ClearMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedClearMsg( message );
    }
    
    MessageType IMessage.MessageType => MessageType.ClearMsg;

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

    string IClearMsg.Login {
        get {
            if (string.IsNullOrEmpty(m_login)) {
                m_login = m_inner.Login;
            }
            return m_login;
        }
    }

    string IClearMsg.RoomId {
        get {
            if (string.IsNullOrEmpty(m_roomId)) {
                m_roomId = m_inner.RoomId;
            }
            return m_roomId;
        }
    }

    string IClearMsg.TargetMessageId {
        get {
            if (string.IsNullOrEmpty(m_targetMessageId)) {
                m_targetMessageId = m_inner.TargetMessageId;
            }
            return m_targetMessageId;
        }
    }

    DateTime IClearMsg.Timestamp {
        get {
            m_timestamp ??= m_inner.Timestamp;
            return m_timestamp.Value;
        }
    }

    string IClearMsg.Text {
        get {
            if (string.IsNullOrEmpty(m_text)) {
                m_text = m_inner.Text;
            }
            return m_text;
        }
    }
}
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class ClearChatMsg : IClearChatMsg {
    private string m_rawMessage;
    private string m_channel;
    private long? m_duration;
    private string m_roomId;
    private string m_targetUserId;
    private DateTime? m_timestamp;
    private string m_text;
    private string m_targetUserName;
    private IClearChatMsg m_inner;

    public ClearChatMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedClearChatMsg( message );
    }

    MessageType IMessage.MessageType => MessageType.ClearChat;

    string IMessage.RawMessage => m_rawMessage;

    string IMessage.Channel => m_channel;

    long? IClearChatMsg.Duration {
        get {
            m_duration ??= m_inner.Duration;
            return m_duration;
        }
    }

    string IClearChatMsg.RoomId {
        get {
            if( string.IsNullOrEmpty( m_roomId ) ) {
                m_roomId = m_inner.RoomId;
            }
            return m_roomId;
        }
    }

    string IClearChatMsg.TargetUserId {
        get {
            if( string.IsNullOrEmpty( m_targetUserId ) ) {
                m_targetUserId = m_inner.TargetUserId;
            }
            return m_targetUserId;
        }
    }

    DateTime IClearChatMsg.Timestamp {
        get {
            m_timestamp ??= m_inner.Timestamp;

            return m_timestamp.Value;
        }
    }

    string IClearChatMsg.Text {
        get {
            if( string.IsNullOrEmpty( m_text ) ) {
                m_text = m_inner.Text;
            }
            return m_text;
        }
    }

    string IClearChatMsg.TargetUserName {
        get {
            if( string.IsNullOrEmpty( m_targetUserName ) ) {
                m_targetUserName = m_inner.TargetUserName;
            }
            return m_targetUserName;
        }
    }
}
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class JoinEndMsg : IJoinEndMsg {
    private MessageType m_messageType;
    private string m_rawMessage;
    private string m_channel;
    private readonly IJoinEndMsg m_inner;

    public JoinEndMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedJoinEndMsg( message );
    }
    
    MessageType IMessage.MessageType => MessageType.JoinEnd;

    string IMessage.RawMessage {
        get {
            if( string.IsNullOrEmpty( m_rawMessage ) ) {
                m_rawMessage = m_inner.RawMessage;
            }
            return m_rawMessage;
        }
    }

    string IMessage.Channel {
        get {
            if( string.IsNullOrEmpty( m_channel ) ) {
                m_channel = m_inner.Channel;
            }
            return m_channel;
        }
    }
}
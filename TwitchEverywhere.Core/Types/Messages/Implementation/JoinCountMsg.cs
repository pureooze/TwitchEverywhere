using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class JoinCountMsg : IJoinCountMsg {
    private MessageType m_messageType;
    private string m_rawMessage;
    private string m_channel;
    private readonly IJoinCountMsg m_inner;

    public JoinCountMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedJoinCountMsg( message );
    }
    
    MessageType IMessage.MessageType => MessageType.JoinCount;

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
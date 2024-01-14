using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class JoinMsg : IJoinMsg {
    private string m_rawMessage;
    private string m_channel;
    private string m_user;
    private readonly IJoinMsg m_inner;

    public JoinMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedJoinMsg( message );
    }
    
    MessageType IMessage.MessageType => MessageType.Join;

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

    string IJoinMsg.User {
        get {
            if( string.IsNullOrEmpty( m_user ) ) {
                m_user = m_inner.User;
            }
            
            return m_user;
        }
    }
    
}
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class JoinMsg : IJoinMsg {
    private MessageType m_messageType;
    private string m_rawMessage;
    private string m_channel;
    private string m_user;
    private IJoinMsg m_inner;

    public JoinMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedJoinMsg( message );
    }
    
    MessageType IMessage.MessageType => MessageType.Join;

    string IMessage.RawMessage => m_rawMessage;

    string IMessage.Channel => m_channel;

    string IJoinMsg.User {
        get {
            if( string.IsNullOrEmpty( m_user ) ) {
                m_user = m_inner.User;
            }
            
            return m_user;
        }
    }
    
}
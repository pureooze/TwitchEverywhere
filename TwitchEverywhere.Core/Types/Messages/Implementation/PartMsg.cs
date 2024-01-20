using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class PartMsg : IPartMsg {
    private MessageType m_messageType;
    private string m_rawMessage;
    private string m_channel;
    private string m_user;
    private readonly IPartMsg m_inner;

    public PartMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedPartMsg( message );
    }
    
    MessageType IMessage.MessageType => MessageType.Part;

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

    string IPartMsg.User {
        get {
            if( string.IsNullOrEmpty( m_user ) ) {
                m_user = m_inner.User;
            }
            
            return m_user;
        }
    }
}
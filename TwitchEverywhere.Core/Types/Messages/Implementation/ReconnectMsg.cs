using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class ReconnectMsg : IReconnectMsg {
    private MessageType m_messageType;
    private string m_rawMessage;
    private string m_channel;
    private bool m_emoteOnly;
    private int m_followersOnly;
    private bool m_r9K;
    private string m_roomId;
    private int m_slow;
    private bool m_subsOnly;
    private readonly IReconnectMsg m_inner;

    public ReconnectMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedReconnectMsg( message );
    }
    
    MessageType IMessage.MessageType => MessageType.Reconnect;

    string IMessage.RawMessage {
        get {
            if ( string.IsNullOrEmpty( m_rawMessage ) ) {
                m_rawMessage = m_inner.RawMessage;
            }

            return m_rawMessage;
        }
    }
    
    string IMessage.Channel {
        get {
            if ( string.IsNullOrEmpty( m_channel ) ) {
                m_channel = m_inner.Channel;
            }

            return m_channel;
        }
    }
}
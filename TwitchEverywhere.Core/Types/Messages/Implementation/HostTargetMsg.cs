using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class HostTargetMsg : IHostTargetMsg {
    private MessageType m_messageType;
    private string m_rawMessage;
    private string m_channel;
    private string m_hostingChannel;
    private int? m_numberOfViewers;
    private bool? m_isHostingChannel;
    private readonly IHostTargetMsg m_inner;

    public HostTargetMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedHostTargetMsg( message );
    }
    
    MessageType IMessage.MessageType => MessageType.HostTarget;

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

    string IHostTargetMsg.HostingChannel {
        get {
            if (string.IsNullOrEmpty(m_hostingChannel)) {
                m_hostingChannel = m_inner.HostingChannel;
            }
            return m_hostingChannel;
        }
    }

    int IHostTargetMsg.NumberOfViewers {
        get {
            m_numberOfViewers ??= m_inner.NumberOfViewers;
            return m_numberOfViewers.Value;
        }
    }

    bool IHostTargetMsg.IsHostingChannel {
        get {
            m_isHostingChannel ??= m_inner.IsHostingChannel;
            return m_isHostingChannel.Value;
        }
    }
}
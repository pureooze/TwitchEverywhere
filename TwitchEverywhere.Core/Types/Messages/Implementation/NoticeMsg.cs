using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class NoticeMsg : INoticeMsg {
    private string m_rawMessage;
    private string m_channel;
    private NoticeMsgIdType? m_msgId;
    private string m_targetUserId;
    private readonly LazyLoadedNoticeMsg m_inner;

    public NoticeMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedNoticeMsg( message );
    }

    MessageType IMessage.MessageType => MessageType.Notice;

    string IMessage.RawMessage {
        get {
            if( string.IsNullOrEmpty( m_rawMessage ) ) {
                m_rawMessage = m_inner.RawMessage;
            }
            return m_rawMessage;
        }
    }

    NoticeMsgIdType INoticeMsg.MsgId {
        get {
            m_msgId ??= m_inner.MsgId;
            return m_msgId.Value;
        }
    }

    string INoticeMsg.TargetUserId {
        get {
            if( string.IsNullOrEmpty( m_targetUserId ) ) {
                m_targetUserId = m_inner.TargetUserId;
            }
            return m_targetUserId;
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
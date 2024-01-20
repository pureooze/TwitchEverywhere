using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class RoomStateMsg : IRoomStateMsg {
    private MessageType m_messageType;
    private string m_rawMessage;
    private string m_channel;
    private bool? m_emoteOnly;
    private int? m_followersOnly;
    private bool? m_r9K;
    private string m_roomId;
    private int? m_slow;
    private bool? m_subsOnly;
    private readonly IRoomStateMsg m_inner;

    public RoomStateMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedRoomStateMsg( message );
    }
    
    MessageType IMessage.MessageType => MessageType.RoomState;

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

    bool IRoomStateMsg.EmoteOnly {
        get {
            m_emoteOnly ??= m_inner.EmoteOnly;
            return m_emoteOnly.Value;
        }
    }

    int IRoomStateMsg.FollowersOnly {
        get {
            m_followersOnly ??= m_inner.FollowersOnly;
            return m_followersOnly.Value;
        }
    }

    bool IRoomStateMsg.R9K {
        get {
            m_r9K ??= m_inner.R9K;
            return m_r9K.Value;
        }
    }

    string IRoomStateMsg.RoomId {
        get {
            if (string.IsNullOrEmpty(m_roomId)) {
                m_roomId = m_inner.RoomId;
            }
            return m_roomId;
        }
    }

    int IRoomStateMsg.Slow {
        get {
            m_slow ??= m_inner.Slow;
            return m_slow.Value;
        }
    }

    bool IRoomStateMsg.SubsOnly {
        get {
            m_subsOnly ??= m_inner.SubsOnly;
            return m_subsOnly.Value;
        }
    }
}
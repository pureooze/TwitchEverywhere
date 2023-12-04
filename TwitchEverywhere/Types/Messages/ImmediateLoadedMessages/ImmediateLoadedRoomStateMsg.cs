using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedRoomStateMsg : Message, IRoomStateMsg {
    private readonly bool m_emoteOnly;
    private readonly int m_followersOnly;
    private readonly bool m_r9K;
    private readonly string m_roomId;
    private readonly int m_slow;
    private readonly bool m_subsOnly;
    
    public ImmediateLoadedRoomStateMsg(
        bool emoteOnly,
        int followersOnly,
        bool r9K,
        string roomId,
        int slow,
        bool subsOnly
    ) {
        m_emoteOnly = emoteOnly;
        m_followersOnly = followersOnly;
        m_r9K = r9K;
        m_roomId = roomId;
        m_slow = slow;
        m_subsOnly = subsOnly;
    }

    public override MessageType MessageType => MessageType.RoomState;
    
    public override string RawMessage => "";

    bool IRoomStateMsg.EmoteOnly => m_emoteOnly;

    int IRoomStateMsg.FollowersOnly => m_followersOnly;

    bool IRoomStateMsg.R9K => m_r9K;

    string IRoomStateMsg.RoomId => m_roomId;

    int IRoomStateMsg.Slow => m_slow;

    bool IRoomStateMsg.SubsOnly => m_subsOnly;
}
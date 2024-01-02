using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedRoomStateMsg : IRoomStateMsg {
    private readonly bool m_emoteOnly;
    private readonly int m_followersOnly;
    private readonly bool m_r9K;
    private readonly string m_roomId;
    private readonly int m_slow;
    private readonly bool m_subsOnly;
    
    public ImmediateLoadedRoomStateMsg(
        string channel,
        bool emoteOnly,
        int followersOnly,
        bool r9K,
        string roomId,
        int slow,
        bool subsOnly
    ) {
        Channel = channel;
        m_emoteOnly = emoteOnly;
        m_followersOnly = followersOnly;
        m_r9K = r9K;
        m_roomId = roomId;
        m_slow = slow;
        m_subsOnly = subsOnly;
    }

    public MessageType MessageType => MessageType.RoomState;
    
    public string RawMessage => "";
    public string Channel { get; }

    bool IRoomStateMsg.EmoteOnly => m_emoteOnly;

    int IRoomStateMsg.FollowersOnly => m_followersOnly;

    bool IRoomStateMsg.R9K => m_r9K;

    string IRoomStateMsg.RoomId => m_roomId;

    int IRoomStateMsg.Slow => m_slow;

    bool IRoomStateMsg.SubsOnly => m_subsOnly;
}
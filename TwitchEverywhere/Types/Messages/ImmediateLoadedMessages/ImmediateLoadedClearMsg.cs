using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedClearMsg : Message, IClearMsg {
    private readonly string m_login;
    private readonly string m_roomId;
    private readonly string m_targetMessageId;
    private readonly DateTime m_timestamp;

    public ImmediateLoadedClearMsg(
        string? login = null,
        string? roomId = null,
        string? targetMessageId = null,
        DateTime timestamp = default
    ) {
        m_login = login ?? string.Empty;
        m_roomId = roomId ?? string.Empty;
        m_targetMessageId = targetMessageId ?? string.Empty;
        m_timestamp = timestamp;
    }
    public override MessageType MessageType => MessageType.ClearMsg;

    string IClearMsg.Login => m_login;

    string IClearMsg.RoomId => m_roomId;

    string IClearMsg.TargetMessageId => m_targetMessageId;

    DateTime IClearMsg.Timestamp => m_timestamp;
}
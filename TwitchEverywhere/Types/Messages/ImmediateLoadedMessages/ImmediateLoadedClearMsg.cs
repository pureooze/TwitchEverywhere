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
    public override string RawMessage => GetRawMessage();
    string IClearMsg.Login => m_login;

    string IClearMsg.RoomId => m_roomId;

    string IClearMsg.TargetMessageId => m_targetMessageId;

    DateTime IClearMsg.Timestamp => m_timestamp;

    private string GetRawMessage() {
        string message = string.Empty;

        if( !string.IsNullOrEmpty( m_login ) ) {
            message += $"login={m_login};";
        }
        if( !string.IsNullOrEmpty( m_roomId ) ) {
            message += $"room-id={m_roomId};";
        }
        if( !string.IsNullOrEmpty( m_targetMessageId ) ) {
            message += $"target-msg-id={m_targetMessageId};";
        }
        if( m_timestamp != default ) {
            message += $"tmi-sent-ts={m_timestamp};";
        }

        return message;
    }
}
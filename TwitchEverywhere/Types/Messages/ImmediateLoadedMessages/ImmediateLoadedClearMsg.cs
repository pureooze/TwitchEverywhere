using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedClearMsg : IClearMsg {
    private readonly string m_login;
    private readonly string m_roomId;
    private readonly string m_targetMessageId;
    private readonly DateTime m_timestamp;

    public ImmediateLoadedClearMsg(
        string channel,
        string? login = null,
        string? roomId = null,
        string? targetMessageId = null,
        DateTime timestamp = default
    ) {
        Channel = channel;
        m_login = login ?? string.Empty;
        m_roomId = roomId ?? string.Empty;
        m_targetMessageId = targetMessageId ?? string.Empty;
        m_timestamp = timestamp;
    }
    public MessageType MessageType => MessageType.ClearMsg;
    public string RawMessage => GetRawMessage();
    
    public string Channel { get; }
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
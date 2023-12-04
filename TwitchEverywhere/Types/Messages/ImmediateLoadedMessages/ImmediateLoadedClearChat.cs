using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedClearChat : Message, IClearChat {
    
    public ImmediateLoadedClearChat(
        long? duration,
        string roomId,
        string userId,
        DateTime timestamp,
        string text
    ) {
        Duration = duration;
        RoomId = roomId;
        UserId = userId;
        Timestamp = timestamp;
        Text = text;
    }

    public override MessageType MessageType => MessageType.ClearChat;
    public override string RawMessage => GetRawMessage();
    public long? Duration { get; }
    public string RoomId { get; }
    public string UserId { get; }
    public DateTime Timestamp { get; }
    public string Text { get; }

    private string GetRawMessage() {
        string message = string.Empty;
        
        if( Duration.HasValue ) {
            message += $"duration={Duration};";
        }
        if( !string.IsNullOrEmpty( RoomId ) ) {
            message += $"room-id={RoomId};";
        }
        if( !string.IsNullOrEmpty( UserId ) ) {
            message += $"user-id={UserId};";
        }
        if( Timestamp != default(DateTime) ) {
            message += $"tmi-sent-ts={Timestamp};";
        }
        if( !string.IsNullOrEmpty( Text ) ) {
            message += $"text={Text};";
        }

        return message;
    }
}
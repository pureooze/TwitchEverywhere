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
    public long? Duration { get; }
    public string RoomId { get; }
    public string UserId { get; }
    public DateTime Timestamp { get; }
    public string Text { get; }
}
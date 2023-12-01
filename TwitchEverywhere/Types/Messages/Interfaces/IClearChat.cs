namespace TwitchEverywhere.Types.Messages.Interfaces;

public interface IClearChat {
    MessageType MessageType { get; }
    long? Duration { get; }
    string RoomId { get; }
    string UserId { get; }
    DateTime Timestamp { get; }
    string Text { get; }
}
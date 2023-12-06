namespace TwitchEverywhere.Types.Messages.Interfaces;

public interface IClearChat : IMessage {
    long? Duration { get; }
    string RoomId { get; }
    string TargetUserId { get; }
    DateTime Timestamp { get; }
    string Text { get; }
    string TargetUserName { get; }
}
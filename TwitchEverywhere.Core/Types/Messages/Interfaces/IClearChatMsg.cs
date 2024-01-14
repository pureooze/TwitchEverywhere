namespace TwitchEverywhere.Core.Types.Messages.Interfaces;

public interface IClearChatMsg : IMessage {
    long? Duration { get; }
    string RoomId { get; }
    string TargetUserId { get; }
    DateTime Timestamp { get; }
    string Text { get; }
    string TargetUserName { get; }
}
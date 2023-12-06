namespace TwitchEverywhere.Types.Messages.Interfaces;

public interface IClearMsg : IMessage {
    string Login { get; }
    string RoomId { get; }
    string TargetMessageId { get; }
    DateTime Timestamp { get; }
    string Text { get; }
}
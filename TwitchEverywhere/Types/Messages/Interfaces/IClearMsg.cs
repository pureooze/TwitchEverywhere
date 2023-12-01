namespace TwitchEverywhere.Types.Messages.Interfaces;

public interface IClearMsg {
    MessageType MessageType { get; }
    string Login { get; }
    string RoomId { get; }
    string TargetMessageId { get; }
    DateTime Timestamp { get; }
}
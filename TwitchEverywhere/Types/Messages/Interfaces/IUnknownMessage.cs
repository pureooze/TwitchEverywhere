namespace TwitchEverywhere.Types.Messages.Interfaces;

public interface IUnknownMessage {
    MessageType MessageType { get; }
    string Message { get; }
}
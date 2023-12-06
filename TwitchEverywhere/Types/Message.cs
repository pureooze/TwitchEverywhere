namespace TwitchEverywhere.Types;

public interface IMessage {
    public MessageType MessageType { get; }
    
    public string RawMessage { get; }
    
    public string Channel { get; }
}
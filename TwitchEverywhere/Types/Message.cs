namespace TwitchEverywhere.Types;

public abstract class Message {
    public abstract MessageType MessageType { get; }
    
    public abstract string RawMessage { get; }
}
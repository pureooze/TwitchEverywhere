namespace TwitchEverywhere.Types;

public abstract class Message {
    public abstract MessageType MessageType { get; }
}
namespace TwitchEverywhere.Types.Messages.Interfaces; 

public interface IJoinMsg {
    MessageType MessageType { get; }
    string User { get; }
}
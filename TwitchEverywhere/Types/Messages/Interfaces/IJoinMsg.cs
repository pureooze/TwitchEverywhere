namespace TwitchEverywhere.Types.Messages.Interfaces; 

public interface IJoinMsg : IMessage {
    string User { get; }
}
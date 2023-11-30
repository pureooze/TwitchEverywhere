namespace TwitchEverywhere.Types.Messages.Interfaces; 

public interface IPartMsg {
    MessageType MessageType { get; }
    string User { get; }
}
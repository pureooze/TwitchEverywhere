namespace TwitchEverywhere.Types.Messages.Interfaces;

public interface IHostTargetMsg : IMessage {

    string HostingChannel { get; }
    
    int NumberOfViewers { get; }

    bool IsHostingChannel { get; }
}
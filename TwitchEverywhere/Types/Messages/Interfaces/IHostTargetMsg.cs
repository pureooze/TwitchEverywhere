namespace TwitchEverywhere.Types.Messages.Interfaces;

public interface IHostTargetMsg {
    MessageType MessageType { get; }

    string HostingChannel { get; }

    string Channel { get; }

    int NumberOfViewers { get; }

    bool IsHostingChannel { get; }
}
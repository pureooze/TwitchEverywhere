using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedUnknownMessage(
    string channel,
    string message
) : IUnknownMessage {

    public MessageType MessageType => MessageType.Unknown;
    public string RawMessage => message;
    
    public string Channel { get; } = channel;
}
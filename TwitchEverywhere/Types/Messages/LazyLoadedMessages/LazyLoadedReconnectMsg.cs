using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedReconnectMsg : IReconnectMsg {
    
    public LazyLoadedReconnectMsg(
        string channel
    ) {
        Channel = channel;
    }

    public MessageType MessageType => MessageType.Reconnect;
    
    public string RawMessage => "";
    
    public string Channel { get; }
}
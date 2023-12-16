using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedReconnectMsg( string channel ) : IReconnectMsg {

    public MessageType MessageType => MessageType.Reconnect;
    
    public string RawMessage => "";
    
    public string Channel { get; } = channel;
}
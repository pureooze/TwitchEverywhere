using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedReconnectMsg( string channel ) : IReconnectMsg {

    public MessageType MessageType => MessageType.Reconnect;
    
    public string RawMessage => "";
    
    public string Channel { get; } = channel;
}
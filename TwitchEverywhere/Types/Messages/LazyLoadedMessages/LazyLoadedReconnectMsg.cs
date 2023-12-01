using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedReconnectMsg : Message, IReconnectMsg {

    public override MessageType MessageType => MessageType.Reconnect;
}
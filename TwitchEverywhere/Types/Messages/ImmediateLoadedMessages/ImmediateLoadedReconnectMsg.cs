using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedReconnectMsg : Message, IReconnectMsg {

    public override MessageType MessageType => MessageType.Reconnect;
}
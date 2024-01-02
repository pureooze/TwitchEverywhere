using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

public class LazyLoadedCapReq(
    string channel,
    string message
) : ICapReq {

    MessageType IMessage.MessageType => MessageType.CapReq;

    string IMessage.RawMessage => message;

    string IMessage.Channel => channel;
}
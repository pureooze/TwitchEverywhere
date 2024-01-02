using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.ImmediateLoadedMessages;

public class ImmediatelyLoadedCapReqMsg (
    string message,
    string channel
) : ICapReq {
    
    MessageType IMessage.MessageType => MessageType.CapReq;

    string IMessage.RawMessage => message;

    string IMessage.Channel => channel;
}
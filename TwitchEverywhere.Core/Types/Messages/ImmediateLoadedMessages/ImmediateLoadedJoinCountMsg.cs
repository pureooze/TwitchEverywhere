using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedJoinCountMsg(
    string rawMessage,
    string channel
) : IJoinCountMsg {

    MessageType IMessage.MessageType => MessageType.JoinCount;

    string IMessage.RawMessage => rawMessage;

    string IMessage.Channel => channel;
}
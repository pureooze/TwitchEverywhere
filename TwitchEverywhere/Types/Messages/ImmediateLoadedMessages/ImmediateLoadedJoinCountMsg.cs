using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedJoinCountMsg(
    string rawMessage,
    string channel
) : IJoinCountMsg {

    MessageType IMessage.MessageType => MessageType.JoinCount;

    string IMessage.RawMessage => rawMessage;

    string IMessage.Channel => channel;
}
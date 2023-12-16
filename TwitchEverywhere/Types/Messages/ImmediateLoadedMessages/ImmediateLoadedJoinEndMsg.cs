using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedJoinEndMsg(
    string rawMessage,
    string channel
) : IJoinCountMsg {

    MessageType IMessage.MessageType => MessageType.JoinEnd;

    string IMessage.RawMessage => rawMessage;

    string IMessage.Channel => channel;
}
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages;

public class LazyLoadedJoinCountMsg(
    string channel,
    string message
) : IJoinCountMsg {

    MessageType IMessage.MessageType => MessageType.JoinCount;

    string IMessage.RawMessage => message;

    string IMessage.Channel => channel;
}
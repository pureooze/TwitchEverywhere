using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

public class LazyLoadedJoinCountMsg(
    string channel,
    string message
) : IJoinCountMsg {

    MessageType IMessage.MessageType => MessageType.JoinCount;

    string IMessage.RawMessage => message;

    string IMessage.Channel => channel;
}
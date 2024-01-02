using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

public class LazyLoadedJoinEndMsg(
    string channel,
    string message
) : IJoinEndMsg {

    MessageType IMessage.MessageType => MessageType.JoinEnd;

    string IMessage.RawMessage => message;

    string IMessage.Channel => channel;
}
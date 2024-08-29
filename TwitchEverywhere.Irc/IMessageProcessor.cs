using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;


namespace TwitchEverywhere.Irc;

public interface IMessageProcessor {
    void ProcessMessage(
        RawMessage response,
        string channel,
        Action<IMessage> callback
    );
}
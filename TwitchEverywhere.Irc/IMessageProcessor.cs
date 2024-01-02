using TwitchEverywhere.Core.Types;

namespace TwitchEverywhere.Irc; 

public interface IMessageProcessor {
    void ProcessMessage(
        string response,
        string channel,
        Action<IMessage> callback
    );
}
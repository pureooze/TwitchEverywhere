using TwitchEverywhere.Types;

namespace TwitchEverywhere; 

public interface IMessageProcessor {
    void ProcessMessage(
        string response,
        string channel,
        Action<Message> callback
    );
}
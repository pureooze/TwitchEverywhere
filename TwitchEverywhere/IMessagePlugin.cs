using TwitchEverywhere.Types;

namespace TwitchEverywhere; 

public interface IMessagePlugin {
    bool CanHandle(
        string response,
        string channel
    );
    
    IMessage GetMessageData(
        string response,
        string channel
    );
}
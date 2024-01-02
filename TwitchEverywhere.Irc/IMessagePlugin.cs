using TwitchEverywhere.Core.Types;

namespace TwitchEverywhere.Irc; 

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
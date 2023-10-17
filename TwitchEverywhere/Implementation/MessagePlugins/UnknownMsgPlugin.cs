using TwitchEverywhere.Types;

namespace TwitchEverywhere.Implementation.MessagePlugins; 

public class UnknownMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return true;
    }
    
    Message IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        return new UnknownMessage( response );
    }
}
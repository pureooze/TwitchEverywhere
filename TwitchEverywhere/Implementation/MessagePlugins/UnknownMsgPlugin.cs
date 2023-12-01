using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

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
        return new LazyLoadedUnknownMessage( response );
    }
}
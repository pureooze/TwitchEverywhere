using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class GlobalUserStatePlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        throw new NotImplementedException();
    }

    public IMessage GetMessageData(
        string response,
        string channel
    ) {
        return new LazyLoadedGlobalUserState( 
            channel: channel,
            message: response 
        );
    }
    IMessage IMessagePlugin.GetMessageData(
        RawMessage response
    ) {
        throw new NotImplementedException();
    }
}
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class PartMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        throw new NotImplementedException();
    }
    public IMessage GetMessageData(
        string response,
        string channel
    ) {
        return new LazyLoadedPartMsg( 
            message: response, 
            channel: channel 
        );
    }
    IMessage IMessagePlugin.GetMessageData(
        RawMessage response
    ) {
        throw new NotImplementedException();
    }
}
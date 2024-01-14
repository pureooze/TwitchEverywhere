using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class UnknownMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        ReadOnlyMemory<byte> response,
        MessageType messageType
    ) {
        throw new NotImplementedException();
    }

    public IMessage GetMessageData(
        string response,
        string channel
    ) {
        return new LazyLoadedUnknownMessage( 
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
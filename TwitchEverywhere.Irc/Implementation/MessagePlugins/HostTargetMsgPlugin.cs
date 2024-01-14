using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class HostTargetMsgPlugin : IMessagePlugin {

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
        return new LazyLoadedHostTargetMsg( 
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
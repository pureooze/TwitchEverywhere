using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class CapReqPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.CapReq;
    }

    IMessage IMessagePlugin.GetMessageData(
        RawMessage response
    ) {
        return new CapReq( response );
    }
}
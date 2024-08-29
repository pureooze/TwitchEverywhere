using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Core.Types.Messages.Interfaces;


namespace TwitchEverywhere.Irc.MessagePlugins; 

public class HostTargetMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.HostTarget;
    }
    
    IMessage IMessagePlugin.GetMessageData(
        RawMessage response
    ) {
        return new HostTargetMsg( response );
    }
}
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;


namespace TwitchEverywhere.Irc.MessagePlugins; 

public class PartMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.Part;
    }

    IMessage IMessagePlugin.GetMessageData(
        RawMessage response
    ) {
        return new LazyLoadedPartMsg( 
            response
        );
    }
}
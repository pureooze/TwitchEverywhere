using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class PrivMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        ReadOnlyMemory<byte> response,
        MessageType messageType
    ) {
        return messageType == MessageType.PrivMsg;
    }
    
    IMessage IMessagePlugin.GetMessageData(
        RawMessage response
    ) {
        return new PrivMsg( response );
    }
}
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class JoinCountMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.JoinCount;
    }

 
    IMessage IMessagePlugin.GetMessageData(
        RawMessage response
    ) {
        throw new NotImplementedException();
    }
}
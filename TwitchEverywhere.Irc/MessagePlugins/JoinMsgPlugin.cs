using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Core.Types.Messages.Interfaces;


namespace TwitchEverywhere.Irc.MessagePlugins; 

public class JoinMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.Join;
    }

    IMessage IMessagePlugin.GetMessageData(
        RawMessage response
    ) {
        return new JoinMsg( response );
    }
}
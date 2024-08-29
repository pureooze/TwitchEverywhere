using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Core.Types.Messages.Interfaces;


namespace TwitchEverywhere.Irc.MessagePlugins; 

public class UserStateMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.UserState;
    }

    IMessage IMessagePlugin.GetMessageData(
        RawMessage response
    ) {
        return new UserStateMsg( response );
    }
}
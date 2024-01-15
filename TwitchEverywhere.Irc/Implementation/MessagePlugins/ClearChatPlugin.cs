using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class ClearChatPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.ClearChat;
    }

    IMessage IMessagePlugin.GetMessageData(
        RawMessage response
    ) {
        return new ClearChatMsg( response );
    }
}
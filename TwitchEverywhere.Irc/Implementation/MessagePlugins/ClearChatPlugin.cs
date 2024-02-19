using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class ClearChatPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.ClearChat;
    }

    IMessage IMessagePlugin.GetMessageData(
        IrcClientObservable observer,
        RawMessage response
    ) {
        return new ClearChatMsg( response );
    }
}
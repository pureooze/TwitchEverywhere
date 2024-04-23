using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class JoinMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.Join;
    }

    void IMessagePlugin.ProcessMessage(IObserver<IMessage> observer,
        RawMessage response) {
        observer.OnNext(new JoinMsg( response ));
    }
}
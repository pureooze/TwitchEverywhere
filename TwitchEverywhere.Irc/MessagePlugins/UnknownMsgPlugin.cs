using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Irc.Rx;

namespace TwitchEverywhere.Irc.MessagePlugins; 

public class UnknownMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.Unknown;
    }

    void IMessagePlugin.ProcessMessage(
        IrcClientObserver observer,
        RawMessage response
    ) {
        if (observer.UnknownObservables == null) {
            return;
        }
        
        foreach (IObserver<IUnknownMsg> observable in observer.UnknownObservables) {
            observable.OnNext(new UnknownMsg(response));
        }
    }
}
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Irc.Rx;

namespace TwitchEverywhere.Irc.MessagePlugins; 

public class JoinEndMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.JoinEnd;
    }

    void IMessagePlugin.ProcessMessage(
        IrcClientObserver observer,
        RawMessage response
    ) {
        if (observer.JoinEndObservables == null) {
            return;
        }
        
        foreach (IObserver<IJoinEndMsg> observable in observer.JoinEndObservables) {
            observable.OnNext(new JoinEndMsg(response));
        }
    }
}
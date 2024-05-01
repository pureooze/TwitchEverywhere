using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Irc.Rx;

namespace TwitchEverywhere.Irc.MessagePlugins; 

public class GlobalUserStatePlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.GlobalUserState;
    }
    
    void IMessagePlugin.ProcessMessage(
        IrcClientObserver observer,
        RawMessage response
    ) {
        if (observer.GlobalUserStateObservables == null) {
            return;
        }
        
        foreach (IObserver<IGlobalUserStateMsg> observable in observer.GlobalUserStateObservables) {
            observable.OnNext(new GlobalUserStateMsg(response));
        }
    }
}
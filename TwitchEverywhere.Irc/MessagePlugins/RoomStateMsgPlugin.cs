using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Irc.Rx;

namespace TwitchEverywhere.Irc.MessagePlugins; 

public class RoomStateMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.RoomState;
    }

    void IMessagePlugin.ProcessMessage(
        IrcClientObserver observer,
        RawMessage response
    ) {
        if (observer.RoomStateObservables == null) {
            return;
        }
        
        foreach (IObserver<IRoomStateMsg> observable in observer.RoomStateObservables) {
            observable.OnNext(new RoomStateMsg(response));
        }
    }
}
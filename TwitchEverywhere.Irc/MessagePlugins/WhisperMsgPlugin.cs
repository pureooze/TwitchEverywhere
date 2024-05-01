using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Irc.Rx;

namespace TwitchEverywhere.Irc.MessagePlugins; 

public class WhisperMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.Whisper;
    }
 
    void IMessagePlugin.ProcessMessage(
        IrcClientObserver observer,
        RawMessage response
    ) {
        if (observer.WhisperObservables == null) {
            return;
        }
        
        foreach (IObserver<IWhisperMsg> observable in observer.WhisperObservables) {
            observable.OnNext(new WhisperMsg(response));
        }
    }
}
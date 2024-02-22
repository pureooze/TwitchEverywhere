using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class CapReqPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.CapReq;
    }

    void IMessagePlugin.ProcessMessage(
        IrcClientSubject observer,
        RawMessage response
    ) {
        observer.CapReqSubject.OnNext(new CapReq( response ));
    }
}
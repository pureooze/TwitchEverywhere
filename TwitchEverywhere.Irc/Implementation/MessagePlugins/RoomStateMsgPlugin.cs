using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class RoomStateMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.RoomState;
    }

    void IMessagePlugin.ProcessMessage(IObserver<IMessage> observer,
        RawMessage response) {
        observer.OnNext( new RoomStateMsg( response ) );
    }
}
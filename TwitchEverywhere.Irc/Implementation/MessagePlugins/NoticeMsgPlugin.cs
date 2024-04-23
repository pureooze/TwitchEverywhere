using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class NoticeMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.Notice;
    }

    void IMessagePlugin.ProcessMessage(IObserver<IMessage> observer,
        RawMessage response) {
        observer.OnNext( new NoticeMsg( response ) );
    }
}
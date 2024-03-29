using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class UnknownMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.Unknown;
    }

    void IMessagePlugin.ProcessMessage(
        IrcClientSubject subjects,
        RawMessage response
    ) {
        subjects.UnknownSubject.OnNext( new UnknownMsg( response ) );
    }
}
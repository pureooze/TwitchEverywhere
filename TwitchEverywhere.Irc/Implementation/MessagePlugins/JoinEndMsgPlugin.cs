using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class JoinEndMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.JoinEnd;
    }

    void IMessagePlugin.ProcessMessage(
        IrcClientSubject subjects,
        RawMessage response
    ) {
        subjects.JoinEndSubject.OnNext( new JoinEndMsg( response ) );
    }
}
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class PrivMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.PrivMsg;
    }
    
    void IMessagePlugin.ProcessMessage(
        IrcClientSubject subjects,
        RawMessage response
    ) {
        subjects.PrivMsgSubject.OnNext( new PrivMsg( response ) );
    }
}
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc; 

public interface IMessagePlugin {
    
    bool CanHandle(
        MessageType messageType
    );

    IMessage GetMessageData(
        IrcClientObservable observer,
        RawMessage response
    );
}
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc; 

public interface IMessagePlugin {
    
    bool CanHandle(
        MessageType messageType
    );

    void ProcessMessage(
        IrcClientSubject subjects,
        RawMessage response
    );
}
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc; 

public interface IMessageProcessor {
    
    void ProcessMessageRx(
        RawMessage response,
        string channel,
        IrcClientSubject subjects
    );
}
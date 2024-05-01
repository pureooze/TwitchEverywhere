using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Irc.Rx;

namespace TwitchEverywhere.Irc; 

public interface IMessageProcessor {
    
    void ProcessMessageRx(
        RawMessage response,
        string channel,
        IrcClientObserver observer
    );
}
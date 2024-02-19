using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc; 

public interface IMessageProcessor {

    void ProcessMessage(
        RawMessage response,
        string channel,
        Action<IMessage> callback
    );
    
    void ProcessMessageRx(
        RawMessage response,
        string channel,
        IrcClientObservable observer
    );
}
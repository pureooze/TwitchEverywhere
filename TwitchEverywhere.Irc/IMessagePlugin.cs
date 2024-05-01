using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Irc.Rx;

namespace TwitchEverywhere.Irc;

public interface IMessagePlugin {
    bool CanHandle(
        MessageType messageType
    );

    void ProcessMessage(
        IrcClientObserver observer,
        RawMessage response
    );
}
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc; 

public interface ITwitchConnector {
    
    internal IrcClientObservable TryConnectRx(
        string channel
    );

    internal Task<bool> SendMessage(
        IMessage message,
        MessageType messageType
    );

    internal Task<bool> Disconnect();

}
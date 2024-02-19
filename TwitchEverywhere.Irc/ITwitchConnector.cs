using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc; 

public interface ITwitchConnector {
    internal Task<bool> TryConnect(
        TwitchConnectionOptions options,
        Action<IMessage> messageCallback
    );
    
    internal Task<IrcClientObservable> TryConnectRx(
        TwitchConnectionOptions options
    );

    internal Task<bool> SendMessage(
        IMessage message,
        MessageType messageType
    );

    internal Task<bool> Disconnect();

}
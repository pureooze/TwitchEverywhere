using System.Net.WebSockets;
using TwitchEverywhere.Types;

namespace TwitchEverywhere; 

public interface ITwitchConnector {
    internal Task<bool> TryConnect(
        TwitchConnectionOptions options,
        Action<PrivMsg> privCallback,
        Action<ClearChat> clearChatCallback,
        Action<ClearMsg> clearMsgCallback
    );
}
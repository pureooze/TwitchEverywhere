using System.Net.WebSockets;
using TwitchEverywhere.Types;

namespace TwitchEverywhere; 

public interface ITwitchConnector {
    internal Task<bool> TryConnect(
        TwitchConnectionOptions options,
        Action<PrivMessage> privCallback,
        Action<ClearMessage> clearCallback
    );
}
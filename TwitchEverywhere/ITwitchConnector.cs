using System.Net.WebSockets;

namespace TwitchEverywhere; 

public interface ITwitchConnector {
    internal Task<bool> TryConnect(
        TwitchConnectionOptions options,
        Action<string> messageCallback
    );
}
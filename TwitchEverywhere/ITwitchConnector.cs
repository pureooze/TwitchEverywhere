using System.Net.WebSockets;

namespace TwitchEverywhere; 

public interface ITwitchConnector {
    internal Task<bool> Connect(
        TwitchConnectionOptions options
    );
}
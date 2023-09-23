using System.Net.WebSockets;
using TwitchEverywhere.Implementation;

namespace TwitchEverywhere;

public sealed class TwitchEverywhere {
    private string[] m_channels;
    private ITwitchConnector m_twitchConnector;
    
    public TwitchEverywhere(
        string[] channels
    ) {
        m_channels = channels;
        m_twitchConnector = new TwitchConnector();
    }

    public async Task<bool> TryConnectToChannel(
        TwitchConnectionOptions options
    ) {
        // try connecting
        bool connectionSuccess = await m_twitchConnector.Connect(
            options
        );
        if ( !connectionSuccess ) {
            return false;
        }
        
        // if we cant connect, then it was a fail
        return false;
    }
}
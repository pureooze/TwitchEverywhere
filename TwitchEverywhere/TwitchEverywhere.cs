using System.Net.WebSockets;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;

namespace TwitchEverywhere;

public sealed class TwitchEverywhere {
    private readonly ITwitchConnector m_twitchConnector;
    private readonly TwitchConnectionOptions m_options;

    public TwitchEverywhere(
        TwitchConnectionOptions options
    ) {
        m_options = options;

        IAuthorizer authorizer = new Authorizer(
            accessToken: m_options.AccessToken ?? "",
            refreshToken: m_options.RefreshToken ?? "",
            clientId: m_options.ClientId ?? "",
            clientSecret: m_options.ClientSecret ?? ""
        );
        
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer,
            webSocketConnection: new WebSocketConnection()
        );
    }

    public async Task ConnectToChannel(
        Action<Message> messageCallback
    ) {
        await m_twitchConnector.TryConnect(
            options: m_options,
            messageCallback: messageCallback
        );
    }
}
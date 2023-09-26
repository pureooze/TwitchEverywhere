using System.Net.WebSockets;
using TwitchEverywhere.Implementation;

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
            authorizer: authorizer 
        );
    }

    public async Task ConnectToChannel(
        Action<string> messageCallback
    ) {
        // try connecting
        await m_twitchConnector.Connect(
            m_options,
            messageCallback
        );
    }
}
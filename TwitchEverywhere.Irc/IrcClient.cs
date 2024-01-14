using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Irc.Implementation;

namespace TwitchEverywhere.Irc;

public sealed class IrcClient {
    private readonly ITwitchConnector m_twitchConnector;
    private readonly TwitchConnectionOptions m_options;

    public IrcClient(
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
            webSocketConnection: new WebSocketConnection(),
            messageProcessor: new MessageProcessor()
        );
    }

    public async Task ConnectToChannel(
        Action<IMessage> messageCallback
    ) {
        await m_twitchConnector.TryConnect(
            options: m_options,
            messageCallback: messageCallback
        );
    }

    public async Task<bool> SendMessage(
        IMessage message,
        MessageType messageType
    ) {
        return await m_twitchConnector.SendMessage( message, messageType );
    }

    public Task<bool> Disconnect() {
        return m_twitchConnector.Disconnect();
    }
}
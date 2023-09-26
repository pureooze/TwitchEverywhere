using TwitchEverywhere.Implementation;

namespace TwitchEverywhere.UnitTests.TwitchConnector;

[TestFixture]
public class TwitchConnectorTests {
    private ITwitchConnector m_twitchConnector;
    private IWebSocketConnection m_webSocketConnection;
    
    private readonly TwitchConnectionOptions m_options = new(
        Channel: "TestConnection",
        AccessToken: "accessToken",
        RefreshToken: "refreshToken",
        ClientId: "clientId",
        ClientSecret: "clientSecret",
        BufferSize: 20
    );
    
    private readonly Action<string> m_callback = delegate(
        string s
    ) {
        Assert.That( s, Is.Not.Null );
    };
    
    [SetUp]
    public void Setup() {
        IAuthorizer authorizer = new Authorizer(
            accessToken: m_options.AccessToken ?? "",
            refreshToken: m_options.RefreshToken ?? "",
            clientId: m_options.ClientId ?? "",
            clientSecret: m_options.ClientSecret ?? ""
        );

        m_twitchConnector = new Implementation.TwitchConnector( 
            authorizer: authorizer,
            webSocketConnection: m_webSocketConnection
        );
    }

    [Test]
    public async Task ValidOptions_Connect_ReturnsTrue() {
        bool result = await m_twitchConnector.TryConnect( m_options, m_callback );
        Assert.That( result, Is.True );
    }
}
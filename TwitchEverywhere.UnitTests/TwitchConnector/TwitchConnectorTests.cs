using TwitchEverywhere.Implementation;

namespace TwitchEverywhere.UnitTests.TwitchConnector;

[TestFixture]
public class TwitchConnectorTests {
    private ITwitchConnector m_twitchConnector;
    private readonly TwitchConnectionOptions m_options = new(
        Channel: "TestConnection",
        AccessToken: "accessToken",
        RefreshToken: "refreshToken",
        ClientId: "clientId",
        ClientSecret: "clientSecret",
        BufferSize: 20
    );

    [SetUp]
    public void Setup() {
        IAuthorizer authorizer = new Authorizer(
            accessToken: m_options.AccessToken ?? "",
            refreshToken: m_options.RefreshToken ?? "",
            clientId: m_options.ClientId ?? "",
            clientSecret: m_options.ClientSecret ?? ""
        );
        
        ICompressor compressor = new BrotliCompressor();
        
        m_twitchConnector = new Implementation.TwitchConnector( 
            authorizer: authorizer, 
            compressor: compressor, 
            bufferSize: 20
        );
    }

    [Test]
    public void NoOptions_TryConnectToChannel_ReturnsFail() {
        m_twitchConnector.Connect(m_options);
    }
}
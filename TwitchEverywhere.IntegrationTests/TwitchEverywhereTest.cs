namespace TwitchEverywhere.IntegrationTests;

[TestFixture]
public class TwitchEverywhereTest {
    private TwitchEverywhere m_twitchEverywhereTest;
    private readonly Action<string> m_callback = delegate(
        string s
    ) {
        Assert.That( s, Is.Not.Null );
    };

    [SetUp]
    public void Setup() {
        TwitchConnectionOptions options = new(
            Channel: "TestConnection",
            AccessToken: "accessToken",
            RefreshToken: "refreshToken",
            ClientId: "clientId",
            ClientSecret: "clientSecret",
            BufferSize: 20
        );
        
        m_twitchEverywhereTest = new TwitchEverywhere(
            options: options
        );
    }

    [Test]
    public async Task NoOptions_TryConnectToChannel_ReturnsFail() {
        await m_twitchEverywhereTest.ConnectToChannel( m_callback );
    }
}
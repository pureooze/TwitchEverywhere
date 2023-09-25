namespace TwitchEverywhere.IntegrationTests;

[TestFixture]
public class TwitchEverywhereTest {
    private TwitchEverywhere m_twitchEverywhereTest;

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
        await m_twitchEverywhereTest.TryConnectToChannel();
    }
}
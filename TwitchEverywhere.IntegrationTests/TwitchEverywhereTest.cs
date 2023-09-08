namespace TwitchEverywhere.IntegrationTests;

[TestFixture]
public class TwitchEverywhereTest {
    private TwitchEverywhere m_twitchEverywhereTest;

    [SetUp]
    public void Setup() {
        m_twitchEverywhereTest = new TwitchEverywhere(
            channels: new[] { "test" }
        );
    }

    [Test]
    public async Task NoOptions_TryConnectToChannel_ReturnsFail() {
        TwitchConnectionOptions options = new(
            Channel: "TestConnection",
            Tags: new[] { "tag1" },
            Message: "Hello!"
        );

        await m_twitchEverywhereTest.TryConnectToChannel(
            options: options
        );
    }
}
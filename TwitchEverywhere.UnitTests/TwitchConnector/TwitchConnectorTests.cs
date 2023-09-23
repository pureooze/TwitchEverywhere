namespace TwitchEverywhere.UnitTests.TwitchConnector;

[TestFixture]
public class TwitchConnectorTests {
    private ITwitchConnector m_twitchConnector;

    [SetUp]
    public void Setup() {
        m_twitchConnector = new Implementation.TwitchConnector();
    }

    [Test]
    public void NoOptions_TryConnectToChannel_ReturnsFail() {
        TwitchConnectionOptions options = new(
            Channel: "TestConnection",
            Tags: new[] { "tag1" },
            Message: "Hello!"
        );
        
        m_twitchConnector.Connect(options);
    }
}
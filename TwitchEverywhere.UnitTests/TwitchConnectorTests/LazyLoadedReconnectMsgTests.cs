using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.TwitchConnectorTests; 

[TestFixture]
public class LazyLoadedReconnectMsgTests {
    private readonly TwitchConnectionOptions m_options = new(
        "channel",
        "access_token",
        "refresh_token",
        "client_id",
        "client_secret",
        "client_name"
    );

    private readonly DateTime m_startTime = DateTimeOffset.FromUnixTimeMilliseconds(1507246572675).UtcDateTime;
        
    private ITwitchConnector m_twitchConnector;

    [Test]
    [TestCaseSource(sourceName: nameof(ReconnectMsgMessages))]
    public async Task ReconnectMsg( IImmutableList<string> messages, LazyLoadedReconnectMsg expectedMessage ) {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages: messages );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );

        void MessageCallback(
            IMessage message
        ) {
            Assert.That( message, Is.Not.Null );
            Assert.That( message.MessageType, Is.EqualTo( expectedMessage.MessageType ), "Incorrect message type set" );
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
        );
        
        bool result = await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
        Assert.That( actual: result, expression: Is.True );
    }
    
    private static IEnumerable<TestCaseData> ReconnectMsgMessages() {
        yield return new TestCaseData(
            new List<string> {
                $":tmi.twitch.tv RECONNECT"
            }.ToImmutableList(),
            new LazyLoadedReconnectMsg( channel: "channel" )
        ).SetName("Reconnect message");
    }
}
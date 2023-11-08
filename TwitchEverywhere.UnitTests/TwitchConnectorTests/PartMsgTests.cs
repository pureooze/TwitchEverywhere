using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;

namespace TwitchEverywhere.UnitTests.TwitchConnectorTests; 

public class PartMsgTests {
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
    [TestCaseSource(nameof(PartMsgMessages))]
    public async Task PartMsg( IImmutableList<string> messages, PartMsg expectedMessage ) {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( MockBehavior.Strict );
        dateTimeService.Setup( dts => dts.GetStartTime() ).Returns( m_startTime );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );

        void MessageCallback(
            Message message
        ) {
            Assert.That( message, Is.Not.Null );
            Assert.That( message.MessageType, Is.EqualTo( expectedMessage.MessageType ), "Incorrect message type set" );

            PartMsg msg = (PartMsg)message;
            PartMsgCallback( msg, expectedMessage );
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
        );
        
        bool result = await m_twitchConnector.TryConnect( m_options, MessageCallback );
        Assert.That( result, Is.True );
    }
    
    private void PartMsgCallback(
        PartMsg partMsg,
        PartMsg? expectedPartMsg
    ) {
        Assert.Multiple(() => {
            Assert.That(partMsg.User, Is.EqualTo(expectedPartMsg?.User), "User was not equal to expected value");
            Assert.That(partMsg.Channel, Is.EqualTo(expectedPartMsg?.Channel), "Channel was not equal to expected value");
        });
    }
    
    private static IEnumerable<TestCaseData> PartMsgMessages() {
        yield return new TestCaseData(
            new List<string> {
                $":ronni!ronni@ronni.tmi.twitch.tv PART #channel"
            }.ToImmutableList(),
            new PartMsg(
                message: $":ronni!ronni@ronni.tmi.twitch.tv PART #channel",
                channel: "channel"
            )
        ).SetName("Ronni left the channel");
    }
}
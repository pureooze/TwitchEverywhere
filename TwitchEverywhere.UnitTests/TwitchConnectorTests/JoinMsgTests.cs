using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.TwitchConnectorTests; 

public class JoinMsgTests {
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
    [TestCaseSource(nameof(JoinMsgMessages))]
    public async Task JoinMsg( IImmutableList<string> messages, LazyLoadedJoinMsg expectedMessage ) {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( MockBehavior.Strict );
        dateTimeService.Setup( dts => dts.GetStartTime() ).Returns( m_startTime );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );

        void MessageCallback(
            IMessage message
        ) {
            Assert.That( message, Is.Not.Null );
            Assert.That( message.MessageType, Is.EqualTo( expectedMessage.MessageType ), "Incorrect message type set" );

            LazyLoadedJoinMsg msg = (LazyLoadedJoinMsg)message;
            JoinMsgCallback( msg, expectedMessage );
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
    
    private void JoinMsgCallback(
        LazyLoadedJoinMsg globalUserState,
        LazyLoadedJoinMsg? expectedGlobalUserState
    ) {
        Assert.Multiple(() => {
            Assert.That(globalUserState.User, Is.EqualTo(expectedGlobalUserState?.User), "User was not equal to expected value");
            Assert.That(globalUserState.Channel, Is.EqualTo(expectedGlobalUserState?.Channel), "Channel was not equal to expected value");
        });
    }
    
    private static IEnumerable<TestCaseData> JoinMsgMessages() {
        yield return new TestCaseData(
            new List<string> {
                $":ronni!ronni@ronni.tmi.twitch.tv JOIN #channel"
            }.ToImmutableList(),
            new LazyLoadedJoinMsg(
                message: $":ronni!ronni@ronni.tmi.twitch.tv JOIN #channel",
                channel: "channel"
            )
        ).SetName("Ronni joined the channel");
    }
}
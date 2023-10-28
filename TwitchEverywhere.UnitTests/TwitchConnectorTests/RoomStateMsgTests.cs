using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;

namespace TwitchEverywhere.UnitTests.TwitchConnectorTests; 

[TestFixture]
public class RoomStateMsgTests {
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
    [TestCaseSource(nameof(RoomStateMsgMessages))]
    public async Task RoomStateMsg( IImmutableList<string> messages, RoomStateMsg expectedMessage ) {
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
            
            RoomStateMsg msg = (RoomStateMsg)message;
            RoomStateMsgMessageCallback( msg, expectedMessage );
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
    
    private void RoomStateMsgMessageCallback(
        RoomStateMsg roomStateMsg,
        RoomStateMsg? expectedRoomStateMsg
    ) {
        Assert.Multiple(() => {
            Assert.That(roomStateMsg.EmoteOnly, Is.EqualTo(expectedRoomStateMsg?.EmoteOnly), "EmoteOnly was not equal to expected value");
            Assert.That(roomStateMsg.FollowersOnly, Is.EqualTo(expectedRoomStateMsg?.FollowersOnly), "FollowersOnly was not equal to expected value");
            Assert.That(roomStateMsg.R9K, Is.EqualTo(expectedRoomStateMsg?.R9K), "R9K was not equal to expected value");
            Assert.That(roomStateMsg.Slow, Is.EqualTo(expectedRoomStateMsg?.Slow), "Slow was not equal to expected value");
            Assert.That(roomStateMsg.RoomId, Is.EqualTo(expectedRoomStateMsg?.RoomId), "RoomId was not equal to expected value");
            Assert.That(roomStateMsg.SubsOnly, Is.EqualTo(expectedRoomStateMsg?.SubsOnly), "SubsOnly was not equal to expected value");
            Assert.That(roomStateMsg.MessageType, Is.EqualTo(expectedRoomStateMsg?.MessageType), "MessageType was not equal to expected value");
        });
    }

    private static IEnumerable<TestCaseData> RoomStateMsgMessages() {
        yield return new TestCaseData(
            new List<string> {
                $"@emote-only=0;followers-only=0;r9k=0;slow=0;subs-only=0 :tmi.twitch.tv ROOMSTATE #channel"
            }.ToImmutableList(),
            new RoomStateMsg(
                message: $"@emote-only=0;followers-only=0;r9k=0;slow=0;subs-only=0 :tmi.twitch.tv ROOMSTATE #channel"
            )
        ).SetName("User joined channel with settings");
    }
}
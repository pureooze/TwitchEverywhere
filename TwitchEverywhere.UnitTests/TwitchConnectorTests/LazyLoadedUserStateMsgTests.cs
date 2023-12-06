using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.TwitchConnectorTests; 

[TestFixture]
public class LazyLoadedUserStateMsgTests {
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
    [TestCaseSource(nameof(UserStateMsgMessages))]
    public async Task UserStateMsg( IImmutableList<string> messages, LazyLoadedUserStateMsg expectedMessage ) {
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

            LazyLoadedUserStateMsg msg = (LazyLoadedUserStateMsg)message;
            UserStateMsgMessageCallback( msg, expectedMessage );
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
    
    private void UserStateMsgMessageCallback(
        LazyLoadedUserStateMsg message,
        LazyLoadedUserStateMsg? expectedMessage
    ) {
        Assert.Multiple(() => {
            Assert.That(message.BadgeInfo, Is.EqualTo(expectedMessage?.BadgeInfo), "BadgeInfo was not equal to expected value");
            Assert.That(message.Badges, Is.EqualTo(expectedMessage?.Badges), "Badges was not equal to expected value");
            Assert.That(message.Color, Is.EqualTo(expectedMessage?.Color), "Color was not equal to expected value");
            Assert.That(message.DisplayName, Is.EqualTo(expectedMessage?.DisplayName), "DisplayName was not equal to expected value");
            Assert.That(message.EmoteSets, Is.EqualTo(expectedMessage?.EmoteSets), "EmoteSets was not equal to expected value");
            Assert.That(message.Id, Is.EqualTo(expectedMessage?.Id), "MessageType was not equal to expected value");
            Assert.That(message.Mod, Is.EqualTo(expectedMessage?.Mod), "Mod was not equal to expected value");
            Assert.That(message.Subscriber, Is.EqualTo(expectedMessage?.Subscriber), "Subscriber was not equal to expected value");
            Assert.That(message.Turbo, Is.EqualTo(expectedMessage?.Turbo), "Turbo was not equal to expected value");
            Assert.That(message.UserType, Is.EqualTo(expectedMessage?.UserType), "UserType was not equal to expected value");
        });
    }

    private static IEnumerable<TestCaseData> UserStateMsgMessages() {
        yield return new TestCaseData(
            new List<string> {
                $"@badge-info=;badges=staff/1;color=#0D4200;display-name=ronni;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;mod=1;subscriber=1;turbo=1;user-type=staff :tmi.twitch.tv USERSTATE #channel"
            }.ToImmutableList(),
            new LazyLoadedUserStateMsg(
                channel: "channel", 
                message: $"@badge-info=;badges=staff/1;color=#0D4200;display-name=ronni;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;mod=1;subscriber=1;turbo=1;user-type=staff :tmi.twitch.tv USERSTATE #channel"
            )
        ).SetName("Joining a channel");
    }
}
using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.UnitTests.TwitchConnectorTests; 

[TestFixture]
public class GlobalUserStateTests {
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
    [TestCaseSource(nameof(GlobalUserStateMessages))]
    public async Task ClearChat( IImmutableList<string> messages, GlobalUserState expectedMessage ) {
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

            GlobalUserState msg = (GlobalUserState)message;
            GlobalUserStateMessageCallback( msg, expectedMessage );
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
    
    private void GlobalUserStateMessageCallback(
        GlobalUserState globalUserState,
        GlobalUserState? expectedGlobalUserState
    ) {
        Assert.Multiple(() => {
            Assert.That(globalUserState.UserId, Is.EqualTo(expectedGlobalUserState?.UserId), "UserId was not equal to expected value");
            Assert.That(globalUserState.Badges, Is.EqualTo(expectedGlobalUserState?.Badges), "Badges was not equal to expected value");
            Assert.That(globalUserState.BadgeInfo, Is.EqualTo(expectedGlobalUserState?.BadgeInfo), "BadgeInfo was not equal to expected value");
            Assert.That(globalUserState.Color, Is.EqualTo(expectedGlobalUserState?.Color), "Color was not equal to expected value");
            Assert.That(globalUserState.DisplayName, Is.EqualTo(expectedGlobalUserState?.DisplayName), "DisplayName was not equal to expected value");
            Assert.That(globalUserState.EmoteSets, Is.EqualTo(expectedGlobalUserState?.EmoteSets), "EmoteSets was not equal to expected value");
            Assert.That(globalUserState.Turbo, Is.EqualTo(expectedGlobalUserState?.Turbo), "Turbo was not equal to expected value");
            Assert.That(globalUserState.UserId, Is.EqualTo(expectedGlobalUserState?.UserId), "UserId was not equal to expected value");
            Assert.That(globalUserState.UserType, Is.EqualTo(expectedGlobalUserState?.UserType), "UserType was not equal to expected value");
            Assert.That(globalUserState.MessageType, Is.EqualTo(expectedGlobalUserState?.MessageType), "MessageType was not equal to expected value");
        });
    }
    
    private static IEnumerable<TestCaseData> GlobalUserStateMessages() {
        yield return new TestCaseData(
            new List<string> {
                $"@badge-info=subscriber/8;badges=subscriber/6;color=#0D4200;display-name=dallas;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;turbo=0;user-id=12345678;user-type=admin :tmi.twitch.tv GLOBALUSERSTATE"
            }.ToImmutableList(),
            new GlobalUserState(
                message: $"@badge-info=subscriber/8;badges=subscriber/6;color=#0D4200;display-name=dallas;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;turbo=0;user-id=12345678;user-type=admin :tmi.twitch.tv GLOBALUSERSTATE"
            )
        ).SetName("After user authenticated ");
    }
}
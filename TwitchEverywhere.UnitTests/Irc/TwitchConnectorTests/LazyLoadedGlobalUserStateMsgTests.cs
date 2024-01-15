using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;
using TwitchEverywhere.Irc;
using TwitchEverywhere.Irc.Implementation;

namespace TwitchEverywhere.UnitTests.Irc.TwitchConnectorTests;

[TestFixture]
public class LazyLoadedGlobalUserStateMsgTests {
    private readonly TwitchConnectionOptions m_options = new(
        "channel",
        "access_token",
        "refresh_token",
        "client_id",
        "client_secret",
        "client_name"
    );

    private bool m_messageCallbackCalled;
    private ITwitchConnector m_twitchConnector;

    [SetUp]
    public void Setup() {
        m_messageCallbackCalled = false;
    }
    
    [Test]
    public async Task AfterUserAuthenticated()
    {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            $"@badge-info=subscriber/8;badges=subscriber/6;color=#0D4200;display-name=dallas;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;turbo=0;user-id=12345678;user-type=admin :tmi.twitch.tv GLOBALUSERSTATE"
        }.ToImmutableList();
        
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            IGlobalUserStateMsg lazyLoadedGlobalUserStateMsg = (IGlobalUserStateMsg)message;

            IImmutableList<Badge> expectedBadgeInfo = new List<Badge> {
                new( "subscriber", "8" )
            }.ToImmutableList();
            IImmutableList<Badge> expectedBadges = new List<Badge> {
                new( "subscriber", "6" )
            }.ToImmutableList();
            
            IImmutableList<string> expectedEmoteSets = new List<string> {
                "0", "33", "50", "237", "793", "2126", "3517", "4578", "5569", "9400", "10337", "12239"
            }.ToImmutableList();
            
            Assert.Multiple(
                () => {
                    Assert.That( lazyLoadedGlobalUserStateMsg, Is.Not.Null );
                    Assert.That( lazyLoadedGlobalUserStateMsg.MessageType, Is.EqualTo( MessageType.GlobalUserState ), "Incorrect message type set" );
                    Assert.That( lazyLoadedGlobalUserStateMsg.UserId, Is.EqualTo( "12345678" ), "UserId was not equal to expected value" );
                    Assert.That( lazyLoadedGlobalUserStateMsg.Badges, Is.EqualTo( expectedBadges ), "Badges was not equal to expected value" );
                    Assert.That( lazyLoadedGlobalUserStateMsg.BadgeInfo, Is.EqualTo( expectedBadgeInfo ), "BadgeInfo was not equal to expected value" );
                    Assert.That( lazyLoadedGlobalUserStateMsg.Color, Is.EqualTo( "#0D4200" ), "Color was not equal to expected value" );
                    Assert.That( lazyLoadedGlobalUserStateMsg.DisplayName, Is.EqualTo( "dallas" ), "DisplayName was not equal to expected value" );
                    Assert.That( lazyLoadedGlobalUserStateMsg.EmoteSets, Is.EqualTo( expectedEmoteSets ), "EmoteSets was not equal to expected value" );
                    Assert.That( lazyLoadedGlobalUserStateMsg.Turbo, Is.EqualTo( true ), "Turbo was not equal to expected value" );
                    Assert.That( lazyLoadedGlobalUserStateMsg.UserId, Is.EqualTo( "12345678" ), "UserId was not equal to expected value" );
                    Assert.That( lazyLoadedGlobalUserStateMsg.UserType, Is.EqualTo( UserType.Admin ), "UserType was not equal to expected value" );
                }
            );
        }

        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector(
            authorizer: authorizer.Object,
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
        );

        bool result = await m_twitchConnector.TryConnect( m_options, MessageCallback );
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(m_messageCallbackCalled, Is.True);
        });
    }
}
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
public class LazyLoadedUserStateMsgTests {
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

    [Test]
    public async Task JoiningAChannel() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            $"@badge-info=;badges=staff/1;color=#0D4200;display-name=ronni;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;mod=1;subscriber=1;turbo=1;user-type=staff :tmi.twitch.tv USERSTATE #channel"
        }.ToImmutableList();
        
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        
        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            IUserStateMsg lazyLoadedUserStateMsg = (IUserStateMsg)message;
            
            IImmutableList<Badge> expectedBadgeInfo = new List<Badge>().ToImmutableList();
            IImmutableList<Badge> expectedBadges = new List<Badge> {
                new( "staff", "1" )
            }.ToImmutableList();
            
            IImmutableList<string> expectedEmoteSets = new List<string> {
                "0", "33", "50", "237", "793", "2126", "3517", "4578", "5569", "9400", "10337", "12239"
            }.ToImmutableList();
            
            Assert.Multiple(
                () => {
                    Assert.That( lazyLoadedUserStateMsg, Is.Not.Null );
                    Assert.That( lazyLoadedUserStateMsg.MessageType, Is.EqualTo( MessageType.UserState ), "Incorrect message type set" );
                    Assert.That( lazyLoadedUserStateMsg.BadgeInfo, Is.EqualTo( expectedBadgeInfo ), "BadgeInfo was not equal to expected value" );
                    Assert.That( lazyLoadedUserStateMsg.Badges, Is.EqualTo( expectedBadges ), "Badges was not equal to expected value" );
                    Assert.That( lazyLoadedUserStateMsg.Color, Is.EqualTo( "#0D4200" ), "Color was not equal to expected value" );
                    Assert.That( lazyLoadedUserStateMsg.DisplayName, Is.EqualTo( "ronni" ), "DisplayName was not equal to expected value" );
                    Assert.That( lazyLoadedUserStateMsg.EmoteSets, Is.EqualTo( expectedEmoteSets ), "EmoteSets was not equal to expected value" );
                    Assert.That( lazyLoadedUserStateMsg.Id, Is.EqualTo( "" ), "MessageType was not equal to expected value" );
                    Assert.That( lazyLoadedUserStateMsg.Mod, Is.EqualTo( true ), "Mod was not equal to expected value" );
                    Assert.That( lazyLoadedUserStateMsg.Subscriber, Is.EqualTo( true ), "Subscriber was not equal to expected value" );
                    Assert.That( lazyLoadedUserStateMsg.Turbo, Is.EqualTo( true ), "Turbo was not equal to expected value" );
                    Assert.That( lazyLoadedUserStateMsg.UserType, Is.EqualTo( UserType.Staff ), "UserType was not equal to expected value" );
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
        Assert.That( result, Is.True );
        Assert.That( m_messageCallbackCalled, Is.True, "Message callback was not called" );
    }
}
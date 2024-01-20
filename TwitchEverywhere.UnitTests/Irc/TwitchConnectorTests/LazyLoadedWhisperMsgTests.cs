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
public class LazyLoadedWhisperMsgTests {
    private readonly TwitchConnectionOptions m_options = new(
        "channel",
        "access_token",
        "refresh_token",
        "client_id",
        "client_secret",
        "client_name"
    );

    private ITwitchConnector m_twitchConnector;
    private bool m_messageCallbackCalled;

    [SetUp]
    public void Setup() {
        m_messageCallbackCalled = false;
    }

    [Test]
    public async Task WhisperFromAUser() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            $"@badges=staff/1,bits-charity/1;color=#8A2BE2;display-name=PetsgomOO;emotes=;message-id=306;thread-id=12345678_87654321;turbo=0;user-id=87654321;user-type=staff :petsgomoo!petsgomoo@petsgomoo.tmi.twitch.tv WHISPER foo :hello"
        }.ToImmutableList();

        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            IWhisperMsg lazyLoadedWhisperMsg = (IWhisperMsg)message;

            IImmutableList<Badge> expectedBadges = new List<Badge> {
                new( "staff", "1" ),
                new( "bits-charity", "1" )
            }.ToImmutableList();

            Assert.Multiple(
                () => {
                    Assert.That( lazyLoadedWhisperMsg, Is.Not.Null );
                    Assert.That( lazyLoadedWhisperMsg.MessageType, Is.EqualTo( MessageType.Whisper ), "Incorrect message type set" );
                    Assert.That( lazyLoadedWhisperMsg.Badges, Is.EqualTo( expectedBadges ), "Badges was not equal to expected value" );
                    Assert.That( lazyLoadedWhisperMsg.Color, Is.EqualTo( "#8A2BE2" ), "Color was not equal to expected value" );
                    Assert.That( lazyLoadedWhisperMsg.DisplayName, Is.EqualTo( "PetsgomOO" ), "DisplayName was not equal to expected value" );
                    Assert.That( lazyLoadedWhisperMsg.MsgId, Is.EqualTo( "306" ), "Id was not equal to expected value" );
                    Assert.That( lazyLoadedWhisperMsg.ThreadId, Is.EqualTo( "12345678_87654321" ), "ThreadId was not equal to expected value" );
                    Assert.That( lazyLoadedWhisperMsg.Turbo, Is.EqualTo( false ), "Turbo was not equal to expected value" );
                    Assert.That( lazyLoadedWhisperMsg.UserType, Is.EqualTo( UserType.Staff ), "UserType was not equal to expected value" );
                    Assert.That( lazyLoadedWhisperMsg.Emotes, Is.Null, "Emotes was not equal to expected value" );
                    Assert.That( lazyLoadedWhisperMsg.UserId, Is.EqualTo( "87654321" ), "UserId was not equal to expected value" );
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
        Assert.Multiple(
            () => {
                Assert.That( result, Is.True );
                Assert.That( m_messageCallbackCalled, Is.True );
            }
        );

    }
}
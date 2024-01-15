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
public class LazyLoadedClearMsgTests {
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
    public async Task ClearSingleMessageWithId() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            "@login=ronni;room-id=;target-msg-id=abc-123-def;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARMSG #channel :HeyGuys"
        }.ToImmutableList();
        
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        
        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            IClearMsg lazyLoadedClearMsg = (LazyLoadedClearMsg)message;

            Assert.Multiple(
                () => {
                    Assert.That( lazyLoadedClearMsg, Is.Not.Null );
                    Assert.That( lazyLoadedClearMsg.MessageType, Is.EqualTo( MessageType.ClearMsg ), "Incorrect message type set" );
                    Assert.That( lazyLoadedClearMsg.Login, Is.EqualTo( ""), "Login was not equal to expected value" );
                    Assert.That( lazyLoadedClearMsg.RoomId, Is.EqualTo( "" ), "RoomId was not equal to expected value" );
                    Assert.That( lazyLoadedClearMsg.TargetMessageId, Is.EqualTo( "" ), "TargetMessageId was not equal to expected value" );
                    Assert.That( lazyLoadedClearMsg.Timestamp, Is.EqualTo( DateTime.Now ), "Timestamp was not equal to expected value" );
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
        Assert.Multiple(() => {
            Assert.That(actual: result, expression: Is.True);
            Assert.That(m_messageCallbackCalled, Is.True, "Message callback was not called");
        });
    }
}
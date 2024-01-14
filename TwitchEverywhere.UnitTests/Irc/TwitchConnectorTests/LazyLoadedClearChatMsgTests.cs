using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Irc;
using TwitchEverywhere.Irc.Implementation;

namespace TwitchEverywhere.UnitTests.Irc.TwitchConnectorTests;

[TestFixture]
public class LazyLoadedClearChatMsgTests {
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

    [Test]
    public async Task PermanentBan() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            $"@room-id=12345678;target-user-id=87654321;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARCHAT #channel :ronni"
        }.ToImmutableList();
        
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            IClearChatMsg lazyLoadedClearChatMsg = (IClearChatMsg) message;
            
            Assert.Multiple(() => {
                Assert.That(lazyLoadedClearChatMsg, Is.Not.Null );
                Assert.That(lazyLoadedClearChatMsg.MessageType, Is.EqualTo( MessageType.ClearChat ), "Incorrect message type set" );
                Assert.That(lazyLoadedClearChatMsg.Duration, Is.EqualTo( null ), "Duration was not equal to expected value");
                Assert.That(lazyLoadedClearChatMsg.RoomId, Is.EqualTo("12345678"), "RoomId was not equal to expected value");
                Assert.That(lazyLoadedClearChatMsg.TargetUserId, Is.EqualTo("87654321"), "UserId was not equal to expected value");
                Assert.That(lazyLoadedClearChatMsg.Timestamp, Is.EqualTo( new DateTime(2017, 10, 5, 23, 36, 12, 675) ), "Timestamp was not equal to expected value");
                Assert.That(lazyLoadedClearChatMsg.Text, Is.EqualTo("ronni"), "Text was not equal to expected value");
            });
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
    
    [Test]
    public async Task AllMessagesRemoved() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            $"@room-id=12345678;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARCHAT #channel"
        }.ToImmutableList();
        
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            IClearChatMsg lazyLoadedClearChatMsg = (IClearChatMsg) message;
            
            Assert.Multiple(() => {
                Assert.That(lazyLoadedClearChatMsg, Is.Not.Null );
                Assert.That(lazyLoadedClearChatMsg.MessageType, Is.EqualTo( MessageType.ClearChat ), "Incorrect message type set" );
                Assert.That(lazyLoadedClearChatMsg.Duration, Is.EqualTo( null ), "Duration was not equal to expected value");
                Assert.That(lazyLoadedClearChatMsg.RoomId, Is.EqualTo("12345678"), "RoomId was not equal to expected value");
                Assert.That(lazyLoadedClearChatMsg.TargetUserId, Is.EqualTo(""), "UserId was not equal to expected value");
                Assert.That(lazyLoadedClearChatMsg.Timestamp, Is.EqualTo( new DateTime(2017, 10, 5, 23, 36, 12, 675) ), "Timestamp was not equal to expected value");
                Assert.That(lazyLoadedClearChatMsg.Text, Is.EqualTo(""), "Text was not equal to expected value");
            });
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
    
    [Test]
    public async Task TimeoutUserAndRemoveAllOfTheirMessages() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            $"@ban-duration=350;room-id=12345678;target-user-id=87654321;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARCHAT #channel :ronni"
        }.ToImmutableList();
        
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            IClearChatMsg lazyLoadedClearChatMsg = (IClearChatMsg) message;
            
            Assert.Multiple(() => {
                Assert.That(lazyLoadedClearChatMsg, Is.Not.Null );
                Assert.That(lazyLoadedClearChatMsg.MessageType, Is.EqualTo( MessageType.ClearChat ), "Incorrect message type set" );
                Assert.That(lazyLoadedClearChatMsg.Duration, Is.EqualTo( 350 ), "Duration was not equal to expected value");
                Assert.That(lazyLoadedClearChatMsg.RoomId, Is.EqualTo("12345678"), "RoomId was not equal to expected value");
                Assert.That(lazyLoadedClearChatMsg.TargetUserId, Is.EqualTo("87654321"), "UserId was not equal to expected value");
                Assert.That(lazyLoadedClearChatMsg.Timestamp, Is.EqualTo( new DateTime(2017, 10, 5, 23, 36, 12, 675) ), "Timestamp was not equal to expected value");
                Assert.That(lazyLoadedClearChatMsg.Text, Is.EqualTo("ronni"), "Text was not equal to expected value");
            });
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
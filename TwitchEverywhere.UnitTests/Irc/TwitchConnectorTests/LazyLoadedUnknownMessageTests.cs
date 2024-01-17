using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;
using TwitchEverywhere.Irc;
using TwitchEverywhere.Irc.Implementation;

namespace TwitchEverywhere.UnitTests.Irc.TwitchConnectorTests; 

public class LazyLoadedUnknownMessageTests {
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
    public async Task IgnoreMessagesWithInvalidFormat() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            $"foo bar baz"
        }.ToImmutableList();
        
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            IUnknownMsg unknownMsg = (IUnknownMsg)message;
            
            Assert.Multiple(() => {
                Assert.That( unknownMsg, Is.Not.Null );
                Assert.That( unknownMsg.MessageType, Is.EqualTo( unknownMsg.MessageType ), "Incorrect message type set" );
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
    }
    
    [Test]
    public async Task IgnoreUnknownCommands() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            $"@emote-only=0;followers-only=0;r9k=0;slow=0;subs-only=0 :tmi.twitch.tv NEWCOMMAND"
        }.ToImmutableList();
        
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            IUnknownMsg unknownMsg = (IUnknownMsg)message;
            
            Assert.Multiple(() => {
                Assert.That( unknownMsg, Is.Not.Null );
                Assert.That( unknownMsg.MessageType, Is.EqualTo( unknownMsg.MessageType ), "Incorrect message type set" );
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
    }
}
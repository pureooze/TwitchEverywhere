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
public class LazyLoadedReconnectMsgTests {
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
    public async Task ReconnectMessage() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            $":tmi.twitch.tv RECONNECT"
        }.ToImmutableList();
        
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        
        IWebSocketConnection webSocket = new TestWebSocketConnection( messages: messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            
            IReconnectMsg lazyLoadedRoomStateMsg = (IReconnectMsg)message;
            
            Assert.That( message, Is.Not.Null );
            Assert.That( message.MessageType, Is.EqualTo( MessageType.Reconnect ), "Incorrect message type set" );
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
        );
        
        bool result = await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
        Assert.Multiple(
            () => {
                Assert.That( result, Is.True );
                Assert.That( m_messageCallbackCalled, Is.True );
            }
        );
    }
}
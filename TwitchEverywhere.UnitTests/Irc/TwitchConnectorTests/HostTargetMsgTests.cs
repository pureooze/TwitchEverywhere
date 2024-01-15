using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;
using TwitchEverywhere.Irc;
using TwitchEverywhere.Irc.Implementation;

namespace TwitchEverywhere.UnitTests.Irc.TwitchConnectorTests;

public class HostTargetMsgTests {
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
    public async Task HostingChannelWith10Viewers() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            $":tmi.twitch.tv HOSTTARGET #channel :xyz 10"
        }.ToImmutableList();
        
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            IHostTargetMsg lazyLoadedHostTargetMsg = (LazyLoadedHostTargetMsg)message;

            Assert.Multiple(
                () => {
                    Assert.That( lazyLoadedHostTargetMsg, Is.Not.Null );
                    Assert.That( lazyLoadedHostTargetMsg.MessageType, Is.EqualTo( MessageType.HostTarget ), "Incorrect message type set" );
                    Assert.That( lazyLoadedHostTargetMsg.HostingChannel, Is.EqualTo( "" ), "HostingChannel was not equal to expected value" );
                    Assert.That( lazyLoadedHostTargetMsg.IsHostingChannel, Is.EqualTo( true ), "IsHostingChannel was not equal to expected value" );
                    Assert.That( lazyLoadedHostTargetMsg.NumberOfViewers, Is.EqualTo( 10 ), "NumberOfViewers was not equal to expected value" );
                    Assert.That( lazyLoadedHostTargetMsg.Channel, Is.EqualTo( "" ), "Channel was not equal to expected value" );
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
    }
    
    [Test]
    public async Task NotHostingAnyChannel() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            $":tmi.twitch.tv HOSTTARGET #channel :- 10"
        }.ToImmutableList();
        
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            IHostTargetMsg lazyLoadedHostTargetMsg = (LazyLoadedHostTargetMsg)message;

            Assert.Multiple(
                () => {
                    Assert.That( lazyLoadedHostTargetMsg, Is.Not.Null );
                    Assert.That( lazyLoadedHostTargetMsg.MessageType, Is.EqualTo( MessageType.HostTarget ), "Incorrect message type set" );
                    Assert.That( lazyLoadedHostTargetMsg.HostingChannel, Is.EqualTo( "" ), "HostingChannel was not equal to expected value" );
                    Assert.That( lazyLoadedHostTargetMsg.IsHostingChannel, Is.EqualTo( true ), "IsHostingChannel was not equal to expected value" );
                    Assert.That( lazyLoadedHostTargetMsg.NumberOfViewers, Is.EqualTo( 10 ), "NumberOfViewers was not equal to expected value" );
                    Assert.That( lazyLoadedHostTargetMsg.Channel, Is.EqualTo( "" ), "Channel was not equal to expected value" );
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
    }
}
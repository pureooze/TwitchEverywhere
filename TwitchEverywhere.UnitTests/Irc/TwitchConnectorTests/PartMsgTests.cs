using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;
using TwitchEverywhere.Irc;
using TwitchEverywhere.Irc.Implementation;

namespace TwitchEverywhere.UnitTests.Irc.TwitchConnectorTests;

public class PartMsgTests {
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
    public async Task UserLeftTheChannel() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            $":ronni!ronni@ronni.tmi.twitch.tv PART #channel"
        }.ToImmutableList();
        
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            IPartMsg lazyLoadedPartMsg = (IPartMsg)message;

            Assert.Multiple(
                () => {
                    Assert.That( lazyLoadedPartMsg, Is.Not.Null );
                    Assert.That( lazyLoadedPartMsg.MessageType, Is.EqualTo( MessageType.Part ), "Incorrect message type set" );
                    Assert.That( lazyLoadedPartMsg.User, Is.EqualTo( "ronni" ), "User was not equal to expected value" );
                    Assert.That( lazyLoadedPartMsg.Channel, Is.EqualTo( "channel" ), "Channel was not equal to expected value" );
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
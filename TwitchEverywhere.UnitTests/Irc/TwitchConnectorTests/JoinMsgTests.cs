using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;
using TwitchEverywhere.Irc;
using TwitchEverywhere.Irc.Implementation;

namespace TwitchEverywhere.UnitTests.Irc.TwitchConnectorTests;

public class JoinMsgTests {
    private readonly TwitchConnectionOptions m_options = new(
        "channel",
        "access_token",
        "refresh_token",
        "client_id",
        "client_secret",
        "client_name"
    );

    private readonly DateTime m_startTime = DateTimeOffset.FromUnixTimeMilliseconds( 1507246572675 ).UtcDateTime;

    private ITwitchConnector m_twitchConnector;
    private bool m_messageCallbackCalled;

    [Test]
    public async Task UserJoinsChannel() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            $":ronni!ronni@ronni.tmi.twitch.tv JOIN #channel"
        }.ToImmutableList();

        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            IJoinMsg lazyLoadedClearChatMsg = (LazyLoadedJoinMsg)message;

            Assert.Multiple(
                () => {
                    Assert.That( lazyLoadedClearChatMsg, Is.Not.Null );
                    Assert.That( lazyLoadedClearChatMsg.MessageType, Is.EqualTo( MessageType.Join ), "Incorrect message type set" );
                    Assert.That( lazyLoadedClearChatMsg.User, Is.EqualTo( "" ), "User was not equal to expected value" );
                    Assert.That( lazyLoadedClearChatMsg.Channel, Is.EqualTo( "" ), "Channel was not equal to expected value" );
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
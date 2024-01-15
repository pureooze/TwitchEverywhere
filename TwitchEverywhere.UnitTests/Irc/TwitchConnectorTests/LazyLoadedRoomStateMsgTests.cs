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
public class LazyLoadedRoomStateMsgTests {
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
    public async Task UserJoinedChannelWithSettings() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            $"@emote-only=0;followers-only=0;r9k=0;slow=0;subs-only=0 :tmi.twitch.tv ROOMSTATE #channel"
        }.ToImmutableList();

        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;

            IRoomStateMsg lazyLoadedRoomStateMsg = (IRoomStateMsg)message;

            Assert.Multiple(
                () => {
                    Assert.That( lazyLoadedRoomStateMsg, Is.Not.Null );
                    Assert.That( lazyLoadedRoomStateMsg.MessageType, Is.EqualTo( MessageType.RoomState ), "Incorrect message type set" );
                    Assert.That( lazyLoadedRoomStateMsg.EmoteOnly, Is.EqualTo( true ), "EmoteOnly was not equal to expected value" );
                    Assert.That( lazyLoadedRoomStateMsg.FollowersOnly, Is.EqualTo( 1 ), "FollowersOnly was not equal to expected value" );
                    Assert.That( lazyLoadedRoomStateMsg.R9K, Is.EqualTo( true ), "R9K was not equal to expected value" );
                    Assert.That( lazyLoadedRoomStateMsg.Slow, Is.EqualTo( 1 ), "Slow was not equal to expected value" );
                    Assert.That( lazyLoadedRoomStateMsg.RoomId, Is.EqualTo( "" ), "RoomId was not equal to expected value" );
                    Assert.That( lazyLoadedRoomStateMsg.SubsOnly, Is.EqualTo( true ), "SubsOnly was not equal to expected value" );
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
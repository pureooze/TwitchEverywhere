using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.TwitchConnectorTests;

[TestFixture]
public class LazyLoadedClearChatTests {
    private readonly TwitchConnectionOptions m_options = new(
        "channel",
        "access_token",
        "refresh_token",
        "client_id",
        "client_secret",
        "client_name"
    );

    private readonly DateTime m_startTime = DateTimeOffset.FromUnixTimeMilliseconds(1507246572675).UtcDateTime;
        
    private ITwitchConnector m_twitchConnector;

    [Test]
    [TestCaseSource(nameof(ClearChatMessages))]
    public async Task ClearChat( IImmutableList<string> messages, LazyLoadedClearChat expectedMessage ) {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( MockBehavior.Strict );
        dateTimeService.Setup( dts => dts.GetStartTime() ).Returns( m_startTime );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );

        void MessageCallback(
            Message message
        ) {
            Assert.That( message, Is.Not.Null );
            Assert.That( message.MessageType, Is.EqualTo( expectedMessage.MessageType ), "Incorrect message type set" );

            LazyLoadedClearChat msg = (LazyLoadedClearChat)message;
            ClearChatMessageCallback( msg, expectedMessage );
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
    
    private void ClearChatMessageCallback(
        LazyLoadedClearChat lazyLoadedClearChat,
        LazyLoadedClearChat? expectedClearChatMessage
    ) {
        Assert.Multiple(() => {
            Assert.That(lazyLoadedClearChat.Duration, Is.EqualTo(expectedClearChatMessage?.Duration), "Duration was not equal to expected value");
            Assert.That(lazyLoadedClearChat.RoomId, Is.EqualTo(expectedClearChatMessage?.RoomId), "RoomId was not equal to expected value");
            Assert.That(lazyLoadedClearChat.UserId, Is.EqualTo(expectedClearChatMessage?.UserId), "UserId was not equal to expected value");
            Assert.That(lazyLoadedClearChat.Timestamp, Is.EqualTo(expectedClearChatMessage?.Timestamp), "Timestamp was not equal to expected value");
            Assert.That(lazyLoadedClearChat.Text, Is.EqualTo(expectedClearChatMessage?.Text), "Text was not equal to expected value");
            Assert.That(lazyLoadedClearChat.MessageType, Is.EqualTo(expectedClearChatMessage?.MessageType), "MessageType was not equal to expected value");
        });
    }

    private static IEnumerable<TestCaseData> ClearChatMessages() {
        yield return new TestCaseData(
            new List<string> {
                $"@room-id=12345678;target-user-id=87654321;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARCHAT #channel :ronni"
            }.ToImmutableList(),
            new LazyLoadedClearChat(
                channel: "channel",
                message: $"@room-id=12345678;target-user-id=87654321;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARCHAT #channel :ronni"
            )
        ).SetName("Permanent Ban");
        
        yield return new TestCaseData(
            new List<string> {
                $"@room-id=12345678;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARCHAT #channel"
            }.ToImmutableList(),
            new LazyLoadedClearChat(
                channel: "channel",
                message: $"@room-id=12345678;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARCHAT #channel"
            )
        ).SetName("All Messages Removed");
        
        yield return new TestCaseData(
            new List<string> {
                $"@ban-duration=350;room-id=12345678;target-user-id=87654321;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARCHAT #channel :ronni"
            }.ToImmutableList(),
            new LazyLoadedClearChat(
                channel: "channel",
                message: $"@ban-duration=350;room-id=12345678;target-user-id=87654321;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARCHAT #channel :ronni"
            )
        ).SetName("Timeout User And Remove All Of Their Messages");
    }
}
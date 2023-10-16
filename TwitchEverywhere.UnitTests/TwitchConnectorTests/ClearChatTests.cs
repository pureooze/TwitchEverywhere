using System.Collections.Immutable;
using Moq;
using NUnit.Framework.Internal;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.UnitTests.TwitchConnectorTests;

[TestFixture]
public class ClearChatTests {
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
    public async Task ClearChat( IImmutableList<string> messages, ClearChat? expectedMessage ) {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( MockBehavior.Strict );
        dateTimeService.Setup( dts => dts.GetStartTime() ).Returns( m_startTime );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );

        void MessageCallback(
            Message message
        ) {
            Assert.That( message, Is.Not.Null );

            switch( message.MessageType ) {
                case MessageType.ClearChat: {
                    ClearChat msg = (ClearChat)message;
                    ClearChatMessageCallback( msg, expectedMessage );
                    break;
                }
                case MessageType.PrivMsg:
                case MessageType.ClearMsg:
                case MessageType.GlobalUserState:
                case MessageType.Notice:
                case MessageType.RoomState:
                case MessageType.UserNotice:
                case MessageType.UserState:
                case MessageType.Whisper:
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
        ClearChat clearChat,
        ClearChat? expectedClearChatMessage
    ) {
        Assert.Multiple(() => {
            Assert.That(clearChat.Duration, Is.EqualTo(expectedClearChatMessage?.Duration), "Duration was not equal to expected value");
            Assert.That(clearChat.RoomId, Is.EqualTo(expectedClearChatMessage?.RoomId), "RoomId was not equal to expected value");
            Assert.That(clearChat.UserId, Is.EqualTo(expectedClearChatMessage?.UserId), "UserId was not equal to expected value");
            Assert.That(clearChat.Timestamp, Is.EqualTo(expectedClearChatMessage?.Timestamp), "Timestamp was not equal to expected value");
            Assert.That(clearChat.Text, Is.EqualTo(expectedClearChatMessage?.Text), "Text was not equal to expected value");
            Assert.That(clearChat.MessageType, Is.EqualTo(expectedClearChatMessage?.MessageType), "MessageType was not equal to expected value");
        });
    }

    private static IEnumerable<TestCaseData> ClearChatMessages() {
        yield return new TestCaseData(
            new List<string> {
                $"@room-id=12345678;target-user-id=87654321;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARCHAT #channel :ronni"
            }.ToImmutableList(),
            new ClearChat(
                channel: "channel",
                message: $"@room-id=12345678;target-user-id=87654321;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARCHAT #channel :ronni"
            )
        ).SetName("Permanent Ban");
        
        yield return new TestCaseData(
            new List<string> {
                $"@room-id=12345678;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARCHAT #channel"
            }.ToImmutableList(),
            new ClearChat(
                channel: "channel",
                message: $"@room-id=12345678;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARCHAT #channel"
            )
        ).SetName("All Messages Removed");
        
        yield return new TestCaseData(
            new List<string> {
                $"@ban-duration=350;room-id=12345678;target-user-id=87654321;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARCHAT #channel :ronni"
            }.ToImmutableList(),
            new ClearChat(
                channel: "channel",
                message: $"@ban-duration=350;room-id=12345678;target-user-id=87654321;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARCHAT #channel :ronni"
            )
        ).SetName("Timeout User And Remove All Of Their Messages");
    }
}
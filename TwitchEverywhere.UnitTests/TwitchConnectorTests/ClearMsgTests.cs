using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.UnitTests.TwitchConnectorTests;

[TestFixture]
public class ClearMsgTests {
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
    [TestCaseSource(nameof(ClearMsgMessages))]
    public async Task ClearMsg( IImmutableList<string> messages, ClearMsg? expectedMessage ) {
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
                case MessageType.ClearMsg: {
                    ClearMsg msg = (ClearMsg)message;
                    ClearMsgMessageCallback( msg, expectedMessage );
                    break;
                }
                case MessageType.PrivMsg:
                case MessageType.ClearChat:
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
    
    private void ClearMsgMessageCallback(
        ClearMsg clearMsg,
        ClearMsg? expectedClearMessage
    ) {
        Assert.Multiple(() => {
            Assert.That(clearMsg.Login, Is.EqualTo(expectedClearMessage?.Login), "Login was not equal to expected value");
            Assert.That(clearMsg.RoomId, Is.EqualTo(expectedClearMessage?.RoomId), "RoomId was not equal to expected value");
            Assert.That(clearMsg.TargetMessageId, Is.EqualTo(expectedClearMessage?.TargetMessageId), "TargetMessageId was not equal to expected value");
            Assert.That(clearMsg.Timestamp, Is.EqualTo(expectedClearMessage?.Timestamp), "Timestamp was not equal to expected value");
            Assert.That(clearMsg.MessageType, Is.EqualTo(expectedClearMessage?.MessageType), "MessageType was not equal to expected value");
        });
    }

    private static IEnumerable<TestCaseData> ClearMsgMessages() {
        yield return new TestCaseData(
            new List<string> {
                $"foo bar baz"
            }.ToImmutableList(),
            null
        ).SetName("Random message should be ignored");
        
        yield return new TestCaseData(
            new List<string> {
                $"@login=ronni;room-id=;target-msg-id=abc-123-def;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARMSG #channel :HeyGuys"
            }.ToImmutableList(),
            new ClearMsg(
                channel: "channel",
                message: $"@login=ronni;room-id=;target-msg-id=abc-123-def;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARMSG #channel :HeyGuys"
            )
        ).SetName("Clear single message with Id");;
    }
}
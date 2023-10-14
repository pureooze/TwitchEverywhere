using System.Collections.Immutable;
using Moq;
using NUnit.Framework.Internal;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.UnitTests.TwitchConnectorTests;

[TestFixture]
public class NoticeTests {
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
    [TestCaseSource(nameof(NoticeMessages))]
    public async Task Notice( IImmutableList<string> messages, Message? expectedMessage ) {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( MockBehavior.Strict );
        dateTimeService.Setup( dts => dts.GetStartTime() ).Returns( m_startTime );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );

        void MessageCallback(
            Message message
        ) {
            Assert.That( message, Is.Not.Null );

            switch( message.MessageType ) {
                case MessageType.Notice: {
                    NoticeMsg msg = (NoticeMsg)message;
                    NoticeMsg? expectedPrivMessage = (NoticeMsg)expectedMessage;
                    NoticeMessageCallback( msg, expectedPrivMessage );
                    break;
                }
                case MessageType.PrivMsg:
                case MessageType.ClearMsg:
                case MessageType.ClearChat:
                case MessageType.GlobalUserState:
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
            dateTimeService: dateTimeService.Object
        );
        
        bool result = await m_twitchConnector.TryConnect( m_options, MessageCallback );
        Assert.That( result, Is.True );
    }
    
    private void NoticeMessageCallback(
        NoticeMsg noticeMsg,
        NoticeMsg? expectedNoticeMessage
    ) {
        Assert.That( noticeMsg.MsgId, Is.EqualTo( expectedNoticeMessage?.MsgId ), "MsgId was not equal to expected value");
        Assert.That( noticeMsg.TargetUserId, Is.EqualTo( expectedNoticeMessage?.TargetUserId ), "TargetUserId was not equal to expected value");
        Assert.That( noticeMsg.MessageType, Is.EqualTo( expectedNoticeMessage?.MessageType ), "MessageType was not equal to expected value");
    }
    
    private static IEnumerable<TestCaseData> NoticeMessages() {
        yield return new TestCaseData(
            new List<string> {
                $"@msg-id=delete_message_success :tmi.twitch.tv NOTICE #channel :The message from foo is now deleted."
            }.ToImmutableList(),
            new NoticeMsg(
                MsgId: "delete_message_success",
                TargetUserId: ""
            )
        ).SetName("Message Delete Success, No User ID");
        
        yield return new TestCaseData(
            new List<string> {
                $"@msg-id=whisper_restricted;target-user-id=12345678 :tmi.twitch.tv NOTICE #channel :Your settings prevent you from sending this whisper."
            }.ToImmutableList(),
            new NoticeMsg(
                MsgId: "whisper_restricted",
                TargetUserId: "12345678"
            )
        ).SetName("Whisper Restricted, With User ID");
    }
}
using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages.Interfaces;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

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
    public async Task Notice( IImmutableList<string> messages, INoticeMsg expectedMessage ) {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( MockBehavior.Strict );
        dateTimeService.Setup( dts => dts.GetStartTime() ).Returns( m_startTime );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );
        
        void MessageCallback(
            IMessage message
        ) {
            Assert.That( message, Is.Not.Null );
            Assert.That( message.MessageType, Is.EqualTo( expectedMessage.MessageType ), "Incorrect message type set" );

            INoticeMsg msg = (LazyLoadedNoticeMsg)message;
            NoticeMessageCallback( msg, expectedMessage );
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor,
            restApiService: new RestApiService( m_options )
        );
        
        bool result = await m_twitchConnector.TryConnect( m_options, MessageCallback );
        Assert.That( result, Is.True );
    }
    
    private void NoticeMessageCallback(
        INoticeMsg immediateLoadedNoticeMsg,
        INoticeMsg expectedNoticeMessage
    ) {
        Assert.That( immediateLoadedNoticeMsg.MsgId, Is.EqualTo( expectedNoticeMessage.MsgId ), "MsgId was not equal to expected value");
        Assert.That( immediateLoadedNoticeMsg.TargetUserId, Is.EqualTo( expectedNoticeMessage?.TargetUserId ), "TargetUserId was not equal to expected value");
        Assert.That( immediateLoadedNoticeMsg.MessageType, Is.EqualTo( expectedNoticeMessage?.MessageType ), "MessageType was not equal to expected value");
    }
    
    private static IEnumerable<TestCaseData> NoticeMessages() {
        yield return new TestCaseData(
            new List<string> {
                $"@msg-id=delete_message_success :tmi.twitch.tv NOTICE #channel :The message from foo is now deleted."
            }.ToImmutableList(),
            new LazyLoadedNoticeMsg(
                channel: "channel", 
                message: $"@msg-id=delete_message_success :tmi.twitch.tv NOTICE #channel :The message from foo is now deleted."
            )
        ).SetName("Message Delete Success, No User ID");
        
        yield return new TestCaseData(
            new List<string> {
                $"@msg-id=whisper_restricted;target-user-id=12345678 :tmi.twitch.tv NOTICE #channel :Your settings prevent you from sending this whisper."
            }.ToImmutableList(),
            new LazyLoadedNoticeMsg(
                channel: "channel", 
                message: $"@msg-id=whisper_restricted;target-user-id=12345678 :tmi.twitch.tv NOTICE #channel :Your settings prevent you from sending this whisper."
            )
        ).SetName("Whisper Restricted, With User ID");
    }
}
using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Irc;
using TwitchEverywhere.Irc.Implementation;

namespace TwitchEverywhere.UnitTests.Irc.TwitchConnectorTests;

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

    private bool m_messageCallbackCalled;
    private ITwitchConnector m_twitchConnector;

    [Test]
    public async Task MessageDeleteSuccessNoUserId() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            $"@msg-id=delete_message_success :tmi.twitch.tv NOTICE #channel :The message from foo is now deleted."
        }.ToImmutableList();
        
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();
        
        void MessageCallback(
            IMessage message
        )
        {
            m_messageCallbackCalled = true;
            INoticeMsg lazyLoadedNoticeMsg = (INoticeMsg)message;
            
            Assert.Multiple(() =>
            {
                Assert.That( message, Is.Not.Null );
                Assert.That( message.MessageType, Is.EqualTo( MessageType.Notice ), "Incorrect message type set" );
                Assert.That(lazyLoadedNoticeMsg.MsgId, Is.EqualTo(NoticeMsgIdType.DeleteMessageSuccess), "MsgId was not equal to expected value");
                Assert.That(lazyLoadedNoticeMsg.TargetUserId, Is.Empty, "TargetUserId was not equal to expected value");
            });
        }

        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
        );
        
        bool result = await m_twitchConnector.TryConnect( m_options, MessageCallback );
        Assert.Multiple(() => {
            Assert.That(actual: result, expression: Is.True);
            Assert.That(m_messageCallbackCalled, Is.True, "Message callback was not called");
        });
    }
    
    [Test]
    public async Task WhisperRestrictedWithUserId() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            $"@msg-id=whisper_restricted;target-user-id=12345678 :tmi.twitch.tv NOTICE #channel :Your settings prevent you from sending this whisper."
        }.ToImmutableList();
        
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();
        
        void MessageCallback(
            IMessage message
        )
        {
            m_messageCallbackCalled = true;
            INoticeMsg lazyLoadedNoticeMsg = (INoticeMsg)message;
            
            Assert.Multiple(() =>
            {
                Assert.That( message, Is.Not.Null );
                Assert.That( message.MessageType, Is.EqualTo( MessageType.Notice ), "Incorrect message type set" );
                Assert.That(lazyLoadedNoticeMsg.MsgId, Is.EqualTo(NoticeMsgIdType.WhisperRestricted), "MsgId was not equal to expected value");
                Assert.That(lazyLoadedNoticeMsg.TargetUserId, Is.EqualTo("12345678"), "TargetUserId was not equal to expected value");
            });
        }

        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
        );
        
        bool result = await m_twitchConnector.TryConnect( m_options, MessageCallback );
        Assert.Multiple(() => {
            Assert.That(actual: result, expression: Is.True);
            Assert.That(m_messageCallbackCalled, Is.True, "Message callback was not called");
        });
    }
}
using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.TwitchConnectorTests; 

[TestFixture]
public class LazyLoadedWhisperMsgTests {
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
    [TestCaseSource(nameof(WhisperMsgMessages))]
    public async Task WhisperMsg( IImmutableList<string> messages, LazyLoadedWhisperMsg expectedMessage ) {
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

            LazyLoadedWhisperMsg msg = (LazyLoadedWhisperMsg)message;
            WhisperMsgMessageCallback( msg, expectedMessage );
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
    
    private void WhisperMsgMessageCallback(
        LazyLoadedWhisperMsg lazyLoadedWhisperMsg,
        LazyLoadedWhisperMsg? expectedWhisperMsgMessage
    ) {
        Assert.Multiple(() => {
            Assert.That(lazyLoadedWhisperMsg.Badges, Is.EqualTo(expectedWhisperMsgMessage?.Badges), "Badges was not equal to expected value");
            Assert.That(lazyLoadedWhisperMsg.Color, Is.EqualTo(expectedWhisperMsgMessage?.Color), "Color was not equal to expected value");
            Assert.That(lazyLoadedWhisperMsg.DisplayName, Is.EqualTo(expectedWhisperMsgMessage?.DisplayName), "DisplayName was not equal to expected value");
            Assert.That(lazyLoadedWhisperMsg.MsgId, Is.EqualTo(expectedWhisperMsgMessage?.MsgId), "Id was not equal to expected value");
            Assert.That(lazyLoadedWhisperMsg.ThreadId, Is.EqualTo(expectedWhisperMsgMessage?.ThreadId), "ThreadId was not equal to expected value");
            Assert.That(lazyLoadedWhisperMsg.Turbo, Is.EqualTo(expectedWhisperMsgMessage?.Turbo), "Turbo was not equal to expected value");
            Assert.That(lazyLoadedWhisperMsg.UserType, Is.EqualTo(expectedWhisperMsgMessage?.UserType), "UserType was not equal to expected value");
            Assert.That(lazyLoadedWhisperMsg.Emotes, Is.EqualTo(expectedWhisperMsgMessage?.Emotes), "Emotes was not equal to expected value");
            Assert.That(lazyLoadedWhisperMsg.UserId, Is.EqualTo(expectedWhisperMsgMessage?.UserId), "UserId was not equal to expected value");
        });
    }

    private static IEnumerable<TestCaseData> WhisperMsgMessages() {
        yield return new TestCaseData(
            new List<string> {
                $"@badges=staff/1,bits-charity/1;color=#8A2BE2;display-name=PetsgomOO;emotes=;message-id=306;thread-id=12345678_87654321;turbo=0;user-id=87654321;user-type=staff :petsgomoo!petsgomoo@petsgomoo.tmi.twitch.tv WHISPER foo :hello"
            }.ToImmutableList(),
            new LazyLoadedWhisperMsg(
                channel: "channel", 
                message: $"@badges=staff/1,bits-charity/1;color=#8A2BE2;display-name=PetsgomOO;emotes=;message-id=306;thread-id=12345678_87654321;turbo=0;user-id=87654321;user-type=staff :petsgomoo!petsgomoo@petsgomoo.tmi.twitch.tv WHISPER foo :hello"
            )
        ).SetName("Whisper from a user");
    }
}
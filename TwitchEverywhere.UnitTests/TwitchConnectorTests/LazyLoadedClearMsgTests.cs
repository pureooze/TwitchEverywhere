using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;
using TwitchEverywhere.Irc;
using TwitchEverywhere.Irc.Implementation;

namespace TwitchEverywhere.UnitTests.TwitchConnectorTests;

[TestFixture]
public class LazyLoadedClearMsgTests {
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
    public async Task ClearMsg( IImmutableList<string> messages, LazyLoadedClearMsg expectedMessage ) {
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

            LazyLoadedClearMsg msg = (LazyLoadedClearMsg)message;
            ClearMsgMessageCallback( msg, expectedMessage );
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
        LazyLoadedClearMsg lazyLoadedClearMsg,
        LazyLoadedClearMsg? expectedClearMessage
    ) {
        Assert.Multiple(() => {
            Assert.That(lazyLoadedClearMsg.Login, Is.EqualTo(expectedClearMessage?.Login), "Login was not equal to expected value");
            Assert.That(lazyLoadedClearMsg.RoomId, Is.EqualTo(expectedClearMessage?.RoomId), "RoomId was not equal to expected value");
            Assert.That(lazyLoadedClearMsg.TargetMessageId, Is.EqualTo(expectedClearMessage?.TargetMessageId), "TargetMessageId was not equal to expected value");
            Assert.That(lazyLoadedClearMsg.Timestamp, Is.EqualTo(expectedClearMessage?.Timestamp), "Timestamp was not equal to expected value");
            Assert.That(lazyLoadedClearMsg.MessageType, Is.EqualTo(expectedClearMessage?.MessageType), "MessageType was not equal to expected value");
        });
    }

    private static IEnumerable<TestCaseData> ClearMsgMessages() {
        yield return new TestCaseData(
            new List<string> {
                $"@login=ronni;room-id=;target-msg-id=abc-123-def;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARMSG #channel :HeyGuys"
            }.ToImmutableList(),
            new LazyLoadedClearMsg(
                channel: "channel", 
                message: $"@login=ronni;room-id=;target-msg-id=abc-123-def;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARMSG #channel :HeyGuys"
            )
        ).SetName("Clear single message with Id");
    }
}
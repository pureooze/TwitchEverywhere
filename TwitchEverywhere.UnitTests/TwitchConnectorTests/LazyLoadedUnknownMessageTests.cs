using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;
using TwitchEverywhere.Types.Messages.Interfaces;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.TwitchConnectorTests; 

public class LazyLoadedUnknownMessageTests {
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
    [TestCaseSource(nameof(UnknownMessageMessages))]
    public async Task ClearChat( IImmutableList<string> messages, LazyLoadedUnknownMessage expectedMessage ) {
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

            LazyLoadedUnknownMessage msg = (LazyLoadedUnknownMessage)message;
            UnknownMessageCallback( msg, expectedMessage );
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
    
    private void UnknownMessageCallback(
        IUnknownMessage globalUserState,
        IUnknownMessage? expectedGlobalUserState
    ) {
        Assert.Multiple(() => {
            Assert.That(globalUserState.MessageType, Is.EqualTo(expectedGlobalUserState?.MessageType), "MessageType was not equal to expected value");
        });
    }
    
    private static IEnumerable<TestCaseData> UnknownMessageMessages() {
        yield return new TestCaseData(
            new List<string> {
                $"foo bar baz"
            }.ToImmutableList(),
            new LazyLoadedUnknownMessage(
                channel: "channel", 
                message: $"foo bar baz"
            )
        ).SetName("Ignore messages with invalid format");
        
        yield return new TestCaseData(
            new List<string> {
                $"@emote-only=0;followers-only=0;r9k=0;slow=0;subs-only=0 :tmi.twitch.tv ROOMSTATE"
            }.ToImmutableList(),
            new LazyLoadedUnknownMessage(
                channel: "channel", 
                message: $"@emote-only=0;followers-only=0;r9k=0;slow=0;subs-only=0 :tmi.twitch.tv ROOMSTATE"
            )
        ).SetName("Ignore messages missing channel when its required");
    }
}
using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Irc;
using TwitchEverywhere.Irc.Implementation;

namespace TwitchEverywhere.UnitTests.TwitchConnectorTests; 

public class MixedMessageTests {
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
    [TestCaseSource(sourceName: nameof(MixedMessages))]
    public async Task MixedMessage( IImmutableList<string> messages,  IImmutableList<MessageType> expectedMessageTypes ) {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages: messages );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );

        int index = 0;
        
        void MessageCallback(
            IMessage message
        ) {
            Assert.That( message, Is.Not.Null );
            Assert.That( message.MessageType, Is.EqualTo( expectedMessageTypes[index] ), "Incorrect message type set" );

            index += 1;
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor);
        
        bool result = await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
        Assert.That( actual: result, expression: Is.True );
    }
    
    private static IEnumerable<TestCaseData> MixedMessages() {
        yield return new TestCaseData(
            new List<string> {
                $":foo!foo@foo.tmi.twitch.tv JOIN #channel\r\n:foo.tmi.twitch.tv 353 foo = #channel :foo\r\n:foo.tmi.twitch.tv 366 foo #channel :End of /NAMES list\r\n@badge-info=;badges=moderator/1;color=;display-name=foo;emote-sets=0,300374282;mod=1;subscriber=0;user-type=mod :tmi.twitch.tv USERSTATE #channel\r\n@emote-only=0;followers-only=-1;r9k=0;rituals=0;room-id=12345678;slow=0;subs-only=0 :tmi.twitch.tv ROOMSTATE #channel\r\n:foo!foo@foo.tmi.twitch.tv PART #channel\r\n"
            }.ToImmutableList(),
            new List<MessageType> {
                MessageType.Join,
                MessageType.JoinCount,
                MessageType.JoinEnd,
                MessageType.UserState,
                MessageType.RoomState,
                MessageType.Part
            }.ToImmutableList()
        ).SetName("Multiple messages");
    }
}
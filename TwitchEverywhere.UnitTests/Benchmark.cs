using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.UnitTests;

[TestFixture]
public class Tests {
    private readonly TwitchConnectionOptions m_options = new(
        "channel",
        "access_token",
        "refresh_token",
        "client_id",
        "client_secret",
        "client_name"
    ); 
        
    private ITwitchConnector m_twitchConnector;

    [Test]
    [TestCaseSource(nameof(Messages))]
    public async Task Test1( IImmutableList<string> messages, Message? expectedMessage ) {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );

        void MessageCallback(
            Message message
        ) {
            Assert.That( message, Is.Not.Null );

            switch( message.MessageType ) {
                case MessageType.PrivMsg: {
                    PrivMsg privMsg = (PrivMsg)message;
                    PrivMsg? expectedPrivMessage = (PrivMsg)expectedMessage;
                    PrivMessageCallback( privMsg, expectedPrivMessage );
                    break;
                }
                case MessageType.ClearChat:
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
            webSocketConnection: webSocket 
        );
        
        bool result = await m_twitchConnector.TryConnect( m_options, MessageCallback );
        Assert.That( result, Is.True );
    }
    
    private void PrivMessageCallback(
        PrivMsg privMsg,
        PrivMsg? expectedPrivMessage
    ) {
        Assert.That( privMsg.DisplayName, Is.EqualTo( expectedPrivMessage?.DisplayName ) );
    }
    
    private static IEnumerable<TestCaseData> Messages() {
        yield return new TestCaseData(
            new List<string> {
                $"random message should be ignored"
            }.ToImmutableList(),
            null
        );
        
        yield return new TestCaseData(
            new List<string> {
                $"@badge-info=;badges=turbo/1;color=#0D4200;display-name=ronni;emotes=25:0-4,12-16/1902:6-10;id=b34ccfc7-4977-403a-8a94-33c6bac34fb8;mod=0;room-id=1337;subscriber=0;tmi-sent-ts=1507246572675;turbo=1;user-id=1337;user-type=global_mod :ronni!ronni@ronni.tmi.twitch.tv PRIVMSG #channel :Kappa Keepo Kappa"
            }.ToImmutableList(),
            new PrivMsg(
                new List<Badge>().ToImmutableList(),
                "",
                "",
                "",
                new List<Emote>().ToImmutableList(),
                "",
                false,
                null,
                "",
                null,
                null,
                false,
                "",
                "",
                "",
                "",
                "",
                "",
                false,
                DateTime.Now,
                false,
                "",
                UserType.Normal,
                false,
                TimeSpan.Zero,
                "",
                MessageType.PrivMsg
            )
        );
    }
}
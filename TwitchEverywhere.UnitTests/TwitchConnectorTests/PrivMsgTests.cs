using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.UnitTests.TwitchConnectorTests;

[TestFixture]
public class PrivMsgTests {
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
    [TestCaseSource(sourceName: nameof(PrivMsgMessages))]
    public async Task PrivMsg( IImmutableList<string> messages, PrivMsg? expectedMessage ) {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages: messages );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );

        void MessageCallback(
            Message message
        ) {
            Assert.That( actual: message, expression: Is.Not.Null );

            switch( message.MessageType ) {
                case MessageType.PrivMsg: {
                    PrivMsg msg = (PrivMsg)message;
                    PrivMessageCallback( privMsg: msg, expectedPrivMessage: expectedMessage );
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
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
        );
        
        bool result = await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
        Assert.That( actual: result, expression: Is.True );
    }
    
    private void PrivMessageCallback(
        PrivMsg privMsg,
        PrivMsg? expectedPrivMessage
    ) {
        CollectionAssert.AreEqual( privMsg.Badges, expectedPrivMessage?.Badges, "Badges are not equal" );
        Assert.That( privMsg.Bits, Is.EqualTo( expectedPrivMessage?.Bits ), "Bits are not equal");
        Assert.That( privMsg.Color, Is.EqualTo( expectedPrivMessage?.Color ), "Colors are not equal");
        Assert.That( privMsg.DisplayName, Is.EqualTo( expectedPrivMessage?.DisplayName ), "DisplayNames are not equal");
        CollectionAssert.AreEqual( privMsg.Emotes, expectedPrivMessage?.Emotes, "Emotes are not equal" );
        Assert.That( privMsg.Id, Is.EqualTo( expectedPrivMessage?.Id ), "Ids are not equal");
        Assert.That( privMsg.Mod, Is.EqualTo( expectedPrivMessage?.Mod ), "Mods are not equal");
        Assert.That( privMsg.PinnedChatPaidAmount, Is.EqualTo( expectedPrivMessage?.PinnedChatPaidAmount ), "PinnedChatPaidAmounts are not equal");
        Assert.That( privMsg.PinnedChatPaidCurrency, Is.EqualTo( expectedPrivMessage?.PinnedChatPaidCurrency ), "PinnedChatPaidCurrencys are not equal");
        Assert.That( privMsg.PinnedChatPaidExponent, Is.EqualTo( expectedPrivMessage?.PinnedChatPaidExponent ), "PinnedChatPaidExponents are not equal");
        Assert.That( privMsg.PinnedChatPaidLevel, Is.EqualTo( expectedPrivMessage?.PinnedChatPaidLevel ), "PinnedChatPaidLevels are not equal");
        Assert.That( privMsg.PinnedChatPaidIsSystemMessage, Is.EqualTo( expectedPrivMessage?.PinnedChatPaidIsSystemMessage ), "PinnedChatPaidIsSystemMessage are not equal");
        Assert.That( privMsg.ReplyParentMsgId, Is.EqualTo( expectedPrivMessage?.ReplyParentMsgId ), "ReplyParentMsgIds are not equal");
        Assert.That( privMsg.ReplyParentUserId, Is.EqualTo( expectedPrivMessage?.ReplyParentUserId ), "ReplyParentUserIds are not equal");
        Assert.That( privMsg.ReplyParentUserLogin, Is.EqualTo( expectedPrivMessage?.ReplyParentUserLogin ), "ReplyParentUserLogins are not equal");
        Assert.That( privMsg.ReplyParentDisplayName, Is.EqualTo( expectedPrivMessage?.ReplyParentDisplayName ), "ReplyParentDisplayNames are not equal");
        Assert.That( privMsg.ReplyThreadParentMsg, Is.EqualTo( expectedPrivMessage?.ReplyThreadParentMsg ), "ReplyThreadParentMsgs are not equal");
        Assert.That( privMsg.RoomId, Is.EqualTo( expectedPrivMessage?.RoomId ), "RoomIds are not equal");
        Assert.That( privMsg.Subscriber, Is.EqualTo( expectedPrivMessage?.Subscriber ), "Subscribers are not equal");
        Assert.That( privMsg.Timestamp, Is.EqualTo( expectedPrivMessage?.Timestamp ), "Timestamps are not equal");
        Assert.That( privMsg.Turbo, Is.EqualTo( expectedPrivMessage?.Turbo ), "Turbos are not equal");
        Assert.That( privMsg.UserId, Is.EqualTo( expectedPrivMessage?.UserId ), "UserIds are not equal");
        Assert.That( privMsg.UserType, Is.EqualTo( expectedPrivMessage?.UserType ), "UserTypes are not equal");
        Assert.That( privMsg.Vip, Is.EqualTo( expectedPrivMessage?.Vip ), "Vips are not equal");
        Assert.That( privMsg.SinceStartOfStream, Is.EqualTo( expectedPrivMessage?.SinceStartOfStream ), "SinceStartOfStreams are not equal");
        Assert.That( privMsg.Text, Is.EqualTo( expectedPrivMessage?.Text ), "Texts are not equal");
    }
    
    private static IEnumerable<TestCaseData> PrivMsgMessages() {
        yield return new TestCaseData(
            new List<string> {
                $"foo bar baz"
            }.ToImmutableList(),
            null
        ).SetName("Random message should be ignored");
        
        yield return new TestCaseData(
            new List<string> {
                $"@badge-info=;badges=turbo/1;color=#0D4200;display-name=ronni;emotes=25:0-4,12-16/1902:6-10;id=b34ccfc7-4977-403a-8a94-33c6bac34fb8;mod=0;room-id=1337;subscriber=0;tmi-sent-ts=1507246572675;turbo=1;user-id=1337;user-type=global_mod :ronni!ronni@ronni.tmi.twitch.tv PRIVMSG #channel :Kappa Keepo Kappa"
            }.ToImmutableList(),
            new PrivMsg(
                Badges: new List<Badge>() {
                    new( Name: "turbo", Version: "1" )
                }.ToImmutableList(),
                Bits: "",
                Color: "#0D4200",
                DisplayName: "ronni",
                Emotes: new List<Emote>() {
                    new Emote("25", 0, 4),
                    new Emote("25", 12, 16),
                    new Emote("1902", 6, 10)
                }.ToImmutableList(),
                Id: "b34ccfc7-4977-403a-8a94-33c6bac34fb8",
                Mod: false,
                PinnedChatPaidAmount: null,
                PinnedChatPaidCurrency: "",
                PinnedChatPaidExponent: null,
                PinnedChatPaidLevel: null,
                PinnedChatPaidIsSystemMessage: false,
                ReplyParentMsgId: "",
                ReplyParentUserId: "",
                ReplyParentUserLogin: "",
                ReplyParentDisplayName: "",
                ReplyThreadParentMsg: "",
                RoomId: "1337",
                Subscriber: false,
                Timestamp: DateTime.Parse( "2017-10-05 23:36:12.675" ),
                Turbo: true,
                UserId: "1337",
                UserType: UserType.GlobalMod,
                Vip: false,
                SinceStartOfStream: TimeSpan.Zero,
                Text: "Kappa Keepo Kappa"
            )
        ).SetName("Message with badges and emotes");;
    }
}
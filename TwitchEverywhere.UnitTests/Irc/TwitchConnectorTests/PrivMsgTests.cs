using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Irc;
using TwitchEverywhere.Irc.Implementation;

namespace TwitchEverywhere.UnitTests.Irc.TwitchConnectorTests;

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
    
    private bool m_messageCallbackCalled;
    private ITwitchConnector m_twitchConnector;

    [SetUp]
    public void Setup() {
        m_messageCallbackCalled = false;
    }
    
    [Test]
    [TestCaseSource(sourceName: nameof(PrivMsgMessages))]
    public async Task PrivMsg( IImmutableList<string> messages )
    {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages: messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            Assert.That( message, Is.Not.Null );
            Assert.That( message.MessageType, Is.EqualTo( MessageType.PrivMsg ), "Incorrect message type set" );

            PrivMsg msg = (PrivMsg)message;
            PrivMessageCallback( message: msg );
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
        );
        
        bool result = await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
        Assert.Multiple(() => {
            Assert.That(actual: result, expression: Is.True);
            Assert.That(m_messageCallbackCalled, Is.True, "Message callback was not called");
        });
    }

    private void PrivMessageCallback(
        IPrivMsg message
    ) {
        IImmutableList<Badge> expectedBadges = new List<Badge> {
            new( "turbo", "1" )
        }.ToImmutableList();
        
        IImmutableList<Emote> expectedEmotes = new List<Emote> {
            new( "25", 0, 4 ),
            new( "25", 12, 16 ),
            new( "1902", 6, 10 )
        }.ToImmutableList();
        
        Assert.Multiple(() => {
            CollectionAssert.AreEqual( message.Badges, expectedBadges, "Badges are not equal" );
            Assert.That( message.Bits, Is.EqualTo( string.Empty ), "Bits are not equal");
            Assert.That( message.Color, Is.EqualTo( "#0D4200" ), "Colors are not equal");
            Assert.That( message.DisplayName, Is.EqualTo( "ronni" ), "DisplayNames are not equal");
            CollectionAssert.AreEqual( message.Emotes, expectedEmotes, "Emotes are not equal" );
            Assert.That(message.Id, Is.EqualTo("b34ccfc7-4977-403a-8a94-33c6bac34fb8"), "Ids are not equal");
            Assert.That(message.Mod, Is.EqualTo(false), "Mods are not equal");
            Assert.That(message.PinnedChatPaidAmount, Is.Null, "PinnedChatPaidAmounts are not equal");
            Assert.That(message.PinnedChatPaidCurrency, Is.EqualTo(string.Empty), "PinnedChatPaidCurrencys are not equal");
            Assert.That(message.PinnedChatPaidExponent, Is.Null, "PinnedChatPaidExponents are not equal");
            Assert.That(message.PinnedChatPaidLevel, Is.Null, "PinnedChatPaidLevels are not equal");
            Assert.That(message.PinnedChatPaidIsSystemMessage, Is.EqualTo(false), "PinnedChatPaidIsSystemMessage are not equal");
            Assert.That(message.ReplyParentMsgId, Is.EqualTo(string.Empty), "ReplyParentMsgIds are not equal");
            Assert.That(message.ReplyParentUserId, Is.EqualTo(string.Empty), "ReplyParentUserIds are not equal");
            Assert.That(message.ReplyParentUserLogin, Is.EqualTo(string.Empty), "ReplyParentUserLogins are not equal");
            Assert.That(message.ReplyParentDisplayName, Is.EqualTo(string.Empty), "ReplyParentDisplayNames are not equal");
            Assert.That(message.ReplyThreadParentMsg, Is.EqualTo(string.Empty), "ReplyThreadParentMsgs are not equal");
            Assert.That(message.RoomId, Is.EqualTo("1337"), "RoomIds are not equal");
            Assert.That(message.Subscriber, Is.EqualTo(false), "Subscribers are not equal");
            Assert.That(message.Timestamp, Is.EqualTo(new DateTime(2017, 10, 5, 23, 36, 12, 675)), "Timestamps are not equal");
            Assert.That(message.Turbo, Is.EqualTo(true), "Turbos are not equal");
            Assert.That(message.UserId, Is.EqualTo("1337"), "UserIds are not equal");
            Assert.That(message.UserType, Is.EqualTo(UserType.GlobalMod), "UserTypes are not equal");
            Assert.That(message.Vip, Is.EqualTo(false), "Vips are not equal");
            Assert.That(message.Text, Is.EqualTo("Kappa Keepo Kappa"), "Texts are not equal");
        });
    }

    private static IEnumerable<TestCaseData> PrivMsgMessages() {
        yield return new TestCaseData(
            new List<string> {
                $"@badge-info=;badges=turbo/1;color=#0D4200;display-name=ronni;emotes=25:0-4,12-16/1902:6-10;id=b34ccfc7-4977-403a-8a94-33c6bac34fb8;mod=0;room-id=1337;subscriber=0;tmi-sent-ts=1507246572675;turbo=1;user-id=1337;user-type=global_mod :ronni!ronni@ronni.tmi.twitch.tv PRIVMSG #channel :Kappa Keepo Kappa"
            }.ToImmutableList()
        ).SetName("Message with badges and emotes");
    }
}
using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;
using TwitchEverywhere.Types.Messages.Interfaces;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

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
    public async Task PrivMsg( IImmutableList<string> messages, LazyLoadedPrivMsg expectedMessage ) {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages: messages );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );

        void MessageCallback(
            Message message
        ) {
            Assert.That( message, Is.Not.Null );
            Assert.That( message.MessageType, Is.EqualTo( expectedMessage.MessageType ), "Incorrect message type set" );

            LazyLoadedPrivMsg msg = (LazyLoadedPrivMsg)message;
            PrivMessageCallback( lazyLoadedPrivMsg: msg, expectedPrivMessage: expectedMessage );
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
        IPrivMsg lazyLoadedPrivMsg,
        IPrivMsg? expectedPrivMessage
    ) {
        CollectionAssert.AreEqual( lazyLoadedPrivMsg.Badges, expectedPrivMessage?.Badges, "Badges are not equal" );
        Assert.That( lazyLoadedPrivMsg.Bits, Is.EqualTo( expectedPrivMessage?.Bits ), "Bits are not equal");
        Assert.That( lazyLoadedPrivMsg.Color, Is.EqualTo( expectedPrivMessage?.Color ), "Colors are not equal");
        Assert.That( lazyLoadedPrivMsg.DisplayName, Is.EqualTo( expectedPrivMessage?.DisplayName ), "DisplayNames are not equal");
        CollectionAssert.AreEqual( lazyLoadedPrivMsg.Emotes, expectedPrivMessage?.Emotes, "Emotes are not equal" );
        Assert.That( lazyLoadedPrivMsg.Id, Is.EqualTo( expectedPrivMessage?.Id ), "Ids are not equal");
        Assert.That( lazyLoadedPrivMsg.Mod, Is.EqualTo( expectedPrivMessage?.Mod ), "Mods are not equal");
        Assert.That( lazyLoadedPrivMsg.PinnedChatPaidAmount, Is.EqualTo( expectedPrivMessage?.PinnedChatPaidAmount ), "PinnedChatPaidAmounts are not equal");
        Assert.That( lazyLoadedPrivMsg.PinnedChatPaidCurrency, Is.EqualTo( expectedPrivMessage?.PinnedChatPaidCurrency ), "PinnedChatPaidCurrencys are not equal");
        Assert.That( lazyLoadedPrivMsg.PinnedChatPaidExponent, Is.EqualTo( expectedPrivMessage?.PinnedChatPaidExponent ), "PinnedChatPaidExponents are not equal");
        Assert.That( lazyLoadedPrivMsg.PinnedChatPaidLevel, Is.EqualTo( expectedPrivMessage?.PinnedChatPaidLevel ), "PinnedChatPaidLevels are not equal");
        Assert.That( lazyLoadedPrivMsg.PinnedChatPaidIsSystemMessage, Is.EqualTo( expectedPrivMessage?.PinnedChatPaidIsSystemMessage ), "PinnedChatPaidIsSystemMessage are not equal");
        Assert.That( lazyLoadedPrivMsg.ReplyParentMsgId, Is.EqualTo( expectedPrivMessage?.ReplyParentMsgId ), "ReplyParentMsgIds are not equal");
        Assert.That( lazyLoadedPrivMsg.ReplyParentUserId, Is.EqualTo( expectedPrivMessage?.ReplyParentUserId ), "ReplyParentUserIds are not equal");
        Assert.That( lazyLoadedPrivMsg.ReplyParentUserLogin, Is.EqualTo( expectedPrivMessage?.ReplyParentUserLogin ), "ReplyParentUserLogins are not equal");
        Assert.That( lazyLoadedPrivMsg.ReplyParentDisplayName, Is.EqualTo( expectedPrivMessage?.ReplyParentDisplayName ), "ReplyParentDisplayNames are not equal");
        Assert.That( lazyLoadedPrivMsg.ReplyThreadParentMsg, Is.EqualTo( expectedPrivMessage?.ReplyThreadParentMsg ), "ReplyThreadParentMsgs are not equal");
        Assert.That( lazyLoadedPrivMsg.RoomId, Is.EqualTo( expectedPrivMessage?.RoomId ), "RoomIds are not equal");
        Assert.That( lazyLoadedPrivMsg.Subscriber, Is.EqualTo( expectedPrivMessage?.Subscriber ), "Subscribers are not equal");
        Assert.That( lazyLoadedPrivMsg.Timestamp, Is.EqualTo( expectedPrivMessage?.Timestamp ), "Timestamps are not equal");
        Assert.That( lazyLoadedPrivMsg.Turbo, Is.EqualTo( expectedPrivMessage?.Turbo ), "Turbos are not equal");
        Assert.That( lazyLoadedPrivMsg.UserId, Is.EqualTo( expectedPrivMessage?.UserId ), "UserIds are not equal");
        Assert.That( lazyLoadedPrivMsg.UserType, Is.EqualTo( expectedPrivMessage?.UserType ), "UserTypes are not equal");
        Assert.That( lazyLoadedPrivMsg.Vip, Is.EqualTo( expectedPrivMessage?.Vip ), "Vips are not equal");
        Assert.That( lazyLoadedPrivMsg.Text, Is.EqualTo( expectedPrivMessage?.Text ), "Texts are not equal");
    }
    
    private static IEnumerable<TestCaseData> PrivMsgMessages() {
        yield return new TestCaseData(
            new List<string> {
                $"@badge-info=;badges=turbo/1;color=#0D4200;display-name=ronni;emotes=25:0-4,12-16/1902:6-10;id=b34ccfc7-4977-403a-8a94-33c6bac34fb8;mod=0;room-id=1337;subscriber=0;tmi-sent-ts=1507246572675;turbo=1;user-id=1337;user-type=global_mod :ronni!ronni@ronni.tmi.twitch.tv PRIVMSG #channel :Kappa Keepo Kappa"
            }.ToImmutableList(),
            new LazyLoadedPrivMsg(
                channel: "channel",
                message: $"@badge-info=;badges=turbo/1;color=#0D4200;display-name=ronni;emotes=25:0-4,12-16/1902:6-10;id=b34ccfc7-4977-403a-8a94-33c6bac34fb8;mod=0;room-id=1337;subscriber=0;tmi-sent-ts=1507246572675;turbo=1;user-id=1337;user-type=global_mod :ronni!ronni@ronni.tmi.twitch.tv PRIVMSG #channel :Kappa Keepo Kappa",
                sinceStartOfStream: TimeSpan.Zero
            )
        ).SetName("Message with badges and emotes");
    }
}
using System.Collections.Immutable;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.Irc.MessageTypeTests; 

[TestFixture]
public class PrivMsgTests {

    [Test]
    [TestCaseSource(nameof(PrivMsgMessages))]
    public void PrivMsg( string message, TestData expectedPrivMessage )
    {
        IPrivMsg actualLazyLoadedPrivMessage = new LazyLoadedPrivMsg( channel: "channel", message: message, TimeSpan.Zero );
        MessageType actualType = actualLazyLoadedPrivMessage.MessageType;
        
        Assert.That(actualType, Is.EqualTo( MessageType.PrivMsg ));
        
        CollectionAssert.AreEqual( actualLazyLoadedPrivMessage.Badges, expectedPrivMessage.Badges, "Badges are not equal" );
        CollectionAssert.AreEqual( actualLazyLoadedPrivMessage.Emotes, expectedPrivMessage.Emotes, "Emotes are not equal" );
        
        Assert.Multiple(() => {
            Assert.That(actualLazyLoadedPrivMessage.Bits, Is.EqualTo(expectedPrivMessage.Bits), "Bits are not equal");
            Assert.That(actualLazyLoadedPrivMessage.Color, Is.EqualTo(expectedPrivMessage.Color), "Colors are not equal");
            Assert.That(actualLazyLoadedPrivMessage.DisplayName, Is.EqualTo(expectedPrivMessage?.DisplayName), "DisplayNames are not equal");
            Assert.That(actualLazyLoadedPrivMessage.Id, Is.EqualTo(expectedPrivMessage?.Id), "Ids are not equal");
            Assert.That(actualLazyLoadedPrivMessage.Mod, Is.EqualTo(expectedPrivMessage?.Mod), "Mods are not equal");
            Assert.That(actualLazyLoadedPrivMessage.PinnedChatPaidAmount, Is.EqualTo(expectedPrivMessage?.PinnedChatPaidAmount), "PinnedChatPaidAmounts are not equal");
            Assert.That(actualLazyLoadedPrivMessage.PinnedChatPaidCurrency, Is.EqualTo(expectedPrivMessage?.PinnedChatPaidCurrency), "PinnedChatPaidCurrencys are not equal");
            Assert.That(actualLazyLoadedPrivMessage.PinnedChatPaidExponent, Is.EqualTo(expectedPrivMessage?.PinnedChatPaidExponent), "PinnedChatPaidExponents are not equal");
            Assert.That(actualLazyLoadedPrivMessage.PinnedChatPaidLevel, Is.EqualTo(expectedPrivMessage?.PinnedChatPaidLevel), "PinnedChatPaidLevels are not equal");
            Assert.That(actualLazyLoadedPrivMessage.PinnedChatPaidIsSystemMessage, Is.EqualTo(expectedPrivMessage?.PinnedChatPaidIsSystemMessage), "PinnedChatPaidIsSystemMessage are not equal");
            Assert.That(actualLazyLoadedPrivMessage.ReplyParentMsgId, Is.EqualTo(expectedPrivMessage?.ReplyParentMsgId), "ReplyParentMsgIds are not equal");
            Assert.That(actualLazyLoadedPrivMessage.ReplyParentUserId, Is.EqualTo(expectedPrivMessage?.ReplyParentUserId), "ReplyParentUserIds are not equal");
            Assert.That(actualLazyLoadedPrivMessage.ReplyParentUserLogin, Is.EqualTo(expectedPrivMessage?.ReplyParentUserLogin), "ReplyParentUserLogins are not equal");
            Assert.That(actualLazyLoadedPrivMessage.ReplyParentDisplayName, Is.EqualTo(expectedPrivMessage?.ReplyParentDisplayName), "ReplyParentDisplayNames are not equal");
            Assert.That(actualLazyLoadedPrivMessage.ReplyThreadParentMsg, Is.EqualTo(expectedPrivMessage?.ReplyThreadParentMsg), "ReplyThreadParentMsgs are not equal");
            Assert.That(actualLazyLoadedPrivMessage.RoomId, Is.EqualTo(expectedPrivMessage?.RoomId), "RoomIds are not equal");
            Assert.That(actualLazyLoadedPrivMessage.Subscriber, Is.EqualTo(expectedPrivMessage?.Subscriber), "Subscribers are not equal");
            Assert.That(actualLazyLoadedPrivMessage.Timestamp, Is.EqualTo(expectedPrivMessage?.Timestamp), "Timestamps are not equal");
            Assert.That(actualLazyLoadedPrivMessage.Turbo, Is.EqualTo(expectedPrivMessage?.Turbo), "Turbos are not equal");
            Assert.That(actualLazyLoadedPrivMessage.UserId, Is.EqualTo(expectedPrivMessage?.UserId), "UserIds are not equal");
            Assert.That(actualLazyLoadedPrivMessage.UserType, Is.EqualTo(expectedPrivMessage?.UserType), "UserTypes are not equal");
            Assert.That(actualLazyLoadedPrivMessage.Vip, Is.EqualTo(expectedPrivMessage?.Vip), "Vips are not equal");
            Assert.That(actualLazyLoadedPrivMessage.Text, Is.EqualTo(expectedPrivMessage?.Text), "Texts are not equal");
        });
    }

    internal static IEnumerable<TestCaseData> PrivMsgMessages() {
        yield return new TestCaseData(
            "@badge-info=;badges=turbo/1;color=#0D4200;display-name=ronni;emotes=25:0-4,12-16/1902:6-10;id=b34ccfc7-4977-403a-8a94-33c6bac34fb8;mod=0;room-id=1337;subscriber=0;tmi-sent-ts=1507246572675;turbo=1;user-id=1337;user-type=global_mod :ronni!ronni@ronni.tmi.twitch.tv PRIVMSG #channel :Kappa Keepo Kappa",
            new TestData( 
                badges: new List<Badge> {
                    new( "turbo", "1" )
                }.ToImmutableList(),
                bits: "",
                color: "#0D4200",
                displayName: "ronni",
                emotes: new List<Emote> {
                    new("25", 0, 4),
                    new("25", 12, 16),
                    new("1902", 6, 10)
                }.ToImmutableList(),
                id: "b34ccfc7-4977-403a-8a94-33c6bac34fb8",
                pinnedChatPaidCurrency: "",
                replyParentMsgId: "",
                replyParentUserId: "",
                replyParentUserLogin: "",
                replyParentDisplayName: "",
                replyThreadParentMsg: "",
                roomId: "1337",
                userId: "1337",
                text: "Kappa Keepo Kappa",
                pinnedChatPaidAmount: null,
                pinnedChatPaidExponent: null,
                pinnedChatPaidLevel: null,
                pinnedChatPaidIsSystemMessage: false,
                subscriber: false,
                timestamp: new DateTime( 2017, 10, 5, 23, 36, 12, 675 ),
                turbo: true,
                userType: UserType.GlobalMod,
                mod: false,
                vip: false,
                sinceStartOfStream: TimeSpan.Zero
            )
        ).SetName("Message from GlobalMod with badges and emotes");
        
        yield return new TestCaseData(
            "@badge-info=;badges=staff/1,bits/1000;bits=100;color=;display-name=ronni;emotes=;id=b34ccfc7-4977-403a-8a94-33c6bac34fb8;mod=0;room-id=12345678;subscriber=0;tmi-sent-ts=1507246572675;turbo=1;user-id=12345678;user-type=staff :ronni!ronni@ronni.tmi.twitch.tv PRIVMSG #channel :cheer100\n",
            new TestData( 
                badges: new List<Badge> {
                    new( "staff", "1" ),
                    new( "bits", "1000" )
                }.ToImmutableList(),
                bits: "100",
                color: "",
                displayName: "ronni",
                emotes: null,
                id: "b34ccfc7-4977-403a-8a94-33c6bac34fb8",
                pinnedChatPaidCurrency: "",
                replyParentMsgId: "",
                replyParentUserId: "",
                replyParentUserLogin: "",
                replyParentDisplayName: "",
                replyThreadParentMsg: "",
                roomId: "12345678",
                userId: "12345678",
                text: "cheer100",
                pinnedChatPaidAmount: null,
                pinnedChatPaidExponent: null,
                pinnedChatPaidLevel: null,
                pinnedChatPaidIsSystemMessage: false,
                subscriber: false,
                timestamp: new DateTime( 2017, 10, 5, 23, 36, 12, 675 ),
                turbo: true,
                userType: UserType.Staff,
                mod: false,
                vip: false,
                sinceStartOfStream: TimeSpan.Zero
            )
        ).SetName("Message from Staff with cheer");
        
        yield return new TestCaseData(
            "@badge-info=;badges=glhf-pledge/1;color=;emotes=;first-msg=0;flags=;id=f6fb34f8-562f-4b4d-b628-32113d0ef4b0;mod=0;pinned-chat-paid-amount=200;pinned-chat-paid-canonical-amount=200;pinned-chat-paid-currency=USD;pinned-chat-paid-exponent=2;pinned-chat-paid-is-system-message=0;pinned-chat-paid-level=ONE;returning-chatter=0;room-id=12345678;subscriber=0;tmi-sent-ts=1687471984306;turbo=0;user-id=12345678;user-type=\n",
            new TestData( 
                badges: new List<Badge> {
                    new( "glhf-pledge", "1" )
                }.ToImmutableList(),
                bits: "",
                color: "",
                displayName: "",
                emotes: null,
                id: "f6fb34f8-562f-4b4d-b628-32113d0ef4b0",
                pinnedChatPaidCurrency: "USD",
                replyParentMsgId: "",
                replyParentUserId: "",
                replyParentUserLogin: "",
                replyParentDisplayName: "",
                replyThreadParentMsg: "",
                roomId: "12345678",
                userId: "12345678",
                text: "",
                pinnedChatPaidAmount: 200,
                pinnedChatPaidExponent: 2,
                pinnedChatPaidLevel: PinnedChatPaidLevel.One,
                pinnedChatPaidIsSystemMessage: true,
                subscriber: false,
                timestamp: new DateTime( 2023, 06, 22, 22, 13, 04, 306 ),
                turbo: false,
                userType: UserType.Normal,
                mod: false,
                vip: false,
                sinceStartOfStream: TimeSpan.Zero
            )
        ).SetName("Hype chat message");
    }
    
        public class TestData {

        public TestData(
            IImmutableList<Badge> badges,
            string bits,
            string? color,
            string displayName,
            IImmutableList<Emote>? emotes,
            string id,
            string pinnedChatPaidCurrency,
            string replyParentMsgId,
            string replyParentUserId,
            string replyParentUserLogin,
            string replyParentDisplayName,
            string replyThreadParentMsg,
            string roomId,
            string userId,
            string text,
            long? pinnedChatPaidAmount,
            long? pinnedChatPaidExponent,
            PinnedChatPaidLevel? pinnedChatPaidLevel,
            bool pinnedChatPaidIsSystemMessage,
            bool subscriber,
            DateTime timestamp,
            bool turbo,
            UserType userType,
            bool vip,
            TimeSpan sinceStartOfStream,
            bool mod
        ) {
            Badges = badges;
            Bits = bits;
            Color = color;
            DisplayName = displayName;
            Emotes = emotes;
            Id = id;
            PinnedChatPaidCurrency = pinnedChatPaidCurrency;
            ReplyParentMsgId = replyParentMsgId;
            ReplyParentUserId = replyParentUserId;
            ReplyParentUserLogin = replyParentUserLogin;
            ReplyParentDisplayName = replyParentDisplayName;
            ReplyThreadParentMsg = replyThreadParentMsg;
            RoomId = roomId;
            UserId = userId;
            Text = text;
            PinnedChatPaidAmount = pinnedChatPaidAmount;
            PinnedChatPaidExponent = pinnedChatPaidExponent;
            PinnedChatPaidLevel = pinnedChatPaidLevel;
            PinnedChatPaidIsSystemMessage = pinnedChatPaidIsSystemMessage;
            Subscriber = subscriber;
            Timestamp = timestamp;
            Turbo = turbo;
            UserType = userType;
            Vip = vip;
            SinceStartOfStream = sinceStartOfStream;
            Mod = mod;
        }
        
        public MessageType MessageType => MessageType.PrivMsg;

        public IImmutableList<Badge> Badges{ get; }

        public string Bits { get; }

        public string? Color { get; }

        public string DisplayName { get; }

        public IImmutableList<Emote>? Emotes { get; }

        public string Id { get; }

        public bool Mod { get; }

        public long? PinnedChatPaidAmount { get; }

        public string PinnedChatPaidCurrency { get; }

        public long? PinnedChatPaidExponent { get; }

        public PinnedChatPaidLevel? PinnedChatPaidLevel { get; }

        public bool PinnedChatPaidIsSystemMessage { get; }

        public string ReplyParentMsgId { get; }

        public string ReplyParentUserId { get; }

        public string ReplyParentUserLogin { get; }

        public string ReplyParentDisplayName { get; }

        public string ReplyThreadParentMsg { get; }

        public string RoomId { get; }

        public bool Subscriber { get; }

        public DateTime Timestamp { get; }

        public bool Turbo { get; }

        public string UserId { get; }

        public UserType UserType { get; }

        public bool Vip { get; }

        public TimeSpan SinceStartOfStream { get; }

        public string Text { get; }
    }
    
}
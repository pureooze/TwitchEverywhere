using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;

namespace TwitchEverywhere.UnitTests.MessageTypeTests;

[TestFixture]
public class WhisperMsgTests {

    [Test]
    [TestCaseSource( nameof(WhisperMsgMessages) )]
    public void WhisperMsg(
        string message,
        TestData expectedWhisperMsgMessage
    ) {
        WhisperMsg actualWhisperMsgMessage = new( message: message );

        Assert.That( actualWhisperMsgMessage.MessageType, Is.EqualTo( MessageType.Whisper ) );
        
        Assert.Multiple(() => {
            Assert.That(actualWhisperMsgMessage.Badges, Is.EqualTo(expectedWhisperMsgMessage?.Badges), "Badges was not equal to expected value");
            Assert.That(actualWhisperMsgMessage.Color, Is.EqualTo(expectedWhisperMsgMessage?.Color), "Color was not equal to expected value");
            Assert.That(actualWhisperMsgMessage.DisplayName, Is.EqualTo(expectedWhisperMsgMessage?.DisplayName), "DisplayName was not equal to expected value");
            Assert.That(actualWhisperMsgMessage.MsgId, Is.EqualTo(expectedWhisperMsgMessage?.MsgId), "Id was not equal to expected value");
            Assert.That(actualWhisperMsgMessage.ThreadId, Is.EqualTo(expectedWhisperMsgMessage?.ThreadId), "ThreadId was not equal to expected value");
            Assert.That(actualWhisperMsgMessage.Turbo, Is.EqualTo(expectedWhisperMsgMessage?.Turbo), "Turbo was not equal to expected value");
            Assert.That(actualWhisperMsgMessage.UserType, Is.EqualTo(expectedWhisperMsgMessage?.UserType), "UserType was not equal to expected value");
            Assert.That(actualWhisperMsgMessage.Emotes, Is.EqualTo(expectedWhisperMsgMessage?.Emotes), "Emotes was not equal to expected value");
            Assert.That(actualWhisperMsgMessage.UserId, Is.EqualTo(expectedWhisperMsgMessage?.UserId), "UserId was not equal to expected value");
            Assert.That(actualWhisperMsgMessage.FromUser, Is.EqualTo(expectedWhisperMsgMessage?.FromUser), "FromUser was not equal to expected value");
            Assert.That(actualWhisperMsgMessage.ToUser, Is.EqualTo(expectedWhisperMsgMessage?.ToUser), "ToUser was not equal to expected value");
        });
    }

    internal static IEnumerable<TestCaseData> WhisperMsgMessages() {
        yield return new TestCaseData(
            "@badges=staff/1,bits-charity/1;color=#8A2BE2;display-name=PetsgomOO;emotes=;message-id=306;thread-id=12345678_87654321;turbo=0;user-id=87654321;user-type=staff :petsgomoo!petsgomoo@petsgomoo.tmi.twitch.tv WHISPER foo :hello",
            new TestData( 
                badges: new List<Badge> {
                    new("staff", "1"),
                    new("bits-charity", "1")
                }.ToImmutableList(),
                color: "#8A2BE2",
                displayName: "PetsgomOO",
                emotes: null,
                msgId: "306",
                threadId: "12345678_87654321",
                turbo: false,
                userId: "87654321",
                userType: UserType.Staff,
                fromUser: "petsgomoo",
                toUser: "foo"
            )
        ).SetName( "Whisper from a user" );
    }

    public class TestData {
        public IImmutableList<Badge> Badges { get; }
        public string Color { get; }
        public string DisplayName { get; }
        public IImmutableList<Emote>? Emotes { get; }
        public string MsgId { get; }
        public string ThreadId { get; }
        public bool Turbo { get; }
        public string UserId { get; }
        public UserType UserType { get; }
        public string FromUser { get; }
        public string ToUser { get; }

        public TestData(
            IImmutableList<Badge> badges,
            string color,
            string displayName,
            IImmutableList<Emote>? emotes,
            string msgId,
            string threadId,
            bool turbo,
            string userId,
            UserType userType,
            string fromUser,
            string toUser
        ) {
            Badges = badges;
            Color = color;
            DisplayName = displayName;
            Emotes = emotes;
            MsgId = msgId;
            ThreadId = threadId;
            Turbo = turbo;
            UserId = userId;
            UserType = userType;
            FromUser = fromUser;
            ToUser = toUser;

        }
    }

}
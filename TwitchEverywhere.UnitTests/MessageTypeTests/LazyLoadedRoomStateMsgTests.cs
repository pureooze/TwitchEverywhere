using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;

namespace TwitchEverywhere.UnitTests.MessageTypeTests;

[TestFixture]
public class LazyLoadedRoomStateMsgTests {

    [Test]
    [TestCaseSource( nameof(RoomStateMsgMessages) )]
    public void RoomStateMsg(
        string message,
        TestData expectedRoomStateMsgMessage
    ) {
        LazyLoadedRoomStateMsg actualLazyLoadedRoomStateMsgMessage = new( message: message );

        Assert.That( actualLazyLoadedRoomStateMsgMessage.MessageType, Is.EqualTo( MessageType.RoomState ) );

        Assert.Multiple(() => {
            Assert.That(actualLazyLoadedRoomStateMsgMessage.EmoteOnly, Is.EqualTo(expectedRoomStateMsgMessage.EmoteOnly), "EmoteOnly was not equal to expected value");
            Assert.That(actualLazyLoadedRoomStateMsgMessage.FollowersOnly, Is.EqualTo(expectedRoomStateMsgMessage.FollowersOnly), "FollowersOnly was not equal to expected value");
            Assert.That(actualLazyLoadedRoomStateMsgMessage.R9K, Is.EqualTo(expectedRoomStateMsgMessage.R9K), "R9K was not equal to expected value");
            Assert.That(actualLazyLoadedRoomStateMsgMessage.Slow, Is.EqualTo(expectedRoomStateMsgMessage.Slow), "Slow was not equal to expected value");
            Assert.That(actualLazyLoadedRoomStateMsgMessage.RoomId, Is.EqualTo(expectedRoomStateMsgMessage.RoomId), "RoomId was not equal to expected value");
            Assert.That(actualLazyLoadedRoomStateMsgMessage.SubsOnly, Is.EqualTo(expectedRoomStateMsgMessage.SubsOnly), "SubsOnly was not equal to expected value");
        });
    }

    internal static IEnumerable<TestCaseData> RoomStateMsgMessages() {
        yield return new TestCaseData(
            "@emote-only=0;followers-only=0;r9k=0;slow=0;subs-only=0;room-id=240866033 :tmi.twitch.tv ROOMSTATE #channel",
            new TestData(
                emoteOnly: false,
                followersOnly: 0,
                r9k: false,
                roomId: "240866033",
                slow: 0,
                subsOnly: false
            )
        ).SetName( "User joined channel with settings" );
        
        yield return new TestCaseData(
            "@emote-only=1;room-id=240866033 :tmi.twitch.tv ROOMSTATE #channel",
            new TestData(
                emoteOnly: false,
                followersOnly: 0,
                r9k: false,
                roomId: "240866033",
                slow: 0,
                subsOnly: false
            )
        ).SetName( "Emote only enabled" );
    }

    public class TestData {
        public bool EmoteOnly { get; }
        public int FollowersOnly { get; }
        public bool R9K { get; }
        public string RoomId { get; }
        public int Slow { get; }
        public bool SubsOnly { get; }

        public TestData(
            bool emoteOnly,
            int followersOnly,
            bool r9k,
            string roomId,
            int slow,
            bool subsOnly
        ) {
            EmoteOnly = emoteOnly;
            FollowersOnly = followersOnly;
            R9K = r9k;
            RoomId = roomId;
            Slow = slow;
            SubsOnly = subsOnly;

        }
    }

}
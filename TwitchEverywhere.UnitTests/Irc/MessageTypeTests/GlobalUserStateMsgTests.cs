using System.Collections.Immutable;
using System.Text;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.Irc.MessageTypeTests;

[TestFixture]
public class GlobalUserStateMsgTests {

    [Test]
    [TestCaseSource( nameof(GlobalUserStateMsgMessages) )]
    public void GlobalUserStateMsg(
        string message,
        TestData expectedGlobalUserStateMessage
    ) {
        RawMessage rawMessage = new( Encoding.UTF8.GetBytes( message ) );
        LazyLoadedGlobalUserStateMsg actualLazyLoadedGlobalUserStateMsgMessage = new( rawMessage );

        Assert.That( actualLazyLoadedGlobalUserStateMsgMessage.MessageType, Is.EqualTo( MessageType.GlobalUserState ) );

        Assert.Multiple(() => {
            Assert.That(actualLazyLoadedGlobalUserStateMsgMessage.UserId, Is.EqualTo(expectedGlobalUserStateMessage.UserId), "UserId was not equal to expected value");
            Assert.That(actualLazyLoadedGlobalUserStateMsgMessage.Badges, Is.EqualTo(expectedGlobalUserStateMessage.Badges), "Badges was not equal to expected value");
            Assert.That(actualLazyLoadedGlobalUserStateMsgMessage.BadgeInfo, Is.EqualTo(expectedGlobalUserStateMessage.BadgeInfo), "BadgeInfo was not equal to expected value");
            Assert.That(actualLazyLoadedGlobalUserStateMsgMessage.Color, Is.EqualTo(expectedGlobalUserStateMessage.Color), "Color was not equal to expected value");
            Assert.That(actualLazyLoadedGlobalUserStateMsgMessage.DisplayName, Is.EqualTo(expectedGlobalUserStateMessage.DisplayName), "DisplayName was not equal to expected value");
            CollectionAssert.AreEqual(actualLazyLoadedGlobalUserStateMsgMessage.EmoteSets, expectedGlobalUserStateMessage.EmoteSets, "EmoteSets was not equal to expected value");
            Assert.That(actualLazyLoadedGlobalUserStateMsgMessage.Turbo, Is.EqualTo(expectedGlobalUserStateMessage.Turbo), "Turbo was not equal to expected value");
            Assert.That(actualLazyLoadedGlobalUserStateMsgMessage.UserId, Is.EqualTo(expectedGlobalUserStateMessage.UserId), "UserId was not equal to expected value");
            Assert.That(actualLazyLoadedGlobalUserStateMsgMessage.UserType, Is.EqualTo(expectedGlobalUserStateMessage.UserType), "UserType was not equal to expected value");
        });
    }

    internal static IEnumerable<TestCaseData> GlobalUserStateMsgMessages() {
        yield return new TestCaseData(
            "@badge-info=subscriber/8;badges=subscriber/6;color=#0D4200;display-name=dallas;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;turbo=0;user-id=12345678;user-type=admin :tmi.twitch.tv GLOBALUSERSTATE\n",
            new TestData(
                badges: new List<Badge> {
                    new( "subscriber", "6" )
                }.ToImmutableList(),
                badgeInfo: new List<Badge> {
                    new( "subscriber", "8" )
                }.ToImmutableList(),
                color: "#0D4200",
                displayName: "dallas",
                emoteSets: new List<string> {
                    "0",
                    "33",
                    "50",
                    "237",
                    "793",
                    "2126",
                    "3517",
                    "4578",
                    "5569",
                    "9400",
                    "10337",
                    "12239"
                }.ToImmutableList(),
                turbo: true,
                userId: "12345678",
                userType: UserType.Admin
            )
        ).SetName( "Admin user logs in" );
    }

    public class TestData {
        public IImmutableList<Badge> Badges { get; }
        public IImmutableList<Badge> BadgeInfo { get; }
        public string? Color { get; }
        public string DisplayName { get; }
        public IImmutableList<string> EmoteSets { get; }
        public bool Turbo { get; }
        public string UserId { get; }
        public UserType UserType { get; }

        public TestData(
            IImmutableList<Badge> badges,
            IImmutableList<Badge> badgeInfo,
            string? color,
            string displayName,
            IImmutableList<string> emoteSets,
            bool turbo,
            string userId,
            UserType userType
        ) {
            Badges = badges;
            BadgeInfo = badgeInfo;
            Color = color;
            DisplayName = displayName;
            EmoteSets = emoteSets;
            Turbo = turbo;
            UserId = userId;
            UserType = userType;
        }


    }

}
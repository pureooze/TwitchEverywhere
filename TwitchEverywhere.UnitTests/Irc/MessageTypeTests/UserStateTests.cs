using System.Collections.Immutable;
using System.Text;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.Irc.MessageTypeTests;

[TestFixture]
public class UserStateTests {

    [Test]
    [TestCaseSource( nameof(UserStateMessages) )]
    public void UserState(
        string message,
        TestData expectedUserStateMessage
    ) {
        RawMessage rawMessage = new( Encoding.UTF8.GetBytes( message ) );
        IUserStateMsg actualLazyLoadedUserStateMsgMessage = new LazyLoadedUserStateMsg( rawMessage );

        Assert.That( actualLazyLoadedUserStateMsgMessage.MessageType, Is.EqualTo( MessageType.UserState ) );
        
        Assert.Multiple(() => {
            Assert.That(actualLazyLoadedUserStateMsgMessage.BadgeInfo, Is.EqualTo(expectedUserStateMessage?.BadgeInfo), "BadgeInfo was not equal to expected value");
            Assert.That(actualLazyLoadedUserStateMsgMessage.Badges, Is.EqualTo(expectedUserStateMessage?.Badges), "Badges was not equal to expected value");
            Assert.That(actualLazyLoadedUserStateMsgMessage.Color, Is.EqualTo(expectedUserStateMessage?.Color), "Color was not equal to expected value");
            Assert.That(actualLazyLoadedUserStateMsgMessage.DisplayName, Is.EqualTo(expectedUserStateMessage?.DisplayName), "DisplayName was not equal to expected value");
            Assert.That(actualLazyLoadedUserStateMsgMessage.EmoteSets, Is.EqualTo(expectedUserStateMessage?.EmoteSets), "EmoteSets was not equal to expected value");
            Assert.That(actualLazyLoadedUserStateMsgMessage.Id, Is.EqualTo(expectedUserStateMessage?.Id), "MessageType was not equal to expected value");
            Assert.That(actualLazyLoadedUserStateMsgMessage.Mod, Is.EqualTo(expectedUserStateMessage?.Mod), "Mod was not equal to expected value");
            Assert.That(actualLazyLoadedUserStateMsgMessage.Subscriber, Is.EqualTo(expectedUserStateMessage?.Subscriber), "Subscriber was not equal to expected value");
            Assert.That(actualLazyLoadedUserStateMsgMessage.Turbo, Is.EqualTo(expectedUserStateMessage?.Turbo), "Turbo was not equal to expected value");
            Assert.That(actualLazyLoadedUserStateMsgMessage.UserType, Is.EqualTo(expectedUserStateMessage?.UserType), "UserType was not equal to expected value");
        });
    }

    internal static IEnumerable<TestCaseData> UserStateMessages() {
        yield return new TestCaseData(
            "@badge-info=;badges=staff/1;color=#0D4200;display-name=ronni;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;mod=1;subscriber=1;turbo=1;user-type=staff :tmi.twitch.tv USERSTATE #channel",
            new TestData( 
                badgeInfo: new List<Badge>().ToImmutableList(),
                badges: new List<Badge> {
                    new("staff", "1")
                }.ToImmutableList(),
                color: "#0D4200",
                displayName: "ronni",
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
                id: "",
                mod: true,
                subscriber: true,
                turbo: true,
                userType: UserType.Staff
            )
        ).SetName( "Joining a channel" );
    }

    public class TestData {
        public IImmutableList<Badge> BadgeInfo { get; }
        public IImmutableList<Badge> Badges { get; }
        public string Color { get; }
        public string DisplayName { get; }
        public IImmutableList<string> EmoteSets { get; }
        public string Id { get; }
        public bool Mod { get; }
        public bool Subscriber { get; }
        public bool Turbo { get; }
        public UserType UserType { get; }


        public TestData(
            IImmutableList<Badge> badgeInfo,
            IImmutableList<Badge> badges,
            string color,
            string displayName,
            IImmutableList<string> emoteSets,
            string id,
            bool mod,
            bool subscriber,
            bool turbo,
            UserType userType
        ) {
            BadgeInfo = badgeInfo;
            Badges = badges;
            Color = color;
            DisplayName = displayName;
            EmoteSets = emoteSets;
            Id = id;
            Mod = mod;
            Subscriber = subscriber;
            Turbo = turbo;
            UserType = userType;
        }
    }

}
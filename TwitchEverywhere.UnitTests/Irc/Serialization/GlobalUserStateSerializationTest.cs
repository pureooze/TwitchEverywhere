using System.Collections.Immutable;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.ImmediateLoadedMessages;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.Irc.Serialization;

[TestFixture]
public class GlobalUserStateSerializationTest {

    [Test]
    public void LazyLoadedGlobalUserStateSerialization() {
        const string channel = "channel";
        const string message =
            "@badge-info=subscriber/8;badges=subscriber/6;color=#0D4200;display-name=dallas;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;turbo=0;user-id=12345678;user-type=admin :tmi.twitch.tv GLOBALUSERSTATE";
        LazyLoadedGlobalUserState lazyLoadedClearChatMsg = new( channel: channel, message: message );

        Assert.That( lazyLoadedClearChatMsg.RawMessage, Is.EqualTo( message ) );
    }

    [Test]
    public void UserAuthenticated_ImmediateLoadedGlobalUserState_SerializationToIRCMessage() {

        ImmediateLoadedGlobalUserState immediateLoadedClearChatMsg = new(
            channel: "dallas",
            badgeInfo: new List<Badge> {
                new Badge( "subscriber", "8" )
            }.ToImmutableList(),
            badges: new List<Badge> {
                new Badge( "subscriber", "6" )
            }.ToImmutableList(),
            color: "#0D4200",
            displayName: "dallas",
            emoteSets: new List<string> {
                "0", "33", "50", "237", "793", "2126", "3517", "4578", "5569", "9400", "10337", "12239"
            }.ToImmutableList(),
            userId: "12345678",
            userType: UserType.Admin
        );

        Assert.That(
            immediateLoadedClearChatMsg.RawMessage,
            Is.EqualTo(
                "@badge-info=subscriber/8;badges=subscriber/6;color=#0D4200;display-name=dallas;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;turbo=0;user-id=12345678;user-type=admin :tmi.twitch.tv GLOBALUSERSTATE"
            )
        );
    }
}
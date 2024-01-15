using System.Collections.Immutable;
using System.Text;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.ImmediateLoadedMessages;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.Irc.Serialization;

[TestFixture]
public class UserStateMsgSerializationTest {

    [Test]
    public void LazyLoadedUserStateMsgSerialization() {
        const string message =
            "@badge-info=;badges=staff/1;color=#0D4200;display-name=ronni;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;mod=1;subscriber=1;turbo=1;user-type=staff :tmi.twitch.tv USERSTATE #dallas";
        RawMessage rawMessage = new( Encoding.UTF8.GetBytes( message ) );
        IUserStateMsg lazyLoadedClearChatMsg = new LazyLoadedUserStateMsg( rawMessage );

        Assert.That( lazyLoadedClearChatMsg.RawMessage, Is.EqualTo( message ) );
    }

    [Test]
    public void MessageDeletedSuccessfully_ImmediateLoadedUserStateMsg_SerializationToIRCMessage() {

        ImmediateLoadedUserStateMsg immediateLoadedNoticeMsg = new(
            channel: "dallas",
            badges: new List<Badge> {
                new ( "staff", "1" )
            }.ToImmutableList(),
            color: "#0D4200",
            displayName: "ronni",
            emoteSets: new List<string> {
                "0", "33", "50", "237", "793", "2126", "3517", "4578", "5569", "9400", "10337", "12239"
            }.ToImmutableList(),
            mod: true,
            subscriber: true,
            turbo: true,
            userType: UserType.Staff
        );

        Assert.That(
            immediateLoadedNoticeMsg.RawMessage,
            Is.EqualTo(
                "@badge-info=;badges=staff/1;color=#0D4200;display-name=ronni;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;mod=1;subscriber=1;turbo=1;user-type=staff :tmi.twitch.tv USERSTATE #dallas"
            )
        );
    }
}
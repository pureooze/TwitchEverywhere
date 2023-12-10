using TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.Serialization;

[TestFixture]
public class ClearMsgSerializationTest {

    [Test]
    public void LazyLoadedClearMsgSerialization() {
        const string channel = "channel";
        const string message =
            "@login=ronni;room-id=;target-msg-id=abc-123-def;tmi-sent-ts=1642720582342 :tmi.twitch.tv CLEARMSG #dallas :HeyGuys\n";
        LazyLoadedClearMsg lazyLoadedClearChatMsg = new( channel: channel, message: message );

        Assert.That( lazyLoadedClearChatMsg.RawMessage, Is.EqualTo( message ) );
    }

    [Test]
    public void ModDeletesMessage_ImmediateLoadedMsg_SerializationToIRCMessage() {

        ImmediateLoadedClearMsg immediateLoadedClearChatMsg = new(
            channel: "dallas",
            login: "ronni",
            targetMessageId: "abc-123-def",
            timestamp: new DateTime( 2022, 1, 20, 23, 16, 22, 342, DateTimeKind.Utc ),
            text: "HeyGuys"
        );

        Assert.That(
            immediateLoadedClearChatMsg.RawMessage,
            Is.EqualTo(
                "@login=ronni;target-msg-id=abc-123-def;tmi-sent-ts=1642720582342 :tmi.twitch.tv CLEARMSG #dallas :HeyGuys"
            )
        );
    }
}
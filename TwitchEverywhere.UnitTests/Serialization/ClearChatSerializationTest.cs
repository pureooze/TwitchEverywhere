using TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.Serialization;

[TestFixture]
public class ClearChatSerializationTest {

    [Test]
    public void LazyLoadedClearChatSerialization() {
        const string channel = "channel";
        const string message =
            "@room-id=12345678;target-user-id=87654321;tmi-sent-ts=1642715756806 :tmi.twitch.tv CLEARCHAT #dallas :ronni";
        LazyLoadedClearChat lazyLoadedClearChatMsg = new( channel: channel, message: message );

        Assert.That( lazyLoadedClearChatMsg.RawMessage, Is.EqualTo( message ) );
    }

    [Test]
    public void UserPermanentBanned_ImmediateLoadedClearChat_SerializationToIRCMessage() {

        ImmediateLoadedClearChat immediateLoadedClearChatMsg = new(
            channel: "dallas",
            roomId: "12345678",
            targetUserId: "87654321",
            timestamp: new DateTime( 2022, 1, 20, 21, 55, 56, 806, DateTimeKind.Utc ),
            targetUserName: "ronni"
        );

        Assert.That(
            immediateLoadedClearChatMsg.RawMessage,
            Is.EqualTo(
                "@room-id=12345678;target-user-id=87654321;tmi-sent-ts=1642715756806 :tmi.twitch.tv CLEARCHAT #dallas :ronni"
            )
        );
    }

    [Test]
    public void RemoveAllMessages_ImmediateLoadedClearChat_SerializationToIRCMessage() {

        ImmediateLoadedClearChat immediateLoadedClearChatMsg = new(
            channel: "dallas",
            roomId: "12345678",
            timestamp: new DateTime( 2022, 1, 20, 21, 55, 56, 806, DateTimeKind.Utc )
        );

        Assert.That(
            immediateLoadedClearChatMsg.RawMessage,
            Is.EqualTo(
                "@room-id=12345678;tmi-sent-ts=1642715756806 :tmi.twitch.tv CLEARCHAT #dallas"
            )
        );
    }

    [Test]
    public void PutUserInTimeoutAndDeletedAllOfTheirMessages_ImmediateLoadedClearChat_SerializationToIRCMessage() {

        ImmediateLoadedClearChat immediateLoadedClearChatMsg = new(
            channel: "dallas",
            banDuration: 350,
            roomId: "12345678",
            targetUserId: "87654321",
            timestamp: new DateTime( 2022, 1, 20, 21, 55, 56, 806, DateTimeKind.Utc ),
            targetUserName: "ronni"
        );

        Assert.That(
            immediateLoadedClearChatMsg.RawMessage,
            Is.EqualTo(
                "@ban-duration=350;room-id=12345678;target-user-id=87654321;tmi-sent-ts=1642715756806 :tmi.twitch.tv CLEARCHAT #dallas :ronni"
            )
        );
    }
}
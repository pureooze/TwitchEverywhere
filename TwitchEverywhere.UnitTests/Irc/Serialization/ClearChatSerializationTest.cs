using System.Text;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.ImmediateLoadedMessages;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.Irc.Serialization;

[TestFixture]
public class ClearChatSerializationTest {

    [Test]
    public void LazyLoadedClearChatSerialization() {
        const string channel = "channel";
        const string message =
            "@room-id=12345678;target-user-id=87654321;tmi-sent-ts=1642715756806 :tmi.twitch.tv CLEARCHAT #dallas :ronni";
        RawMessage rawMessage = new( Encoding.UTF8.GetBytes( message ) );
        
        IClearChatMsg lazyLoadedClearChatMsgMsg = new LazyLoadedClearChatMsg( rawMessage );

        Assert.That( lazyLoadedClearChatMsgMsg.RawMessage, Is.EqualTo( message ) );
    }

    [Test]
    public void UserPermanentBanned_ImmediateLoadedClearChat_SerializationToIRCMessage() {

        ImmediateLoadedClearChatMsg immediateLoadedClearChatMsgMsg = new(
            channel: "dallas",
            roomId: "12345678",
            targetUserId: "87654321",
            timestamp: new DateTime( 2022, 1, 20, 21, 55, 56, 806, DateTimeKind.Utc ),
            targetUserName: "ronni"
        );

        Assert.That(
            immediateLoadedClearChatMsgMsg.RawMessage,
            Is.EqualTo(
                "@room-id=12345678;target-user-id=87654321;tmi-sent-ts=1642715756806 :tmi.twitch.tv CLEARCHAT #dallas :ronni"
            )
        );
    }

    [Test]
    public void RemoveAllMessages_ImmediateLoadedClearChat_SerializationToIRCMessage() {

        ImmediateLoadedClearChatMsg immediateLoadedClearChatMsgMsg = new(
            channel: "dallas",
            roomId: "12345678",
            timestamp: new DateTime( 2022, 1, 20, 21, 55, 56, 806, DateTimeKind.Utc )
        );

        Assert.That(
            immediateLoadedClearChatMsgMsg.RawMessage,
            Is.EqualTo(
                "@room-id=12345678;tmi-sent-ts=1642715756806 :tmi.twitch.tv CLEARCHAT #dallas"
            )
        );
    }

    [Test]
    public void PutUserInTimeoutAndDeletedAllOfTheirMessages_ImmediateLoadedClearChat_SerializationToIRCMessage() {

        ImmediateLoadedClearChatMsg immediateLoadedClearChatMsgMsg = new(
            channel: "dallas",
            banDuration: 350,
            roomId: "12345678",
            targetUserId: "87654321",
            timestamp: new DateTime( 2022, 1, 20, 21, 55, 56, 806, DateTimeKind.Utc ),
            targetUserName: "ronni"
        );

        Assert.That(
            immediateLoadedClearChatMsgMsg.RawMessage,
            Is.EqualTo(
                "@ban-duration=350;room-id=12345678;target-user-id=87654321;tmi-sent-ts=1642715756806 :tmi.twitch.tv CLEARCHAT #dallas :ronni"
            )
        );
    }
}
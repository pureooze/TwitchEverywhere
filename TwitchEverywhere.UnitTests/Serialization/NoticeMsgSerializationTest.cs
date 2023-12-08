using System.Collections.Immutable;
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;
using TwitchEverywhere.Types.Messages.Interfaces;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.Serialization;

[TestFixture]
public class NoticeMsgSerializationTest {

    [Test]
    public void LazyLoadedNoticeMsgSerialization() {
        const string channel = "channel";
        const string message =
            "@room-id=12345678;target-user-id=87654321;tmi-sent-ts=1642715756806 :tmi.twitch.tv CLEARCHAT #dallas :ronni";
        LazyLoadedNoticeMsg lazyLoadedClearChatMsg = new( channel: channel, message: message );

        Assert.That( lazyLoadedClearChatMsg.RawMessage, Is.EqualTo( message ) );
    }

    [Test]
    public void MessageDeletedSuccessfully_ImmediateLoadedNoticeMsg_SerializationToIRCMessage() {

        ImmediateLoadedNoticeMsg immediateLoadedClearChatMsg = new(
            channel: "dallas",
            msgId: NoticeMsgIdType.DeleteMessageSuccess,
            message: "The message from foo is now deleted."
        );

        Assert.That(
            immediateLoadedClearChatMsg.RawMessage,
            Is.EqualTo(
                "@msg-id=delete_message_success :tmi.twitch.tv NOTICE #dallas :The message from foo is now deleted."
            )
        );
    }
    
    [Test]
    public void UserUnableToSendMessage_ImmediateLoadedNoticeMsg_SerializationToIRCMessage() {

        ImmediateLoadedNoticeMsg immediateLoadedClearChatMsg = new(
            channel: "dallas",
            msgId: NoticeMsgIdType.WhisperRestricted,
            message: "Your settings prevent you from sending this whisper.",
            targetUserId: "12345678"
        );

        Assert.That(
            immediateLoadedClearChatMsg.RawMessage,
            Is.EqualTo(
                "@msg-id=whisper_restricted;target-user-id=12345678 :tmi.twitch.tv NOTICE #dallas :Your settings prevent you from sending this whisper."
            )
        );
    }
}
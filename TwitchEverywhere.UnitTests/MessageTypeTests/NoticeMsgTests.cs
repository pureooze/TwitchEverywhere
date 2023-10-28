using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;

namespace TwitchEverywhere.UnitTests.MessageTypeTests;

[TestFixture]
public class NoticeMsgTests {

    [Test]
    [TestCaseSource( nameof(NoticeMsgMessages) )]
    public void NoticeMsg(
        string message,
        TestData expectedNoticeMsgMessage
    ) {
        NoticeMsg actualNoticeMsgMessage = new( channel: "channel", message: message );

        Assert.That( actualNoticeMsgMessage.MessageType, Is.EqualTo( MessageType.Notice ) );

        Assert.Multiple(() => {
            Assert.That( actualNoticeMsgMessage.MsgId, Is.EqualTo( expectedNoticeMsgMessage?.MsgId ), "MsgId was not equal to expected value");
            Assert.That( actualNoticeMsgMessage.TargetUserId, Is.EqualTo( expectedNoticeMsgMessage?.TargetUserId ), "TargetUserId was not equal to expected value");
        });
    }

    internal static IEnumerable<TestCaseData> NoticeMsgMessages() {
        yield return new TestCaseData(
            "@msg-id=delete_message_success :tmi.twitch.tv NOTICE #channel :The message from foo is now deleted.",
            new TestData(
                msgId: NoticeMsgIdType.DeleteMessageSuccess,
                targetUserId: ""
            )
        ).SetName( "Message Delete Success, No User ID" );
        
        yield return new TestCaseData(
            "@msg-id=whisper_restricted;target-user-id=12345678 :tmi.twitch.tv NOTICE #channel :Your settings prevent you from sending this whisper.",
            new TestData(
                msgId: NoticeMsgIdType.WhisperRestricted,
                targetUserId: "12345678"
            )
        ).SetName( "Whisper Restricted, With User ID" );
    }

    public class TestData {
        public NoticeMsgIdType MsgId { get; }
        public string TargetUserId { get; }

        public TestData(
            NoticeMsgIdType msgId,
            string targetUserId            
        ) {
            MsgId = msgId;
            TargetUserId = targetUserId;
        }
    }

}
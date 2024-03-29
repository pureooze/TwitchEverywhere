using System.Text;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.Irc.MessageTypeTests;

[TestFixture]
public class NoticeMsgTests {

    [Test]
    [TestCaseSource( nameof(NoticeMsgMessages) )]
    public void NoticeMsg(
        string message,
        TestData expectedNoticeMsgMessage
    ) {
        RawMessage rawMessage = new( Encoding.UTF8.GetBytes( message ) );
        INoticeMsg actualImmediateLoadedNoticeMsgMessage = new LazyLoadedNoticeMsg( rawMessage );

        Assert.That( actualImmediateLoadedNoticeMsgMessage.MessageType, Is.EqualTo( MessageType.Notice ) );

        Assert.Multiple(() => {
            Assert.That( actualImmediateLoadedNoticeMsgMessage.MsgId, Is.EqualTo( expectedNoticeMsgMessage.MsgId ), "MsgId was not equal to expected value");
            Assert.That( actualImmediateLoadedNoticeMsgMessage.TargetUserId, Is.EqualTo( expectedNoticeMsgMessage.TargetUserId ), "TargetUserId was not equal to expected value");
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
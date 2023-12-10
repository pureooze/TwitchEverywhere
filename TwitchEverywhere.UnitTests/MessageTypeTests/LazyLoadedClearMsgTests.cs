using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.MessageTypeTests; 

[TestFixture]
public class LazyLoadedClearMsgTests {

    [Test]
    [TestCaseSource(nameof(ClearMsgMessages))]
    public void ClearMsg( string message, TestData expectedClearChatMessage ) {
        LazyLoadedClearMsg actualLazyLoadedClearChatMessage = new( channel: "channel", message: message );
        
        Assert.That(actualLazyLoadedClearChatMessage.MessageType, Is.EqualTo( MessageType.ClearMsg ));
        
        Assert.Multiple(() => {
            Assert.That(actualLazyLoadedClearChatMessage.Login, Is.EqualTo(expectedClearChatMessage.Login), "Login was not equal to expected value");
            Assert.That(actualLazyLoadedClearChatMessage.RoomId, Is.EqualTo(expectedClearChatMessage.RoomId), "RoomId was not equal to expected value");
            Assert.That(actualLazyLoadedClearChatMessage.TargetMessageId, Is.EqualTo(expectedClearChatMessage.TargetMessageId), "TargetMessageId was not equal to expected value");
            Assert.That(actualLazyLoadedClearChatMessage.Timestamp, Is.EqualTo(expectedClearChatMessage.Timestamp), "Timestamp was not equal to expected value");
        });
    }

    internal static IEnumerable<TestCaseData> ClearMsgMessages() {
        yield return new TestCaseData(
            "@login=ronni;room-id=;target-msg-id=abc-123-def;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARMSG #channel :HeyGuys",
            new TestData( 
                login: "ronni",
                roomId: "",
                targetMessageId: "abc-123-def",
                timestamp: new DateTime( 2017, 10, 05, 23, 36, 12, 675 )
            )
        ).SetName("Clear single message with Id");
    }
    
    public class TestData {
        public string Login { get; }
        public string RoomId { get; }
        public string TargetMessageId { get; }
        public DateTime Timestamp { get; }
        public TestData(
            string login,
            string roomId,
            string targetMessageId,
            DateTime timestamp
        ) {
            Login = login;
            RoomId = roomId;
            TargetMessageId = targetMessageId;
            Timestamp = timestamp;


        }
    }
}
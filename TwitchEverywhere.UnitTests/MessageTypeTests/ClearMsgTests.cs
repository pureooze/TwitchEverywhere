using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;

namespace TwitchEverywhere.UnitTests.MessageTypeTests; 

[TestFixture]
public class ClearMsgTests {

    [Test]
    [TestCaseSource(nameof(ClearMsgMessages))]
    public void ClearMsg( string message, TestData expectedClearChatMessage ) {
        ClearMsg actualClearChatMessage = new( message: message );
        
        Assert.That(actualClearChatMessage.MessageType, Is.EqualTo( MessageType.ClearMsg ));
        
        Assert.Multiple(() => {
            Assert.That(actualClearChatMessage.Login, Is.EqualTo(expectedClearChatMessage.Login), "Login was not equal to expected value");
            Assert.That(actualClearChatMessage.RoomId, Is.EqualTo(expectedClearChatMessage.RoomId), "RoomId was not equal to expected value");
            Assert.That(actualClearChatMessage.TargetMessageId, Is.EqualTo(expectedClearChatMessage.TargetMessageId), "TargetMessageId was not equal to expected value");
            Assert.That(actualClearChatMessage.Timestamp, Is.EqualTo(expectedClearChatMessage.Timestamp), "Timestamp was not equal to expected value");
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
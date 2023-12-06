using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.MessageTypeTests; 

[TestFixture]
public class ClearChatMsgTests {

    [Test]
    [TestCaseSource(nameof(ClearChatMsgMessages))]
    public void ClearChatMsg( string message, TestData expectedClearChatMessage )
    {
        LazyLoadedClearChat actualLazyLoadedClearChatMessage = new( channel: "channel", message: message );
        
        Assert.That(actualLazyLoadedClearChatMessage.MessageType, Is.EqualTo( MessageType.ClearChat ));
        
        Assert.Multiple(() => {
            Assert.That(actualLazyLoadedClearChatMessage.Duration, Is.EqualTo(expectedClearChatMessage.Duration), "Duration was not equal to expected value");
            Assert.That(actualLazyLoadedClearChatMessage.RoomId, Is.EqualTo(expectedClearChatMessage.RoomId), "RoomId was not equal to expected value");
            Assert.That(actualLazyLoadedClearChatMessage.TargetUserId, Is.EqualTo(expectedClearChatMessage.UserId), "UserId was not equal to expected value");
            Assert.That(actualLazyLoadedClearChatMessage.Timestamp, Is.EqualTo(expectedClearChatMessage.Timestamp), "Timestamp was not equal to expected value");
            Assert.That(actualLazyLoadedClearChatMessage.Text, Is.EqualTo(expectedClearChatMessage.Text), "Text was not equal to expected value");
        });
    }

    internal static IEnumerable<TestCaseData> ClearChatMsgMessages() {
        yield return new TestCaseData(
            "@room-id=12345678;target-user-id=87654321;tmi-sent-ts=1642715756806 :tmi.twitch.tv CLEARCHAT #channel :ronni\n",
            new TestData( 
                duration: null,
                roomId: "12345678",
                userId: "87654321",
                timestamp: new DateTime( 2022, 01, 20, 21, 55, 56, 806 ),
                text: "ronni"
            )
        ).SetName("Permanent ban user and remove all messages");
        
        yield return new TestCaseData(
            "@room-id=12345678;tmi-sent-ts=1642715695392 :tmi.twitch.tv CLEARCHAT #channel\n",
            new TestData( 
                duration: null,
                roomId: "12345678",
                userId: "",
                timestamp: new DateTime( 2022, 01, 20, 21, 54, 55, 392 ),
                text: ""
            )
        ).SetName("Remove all messages");
        
        yield return new TestCaseData(
            "@ban-duration=350;room-id=12345678;target-user-id=87654321;tmi-sent-ts=1642719320727 :tmi.twitch.tv CLEARCHAT #channel :ronni\n",
            new TestData( 
                duration: 350,
                roomId: "12345678",
                userId: "87654321",
                timestamp: new DateTime( 2022, 01, 20, 22, 55, 20, 727 ),
                text: "ronni"
            )
        ).SetName("Timeout user and remove all messages from user");
    }
    
        public class TestData {
            public long? Duration { get; }
            public string RoomId { get; }
            public string UserId { get; }
            public DateTime Timestamp { get; }
            public string Text { get; }

            public TestData(
            long? duration,
            string roomId,
            string userId,
            DateTime timestamp,
            string text
        ) {
                Duration = duration;
                RoomId = roomId;
                UserId = userId;
                Timestamp = timestamp;
                Text = text;

            }
        
        
    }
    
}
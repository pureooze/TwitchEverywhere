using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.Irc.MessageTypeTests;

[TestFixture]
public class PartMsgTests {

    [Test]
    [TestCaseSource( nameof(PartMsgMessages) )]
    public void PartMsg(
        string message,
        TestData expectedPartMsgMessage
    ) {
        IPartMsg actualLazyLoadedPartMsgMessage = new LazyLoadedPartMsg( channel: "channel", message: message );

        Assert.That( actualLazyLoadedPartMsgMessage.MessageType, Is.EqualTo( MessageType.Part ) );

        Assert.Multiple(() => {
            Assert.That(actualLazyLoadedPartMsgMessage.User, Is.EqualTo(expectedPartMsgMessage.User), "User was not equal to expected value");
            Assert.That(actualLazyLoadedPartMsgMessage.Channel, Is.EqualTo(expectedPartMsgMessage.Channel), "Channel was not equal to expected value");
        });
    }

    internal static IEnumerable<TestCaseData> PartMsgMessages() {
        yield return new TestCaseData(
            ":ronni!ronni@ronni.tmi.twitch.tv PART #channel",
            new TestData(
                user: "ronni",
                channel: "channel"
            )
        ).SetName( "Ronni left the channel" );
    }

    public class TestData {
        public string User { get; }
        public string Channel { get; }

        public TestData(
            string user,
            string channel
        ) {
            User = user;
            Channel = channel;

        }
    }

}
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.MessageTypeTests;

[TestFixture]
public class JoinMsgTests {

    [Test]
    [TestCaseSource( nameof(JoinMsgMessages) )]
    public void JoinMsg(
        string message,
        TestData expectedJoinMsgMessage
    ) {
        IJoinMsg actualLazyLoadedJoinMsgMessage = new LazyLoadedJoinMsg( channel: "channel", message: message );

        Assert.That( actualLazyLoadedJoinMsgMessage.MessageType, Is.EqualTo( MessageType.Join ) );

        Assert.Multiple(() => {
            Assert.That(actualLazyLoadedJoinMsgMessage.User, Is.EqualTo(expectedJoinMsgMessage.User), "User was not equal to expected value");
            Assert.That(actualLazyLoadedJoinMsgMessage.Channel, Is.EqualTo(expectedJoinMsgMessage.Channel), "Channel was not equal to expected value");
        });
    }

    internal static IEnumerable<TestCaseData> JoinMsgMessages() {
        yield return new TestCaseData(
            ":ronni!ronni@ronni.tmi.twitch.tv JOIN #channel",
            new TestData(
                user: "ronni",
                channel: "channel"
            )
        ).SetName( "Ronni joined the channel" );
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
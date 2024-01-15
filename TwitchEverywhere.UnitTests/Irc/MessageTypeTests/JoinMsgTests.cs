using System.Text;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.UnitTests.Irc.MessageTypeTests;

[TestFixture]
public class JoinMsgTests {

    [Test]
    [TestCaseSource( nameof(JoinMsgMessages) )]
    public void JoinMsg(
        string message,
        TestData expectedJoinMsgMessage
    ) {
        RawMessage rawMessage = new( Encoding.UTF8.GetBytes( message ) );
        IJoinMsg actualLazyLoadedJoinMsgMessage = new JoinMsg( rawMessage );

        Assert.Multiple(() => {
            Assert.That( actualLazyLoadedJoinMsgMessage.MessageType, Is.EqualTo( MessageType.Join ) );
            Assert.That(actualLazyLoadedJoinMsgMessage.User, Is.EqualTo(expectedJoinMsgMessage.User), "User was not equal to expected value");
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
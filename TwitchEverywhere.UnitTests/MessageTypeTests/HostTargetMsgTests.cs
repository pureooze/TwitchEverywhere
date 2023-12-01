using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;
using TwitchEverywhere.Types.Messages.Interfaces;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.MessageTypeTests;

[TestFixture]
public class HostTargetMsgTests {

    [Test]
    [TestCaseSource( nameof(HostTargetMsgMessages) )]
    public void HostTargetMsg(
        string message,
        TestData expectedHostTargetMessage
    ) {
        IHostTargetMsg actualHostTargetMsgMessage = new LazyLoadedHostTargetMsg( channel: "channel", message: message );

        Assert.That( actualHostTargetMsgMessage.MessageType, Is.EqualTo( MessageType.HostTarget ) );

        Assert.Multiple(() => {
            Assert.That(actualHostTargetMsgMessage.HostingChannel, Is.EqualTo(expectedHostTargetMessage.HostingChannel), "HostingChannel was not equal to expected value");
            Assert.That(actualHostTargetMsgMessage.IsHostingChannel, Is.EqualTo(expectedHostTargetMessage.IsHostingChannel), "IsHostingChannel was not equal to expected value");
            Assert.That(actualHostTargetMsgMessage.NumberOfViewers, Is.EqualTo(expectedHostTargetMessage.NumberOfViewers), "NumberOfViewers was not equal to expected value");
            Assert.That(actualHostTargetMsgMessage.Channel, Is.EqualTo(expectedHostTargetMessage.Channel), "Channel was not equal to expected value");
        });
    }

    internal static IEnumerable<TestCaseData> HostTargetMsgMessages() {
        yield return new TestCaseData(
            ":tmi.twitch.tv HOSTTARGET #channel :xyz 10",
            new TestData(
                hostingChannel: "channel",
                channel: "xyz",
                numberOfViewers: 10,
                isHostingChannel: true
            )
        ).SetName( "Hosting channel xyz with 10 viewers" );
        
        yield return new TestCaseData(
            ":tmi.twitch.tv HOSTTARGET #channel :- 10",
            new TestData(
                hostingChannel: "channel",
                channel: "-",
                numberOfViewers: 10,
                isHostingChannel: false
            )
        ).SetName( "Not hosting any channel" );
    }

    public class TestData {
        public string HostingChannel { get; }
        public string Channel { get; }
        public int NumberOfViewers { get; }
        public bool IsHostingChannel { get; }

        public TestData(
            string hostingChannel,
            string channel,
            int numberOfViewers,
            bool isHostingChannel
        ) {
            HostingChannel = hostingChannel;
            Channel = channel;
            NumberOfViewers = numberOfViewers;
            IsHostingChannel = isHostingChannel;

        }


    }

}
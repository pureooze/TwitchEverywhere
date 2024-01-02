using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.MessageTypeTests;

[TestFixture]
public class LazyLoadedReconnectMsgTests {

    [Test]
    public void ReconnectMsg() {
        LazyLoadedReconnectMsg actualPartMsgMessage = new( channel: "channel" );

        Assert.That( actualPartMsgMessage.MessageType, Is.EqualTo( MessageType.Reconnect ) );
    }
}
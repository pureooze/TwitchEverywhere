using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;

namespace TwitchEverywhere.UnitTests.MessageTypeTests;

[TestFixture]
public class LazyLoadedReconnectMsgTests {

    [Test]
    public void ReconnectMsg() {
        LazyLoadedReconnectMsg actualPartMsgMessage = new();

        Assert.That( actualPartMsgMessage.MessageType, Is.EqualTo( MessageType.Reconnect ) );
    }
}
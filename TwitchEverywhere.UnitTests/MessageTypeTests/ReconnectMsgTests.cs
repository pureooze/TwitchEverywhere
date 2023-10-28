using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;

namespace TwitchEverywhere.UnitTests.MessageTypeTests;

[TestFixture]
public class ReconnectMsgTests {

    [Test]
    public void ReconnectMsg() {
        ReconnectMsg actualPartMsgMessage = new();

        Assert.That( actualPartMsgMessage.MessageType, Is.EqualTo( MessageType.Reconnect ) );
    }
}
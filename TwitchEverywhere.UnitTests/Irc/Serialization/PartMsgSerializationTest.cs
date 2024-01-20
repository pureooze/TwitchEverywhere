using System.Text;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.ImmediateLoadedMessages;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.Irc.Serialization;

[TestFixture]
public class PartMsgSerializationTest {

    [Test]
    public void LazyLoadedPartMsgSerialization() {
        const string message =
            ":ronni!ronni@ronni.tmi.twitch.tv PART #dallas";
        RawMessage rawMessage = new( Encoding.UTF8.GetBytes( message ) );
        IPartMsg lazyLoadedPartMsg = new LazyLoadedPartMsg( rawMessage );

        Assert.That( lazyLoadedPartMsg.RawMessage, Is.EqualTo( message ) );
    }

    [Test]
    public void MessageDeletedSuccessfully_ImmediateLoadedPartMsg_SerializationToIRCMessage() {

        ImmediateLoadedPartMsg immediateLoadedPartMsg = new(
            channel: "dallas",
            user: "ronni"
        );

        Assert.That(
            immediateLoadedPartMsg.RawMessage,
            Is.EqualTo(
                ":ronni!ronni@ronni.tmi.twitch.tv PART #dallas"
            )
        );
    }
    
}
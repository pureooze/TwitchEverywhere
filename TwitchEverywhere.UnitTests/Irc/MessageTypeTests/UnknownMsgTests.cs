using System.Text;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.Irc.MessageTypeTests;

[TestFixture]
public class LazyLoadedUnknownMessageTests {

    [Test]
    [TestCaseSource( nameof(UnknownMessageMessages) )]
    public void UnknownMessage(
        string message,
        TestData expectedUnknownMessage
    ) {
        RawMessage rawMessage = new( Encoding.UTF8.GetBytes( message ) );
        IUnknownMsg actualLazyLoadedUnknownMsg = new UnknownMsg( rawMessage );

        Assert.That( actualLazyLoadedUnknownMsg.MessageType, Is.EqualTo( MessageType.Unknown ) );
    }

    internal static IEnumerable<TestCaseData> UnknownMessageMessages() {
        yield return new TestCaseData(
            "foo bar baz",
            new TestData(
                message: "foo bar baz"
            )
        ).SetName( "Ignore messages with invalid format" );
    }

    public class TestData {
        public string Message { get; }

        public TestData(
            string message
        ) {
            Message = message;
        }
    }

}
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.MessageTypeTests;

[TestFixture]
public class LazyLoadedUnknownMessageTests {

    [Test]
    [TestCaseSource( nameof(UnknownMessageMessages) )]
    public void UnknownMessage(
        string message,
        TestData expectedUnknownMessage
    ) {
        LazyLoadedUnknownMessage actualLazyLoadedUnknownMessage = new( message: message );

        Assert.That( actualLazyLoadedUnknownMessage.MessageType, Is.EqualTo( MessageType.Unknown ) );

        Assert.That( actualLazyLoadedUnknownMessage.Message, Is.EqualTo( expectedUnknownMessage.Message ) );
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
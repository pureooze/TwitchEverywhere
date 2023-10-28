using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;

namespace TwitchEverywhere.UnitTests.MessageTypeTests;

[TestFixture]
public class UnknownMessageTests {

    [Test]
    [TestCaseSource( nameof(UnknownMessageMessages) )]
    public void UnknownMessage(
        string message,
        TestData expectedUnknownMessage
    ) {
        UnknownMessage actualUnknownMessage = new( message: message );

        Assert.That( actualUnknownMessage.MessageType, Is.EqualTo( MessageType.Unknown ) );

        Assert.That( actualUnknownMessage.Message, Is.EqualTo( expectedUnknownMessage.Message ) );
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
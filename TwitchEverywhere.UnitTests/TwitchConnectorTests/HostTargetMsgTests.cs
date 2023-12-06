using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;
using TwitchEverywhere.Types.Messages.Interfaces;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.TwitchConnectorTests; 

public class HostTargetMsgTests {
    private readonly TwitchConnectionOptions m_options = new(
        "channel",
        "access_token",
        "refresh_token",
        "client_id",
        "client_secret",
        "client_name"
    );

    private readonly DateTime m_startTime = DateTimeOffset.FromUnixTimeMilliseconds(1507246572675).UtcDateTime;
        
    private ITwitchConnector m_twitchConnector;
    
    [Test]
    [TestCaseSource(nameof(HostTargetMsgMessages))]
    public async Task HostTargetMsg( IImmutableList<string> messages, IHostTargetMsg expectedMessage ) {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( MockBehavior.Strict );
        dateTimeService.Setup( dts => dts.GetStartTime() ).Returns( m_startTime );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );

        void MessageCallback(
            IMessage message
        ) {
            Assert.That( message, Is.Not.Null );
            Assert.That( message.MessageType, Is.EqualTo( expectedMessage.MessageType ), "Incorrect message type set" );

            IHostTargetMsg msg = (LazyLoadedHostTargetMsg)message;
            HostTargetMsgCallback( msg, expectedMessage );
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
        );
        
        bool result = await m_twitchConnector.TryConnect( m_options, MessageCallback );
        Assert.That( result, Is.True );
    }
    
    private void HostTargetMsgCallback(
        IHostTargetMsg globalUserState,
        IHostTargetMsg? expectedGlobalUserState
    ) {
        Assert.Multiple(() => {
            Assert.That(globalUserState.HostingChannel, Is.EqualTo(expectedGlobalUserState?.HostingChannel), "HostingChannel was not equal to expected value");
            Assert.That(globalUserState.IsHostingChannel, Is.EqualTo(expectedGlobalUserState?.IsHostingChannel), "IsHostingChannel was not equal to expected value");
            Assert.That(globalUserState.NumberOfViewers, Is.EqualTo(expectedGlobalUserState?.NumberOfViewers), "NumberOfViewers was not equal to expected value");
            Assert.That(globalUserState.Channel, Is.EqualTo(expectedGlobalUserState?.Channel), "Channel was not equal to expected value");
        });
    }
    
    private static IEnumerable<TestCaseData> HostTargetMsgMessages() {
        yield return new TestCaseData(
            new List<string> {
                $":tmi.twitch.tv HOSTTARGET #channel :xyz 10"
            }.ToImmutableList(),
            new LazyLoadedHostTargetMsg(
                message: $":tmi.twitch.tv HOSTTARGET #channel :xyz 10",
                channel: "channel"
            )
        ).SetName("Hosting channel xyz with 10 viewers");
        
        yield return new TestCaseData(
            new List<string> {
                $":tmi.twitch.tv HOSTTARGET #channel :- 10"
            }.ToImmutableList(),
            new LazyLoadedHostTargetMsg(
                message: $":tmi.twitch.tv HOSTTARGET #channel :- 10",
                channel: "channel"
            )
        ).SetName("Not hosting any channel");
    }
}
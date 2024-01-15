using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;
using TwitchEverywhere.Irc;
using TwitchEverywhere.Irc.Implementation;

namespace TwitchEverywhere.UnitTests.Irc.TwitchConnectorTests;

[TestFixture]
public class LazyLoadedUserNoticeMsgTests {
    private readonly TwitchConnectionOptions m_options = new(
        "channel",
        "access_token",
        "refresh_token",
        "client_id",
        "client_secret",
        "client_name"
    );

    private bool m_messageCallbackCalled;
    private ITwitchConnector m_twitchConnector;

    [SetUp]
    public void Setup() {
        m_messageCallbackCalled = false;
    }
    
    [Test]
    public async Task Resubscribed() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            @"@badge-info=;badges=staff/1,broadcaster/1,turbo/1;color=#008000;display-name=ronni;emotes=;id=db25007f-7a18-43eb-9379-80131e44d633;login=ronni;mod=0;msg-id=resub;msg-param-cumulative-months=6;msg-param-streak-months=2;msg-param-should-share-streak=1;msg-param-sub-plan=Prime;msg-param-sub-plan-name=Prime;room-id=12345678;subscriber=1;system-msg=ronni\shas\ssubscribed\sfor\s6\smonths!;tmi-sent-ts=1507246572675;turbo=1;user-id=87654321;user-type=staff :tmi.twitch.tv USERNOTICE #channel :Great stream -- keep it up!"
        }.ToImmutableList();

        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            IUserNoticeMsg lazyLoadedUserNoticeMsg = (LazyLoadedUserNoticeMsg)message;

            Assert.Multiple(
                () => {
                    Assert.That( lazyLoadedUserNoticeMsg, Is.Not.Null );
                    Assert.That( lazyLoadedUserNoticeMsg.MessageType, Is.EqualTo( MessageType.UserNotice ), "Incorrect message type set" );
                    Assert.That( lazyLoadedUserNoticeMsg.RoomId, Is.EqualTo( "12345678" ), "RoomId was not equal to expected value" );
                    Assert.That( lazyLoadedUserNoticeMsg.UserId, Is.EqualTo( "12345678" ), "UserId was not equal to expected value" );
                    Assert.That( lazyLoadedUserNoticeMsg.Timestamp, Is.EqualTo( DateTime.Now ), "Timestamp was not equal to expected value" );
                }
            );
        }

        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector(
            authorizer: authorizer.Object,
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
        );

        bool result = await m_twitchConnector.TryConnect( m_options, MessageCallback );
        Assert.That( result, Is.True );
        Assert.That( m_messageCallbackCalled, Is.True, "Message callback was not called" );
    }
    
    [Test]
    public async Task UserGiftedASubscription() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            @"@badge-info=;badges=staff/1,premium/1;color=#0000FF;display-name=TWW2;emotes=;id=e9176cd8-5e22-4684-ad40-ce53c2561c5e;login=tww2;mod=0;msg-id=subgift;msg-param-months=1;msg-param-recipient-display-name=Mr_Woodchuck;msg-param-recipient-id=55554444;msg-param-recipient-name=mr_woodchuck;msg-param-sub-plan-name=House\\sof\\sNyoro~n;msg-param-sub-plan=1000;room-id=19571752;subscriber=0;system-msg=TWW2\\sgifted\\sa\\sTier\\s1\\ssub\\sto\\sMr_Woodchuck!;tmi-sent-ts=1521159445153;turbo=0;user-id=87654321;user-type=staff :tmi.twitch.tv USERNOTICE #channel"
        }.ToImmutableList();

        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            IUserNoticeMsg lazyLoadedUserNoticeMsg = (LazyLoadedUserNoticeMsg)message;

            Assert.Multiple(
                () => {
                    Assert.That( lazyLoadedUserNoticeMsg, Is.Not.Null );
                    Assert.That( lazyLoadedUserNoticeMsg.MessageType, Is.EqualTo( MessageType.UserNotice ), "Incorrect message type set" );
                    Assert.That( lazyLoadedUserNoticeMsg.RoomId, Is.EqualTo( "12345678" ), "RoomId was not equal to expected value" );
                    Assert.That( lazyLoadedUserNoticeMsg.UserId, Is.EqualTo( "12345678" ), "UserId was not equal to expected value" );
                    Assert.That( lazyLoadedUserNoticeMsg.Timestamp, Is.EqualTo( DateTime.Now ), "Timestamp was not equal to expected value" );
                }
            );
        }

        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector(
            authorizer: authorizer.Object,
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
        );

        bool result = await m_twitchConnector.TryConnect( m_options, MessageCallback );
        Assert.That( result, Is.True );
        Assert.That( m_messageCallbackCalled, Is.True, "Message callback was not called" );
    }
    
    [Test]
    public async Task RaidingAChannel() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            @"@badge-info=;badges=turbo/1;color=#9ACD32;display-name=TestChannel;emotes=;id=3d830f12-795c-447d-af3c-ea05e40fbddb;login=testchannel;mod=0;msg-id=raid;msg-param-displayName=TestChannel;msg-param-login=testchannel;msg-param-viewerCount=15;room-id=33332222;subscriber=0;system-msg=15\\sraiders\\sfrom\\sTestChannel\\shave\\sjoined\\n!;tmi-sent-ts=1507246572675;turbo=1;user-id=123456;user-type= :tmi.twitch.tv USERNOTICE #channel"
        }.ToImmutableList();

        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            IUserNoticeMsg lazyLoadedUserNoticeMsg = (LazyLoadedUserNoticeMsg)message;

            Assert.Multiple(
                () => {
                    Assert.That( lazyLoadedUserNoticeMsg, Is.Not.Null );
                    Assert.That( lazyLoadedUserNoticeMsg.MessageType, Is.EqualTo( MessageType.UserNotice ), "Incorrect message type set" );
                    Assert.That( lazyLoadedUserNoticeMsg.RoomId, Is.EqualTo( "12345678" ), "RoomId was not equal to expected value" );
                    Assert.That( lazyLoadedUserNoticeMsg.UserId, Is.EqualTo( "12345678" ), "UserId was not equal to expected value" );
                    Assert.That( lazyLoadedUserNoticeMsg.Timestamp, Is.EqualTo( DateTime.Now ), "Timestamp was not equal to expected value" );
                }
            );
        }

        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector(
            authorizer: authorizer.Object,
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
        );

        bool result = await m_twitchConnector.TryConnect( m_options, MessageCallback );
        Assert.That( result, Is.True );
        Assert.That( m_messageCallbackCalled, Is.True, "Message callback was not called" );
    }
    
    [Test]
    public async Task ANewChatterRitual() {
        // Arrange
        IImmutableList<string> messages = new List<string> {
            @"@badge-info=;badges=;color=;display-name=SevenTest1;emotes=30259:0-6;id=37feed0f-b9c7-4c3a-b475-21c6c6d21c3d;login=seventest1;mod=0;msg-id=ritual;msg-param-ritual-name=new_chatter;room-id=87654321;subscriber=0;system-msg=Seventoes\\sis\\snew\\shere!;tmi-sent-ts=1508363903826;turbo=0;user-id=77776666;user-type= :tmi.twitch.tv USERNOTICE #channel :HeyGuys"
        }.ToImmutableList();

        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );

        IWebSocketConnection webSocket = new TestWebSocketConnection( messages );
        IMessageProcessor messageProcessor = new MessageProcessor();

        void MessageCallback(
            IMessage message
        ) {
            m_messageCallbackCalled = true;
            IUserNoticeMsg lazyLoadedUserNoticeMsg = (LazyLoadedUserNoticeMsg)message;

            Assert.Multiple(
                () => {
                    Assert.That( lazyLoadedUserNoticeMsg, Is.Not.Null );
                    Assert.That( lazyLoadedUserNoticeMsg.MessageType, Is.EqualTo( MessageType.UserNotice ), "Incorrect message type set" );
                    Assert.That( lazyLoadedUserNoticeMsg.RoomId, Is.EqualTo( "12345678" ), "RoomId was not equal to expected value" );
                    Assert.That( lazyLoadedUserNoticeMsg.UserId, Is.EqualTo( "12345678" ), "UserId was not equal to expected value" );
                    Assert.That( lazyLoadedUserNoticeMsg.Timestamp, Is.EqualTo( DateTime.Now ), "Timestamp was not equal to expected value" );
                }
            );
        }

        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector(
            authorizer: authorizer.Object,
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
        );

        bool result = await m_twitchConnector.TryConnect( m_options, MessageCallback );
        Assert.That( result, Is.True );
        Assert.That( m_messageCallbackCalled, Is.True, "Message callback was not called" );
    }
}
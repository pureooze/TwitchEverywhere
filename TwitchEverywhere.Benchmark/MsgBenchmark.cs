using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Jobs;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.Benchmark; 

[MemoryDiagnoser]
[NativeMemoryProfiler]
[SimpleJob(RuntimeMoniker.Net80)]
public class MsgBenchmark {
    
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

    [Params(50)]
    public int Iterations;

    [Benchmark]
    public async Task PrivMsg() {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );
    
        string baseMessage =
            "@badge-info=;badges=turbo/1;color=#0D4200;display-name=ronni;emotes=25:0-4,12-16/1902:6-10;id=b34ccfc7-4977-403a-8a94-33c6bac34fb8;mod=0;room-id=1337;subscriber=0;tmi-sent-ts=1507246572675;turbo=1;user-id=1337;user-type=global_mod :ronni!ronni@ronni.tmi.twitch.tv PRIVMSG #channel :";
        IWebSocketConnection webSocket = new TestWebSocketConnection( iterations: Iterations, baseMessage: baseMessage );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );
    
        void MessageCallback(
            IMessage message
        ) {
            switch( message.MessageType ) {
                case MessageType.PrivMsg: {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor,
            restApiService: new RestApiService( m_options )
        );
        
        await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
    }
    
    [Benchmark]
    public async Task ClearMsg() {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );
    
        string baseMessage = $"@login=ronni;room-id=;target-msg-id=abc-123-def;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARMSG #channel :";
        IWebSocketConnection webSocket = new TestWebSocketConnection( iterations: Iterations, baseMessage: baseMessage );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );
    
        void MessageCallback(
            IMessage message
        ) {
            switch( message.MessageType ) {
                case MessageType.ClearMsg: {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor,
            restApiService: new RestApiService( m_options )
        );
        
        await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
    }
    
    [Benchmark]
    public async Task ClearChat() {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );
    
        string baseMessage = $"@ban-duration=350;room-id=12345678;target-user-id=87654321;tmi-sent-ts=1507246572675 :tmi.twitch.tv CLEARCHAT #channel :";
        IWebSocketConnection webSocket = new TestWebSocketConnection( iterations: Iterations, baseMessage: baseMessage );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );
    
        void MessageCallback(
            IMessage message
        ) {
            switch( message.MessageType ) {
                case MessageType.ClearChat: {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor,
            restApiService: new RestApiService( m_options )
        );
        
        await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
    }
    
    [Benchmark]
    public async Task NoticeMsg() {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );
    
        string baseMessage = $"@msg-id=whisper_restricted;target-user-id=12345678 :tmi.twitch.tv NOTICE #channel :";
        IWebSocketConnection webSocket = new TestWebSocketConnection( iterations: Iterations, baseMessage: baseMessage );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );
    
        void MessageCallback(
            IMessage message
        ) {
            switch( message.MessageType ) {
                case MessageType.Notice: {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor,
            restApiService: new RestApiService( m_options )
        );
        
        await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
    }
    
    [Benchmark]
    public async Task GlobalUserStateMessage() {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );
    
        string baseMessage = $"@badge-info=subscriber/8;badges=subscriber/6;color=#0D4200;display-name=dallas;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;turbo=0;user-id=12345678;user-type=admin :tmi.twitch.tv GLOBALUSERSTATE";
        IWebSocketConnection webSocket = new TestWebSocketConnection( iterations: Iterations, baseMessage: baseMessage );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );
    
        void MessageCallback(
            IMessage message
        ) {
            switch( message.MessageType ) {
                case MessageType.GlobalUserState: {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor,
            restApiService: new RestApiService( m_options )
        );
        
        await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
    }
    
    [Benchmark]
    public async Task WhisperMessage() {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );
    
        string baseMessage = $"@badges=staff/1,bits-charity/1;color=#8A2BE2;display-name=PetsgomOO;emotes=;message-id=306;thread-id=12345678_87654321;turbo=0;user-id=87654321;user-type=staff :petsgomoo!petsgomoo@petsgomoo.tmi.twitch.tv WHISPER foo :hello";
        IWebSocketConnection webSocket = new TestWebSocketConnection( iterations: Iterations, baseMessage: baseMessage );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );
    
        void MessageCallback(
            IMessage message
        ) {
            switch( message.MessageType ) {
                case MessageType.Whisper: {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor,
            restApiService: new RestApiService( m_options )
        );
        
        await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
    }
    
    [Benchmark]
    public async Task UserNoticeMessage() {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );
    
        string baseMessage = $"@badge-info=;badges=staff/1,broadcaster/1,turbo/1;color=#008000;display-name=ronni;emotes=;id=db25007f-7a18-43eb-9379-80131e44d633;login=ronni;mod=0;msg-id=resub;msg-param-cumulative-months=6;msg-param-streak-months=2;msg-param-should-share-streak=1;msg-param-sub-plan=Prime;msg-param-sub-plan-name=Prime;room-id=12345678;subscriber=1;system-msg=ronni\\shas\\ssubscribed\\sfor\\s6\\smonths!;tmi-sent-ts=1507246572675;turbo=1;user-id=87654321;user-type=staff :tmi.twitch.tv USERNOTICE #channel :";
        IWebSocketConnection webSocket = new TestWebSocketConnection( iterations: Iterations, baseMessage: baseMessage );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );
    
        void MessageCallback(
            IMessage message
        ) {
            switch( message.MessageType ) {
                case MessageType.UserNotice: {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor,
            restApiService: new RestApiService( m_options )
        );
        
        await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
    }
    
    [Benchmark]
    public async Task RoomStateMessage() {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );
    
        string baseMessage = $"@emote-only=0;followers-only=0;r9k=0;slow=0;subs-only=0 :tmi.twitch.tv ROOMSTATE #channel";
        IWebSocketConnection webSocket = new TestWebSocketConnection( iterations: Iterations, baseMessage: baseMessage );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );
    
        void MessageCallback(
            IMessage message
        ) {
            switch( message.MessageType ) {
                case MessageType.RoomState: {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor,
            restApiService: new RestApiService( m_options )
        );
        
        await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
    }
    
    [Benchmark]
    public async Task JoinMsg() {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );
    
        string baseMessage = $":ronni!ronni@ronni.tmi.twitch.tv JOIN #channel";
        IWebSocketConnection webSocket = new TestWebSocketConnection( iterations: Iterations, baseMessage: baseMessage );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );
    
        void MessageCallback(
            IMessage message
        ) {
            switch( message.MessageType ) {
                case MessageType.Join: {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor,
            restApiService: new RestApiService( m_options )
        );
        
        await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
    }
    
    [Benchmark]
    public async Task PartMsg() {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );
    
        string baseMessage = $":ronni!ronni@ronni.tmi.twitch.tv PART #channel";
        IWebSocketConnection webSocket = new TestWebSocketConnection( iterations: Iterations, baseMessage: baseMessage );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );
    
        void MessageCallback(
            IMessage message
        ) {
            switch( message.MessageType ) {
                case MessageType.Part: {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor,
            restApiService: new RestApiService( m_options )
        );
        
        await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
    }
    
    [Benchmark]
    public async Task HostTargetMsg() {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );
    
        const string baseMessage = ":tmi.twitch.tv HOSTTARGET #channel :xyz 10";
        IWebSocketConnection webSocket = new TestWebSocketConnection( iterations: Iterations, baseMessage: baseMessage );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );
    
        void MessageCallback(
            IMessage message
        ) {
            switch( message.MessageType ) {
                case MessageType.HostTarget: {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor,
            restApiService: new RestApiService( m_options )
        );
        
        await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
    }
    
    [Benchmark]
    public async Task ReconnectMsg() {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );
    
        const string baseMessage = ":tmi.twitch.tv RECONNECT";
        IWebSocketConnection webSocket = new TestWebSocketConnection( iterations: Iterations, baseMessage: baseMessage );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );
    
        void MessageCallback(
            IMessage message
        ) {
            switch( message.MessageType ) {
                case MessageType.Reconnect: {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor,
            restApiService: new RestApiService( m_options )
        );
        
        await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
    }
    
    [Benchmark]
    public async Task UserStateMsg() {
        Mock<IAuthorizer> authorizer = new( behavior: MockBehavior.Strict );
        Mock<IDateTimeService> dateTimeService = new( behavior: MockBehavior.Strict );
        dateTimeService.Setup( expression: dts => dts.GetStartTime() ).Returns( value: m_startTime );
    
        const string baseMessage = "@badge-info=;badges=staff/1;color=#0D4200;display-name=ronni;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;mod=1;subscriber=1;turbo=1;user-type=staff :tmi.twitch.tv USERSTATE #channel";
        IWebSocketConnection webSocket = new TestWebSocketConnection( iterations: Iterations, baseMessage: baseMessage );
        IMessageProcessor messageProcessor = new MessageProcessor( dateTimeService: dateTimeService.Object );
    
        void MessageCallback(
            IMessage message
        ) {
            switch( message.MessageType ) {
                case MessageType.UserState: {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor,
            restApiService: new RestApiService( m_options )
        );
        
        await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
    }
}
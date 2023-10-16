using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using Moq;
using TwitchEverywhere.Implementation;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.Benchmark; 

[MemoryDiagnoser]
[NativeMemoryProfiler]
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

    [Params(500)]
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
            Message message
        ) {
            switch( message.MessageType ) {
                case MessageType.PrivMsg: {
                    break;
                }
                case MessageType.ClearChat:
                case MessageType.ClearMsg:
                case MessageType.GlobalUserState:
                case MessageType.Notice:
                case MessageType.RoomState:
                case MessageType.UserNotice:
                case MessageType.UserState:
                case MessageType.Whisper:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
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
            Message message
        ) {
            switch( message.MessageType ) {
                case MessageType.ClearMsg: {
                    break;
                }
                case MessageType.PrivMsg:
                case MessageType.ClearChat:
                case MessageType.GlobalUserState:
                case MessageType.Notice:
                case MessageType.RoomState:
                case MessageType.UserNotice:
                case MessageType.UserState:
                case MessageType.Whisper:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
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
            Message message
        ) {
            switch( message.MessageType ) {
                case MessageType.ClearChat: {
                    break;
                }
                case MessageType.PrivMsg:
                case MessageType.ClearMsg:
                case MessageType.GlobalUserState:
                case MessageType.Notice:
                case MessageType.RoomState:
                case MessageType.UserNotice:
                case MessageType.UserState:
                case MessageType.Whisper:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
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
            Message message
        ) {
            switch( message.MessageType ) {
                case MessageType.Notice: {
                    break;
                }
                case MessageType.PrivMsg:
                case MessageType.ClearMsg:
                case MessageType.ClearChat:
                case MessageType.GlobalUserState:
                case MessageType.RoomState:
                case MessageType.UserNotice:
                case MessageType.UserState:
                case MessageType.Whisper:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
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
            Message message
        ) {
            switch( message.MessageType ) {
                case MessageType.GlobalUserState: {
                    break;
                }
                case MessageType.PrivMsg:
                case MessageType.ClearMsg:
                case MessageType.ClearChat:
                case MessageType.Notice:
                case MessageType.RoomState:
                case MessageType.UserNotice:
                case MessageType.UserState:
                case MessageType.Whisper:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        authorizer.Setup( expression: a => a.GetToken() ).ReturnsAsync( value: "token" );
        m_twitchConnector = new TwitchConnector( 
            authorizer: authorizer.Object, 
            webSocketConnection: webSocket,
            messageProcessor: messageProcessor
        );
        
        await m_twitchConnector.TryConnect( options: m_options, messageCallback: MessageCallback );
    }
}
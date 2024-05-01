using System.Net.WebSockets;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Irc.Rx;

public class TwitchConnectorRx {
    private readonly IMessageProcessor m_messageProcessor;
    
    private static readonly ClientWebSocket m_webSocketConnection = new();
    private readonly string m_channel;
    
    public TwitchConnectorRx(
        IMessageProcessor messageProcessor,
        string channel,
        TwitchConnectionOptions options,
        IrcClientObserver ircClientObserver
    ) {
        m_messageProcessor = messageProcessor;
        m_channel = channel;
        
        IObservable<byte[]> websocketObservable = Connect(
            channel,
            options
        );
        
        websocketObservable.Subscribe(
            x => {
                _ = ParseRx(
                    data: x,
                    channel: channel,
                    observer: ircClientObserver
                );
            }
        );
    }
    
    public async Task<bool> SendMessage(
        IMessage message
    ) {
        if (m_webSocketConnection.State != WebSocketState.Open) {
            return false;
        }
        
        switch (message.MessageType) {
            case MessageType.PrivMsg:
                Console.WriteLine(message.RawMessage);
                await SendMessage(message.RawMessage.Replace(
                    $":{m_channel}!{m_channel}@{m_channel}.tmi.twitch.tv", ""));
                break;
            case MessageType.ClearChat:
                await SendMessage($"CLEARCHAT #${m_channel}");
                break;
            case MessageType.ClearMsg:
                await SendMessage($"CLEARMSG ${m_channel}");
                break;
            case MessageType.GlobalUserState:
                await SendMessage($"GLOBALUSERSTATE ${m_channel}");
                break;
            case MessageType.Notice:
                await SendMessage($"NOTICE ${m_channel}");
                break;
            case MessageType.RoomState:
                await SendMessage($"ROOMSTATE ${m_channel}");
                break;
            case MessageType.UserNotice:
                await SendMessage($"USERNOTICE ${m_channel}");
                break;
            case MessageType.UserState:
                await SendMessage($"USERSTATE ${m_channel}");
                break;
            case MessageType.Whisper:
                await SendMessage($"WHISPER ${m_channel}");
                break;
            case MessageType.Join:
                await SendMessage($"JOIN ${m_channel}");
                break;
            case MessageType.Part:
                await SendMessage($"PART ${m_channel}");
                break;
            case MessageType.HostTarget:
                await SendMessage($"HOSTTARGET ${m_channel}");
                break;
            case MessageType.Reconnect:
                await SendMessage($"RECONNECT ${m_channel}");
                break;
            case MessageType.Unknown:
            case MessageType.CapReq:
            case MessageType.JoinCount:
            case MessageType.JoinEnd:
            case MessageType.Ping:
            default:
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(message.MessageType), 
                    actualValue: message.MessageType,
                    message: null
                );
        }
        
        return true;
    }
    
    public async Task<bool> Disconnect() {
        if (m_webSocketConnection.State == WebSocketState.Closed) {
            return true;
        }
        
        await SendMessage($"PART ${m_channel}");
        await m_webSocketConnection.CloseAsync(
            closeStatus: WebSocketCloseStatus.NormalClosure,
            statusDescription: "Disconnect requested",
            cancellationToken: CancellationToken.None
        );
        
        return true;
    }
    
    private IObservable<byte[]> Connect(
        string channel,
        TwitchConnectionOptions options
    ) {
        return Observable.Create<byte[]>(
            async observer => {
                await m_webSocketConnection.ConnectAsync(
                    uri: new Uri(uriString: "ws://irc-ws.chat.twitch.tv:80"),
                    cancellationToken: CancellationToken.None
                );
                
                byte[] buffer = new byte[1024 * 4];
                
                try {
                    await SendMessage(
                        message: "CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands"
                    );
                    await SendMessage(message: $"PASS oauth:{options.AccessToken}");
                    await SendMessage(message: $"NICK {options.ClientName}");
                    await SendMessage(message: $"JOIN #{channel}");
                    while (m_webSocketConnection.State == WebSocketState.Open) {
                        var result = await m_webSocketConnection.ReceiveAsync(
                            new ArraySegment<byte>(buffer),
                            CancellationToken.None
                        );
                        
                        if (result.MessageType == WebSocketMessageType.Close) {
                            await m_webSocketConnection.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty,
                                CancellationToken.None);
                            observer.OnCompleted();
                        } else {
                            observer.OnNext(buffer);
                        }
                    }
                }
                catch (Exception ex) {
                    observer.OnError(ex);
                }
            }
        );
    }
    
    private async Task ParseRx(
        byte[] data,
        string channel,
        IrcClientObserver observer
    ) {
        RawMessage message = new(data);
        
        if (message.Type == MessageType.Ping) {
            await SendMessage("PONG :tmi.twitch.tv");
        } else {
            m_messageProcessor.ProcessMessageRx(
                response: message,
                channel: channel,
                observer: observer
            );
        }
    }
    
    private async Task SendMessage(
        string message
    ) {
        await m_webSocketConnection.SendAsync(
            buffer: Encoding.ASCII.GetBytes(message),
            messageType: WebSocketMessageType.Text,
            endOfMessage: true,
            cancellationToken: CancellationToken.None
        );
    }
}
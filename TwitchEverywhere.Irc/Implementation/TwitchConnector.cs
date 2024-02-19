using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc.Implementation;

internal sealed class TwitchConnector(
    IAuthorizer authorizer,
    IMessageProcessor messageProcessor
) : ITwitchConnector {
    private TwitchConnectionOptions m_options;
    private readonly ClientWebSocket m_webSocketConnection = new();

    async Task<bool> ITwitchConnector.TryConnect(
        TwitchConnectionOptions options,
        Action<IMessage> messageCallback
    ) {
        string token = await authorizer.GetToken();

        m_options = options;

        bool result = await ConnectToWebsocket(
            token: token,
            callback: messageCallback
        );
        return result;
    }

    async Task<IrcClientObservable> ITwitchConnector.TryConnectRx(
        TwitchConnectionOptions options
    ) {
        m_options = options;
        string token = options.AccessToken;

        return await ConnectToWebsocketRx(token);
    }

    async Task<bool> ITwitchConnector.SendMessage(
        IMessage message,
        MessageType messageType
    ) {
        if (m_webSocketConnection.State != WebSocketState.Open) {
            return false;
        }

        switch (messageType) {
            case MessageType.PrivMsg:
                Console.WriteLine(message.RawMessage);
                await SendMessage(message.RawMessage.Replace(
                    $":{m_options.Channel}!{m_options.Channel}@{m_options.Channel}.tmi.twitch.tv", ""));
                break;
            case MessageType.ClearChat:
                await SendMessage($"CLEARCHAT #${m_options.Channel}");
                break;
            case MessageType.ClearMsg:
                await SendMessage($"CLEARMSG ${m_options.Channel}");
                break;
            case MessageType.GlobalUserState:
                await SendMessage($"GLOBALUSERSTATE ${m_options.Channel}");
                break;
            case MessageType.Notice:
                await SendMessage($"NOTICE ${m_options.Channel}");
                break;
            case MessageType.RoomState:
                await SendMessage($"ROOMSTATE ${m_options.Channel}");
                break;
            case MessageType.UserNotice:
                await SendMessage($"USERNOTICE ${m_options.Channel}");
                break;
            case MessageType.UserState:
                await SendMessage($"USERSTATE ${m_options.Channel}");
                break;
            case MessageType.Whisper:
                await SendMessage($"WHISPER ${m_options.Channel}");
                break;
            case MessageType.Join:
                await SendMessage($"JOIN ${m_options.Channel}");
                break;
            case MessageType.Part:
                await SendMessage($"PART ${m_options.Channel}");
                break;
            case MessageType.HostTarget:
                await SendMessage($"HOSTTARGET ${m_options.Channel}");
                break;
            case MessageType.Reconnect:
                await SendMessage($"RECONNECT ${m_options.Channel}");
                break;
            case MessageType.Unknown:
            default:
                throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
        }

        return true;
    }

    async Task<bool> ITwitchConnector.Disconnect() {
        await SendMessage($"PART ${m_options.Channel}");
        await m_webSocketConnection.CloseAsync(
            closeStatus: WebSocketCloseStatus.NormalClosure,
            statusDescription: "Disconnect requested",
            cancellationToken: CancellationToken.None
        );

        return true;
    }

    private async Task<IrcClientObservable> ConnectToWebsocketRx(
        string token
    ) {

        var x = new Subject<ICapReq>();
        var y = new Subject<IJoinMsg>();
        IrcClientObservable observable = new(
            CapReqSubject: x,
            CapReqObservable: x.AsObservable(),
            ClearChatObservable: new Subject<IClearChatMsg>().AsObservable(),
            ClearMsgObservable: new Subject<IClearMsg>().AsObservable(),
            GlobalUserStateObservable: new Subject<IGlobalUserStateMsg>().AsObservable(),
            HostTargetObservable: new Subject<IHostTargetMsg>().AsObservable(),
            JoinCountObservable: new Subject<IJoinCountMsg>().AsObservable(),
            JoinEndObservable: new Subject<IJoinEndMsg>().AsObservable(),
            JoinSubject: y,
            JoinObservable: y.AsObservable(),
            NoticeObservable: new Subject<INoticeMsg>().AsObservable(),
            PartObservable: new Subject<IPartMsg>().AsObservable(),
            PrivMsgObservable: new Subject<IPrivMsg>().AsObservable(),
            ReconnectObservable: new Subject<IReconnectMsg>().AsObservable(),
            RoomStateObservable: new Subject<IRoomStateMsg>().AsObservable(),
            UnknownObservable: new Subject<IUnknownMsg>().AsObservable(),
            UserNoticeObservable: new Subject<IUserNoticeMsg>().AsObservable(),
            UserStateObservable: new Subject<IUserStateMsg>().AsObservable(),
            WhisperObservable: new Subject<IWhisperMsg>().AsObservable()
        );
        
        try {
            await m_webSocketConnection.ConnectAsync(
                uri: new Uri(uriString: "ws://irc-ws.chat.twitch.tv:80"),
                cancellationToken: CancellationToken.None
            );
            byte[] buffer = new byte[4096];

            await SendMessage(
                message: "CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands"
            );
            await SendMessage(message: $"PASS oauth:{token}");
            await SendMessage(message: $"NICK {m_options.ClientName}");
            await SendMessage(message: $"JOIN #{m_options.Channel}");

            while (m_webSocketConnection.State == WebSocketState.Open) {
                await ReceiveWebSocketResponseRx(
                    buffer: buffer,
                    observer: observable
                );
            }
        }
        catch (Exception e) {
            Console.Error.WriteLine(e);
        }
        finally {
            if (m_webSocketConnection.State == WebSocketState.Open) {
                await m_webSocketConnection.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing",
                    CancellationToken.None);
            }
        }

        return observable;
    }

    private async Task ReceiveWebSocketResponseRx(
        byte[] buffer,
        IrcClientObservable observer
    ) {
        WebSocketReceiveResult result = await m_webSocketConnection.ReceiveAsync(
            buffer: buffer,
            cancellationToken: CancellationToken.None
        );

        if (result.MessageType == WebSocketMessageType.Close) {
            await m_webSocketConnection.CloseAsync(
                closeStatus: WebSocketCloseStatus.NormalClosure,
                statusDescription: null,
                cancellationToken: CancellationToken.None
            );
        } else {
            ParseRx(data: buffer, observer: observer);
        }
    }

    private async Task ParseRx(
        byte[] data,
        IrcClientObservable observer
    ) {
        RawMessage message = new(data);

        if (message.Type == MessageType.Ping) {
            await SendMessage("PONG :tmi.twitch.tv");
        } else {
            messageProcessor.ProcessMessageRx(
                response: message,
                channel: m_options.Channel,
                observer: observer
            );
        }
    }

    private async Task<bool> ConnectToWebsocket(
        string token,
        Action<IMessage> callback
    ) {
        try {
            await m_webSocketConnection.ConnectAsync(
                uri: new Uri(uriString: "ws://irc-ws.chat.twitch.tv:80"),
                cancellationToken: CancellationToken.None
            );
            byte[] buffer = new byte[4096];

            await SendMessage(
                message: "CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands"
            );
            await SendMessage(message: $"PASS oauth:{token}");
            await SendMessage(message: $"NICK {m_options.ClientName}");
            await SendMessage(message: $"JOIN #{m_options.Channel}");

            while (m_webSocketConnection.State == WebSocketState.Open) {
                await ReceiveWebSocketResponse(
                    buffer: buffer,
                    callback: callback
                );
            }

            return true;
        }
        catch (Exception e) {
            Console.Error.WriteLine(e);
            return false;
        }
        finally {
            if (m_webSocketConnection.State == WebSocketState.Open) {
                await m_webSocketConnection.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing",
                    CancellationToken.None);
            }
        }
    }

    private async Task ReceiveWebSocketResponse(
        byte[] buffer,
        Action<IMessage> callback
    ) {
        WebSocketReceiveResult result = await m_webSocketConnection.ReceiveAsync(
            buffer: buffer,
            cancellationToken: CancellationToken.None
        );

        if (result.MessageType == WebSocketMessageType.Close) {
            await m_webSocketConnection.CloseAsync(
                closeStatus: WebSocketCloseStatus.NormalClosure,
                statusDescription: null,
                cancellationToken: CancellationToken.None
            );
        } else {
            Parse(data: buffer, callback: callback);
        }
    }

    private async Task Parse(
        ReadOnlyMemory<byte> data,
        Action<IMessage> callback
    ) {
        RawMessage message = new(data);

        if (message.Type == MessageType.Ping) {
            await SendMessage("PONG :tmi.twitch.tv");
        } else {
            messageProcessor.ProcessMessage(
                response: message,
                channel: m_options.Channel,
                callback: callback
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
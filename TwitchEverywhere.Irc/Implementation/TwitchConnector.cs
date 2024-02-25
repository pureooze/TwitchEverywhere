using System.Net.WebSockets;
using System.Reactive;
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

    IrcClientObservable ITwitchConnector.TryConnectRx(
        TwitchConnectionOptions options
    ) {
        m_options = options;
        string token = options.AccessToken;

        IrcClientSubject subjects = new(
            CapReqSubject: new Subject<ICapReq>(),
            ClearChatSubject: new Subject<IClearChatMsg>(),
            ClearMsgSubject: new Subject<IClearMsg>(),
            GlobalUserStateSubject: new Subject<IGlobalUserStateMsg>(),
            HostTargetSubject: new Subject<IHostTargetMsg>(),
            JoinCountSubject: new Subject<IJoinCountMsg>(),
            JoinEndSubject: new Subject<IJoinEndMsg>(),
            JoinSubject: new Subject<IJoinMsg>(),
            NoticeSubject: new Subject<INoticeMsg>(),
            PartSubject: new Subject<IPartMsg>(),
            PrivMsgSubject: new Subject<IPrivMsg>(),
            ReconnectSubject: new Subject<IReconnectMsg>(),
            RoomStateSubject: new Subject<IRoomStateMsg>(),
            UnknownSubject: new Subject<IUnknownMsg>(),
            UserNoticeSubject: new Subject<IUserNoticeMsg>(),
            UserStateSubject: new Subject<IUserStateMsg>(),
            WhisperSubject: new Subject<IWhisperMsg>()
        );
        
        IrcClientObservable observable = new(
            CapReqObservable: subjects.CapReqSubject.AsObservable(),
            ClearChatObservable: subjects.ClearChatSubject.AsObservable(),
            ClearMsgObservable: subjects.ClearMsgSubject.AsObservable(),
            GlobalUserStateObservable: subjects.GlobalUserStateSubject.AsObservable(),
            HostTargetObservable: subjects.HostTargetSubject.AsObservable(),
            JoinCountObservable: subjects.JoinCountSubject.AsObservable(),
            JoinEndObservable: subjects.JoinEndSubject.AsObservable(),
            JoinObservable: subjects.JoinSubject.AsObservable(),
            NoticeObservable: subjects.NoticeSubject.AsObservable(),
            PartObservable: subjects.PartSubject.AsObservable(),
            PrivMsgObservable: subjects.PrivMsgSubject.AsObservable(),
            ReconnectObservable: subjects.ReconnectSubject.AsObservable(),
            RoomStateObservable: subjects.RoomStateSubject.AsObservable(),
            UnknownObservable: subjects.UnknownSubject.AsObservable(),
            UserNoticeObservable: subjects.UserNoticeSubject.AsObservable(),
            UserStateObservable: subjects.UserStateSubject.AsObservable(),
            WhisperObservable: subjects.WhisperSubject.AsObservable()
        );
        
        ConnectToWebsocketRx(token, subjects);
        return observable;
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

    private async Task ConnectToWebsocketRx(
        string token,
        IrcClientSubject subjects
    ) {
        await m_webSocketConnection.ConnectAsync(
            uri: new Uri(uriString: "ws://irc-ws.chat.twitch.tv:80"),
            cancellationToken: CancellationToken.None
        );
        
        byte[] buffer = new byte[4096];
        
        try {

            await SendMessage(
                message: "CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands"
            );
            await SendMessage(message: $"PASS oauth:{token}");
            await SendMessage(message: $"NICK {m_options.ClientName}");
            await SendMessage(message: $"JOIN #{m_options.Channel}");

            while (m_webSocketConnection.State == WebSocketState.Open) {
                await ReceiveWebSocketResponseRx(
                    buffer: buffer,
                    subjects: subjects
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
    }

    private async Task ReceiveWebSocketResponseRx(
        byte[] buffer,
        IrcClientSubject subjects
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
            ParseRx(data: buffer, subjects: subjects);
        }
    }

    private async Task ParseRx(
        byte[] data,
        IrcClientSubject subjects
    ) {
        RawMessage message = new(data);

        if (message.Type == MessageType.Ping) {
            await SendMessage("PONG :tmi.twitch.tv");
        } else {
            messageProcessor.ProcessMessageRx(
                response: message,
                channel: m_options.Channel,
                subjects: subjects
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
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
    IMessageProcessor messageProcessor,
    TwitchConnectionOptions options
) : ITwitchConnector {
    
    private readonly static ClientWebSocket m_webSocketConnection = new();
    private IrcClientObservable? m_observable;
    private IrcClientSubject? m_subjects;
    private string m_channel;
    
    IrcClientObservable ITwitchConnector.TryConnectRx(
        string channel
    ) {
        m_channel = channel;
        string token = options.AccessToken;

        InitializeObservables();
        
        if (m_subjects == null) {
            throw new Exception("Subjects not initialized");
        }
        
        if (m_observable == null) {
            throw new Exception("Observable not initialized");
        }
        
        ConnectToWebsocketRx(token, m_subjects);
        return m_observable;
    }
    private void InitializeObservables() {

        m_subjects ??= new IrcClientSubject(
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
        
        m_observable ??= new IrcClientObservable(
            CapReqObservable: m_subjects.CapReqSubject.AsObservable(),
            ClearChatObservable: m_subjects.ClearChatSubject.AsObservable(),
            ClearMsgObservable: m_subjects.ClearMsgSubject.AsObservable(),
            GlobalUserStateObservable: m_subjects.GlobalUserStateSubject.AsObservable(),
            HostTargetObservable: m_subjects.HostTargetSubject.AsObservable(),
            JoinCountObservable: m_subjects.JoinCountSubject.AsObservable(),
            JoinEndObservable: m_subjects.JoinEndSubject.AsObservable(),
            JoinObservable: m_subjects.JoinSubject.AsObservable(),
            NoticeObservable: m_subjects.NoticeSubject.AsObservable(),
            PartObservable: m_subjects.PartSubject.AsObservable(),
            PrivMsgObservable: m_subjects.PrivMsgSubject.AsObservable(),
            ReconnectObservable: m_subjects.ReconnectSubject.AsObservable(),
            RoomStateObservable: m_subjects.RoomStateSubject.AsObservable(),
            UnknownObservable: m_subjects.UnknownSubject.AsObservable(),
            UserNoticeObservable: m_subjects.UserNoticeSubject.AsObservable(),
            UserStateObservable: m_subjects.UserStateSubject.AsObservable(),
            WhisperObservable: m_subjects.WhisperSubject.AsObservable()
        );
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
            default:
                throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
        }

        return true;
    }

    async Task<bool> ITwitchConnector.Disconnect() {
        
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
            await SendMessage(message: $"NICK {options.ClientName}");
            await SendMessage(message: $"JOIN #{m_channel}");

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
                channel: m_channel,
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
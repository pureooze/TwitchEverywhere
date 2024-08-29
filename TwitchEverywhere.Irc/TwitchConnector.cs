using System.Net.WebSockets;
using System.Text;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;

namespace TwitchEverywhere.Irc;

public class TwitchConnector {
    private readonly IMessageProcessor _messageProcessor;
    private static readonly ClientWebSocket WebSocketConnection = new();

    public TwitchConnector(
        IMessageProcessor messageProcessor,
        TwitchConnectionOptions options,
        string channel,
        Action<IMessage> messageCallback
    ) {
        _messageProcessor = messageProcessor;

        if (string.IsNullOrEmpty(options.AccessToken)) {
            throw new Exception("Connection Error: Access token is not defined");
        }
        
        _ = ConnectToWebsocket(
            options: options,
            callback: messageCallback,
            channel: channel
        );
    }
    
    public async Task<bool> SendMessage(
        IMessage message,
        MessageType messageType,
        string channel,
        TwitchConnectionOptions options
    ) {
        if (WebSocketConnection.State != WebSocketState.Open) {
            return false;
        }

        switch (messageType) {
            case MessageType.PrivMsg:
                Console.WriteLine(message.RawMessage);
                await SendMessage(message.RawMessage.Replace(
                    $":{channel}!{channel}@{channel}.tmi.twitch.tv", ""));
                break;
            case MessageType.ClearChat:
                await SendMessage($"CLEARCHAT #${channel}");
                break;
            case MessageType.ClearMsg:
                await SendMessage($"CLEARMSG ${channel}");
                break;
            case MessageType.GlobalUserState:
                await SendMessage($"GLOBALUSERSTATE ${channel}");
                break;
            case MessageType.Notice:
                await SendMessage($"NOTICE ${channel}");
                break;
            case MessageType.RoomState:
                await SendMessage($"ROOMSTATE ${channel}");
                break;
            case MessageType.UserNotice:
                await SendMessage($"USERNOTICE ${channel}");
                break;
            case MessageType.UserState:
                await SendMessage($"USERSTATE ${channel}");
                break;
            case MessageType.Whisper:
                await SendMessage($"WHISPER ${channel}");
                break;
            case MessageType.Join:
                await SendMessage($"JOIN ${channel}");
                break;
            case MessageType.Part:
                await SendMessage($"PART ${channel}");
                break;
            case MessageType.HostTarget:
                await SendMessage($"HOSTTARGET ${channel}");
                break;
            case MessageType.Reconnect:
                await SendMessage($"RECONNECT ${channel}");
                break;
            case MessageType.Unknown:
            default:
                throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
        }

        return true;
    }
    
    public async Task<bool> Disconnect(
        string channel    
    ) {
        await SendMessage($"PART ${channel}");
        await WebSocketConnection.CloseAsync(
            closeStatus: WebSocketCloseStatus.NormalClosure,
            statusDescription: "Disconnect requested",
            cancellationToken: CancellationToken.None
        );

        return true;
    }
    
    private async Task ConnectToWebsocket(
        TwitchConnectionOptions options,
        string channel,
        Action<IMessage> callback
    ) {
        try {
            await WebSocketConnection.ConnectAsync(
                uri: new Uri(uriString: "ws://irc-ws.chat.twitch.tv:80"),
                cancellationToken: CancellationToken.None
            );
            byte[] buffer = new byte[4096];

            await SendMessage(
                message: "CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands"
            );
            await SendMessage(message: $"PASS oauth:{options.AccessToken}");
            await SendMessage(message: $"NICK {options.ClientName}");
            await SendMessage(message: $"JOIN #{channel}");

            while (WebSocketConnection.State == WebSocketState.Open) {
                await ReceiveWebSocketResponse(
                    buffer: buffer,
                    callback: callback,
                    channel: channel
                );
            }
        }
        catch (Exception e) {
            Console.Error.WriteLine(e);
        }
        finally {
            if (WebSocketConnection.State == WebSocketState.Open) {
                await WebSocketConnection.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing",
                    CancellationToken.None);
            }
        }
    }

    private async Task ReceiveWebSocketResponse(
        byte[] buffer,
        Action<IMessage> callback,
        string channel
    ) {
        WebSocketReceiveResult result = await WebSocketConnection.ReceiveAsync(
            buffer: buffer,
            cancellationToken: CancellationToken.None
        );

        if (result.MessageType == WebSocketMessageType.Close) {
            await WebSocketConnection.CloseAsync(
                closeStatus: WebSocketCloseStatus.NormalClosure,
                statusDescription: null,
                cancellationToken: CancellationToken.None
            );
        } else {
            Parse(
                data: buffer, 
                callback: callback,
                channel: channel
            );
        }
    }

    private async Task Parse(
        ReadOnlyMemory<byte> data,
        Action<IMessage> callback,
        string channel
    ) {
        RawMessage message = new(data);

        if (message.Type == MessageType.Ping) {
            await SendMessage("PONG :tmi.twitch.tv");
        } else {
            _messageProcessor.ProcessMessage(
                response: message,
                channel: channel,
                callback: callback
            );
        }
    }
    
    private async Task SendMessage(
        string message
    ) {
        await WebSocketConnection.SendAsync(
            buffer: Encoding.ASCII.GetBytes(message),
            messageType: WebSocketMessageType.Text,
            endOfMessage: true,
            cancellationToken: CancellationToken.None
        );
    }
}
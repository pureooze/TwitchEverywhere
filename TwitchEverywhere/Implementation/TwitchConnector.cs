using System.Net.WebSockets;
using System.Text;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.Implementation;

internal sealed class TwitchConnector : ITwitchConnector {
    private readonly IAuthorizer m_authorizer;
    private readonly IWebSocketConnection m_webSocketConnection;
    private readonly IMessageProcessor m_messageProcessor;
    private TwitchConnectionOptions m_options;


    public TwitchConnector(
        IAuthorizer authorizer,
        IWebSocketConnection webSocketConnection,
        IMessageProcessor messageProcessor
    ) {
        m_authorizer = authorizer;
        m_webSocketConnection = webSocketConnection;
        m_messageProcessor = messageProcessor;
    }
    
    async Task<bool> ITwitchConnector.TryConnect( 
        TwitchConnectionOptions options, 
        Action<Message> messageCallback
    ) {
        string token = await m_authorizer.GetToken();

        m_options = options;

        bool result = await ConnectToWebsocket( 
            ws: m_webSocketConnection, 
            token: token, 
            callback: messageCallback 
        );
        return result;
    }

    async Task<bool> ITwitchConnector.SendMessage(
        string message,
        MessageType messageType
    ) {
        if( m_webSocketConnection.State != WebSocketState.Open ) {
            return false;
        }

        switch( messageType ) {
            case MessageType.PrivMsg:
                await SendMessage( m_webSocketConnection, $"PRIVMSG #{m_options.Channel} :{message}" );
                break;
            case MessageType.ClearChat:
                await SendMessage( m_webSocketConnection, $"CLEARCHAT #${m_options.Channel}" );
                break;
            case MessageType.ClearMsg:
                await SendMessage( m_webSocketConnection, $"CLEARMSG ${m_options.Channel}" );
                break;
            case MessageType.GlobalUserState:
                await SendMessage( m_webSocketConnection, $"GLOBALUSERSTATE ${m_options.Channel}" );
                break;
            case MessageType.Notice:
                await SendMessage( m_webSocketConnection, $"NOTICE ${m_options.Channel}" );
                break;
            case MessageType.RoomState:
                await SendMessage( m_webSocketConnection, $"ROOMSTATE ${m_options.Channel}" );
                break;
            case MessageType.UserNotice:
                await SendMessage( m_webSocketConnection, $"USERNOTICE ${m_options.Channel}" );
                break;
            case MessageType.UserState:
                await SendMessage( m_webSocketConnection, $"USERSTATE ${m_options.Channel}" );
                break;
            case MessageType.Whisper:
                await SendMessage( m_webSocketConnection, $"WHISPER ${m_options.Channel}" );
                break;
            case MessageType.Join:
                await SendMessage( m_webSocketConnection, $"JOIN ${m_options.Channel}" );
                break;
            case MessageType.Part:
                await SendMessage( m_webSocketConnection, $"PART ${m_options.Channel}" );
                break;
            case MessageType.HostTarget:
                await SendMessage( m_webSocketConnection, $"HOSTTARGET ${m_options.Channel}" );
                break;
            case MessageType.Reconnect:
                await SendMessage( m_webSocketConnection, $"RECONNECT ${m_options.Channel}" );
                break;
            case MessageType.Unknown:
            default:
                throw new ArgumentOutOfRangeException( nameof(messageType), messageType, null );
        }
        
        return true;
    }

    async Task<bool> ITwitchConnector.Disconnect() {
        await SendMessage( m_webSocketConnection, $"PART ${m_options.Channel}" );
        await m_webSocketConnection.CloseAsync( 
            closeStatus: WebSocketCloseStatus.NormalClosure, 
            statusDescription: "Disconnect requested", 
            cancellationToken: CancellationToken.None 
        );

        return true;
    }

    private async Task<bool> ConnectToWebsocket(
        IWebSocketConnection ws,
        string token,
        Action<Message> callback
    ) {
        await ws.ConnectAsync(
            uri: new Uri(uriString: "ws://irc-ws.chat.twitch.tv:80"), 
            cancellationToken: CancellationToken.None
        );
        byte[] buffer = new byte[4096];

        await SendMessage( 
            socketConnection: ws, 
            message: "CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands" 
        );
        await SendMessage( socketConnection: ws, message: $"PASS oauth:{token}" );
        await SendMessage( socketConnection: ws, message: $"NICK {m_options.ClientName}" );
        await SendMessage( socketConnection: ws, message: $"JOIN #{m_options.Channel}" );
        
        while (ws.State == WebSocketState.Open) {
            await ReceiveWebSocketResponse( 
                ws: ws, 
                buffer: buffer, 
                callback: callback 
            );
        }

        return true;
    }

    private async Task ReceiveWebSocketResponse(
        IWebSocketConnection ws,
        byte[] buffer,
        Action<Message> callback
    ) {
        WebSocketReceiveResult result = await ws.ReceiveAsync(
            buffer: buffer, 
            cancellationToken: CancellationToken.None
        );
            
        if ( result.MessageType == WebSocketMessageType.Close ) {
            await ws.CloseAsync(
                closeStatus: WebSocketCloseStatus.NormalClosure, 
                statusDescription: null, 
                cancellationToken: CancellationToken.None
            );
        } else {
            string twitchResponse = Encoding.ASCII.GetString( 
                bytes: buffer, 
                index: 0, 
                count: result.Count 
            );

            string[] responses = twitchResponse.Trim().Split( "\r\n" );
            foreach (string response in responses) {
                // keep alive, let twitch know we are still listening
                if( response.Contains( "PING :tmi.twitch.tv" ) ) {
                    await SendMessage( ws, "PONG :tmi.twitch.tv" );
                } else {
                    m_messageProcessor.ProcessMessage(
                        response: response,
                        channel: m_options.Channel, 
                        callback: callback
                    );
                }
            }
        }
    }

    private async static Task SendMessage(
        IWebSocketConnection socketConnection,
        string message
    ) {
        await socketConnection.SendAsync(
            buffer: Encoding.ASCII.GetBytes(message), 
            messageType: WebSocketMessageType.Text, 
            endOfMessage: true, 
            cancellationToken: CancellationToken.None
        );
    }
    
    
}

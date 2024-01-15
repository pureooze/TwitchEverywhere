using System.Net.WebSockets;
using System.Text;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Irc.Implementation;

internal sealed class TwitchConnector(
    IAuthorizer authorizer,
    IWebSocketConnection webSocketConnection,
    IMessageProcessor messageProcessor
) : ITwitchConnector {
    
    private TwitchConnectionOptions m_options;
    
    async Task<bool> ITwitchConnector.TryConnect( 
        TwitchConnectionOptions options, 
        Action<IMessage> messageCallback
    ) {
        string token = await authorizer.GetToken();

        m_options = options;

        bool result = await ConnectToWebsocket( 
            ws: webSocketConnection, 
            token: token, 
            callback: messageCallback 
        );
        return result;
    }

    async Task<bool> ITwitchConnector.SendMessage(
        IMessage message,
        MessageType messageType
    ) {
        if( webSocketConnection.State != WebSocketState.Open ) {
            return false;
        }

        switch( messageType ) {
            case MessageType.PrivMsg:
                // Console.WriteLine( $"@reply-parent-msg-id={lazyLoadedPrivMsg.ReplyParentMsgId} PRIVMSG #{m_options.Channel} :{lazyLoadedPrivMsg.Text}" );
                Console.WriteLine( message.RawMessage );
                await SendMessage( webSocketConnection, message.RawMessage.Replace( $":{m_options.Channel}!{m_options.Channel}@{m_options.Channel}.tmi.twitch.tv", "" ) );
                break;
            case MessageType.ClearChat:
                await SendMessage( webSocketConnection, $"CLEARCHAT #${m_options.Channel}" );
                break;
            case MessageType.ClearMsg:
                await SendMessage( webSocketConnection, $"CLEARMSG ${m_options.Channel}" );
                break;
            case MessageType.GlobalUserState:
                await SendMessage( webSocketConnection, $"GLOBALUSERSTATE ${m_options.Channel}" );
                break;
            case MessageType.Notice:
                await SendMessage( webSocketConnection, $"NOTICE ${m_options.Channel}" );
                break;
            case MessageType.RoomState:
                await SendMessage( webSocketConnection, $"ROOMSTATE ${m_options.Channel}" );
                break;
            case MessageType.UserNotice:
                await SendMessage( webSocketConnection, $"USERNOTICE ${m_options.Channel}" );
                break;
            case MessageType.UserState:
                await SendMessage( webSocketConnection, $"USERSTATE ${m_options.Channel}" );
                break;
            case MessageType.Whisper:
                await SendMessage( webSocketConnection, $"WHISPER ${m_options.Channel}" );
                break;
            case MessageType.Join:
                await SendMessage( webSocketConnection, $"JOIN ${m_options.Channel}" );
                break;
            case MessageType.Part:
                await SendMessage( webSocketConnection, $"PART ${m_options.Channel}" );
                break;
            case MessageType.HostTarget:
                await SendMessage( webSocketConnection, $"HOSTTARGET ${m_options.Channel}" );
                break;
            case MessageType.Reconnect:
                await SendMessage( webSocketConnection, $"RECONNECT ${m_options.Channel}" );
                break;
            case MessageType.Unknown:
            default:
                throw new ArgumentOutOfRangeException( nameof(messageType), messageType, null );
        }
        
        return true;
    }

    async Task<bool> ITwitchConnector.Disconnect() {
        await SendMessage( webSocketConnection, $"PART ${m_options.Channel}" );
        await webSocketConnection.CloseAsync( 
            closeStatus: WebSocketCloseStatus.NormalClosure, 
            statusDescription: "Disconnect requested", 
            cancellationToken: CancellationToken.None 
        );

        return true;
    }
    Task<GetUsersResponse>? ITwitchConnector.GetUsers(
        IEnumerable<string> users
    ) {
        return null;
    }

    private async Task<bool> ConnectToWebsocket(
        IWebSocketConnection ws,
        string token,
        Action<IMessage> callback
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
        Action<IMessage> callback
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
            
            Parse( data: buffer, callback: callback );
            // string twitchResponse = Encoding.ASCII.GetString( 
            //     bytes: buffer, 
            //     index: 0, 
            //     count: result.Count 
            // );
            //
            // string[] responses = twitchResponse.Trim().Split( "\r\n" );
            // foreach (string response in responses) {
            //     // keep alive, let twitch know we are still listening
            //     if( response.Contains( "PING :tmi.twitch.tv" ) ) {
            //         await SendMessage( ws, "PONG :tmi.twitch.tv" );
            //     } else {
            //         messageProcessor.ProcessMessage(
            //             response: response,
            //             channel: m_options.Channel, 
            //             callback: callback
            //         );
            //     }
            // }
        }
    }
    
    private async Task Parse(ReadOnlyMemory<byte> data, Action<IMessage> callback) {
        RawMessage message = new(data);
        
        if( message.Type == MessageType.Ping ) {
            await SendMessage( webSocketConnection, "PONG :tmi.twitch.tv" );
        } else {
            messageProcessor.ProcessMessage(
                response: message,
                channel: m_options.Channel, 
                callback: callback
            );
        }
        
        
        // if (message.HasMoreMessages) {
        //     Parse(data[message.NextMessageStartIndex..], callback);
        // }
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

using System.Net.WebSockets;
using System.Text;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.Implementation;

internal sealed class TwitchConnector : ITwitchConnector {
    private readonly IAuthorizer m_authorizer;
    private readonly IWebSocketConnection m_webSocketConnection;
    private readonly IMessageProcessor m_messageProcessor;

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

        bool result = await ConnectToWebsocket( 
            ws: m_webSocketConnection, 
            token: token, 
            options: options, 
            callback: messageCallback 
        );
        return result;
    }

    private async Task<bool> ConnectToWebsocket(
        IWebSocketConnection ws,
        string token,
        TwitchConnectionOptions options,
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
        await SendMessage( socketConnection: ws, message: $"NICK {options.ClientName}" );
        await SendMessage( socketConnection: ws, message: $"JOIN #{options.Channel}" );
        
        while (ws.State == WebSocketState.Open) {
            await ReceiveWebSocketResponse( 
                ws: ws, 
                buffer: buffer, 
                options: options, 
                callback: callback 
            );
        }

        return true;
    }

    private async Task ReceiveWebSocketResponse(
        IWebSocketConnection ws,
        byte[] buffer,
        TwitchConnectionOptions options,
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
                        channel: options.Channel, 
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

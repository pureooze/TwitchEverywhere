using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;

namespace TwitchEverywhere.Implementation;

internal sealed partial class TwitchConnector : ITwitchConnector {
    internal bool IsConnected { get; private set; }

    async Task<bool> ITwitchConnector.Connect(
        TwitchConnectionOptions options
    ) {
        string token = GetToken();
        using ClientWebSocket ws = new();
        
        await ConnectToWebsocket( ws, token, options );
        return true;
    }

    private async Task ConnectToWebsocket(
        ClientWebSocket ws,
        string token,
        TwitchConnectionOptions options
    ) {
        
        await ws.ConnectAsync(
            uri: new Uri(uriString: "ws://irc-ws.chat.twitch.tv:80"), 
            cancellationToken: CancellationToken.None
        );
        byte[] buffer = new byte[512];

        await SendMessage( ws, "CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands" );
        Thread.Sleep(1000);
        await SendMessage( ws, $"PASS oauth:{token}" );
        await SendMessage( ws, "NICK chatreaderbot" );
        await SendMessage( ws, $"JOIN #{options.Channel}" );
        
        StringBuilder storedMessageBuffer = new();
        int messageCount = 0;
        
        Func<WebSocketReceiveResult, Task> HandleResponse = async result => {
            string response = Encoding.ASCII.GetString( 
                bytes: buffer, 
                index: 0, 
                count: result.Count 
            );

            // keep alive, let twitch know we are still listening
            if( response == "PING :tmi.twitch.tv" ) {
                await SendMessage( ws, "PONG :tmi.twitch.tv" );
            }

            if( response[0] == '@' ) {
                UserMessage? message = GetUserMessage( response, options.Channel );
                storedMessageBuffer.Append( message );
                messageCount++;
            } else {
                Console.WriteLine( response );
            }

            if( messageCount > 1 ) {
                WriteMessagesToStore( storedMessageBuffer );
                storedMessageBuffer.Clear();
                messageCount = 0;
            }
        };
        
        while (ws.State == WebSocketState.Open) {
            await RecieveWebSocketResponse( ws, buffer, HandleResponse );
        }
    }

    private async Task RecieveWebSocketResponse(
        ClientWebSocket ws,
        byte[] buffer,
        Func<WebSocketReceiveResult, Task> handleResponse
    ) {
        WebSocketReceiveResult result = await ws.ReceiveAsync(
            buffer: buffer, 
            cancellationToken: CancellationToken.None
        );
            
        if (result.MessageType == WebSocketMessageType.Close) {
            await ws.CloseAsync(
                closeStatus: WebSocketCloseStatus.NormalClosure, 
                statusDescription: null, 
                cancellationToken: CancellationToken.None
            );
        } else {
            await handleResponse( result );
        }
    }

    private void WriteMessagesToStore(
        StringBuilder buffer
    ) {
        string path = "test.txt";
        using StreamWriter writer = new( path, append: true );
        
        writer.WriteAsync( buffer );
    }

    private UserMessage? GetUserMessage( string response, string channel ) {
        string[] segments = response.Split( $"PRIVMSG #{channel} :" );
 
        string displayName = DisplayNamePattern()
            .Match( response )
            .Value
            .Split( "=" )[1]
            .TrimEnd( ';' );
        
        if( segments.Length > 1 ) {
            return new UserMessage( 
                DisplayName: displayName,
                Message: segments[1]
            );
        }

        return null;
    }

    private string GetToken() {
        return "ldlqqaf4mhj8kojuotvgxerp12f3v4";
    }

    private static async Task SendMessage(
        WebSocket socket,
        string message
    ) {
        await socket.SendAsync(
            buffer: Encoding.ASCII.GetBytes(message), 
            messageType: WebSocketMessageType.Text, 
            endOfMessage: true, 
            cancellationToken: CancellationToken.None
        );
    }

    [GeneratedRegex("display-name([^;]*);")]
    private static partial Regex DisplayNamePattern();
}
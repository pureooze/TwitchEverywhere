using System.IO.Compression;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;

namespace TwitchEverywhere.Implementation;

internal sealed partial class TwitchConnector : ITwitchConnector {
    private IAuthorizer m_authorizer = new Authorizer();
    private const int BUFFER_SIZE = 500;
    private DateTime m_startTimestamp;
    
    async Task<bool> ITwitchConnector.Connect(
        TwitchConnectionOptions options
    ) {
        string token = await GetToken();
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
        byte[] buffer = new byte[4096];

        await SendMessage( socket: ws, message: "CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands" );
        Thread.Sleep(millisecondsTimeout: 1000);
        await SendMessage( socket: ws, message: $"PASS oauth:{token}" );
        await SendMessage( socket: ws, message: "NICK chatreaderbot" );
        await SendMessage( socket: ws, message: $"JOIN #{options.Channel}" );

        MessageBuffer messageBuffer = new( buffer: new StringBuilder() );
        
        
        while (ws.State == WebSocketState.Open) {
            await ReceiveWebSocketResponse( ws: ws, buffer: buffer, options: options, messageBuffer: messageBuffer );
        }
    }
    private async Task ReceiveWebSocketResponse(
        ClientWebSocket ws,
        byte[] buffer,
        TwitchConnectionOptions options,
        MessageBuffer messageBuffer
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
            string response = Encoding.ASCII.GetString( 
                bytes: buffer, 
                index: 0, 
                count: result.Count 
            );
            
            Console.WriteLine( "response: " + response );

            // keep alive, let twitch know we are still listening
            if( response.Contains("PING :tmi.twitch.tv") ) {
                await SendMessage( ws, "PONG :tmi.twitch.tv" );
            }

            if( response.Contains( $" PRIVMSG #{options.Channel}" ) ) {
                string message = GetUserMessage( response, options.Channel );
                messageBuffer.AddToBuffer( message );
            }
            
            if( messageBuffer.Count > BUFFER_SIZE ) {
                StringBuilder tempBuffer = new( messageBuffer.ReadAsString() );
                messageBuffer.Clear();
                WriteMessagesToStore( tempBuffer );
            }
        }
    }

    private void WriteMessagesToStore(
        StringBuilder buffer
    ) {
        if( buffer.Length == 0 ) {
            return;
        }

        string rawData = buffer.ToString();
        byte[] byteBuffer = Encoding.UTF8.GetBytes( rawData );

        byte[] compressedData = CompressWithBrotli( byteBuffer );
        
        string path = $"{m_startTimestamp.ToUniversalTime().ToString( "yyyy-M-d_H-mm-ss" )}.br";
        SaveBinaryDataToFile( path, compressedData );
    }

    private void SaveBinaryDataToFile(
        string path,
        byte[] compressedData
    ) {
        using FileStream fileStream = new (path, FileMode.Create);
        fileStream.Write( compressedData, 0, compressedData.Length );
    }

    private static byte[] CompressWithBrotli(
        byte[] byteBuffer
    ) {
        using MemoryStream outputStream = new();
        using BrotliStream brotliStream = new( outputStream, CompressionMode.Compress );
        brotliStream.Write(byteBuffer, 0, byteBuffer.Length);
        brotliStream.Close(); // Close the BrotliStream to finalize compression
        return outputStream.ToArray();
    }

    private string GetUserMessage( string response, string channel ) {
        string[] segments = response.Split( $"PRIVMSG #{channel} :" );
 
        string displayName = DisplayNamePattern()
            .Match( response )
            .Value
            .Split( "=" )[1]
            .TrimEnd( ';' );

        long rawTimestamp = Convert.ToInt64(
            MessageTimestampPattern().Match( response ).Value
            .Split( "=" )[1]
            .TrimEnd( ';' )
        );

        m_startTimestamp = DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;
        
        if( segments.Length <= 1 ) {
            throw new UnexpectedUserMessageException();
        }

        return $"{displayName}, {segments[1]}, {m_startTimestamp}\n";
    }

    private async Task<string> GetToken() {
        return await m_authorizer.GetToken();
    }

    private static async Task SendMessage(
        WebSocket socket,
        string message
    ) {
        Console.WriteLine( "WRITING: " + message );
        await socket.SendAsync(
            buffer: Encoding.ASCII.GetBytes(message), 
            messageType: WebSocketMessageType.Text, 
            endOfMessage: true, 
            cancellationToken: CancellationToken.None
        );
    }

    [GeneratedRegex("display-name([^;]*);")]
    private static partial Regex DisplayNamePattern();
    
    [GeneratedRegex("tmi-sent-ts([^;]*);")]
    private static partial Regex MessageTimestampPattern();

    private sealed class MessageBuffer {
        public MessageBuffer(
            StringBuilder buffer
        ) {
            Buffer = buffer;
            Count = 0;
        }

        public void AddToBuffer(
            string message
        ) {
            Buffer.Append( message );
            Count += 1;
        }

        public void Clear() {
            Buffer.Clear();
            Count = 0;
        }

        private StringBuilder Buffer { get; }
        public int Count { get; private set; }

        public string ReadAsString() {
            return Buffer.ToString();
        }
    };
}
using System.IO.Compression;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System;

namespace TwitchEverywhere.Implementation;

internal sealed partial class TwitchConnector : ITwitchConnector {
    private readonly IAuthorizer m_authorizer;
    private readonly ICompressor m_compressor;
    private readonly int m_bufferSize;
    private readonly Action<string> m_messageCallback;
    private DateTime m_startTimestamp;
    private Action<string> m_callback;

    public TwitchConnector(
        IAuthorizer authorizer,
        ICompressor compressor,
        int bufferSize
    ) {
        m_authorizer = authorizer;
        m_compressor = compressor;
        m_bufferSize = bufferSize;
        m_callback = delegate(
            string s
        ) {
            Console.WriteLine( s );
        };
    }
    
    async Task<bool> ITwitchConnector.Connect( 
        TwitchConnectionOptions options, 
        Action<string> messageCallback
    ) {
        m_callback = messageCallback;
        string token = await m_authorizer.GetToken();
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

            // keep alive, let twitch know we are still listening
            if( response.Contains("PING :tmi.twitch.tv") ) {
                await SendMessage( ws, "PONG :tmi.twitch.tv" );
            }

            if( response.Contains( $" PRIVMSG #{options.Channel}" ) ) {
                string message = GetUserMessage( response, options.Channel );
                messageBuffer.AddToBuffer( message );
                m_callback( message );
            }
            
            if( messageBuffer.Count > m_bufferSize ) {
                StringBuilder tempBuffer = new( messageBuffer.ReadAsString() );
                messageBuffer.Clear();
                await WriteMessagesToStore( tempBuffer );
            }
        }
    }

    private async Task WriteMessagesToStore( StringBuilder buffer ) {
        if( buffer.Length == 0 ) {
            return;
        }

        string rawData = buffer.ToString();
        byte[] byteBuffer = Encoding.UTF8.GetBytes( rawData );

        byte[] compressedData = await m_compressor.Compress( byteBuffer );
        
        string path = $"{m_startTimestamp.ToUniversalTime().ToString( "yyyy-M-d_H-mm-ss" )}.csv";
        SaveBinaryDataToFile( path, compressedData );
    }

    private void SaveBinaryDataToFile(
        string path,
        byte[] compressedData
    ) {
        using FileStream fileStream = new (path, FileMode.Create);
        fileStream.Write( compressedData, 0, compressedData.Length );
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

        return $"{m_startTimestamp}, {displayName}, {segments[1]}\n";
    }

    private async Task SendMessage(
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
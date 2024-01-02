using System.Net.WebSockets;

namespace TwitchEverywhere.Irc; 

public interface IWebSocketConnection {
    Task ConnectAsync(
        Uri uri,
        CancellationToken cancellationToken
    );

    Task SendAsync(
        ArraySegment<byte> buffer,
        WebSocketMessageType messageType,
        bool endOfMessage,
        CancellationToken cancellationToken
    );

    Task<WebSocketReceiveResult> ReceiveAsync(
        ArraySegment<byte> buffer, 
        CancellationToken cancellationToken
    );

    Task CloseAsync(
        WebSocketCloseStatus closeStatus, 
        string? statusDescription, 
        CancellationToken cancellationToken
    );

    WebSocketState State { get; }
}
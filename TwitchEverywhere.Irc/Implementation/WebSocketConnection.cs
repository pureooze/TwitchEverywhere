using System.Net.WebSockets;

namespace TwitchEverywhere.Irc.Implementation; 

internal class WebSocketConnection : IWebSocketConnection {
    private readonly ClientWebSocket ws = new();

    async Task IWebSocketConnection.ConnectAsync(
        Uri uri,
        CancellationToken cancellationToken
    ) {
        await ws.ConnectAsync(
            uri, cancellationToken
        );
    }

    async Task IWebSocketConnection.SendAsync(
        ArraySegment<byte> buffer,
        WebSocketMessageType messageType,
        bool endOfMessage,
        CancellationToken cancellationToken
    ) {
        await ws.SendAsync( buffer, messageType, endOfMessage, cancellationToken );
    }

    async Task<WebSocketReceiveResult> IWebSocketConnection.ReceiveAsync(
        ArraySegment<byte> buffer,
        CancellationToken cancellationToken
    ) {
        return await ws.ReceiveAsync( buffer, cancellationToken );
    }

    async Task IWebSocketConnection.CloseAsync(
        WebSocketCloseStatus closeStatus,
        string? statusDescription,
        CancellationToken cancellationToken
    ) {
        await ws.CloseAsync( closeStatus, statusDescription, cancellationToken );
    }

    WebSocketState IWebSocketConnection.State => ws.State;
}
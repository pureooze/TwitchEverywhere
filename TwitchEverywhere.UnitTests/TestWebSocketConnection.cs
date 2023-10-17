using System.Collections.Immutable;
using System.Net.WebSockets;
using System.Text;

namespace TwitchEverywhere.UnitTests; 

internal class TestWebSocketConnection : IWebSocketConnection {
    private readonly IImmutableList<string> m_messages;
    private WebSocketState m_state;

    private int m_invocationCount = 0;

    public TestWebSocketConnection( IImmutableList<string> messages ) {
        m_messages = messages;
    }

    async Task IWebSocketConnection.ConnectAsync(
        Uri uri,
        CancellationToken cancellationToken
    ) {
        m_state = WebSocketState.Open;
        await Task.Delay( 10, cancellationToken );
    }

    async Task IWebSocketConnection.SendAsync(
        ArraySegment<byte> buffer,
        WebSocketMessageType messageType,
        bool endOfMessage,
        CancellationToken cancellationToken
    ) {
        await Task.Delay( 10, cancellationToken );
    }
    async Task<WebSocketReceiveResult> IWebSocketConnection.ReceiveAsync(
        ArraySegment<byte> buffer,
        CancellationToken cancellationToken
    ) {
        await Task.Delay( 10, cancellationToken );
        
        if( m_invocationCount >= m_messages.Count ) {
            m_state = WebSocketState.Closed;
        } else {
            byte[] messageBytes = Encoding.UTF8.GetBytes( m_messages.ElementAt( m_invocationCount ) );
            
            if( buffer.Array != null ) {
                Array.Copy( messageBytes, buffer.Array, messageBytes.Length );

                m_invocationCount += 1;
        
                return await Task.FromResult(
                    new WebSocketReceiveResult(
                        count: messageBytes.Length, // Number of bytes received
                        messageType: WebSocketMessageType.Text, // Message type
                        endOfMessage: true
                    )
                );
            }
        }
        
        return await Task.FromResult(
            new WebSocketReceiveResult(
                count: 0, // Number of bytes received
                messageType: WebSocketMessageType.Close, // Message type
                endOfMessage: true
            )
        );
    }

    async Task IWebSocketConnection.CloseAsync(
        WebSocketCloseStatus closeStatus,
        string? statusDescription,
        CancellationToken cancellationToken
    ) {
        await Task.Delay( 10, cancellationToken );
    }

    WebSocketState IWebSocketConnection.State => m_state;
}
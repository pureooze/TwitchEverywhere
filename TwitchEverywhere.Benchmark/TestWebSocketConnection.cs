using System.Net.WebSockets;
using System.Text;

namespace TwitchEverywhere.Benchmark; 

internal class TestWebSocketConnection : IWebSocketConnection {
    private readonly int m_iterations;
    private readonly string m_baseMessage;
    private WebSocketState m_state;

    private int m_invocationCount;
    
    private IEnumerable<string> TestData() {
        while ( m_invocationCount < m_iterations ) {
            m_invocationCount += 1;
            yield return $"{m_baseMessage} {m_invocationCount}";
        }
    }
    
    public TestWebSocketConnection( int iterations, string baseMessage ) {
        m_iterations = iterations;
        m_baseMessage = baseMessage;
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

        using IEnumerator<string> data = TestData().GetEnumerator();
        
        data.MoveNext();
        
        if( string.IsNullOrEmpty( data.Current ) ) {
            m_state = WebSocketState.Closed;
        } else {
            byte[] messageBytes = Encoding.UTF8.GetBytes( data.Current );
            
            if( buffer.Array != null ) {
                Array.Copy( 
                    sourceArray: messageBytes, 
                    destinationArray: buffer.Array, 
                    length: messageBytes.Length 
                );
                
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
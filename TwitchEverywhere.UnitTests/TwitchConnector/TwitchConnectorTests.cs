using System.Net.WebSockets;
using System.Text;
using Moq;
using TwitchEverywhere.Implementation;

namespace TwitchEverywhere.UnitTests.TwitchConnector;

[TestFixture]
public class TwitchConnectorTests {
    private ITwitchConnector m_twitchConnector;
    private Mock<IAuthorizer> m_authorizer;
    private Mock<IWebSocketConnection> m_webSocketConnection;
    
    [SetUp]
    public void Setup() {
        m_authorizer = new Mock<IAuthorizer>( MockBehavior.Strict );
        m_webSocketConnection = new Mock<IWebSocketConnection>( MockBehavior.Strict );

        m_authorizer
            .Setup( auth => auth.GetToken() )
            .ReturnsAsync( "hello" );

        m_webSocketConnection
            .Setup( 
                ws => 
                    ws.ConnectAsync(
                        new Uri("ws://irc-ws.chat.twitch.tv:80"), 
                        CancellationToken.None
                    )
            ).Returns( Task.CompletedTask );
        
        m_webSocketConnection
            .Setup( 
                ws => 
                    ws.SendAsync(
                        It.IsAny<ArraySegment<byte>>(),
                        WebSocketMessageType.Text, 
                        true, 
                        CancellationToken.None
                    )
            ).Returns( Task.CompletedTask );

        m_webSocketConnection.Setup( ws => ws.State )
            .Returns( WebSocketState.Open );
        
        m_twitchConnector = new Implementation.TwitchConnector( 
            authorizer: m_authorizer.Object,
            webSocketConnection: m_webSocketConnection.Object
        );
    }

    [Test]
    public async Task ValidOptions_Connect_ReturnsTrue() {
        bool result = await m_twitchConnector.TryConnect( m_options, m_callback );
        Assert.That( result, Is.True );
    }
    
    private readonly TwitchConnectionOptions m_options = new(
        Channel: "TestConnection",
        AccessToken: "accessToken",
        RefreshToken: "refreshToken",
        ClientId: "clientId",
        ClientSecret: "clientSecret",
        BufferSize: 20
    );
    
    private readonly Action<string> m_callback = delegate(
        string s
    ) {
        Assert.That( s, Is.Not.Null );
    };
}
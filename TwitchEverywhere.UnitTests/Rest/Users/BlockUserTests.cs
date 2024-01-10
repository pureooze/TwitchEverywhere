using System.Net;
using Moq;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types.RestApi.Users;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;
using TwitchEverywhere.Rest;
using TwitchEverywhere.Rest.Implementation;

namespace TwitchEverywhere.UnitTests.Rest.Users;

[TestFixture]
public class BlockUserTests {
    private TwitchConnectionOptions m_options;
    private readonly Mock<IHttpClientWrapper> m_httpClient = new( MockBehavior.Strict );

    [SetUp]
    public void Setup() {
        m_options = new TwitchConnectionOptions(
            Channel: "test",
            AccessToken: "test",
            RefreshToken: "test",
            ClientId: "test",
            ClientSecret: "test",
            ClientName: "test"
        );
    }

    [Test]
    public async Task NoUserProvided_BlockUser_NoResult() {
        // Arrange
        m_httpClient.Setup(
                hc =>
                    hc.SendAsync(
                        It.IsAny<HttpRequestMessage>(),
                        It.IsAny<HttpCompletionOption>()
                    )
            )
            .ReturnsAsync(
                new HttpResponseMessage( HttpStatusCode.BadRequest ) {
                    Content = new StringContent(
                        """
                        {
                          "error": "Bad Request",
                          "status": 400,
                          "message": "Missing required parameter \"target_user_id\""
                        }
                        """
                    )
                }
            );

        IUsersApiService usersApiService = new UsersApiService( m_options );

        // Act
        HttpStatusCode result = await usersApiService.BlockUser(
            m_httpClient.Object,
            "",
            null,
            null
        );

        // Assert
        Assert.That( result, Is.EqualTo( HttpStatusCode.BadRequest ) );
    }
    
    [Test]
    public async Task ValidUserWithSourceContextAndReason_BlockUser_SuccessResult() {
        // Arrange
        m_httpClient.Setup(
                hc =>
                    hc.SendAsync(
                        It.IsAny<HttpRequestMessage>(),
                        It.IsAny<HttpCompletionOption>()
                    )
            )
            .ReturnsAsync(
                new HttpResponseMessage( HttpStatusCode.NoContent )
            );

        IUsersApiService usersApiService = new UsersApiService( m_options );

        // Act
        HttpStatusCode result = await usersApiService.BlockUser(
            m_httpClient.Object,
            "1234",
            SourceContext.Whisper.ToString().ToLower(),
            Reason.Other.ToString().ToLower()
        );

        // Assert
        Assert.That( result, Is.EqualTo( HttpStatusCode.NoContent ) );
    }
}
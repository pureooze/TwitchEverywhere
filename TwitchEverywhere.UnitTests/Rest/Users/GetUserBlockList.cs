using System.Net;
using Moq;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types.RestApi.Users;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;
using TwitchEverywhere.Rest;
using TwitchEverywhere.Rest.Implementation;

namespace TwitchEverywhere.UnitTests.Rest.Users;

[TestFixture]
public class GetUserBlockList {
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
    public async Task NoUserProvided_GetUserBlockList_NoResults() {
        // Arrange
        m_httpClient.Setup(
                hc =>
                    hc.SendAsync(
                        It.IsAny<HttpRequestMessage>(),
                        It.IsAny<HttpCompletionOption>()
                    )
            )
            .ReturnsAsync(
                new HttpResponseMessage( HttpStatusCode.OK ) {
                    Content = new StringContent( @"{""data"":[]}" )
                }
            );

        IUsersApiService usersApiService = new UsersApiService( m_options );

        // Act
        GetUserBlockListResponse result = await usersApiService.GetUserBlockList(
            m_httpClient.Object,
            ""
        );

        // Assert
        Assert.That( result, Is.Not.Null );

        Assert.Multiple(
            () => {
                Assert.That( result.StatusCode, Is.EqualTo( HttpStatusCode.OK ) );
                Assert.That( result.ApiResponse.Data, Is.Empty );
            }
        );
    }

    [Test]
    public async Task UserProvided_GetUserBlockList_ResultForUser() {
        // Arrange
        m_httpClient.Setup(
                hc =>
                    hc.SendAsync(
                        It.IsAny<HttpRequestMessage>(),
                        It.IsAny<HttpCompletionOption>()
                    )
            )
            .ReturnsAsync(
                new HttpResponseMessage( HttpStatusCode.OK ) {
                    Content = new StringContent(
                        """
                         {
                             "data":
                                 [
                                     {
                                      "user_id": "509728397",
                                      "user_login": "someuser",
                                      "display_name": "User"
                                    }
                                 ]
                         }
                        """
                    )
                }
            );

        IUsersApiService usersApiService = new UsersApiService( m_options );

        // Act
        GetUserBlockListResponse result = await usersApiService.GetUserBlockList(
            m_httpClient.Object,
            "12345"
        );

        // Assert
        Assert.That( result, Is.Not.Null );

        Assert.Multiple(
            () => {
                Assert.That( result.StatusCode, Is.EqualTo( HttpStatusCode.OK ) );
                Assert.That( result.ApiResponse.Data, Has.Length.EqualTo( 1 ) );

                BlockListEntry blockListEntry = result.ApiResponse.Data[0];
                Assert.That( blockListEntry.UserId, Is.EqualTo( "509728397" ) );
                Assert.That( blockListEntry.UserLogin, Is.EqualTo( "someuser" ) );
                Assert.That( blockListEntry.DisplayName, Is.EqualTo( "User" ) );

            }
        );
    }

    [Test]
    public async Task UserProvidedWithMoreResultsPaging_GetUserBlockList_ResultForUser() {
        // Arrange
        m_httpClient.Setup(
                hc =>
                    hc.SendAsync(
                        It.IsAny<HttpRequestMessage>(),
                        It.IsAny<HttpCompletionOption>()
                    )
            )
            .ReturnsAsync(
                new HttpResponseMessage( HttpStatusCode.OK ) {
                    Content = new StringContent(
                        """
                         {
                             "data":
                                 [
                                     {
                                      "user_id": "509728397",
                                      "user_login": "someuser",
                                      "display_name": "User"
                                    }
                                 ],
                             "pagination": {
                                 "cursor": "g87ae1ea66"
                             }
                         }
                        """
                    )
                }
            );

        IUsersApiService usersApiService = new UsersApiService( m_options );

        // Act
        GetUserBlockListResponse result = await usersApiService.GetUserBlockList(
            m_httpClient.Object,
            "12345"
        );

        // Assert
        Assert.That( result, Is.Not.Null );

        Assert.Multiple(
            () => {
                Assert.That( result.StatusCode, Is.EqualTo( HttpStatusCode.OK ) );
                Assert.That( result.ApiResponse.Data, Has.Length.EqualTo( 1 ) );

                BlockListEntry blockListEntry = result.ApiResponse.Data[0];
                Assert.That( blockListEntry.UserId, Is.EqualTo( "509728397" ) );
                Assert.That( blockListEntry.UserLogin, Is.EqualTo( "someuser" ) );
                Assert.That( blockListEntry.DisplayName, Is.EqualTo( "User" ) );

            }
        );
    }
}
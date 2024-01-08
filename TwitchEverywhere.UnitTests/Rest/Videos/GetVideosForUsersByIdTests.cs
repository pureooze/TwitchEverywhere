using System.Net;
using Moq;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types.RestApi.Videos;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;
using TwitchEverywhere.Rest;
using TwitchEverywhere.Rest.Implementation;

namespace TwitchEverywhere.UnitTests.Rest.Videos;

[TestFixture]
public class GetVideosForUsersByIdTests {
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
    public async Task NewDescriptionProvided_UpdateUser_DescriptionUpdated() {
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
                                          "id": "240866033",
                                          "stream_id": null,
                                          "user_id": "141981764",
                                          "user_login": "twitchdev",
                                          "user_name": "TwitchDev",
                                          "title": "Twitch Developers 101",
                                          "description": "Welcome to Twitch development!",
                                          "created_at": "2018-11-14T21:30:18Z",
                                          "published_at": "2018-11-14T22:04:30Z",
                                          "url": "https://www.twitch.tv/videos/335921245",
                                          "thumbnail_url": "https://static-cdn.jtvnw.net/cf_vods/d2nvs31859zcd8/twitchdev/335921245/ce0f3a7f-57a3-4152-bc06-0c6610189fb3/thumb/index-0000000000-%{width}x%{height}.jpg",
                                          "viewable": "public",
                                          "view_count": 1863062,
                                          "language": "en",
                                          "type": "upload",
                                          "duration": "3m21s",
                                          "muted_segments": [
                                            {
                                              "duration": 30,
                                              "offset": 120
                                            }
                                          ]
                                        }
                                 ]
                         }
                        """
                    )
                }
            );

        IVideosApiService videoApiService = new VideosApiService( m_options );

        // Act
        GetVideosResponse result = await videoApiService.GetVideosForUsersById(
            m_httpClient.Object,
            "240866033"
        );

        // Assert
        Assert.That( result, Is.Not.Null );

        Assert.Multiple(
            () => {
                Assert.That( result.StatusCode, Is.EqualTo( HttpStatusCode.OK ) );
                Assert.That( result.ApiResponse.Data, Has.Length.EqualTo( 1 ) );

                VideoEntry userEntry = result.ApiResponse.Data[0];
                Assert.That( userEntry.Id, Is.EqualTo( "240866033" ) );
                Assert.That( userEntry.StreamId, Is.EqualTo( null ) );
                Assert.That( userEntry.UserId, Is.EqualTo( "141981764" ) );
                Assert.That( userEntry.UserLogin, Is.EqualTo( "twitchdev" ) );
                Assert.That( userEntry.UserName, Is.EqualTo( "TwitchDev" ) );
                Assert.That( userEntry.UserId, Is.EqualTo( "141981764" ) );
                Assert.That( userEntry.Title, Is.EqualTo( "Twitch Developers 101" ) );
                Assert.That( userEntry.Description, Is.EqualTo( "Welcome to Twitch development!" ) );
                Assert.That( userEntry.CreatedAt, Is.EqualTo( new DateTime( 2018, 11, 14, 21, 30, 18 ) ) );
                Assert.That( userEntry.PublishedAt, Is.EqualTo( new DateTime( 2018, 11, 14, 22, 04, 30 ) ) );
                Assert.That( userEntry.Url, Is.EqualTo( "https://www.twitch.tv/videos/335921245" ) );
                Assert.That( userEntry.ThumbnailUrl, Is.EqualTo( "https://static-cdn.jtvnw.net/cf_vods/d2nvs31859zcd8/twitchdev/335921245/ce0f3a7f-57a3-4152-bc06-0c6610189fb3/thumb/index-0000000000-%{width}x%{height}.jpg" ) );
                Assert.That( userEntry.Viewable, Is.EqualTo( "public" ) );
                Assert.That( userEntry.ViewCount, Is.EqualTo( 1863062 ) );
                Assert.That( userEntry.Language, Is.EqualTo( "en" ) );
                Assert.That( userEntry.Type, Is.EqualTo( VideoEntryType.Upload ) );
                Assert.That( userEntry.Duration, Is.EqualTo( "3m21s" ) );
                
                Assert.That( userEntry.MutedSegment, Is.Not.Null );
                Assert.That( userEntry.MutedSegment?[0].Duration, Is.EqualTo( 30 ) );
                Assert.That( userEntry.MutedSegment?[0].Offset, Is.EqualTo( 120 ) );
            }
        );
    }
}
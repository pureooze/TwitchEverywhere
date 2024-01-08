using System.Net;
using Moq;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types.RestApi.Users;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;
using TwitchEverywhere.Rest;
using TwitchEverywhere.Rest.Implementation;

namespace TwitchEverywhere.UnitTests.Rest.Users;

[TestFixture]
public class GetUsersByIdTests {
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
    public async Task NoUsersProvided_GetUsers_NoResults() {
        // Arrange
        m_httpClient.Setup( hc => 
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
        GetUsersResponse result = await usersApiService.GetUsersById( 
            m_httpClient.Object, 
            Array.Empty<string>() 
        );
        
        // Assert
        Assert.That( result, Is.Not.Null );
        
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.ApiResponse.Data, Is.Empty);
        });
    }
    
    [Test]
    public async Task UserProvided_GetUsers_ResultForUser() {
        // Arrange
        m_httpClient.Setup( hc => 
            hc.SendAsync( 
                It.IsAny<HttpRequestMessage>(), 
                It.IsAny<HttpCompletionOption>() 
            ) 
        )
        .ReturnsAsync( 
            new HttpResponseMessage( HttpStatusCode.OK ) { 
                Content = new StringContent( """
                 {
                     "data":
                         [
                             {
                               "id": "240866033",
                               "login": "user",
                               "display_name": "User",
                               "type": "",
                               "broadcaster_type": "",
                               "description": "This is my new description from TwitchEverywhere",
                               "profile_image_url": "https://static-cdn.jtvnw.net/jtv_user_pictures/1f2dd13d-99b2-4605-8e90-bf48894ce1a0-profile_image-300x300.png",
                               "offline_image_url": "",
                               "view_count": "0",
                               "created_at": "2018-07-22T00:55:24Z"
                             }
                         ]
                 }
                """ )
            } 
        );
        
        IUsersApiService usersApiService = new UsersApiService( m_options );
        
        // Act
        GetUsersResponse result = await usersApiService.GetUsersById( 
            m_httpClient.Object, 
            ["240866033"]
        );
        
        // Assert
        Assert.That( result, Is.Not.Null );
        
        Assert.Multiple(() => {
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.ApiResponse.Data, Has.Length.EqualTo(1));

            UserEntry userEntry = result.ApiResponse.Data[0];
            Assert.That( userEntry.Id, Is.EqualTo( "240866033" ) );
            Assert.That( userEntry.Login, Is.EqualTo( "user" ) );
            Assert.That( userEntry.DisplayName, Is.EqualTo( "User" ) );
            Assert.That( userEntry.Type, Is.EqualTo( "" ) );
            Assert.That( userEntry.BroadcasterType, Is.EqualTo( "" ) );
            Assert.That( userEntry.Description, Is.EqualTo( "This is my new description from TwitchEverywhere" ) );
            Assert.That( userEntry.ProfileImageUrl, Is.EqualTo( "https://static-cdn.jtvnw.net/jtv_user_pictures/1f2dd13d-99b2-4605-8e90-bf48894ce1a0-profile_image-300x300.png" ) );
            Assert.That( userEntry.OfflineImageUrl, Is.EqualTo( "" ) );
            Assert.That( userEntry.ViewCount, Is.EqualTo( 0 ) );
            Assert.That( userEntry.CreatedAt, Is.EqualTo( new DateTime( 2018, 7, 22, 0, 55, 24 ) ) );

        });
    }
    
    [Test]
    public async Task MultipleUsersProvided_GetUsers_ResultForUsers() {
        // Arrange
        m_httpClient.Setup( hc => 
            hc.SendAsync( 
                It.IsAny<HttpRequestMessage>(), 
                It.IsAny<HttpCompletionOption>() 
            ) 
        )
        .ReturnsAsync( 
            new HttpResponseMessage( HttpStatusCode.OK ) { 
                Content = new StringContent( """
                 {
                     "data":
                         [
                             {
                               "id": "240866033",
                               "login": "user",
                               "display_name": "User",
                               "type": "",
                               "broadcaster_type": "",
                               "description": "This is my new description from TwitchEverywhere",
                               "profile_image_url": "https://static-cdn.jtvnw.net/jtv_user_pictures/1f2dd13d-99b2-4605-8e90-bf48894ce1a0-profile_image-300x300.png",
                               "offline_image_url": "",
                               "view_count": "0",
                               "created_at": "2018-07-22T00:55:24Z"
                             },
                             {
                               "id": "140866000",
                               "login": "anotheruser",
                               "display_name": "AnotherUser",
                               "type": "",
                               "broadcaster_type": "",
                               "description": "I'm the other user",
                               "profile_image_url": "",
                               "offline_image_url": "",
                               "view_count": "0",
                               "created_at": "2010-01-21T14:00:00Z"
                            }
                         ]
                 }
                """ )
            } 
        );
        
        IUsersApiService usersApiService = new UsersApiService( m_options );
        
        // Act
        GetUsersResponse result = await usersApiService.GetUsersById( 
            m_httpClient.Object, 
            ["240866033", "140866000"]
        );
        
        // Assert
        Assert.That( result, Is.Not.Null );
        
        Assert.Multiple(() => {
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.ApiResponse.Data, Has.Length.EqualTo(2));

            UserEntry firstUserEntry = result.ApiResponse.Data[0];
            UserEntry secondUserEntry = result.ApiResponse.Data[1];
            
            Assert.That( firstUserEntry.Id, Is.EqualTo( "240866033" ) );
            Assert.That( firstUserEntry.Login, Is.EqualTo( "user" ) );
            Assert.That( firstUserEntry.DisplayName, Is.EqualTo( "User" ) );
            Assert.That( firstUserEntry.Type, Is.EqualTo( "" ) );
            Assert.That( firstUserEntry.BroadcasterType, Is.EqualTo( "" ) );
            Assert.That( firstUserEntry.Description, Is.EqualTo( "This is my new description from TwitchEverywhere" ) );
            Assert.That( firstUserEntry.ProfileImageUrl, Is.EqualTo( "https://static-cdn.jtvnw.net/jtv_user_pictures/1f2dd13d-99b2-4605-8e90-bf48894ce1a0-profile_image-300x300.png" ) );
            Assert.That( firstUserEntry.OfflineImageUrl, Is.EqualTo( "" ) );
            Assert.That( firstUserEntry.ViewCount, Is.EqualTo( 0 ) );
            Assert.That( firstUserEntry.CreatedAt, Is.EqualTo( new DateTime( 2018, 7, 22, 0, 55, 24 ) ) );

            Assert.That( secondUserEntry.Id, Is.EqualTo( "140866000" ) );
            Assert.That( secondUserEntry.Login, Is.EqualTo( "anotheruser" ) );
            Assert.That( secondUserEntry.DisplayName, Is.EqualTo( "AnotherUser" ) );
            Assert.That( secondUserEntry.Type, Is.EqualTo( "" ) );
            Assert.That( secondUserEntry.BroadcasterType, Is.EqualTo( "" ) );
            Assert.That( secondUserEntry.Description, Is.EqualTo( "I'm the other user" ) );
            Assert.That( secondUserEntry.ProfileImageUrl, Is.EqualTo( "" ) );
            Assert.That( secondUserEntry.OfflineImageUrl, Is.EqualTo( "" ) );
            Assert.That( secondUserEntry.ViewCount, Is.EqualTo( 0 ) );
            Assert.That( secondUserEntry.CreatedAt, Is.EqualTo( new DateTime( 2010, 1, 21, 14, 0, 0 ) ) );

        });
    }
}
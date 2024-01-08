using System.Net;
using Moq;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types.RestApi.Users;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;
using TwitchEverywhere.Rest;
using TwitchEverywhere.Rest.Implementation;

namespace TwitchEverywhere.UnitTests.Rest.Users;

[TestFixture]
public class UpdateUserTests {
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
        string expectedDescription = "test, new description";

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
                               "description": "test, new description",
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
        GetUsersResponse result = await usersApiService.UpdateUser(
            m_httpClient.Object, 
            expectedDescription
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
            Assert.That( userEntry.Description, Is.EqualTo( expectedDescription ) );
            Assert.That( userEntry.ProfileImageUrl, Is.EqualTo( "https://static-cdn.jtvnw.net/jtv_user_pictures/1f2dd13d-99b2-4605-8e90-bf48894ce1a0-profile_image-300x300.png" ) );
            Assert.That( userEntry.OfflineImageUrl, Is.EqualTo( "" ) );
            Assert.That( userEntry.ViewCount, Is.EqualTo( 0 ) );
            Assert.That( userEntry.CreatedAt, Is.EqualTo( new DateTime( 2018, 7, 22, 0, 55, 24 ) ) );

        });
    }
}
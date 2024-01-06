using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest.Implementation;

public class RestApiService( TwitchConnectionOptions option ) : IRestApiService {
    
    private readonly IHttpClientWrapper m_httpClientWrapper = new HttpClientWrapper();
    private readonly IUsersApiService m_usersApiService = new GetUsers( option );
    private readonly IVideosApiService m_videosApiService = new VideosApiService( option );

    async Task<GetUsersResponse> IRestApiService.GetUsers(
        string[] userIds
    ) {
        return await m_usersApiService.GetUsers(
            httpClient: m_httpClientWrapper,
            userIds: userIds
        );
    }
    
    async Task<GetUsersResponse> IRestApiService.UpdateUser(
        string description
    ) {
        return await m_usersApiService.UpdateUser(
            httpClient: m_httpClientWrapper,
            description: description
        );
    }

    async Task<GetVideosResponse> IRestApiService.GetVideos(
        string userId
    ) {
        return await m_videosApiService.GetVideos(
            httpClient: m_httpClientWrapper,
            userId: userId
        );
    }
}
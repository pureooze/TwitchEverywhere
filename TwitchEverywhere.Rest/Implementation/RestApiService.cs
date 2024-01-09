using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest.Implementation;

public class RestApiService( TwitchConnectionOptions option ) : IRestApiService {
    
    private readonly IHttpClientWrapper m_httpClientWrapper = new HttpClientWrapper();
    private readonly IUsersApiService m_usersApiService = new UsersApiService( option );
    private readonly IVideosApiService m_videosApiService = new VideosApiService( option );

    async Task<GetUsersResponse> IRestApiService.GetUsersById(
        string[] userIds
    ) {
        return await m_usersApiService.GetUsersById(
            httpClient: m_httpClientWrapper,
            userIds: userIds
        );
    }
    
    async Task<GetUsersResponse> IRestApiService.GetUsersByLogin(
        string[] logins
    ) {
        return await m_usersApiService.GetUsersByLogin(
            httpClient: m_httpClientWrapper,
            logins: logins
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
    
    async Task<GetUserBlockListResponse> IRestApiService.GetUserBlockList(
        string broadcasterId,
        int? first,
        string? after
    ) {
        return await m_usersApiService.GetUserBlockList(
            httpClient: m_httpClientWrapper,
            broadcasterId, 
            first, 
            after
        );
    }

    async Task<GetVideosResponse> IRestApiService.GetVideosForUsersById(
        string userId
    ) {
        return await m_videosApiService.GetVideosForUsersById(
            httpClient: m_httpClientWrapper,
            userId: userId
        );
    }
}
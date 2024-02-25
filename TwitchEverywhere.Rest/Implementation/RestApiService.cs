using System.Net;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types.RestApi.Users;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest.Implementation;

public class RestApiService( TwitchConnectionOptions option ) : IRestApiService {
    
    private readonly IHttpClientWrapper m_httpClientWrapper = new HttpClientWrapper();
    private readonly IUsersApiService m_usersApiService = new UsersApiService( option );
    private readonly IVideosApiService m_videosApiService = new VideosApiService( option );
    private readonly IChannelApiService m_channelApiService = new ChannelApiService( option );
    private readonly IStreamApiService m_streamsApiService = new StreamApiService( option );

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
            broadcasterId: broadcasterId, 
            first: first, 
            after: after
        );
    }
    async Task<HttpStatusCode> IRestApiService.BlockUser(
        string targetUserId,
        SourceContext? sourceContext,
        Reason? reason
    ) {
        return await m_usersApiService.BlockUser(
            httpClient: m_httpClientWrapper,
            targetUserId, 
            sourceContext.ToString()?.ToLower(), 
            reason.ToString()?.ToLower()
        );
    }
    async Task<GetChannelInfoResponse> IRestApiService.GetChannelInfo(
        string broadcasterId
    ) {
        return await m_channelApiService.GetChannelInfo(
            httpClient: m_httpClientWrapper,
            broadcasterId: broadcasterId
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
    
    async Task<GetStreamsResponse> IRestApiService.GetStreams(
        string[] logins
    ) {
        return await m_streamsApiService.GetStreams(
            httpClientWrapper: m_httpClientWrapper,
            logins: logins
        );
    }
}
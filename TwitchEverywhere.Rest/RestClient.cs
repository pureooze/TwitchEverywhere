using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;
using TwitchEverywhere.Rest.Implementation;

namespace TwitchEverywhere.Rest;

public class RestClient( TwitchConnectionOptions options ) {
    private readonly IRestApiService m_restService = new RestApiService( options );

    public async Task<GetUsersResponse> GetUsers(
        IEnumerable<string> users
    ) {
        return await m_restService.GetUsers( users.ToArray() );
    }
    
    public async Task<GetUsersResponse> UpdateUser(
        string description
    ) {
        return await m_restService.UpdateUser( description );
    }
    
    public async Task<GetVideosResponse> GetVideos(
        string userId
    ) {
        return await m_restService.GetVideos( userId );
    }
}
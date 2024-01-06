using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Rest.Implementation;

namespace TwitchEverywhere.Rest;

public class RestClient {
    private readonly IRestApiService m_restService;

    public RestClient(
        TwitchConnectionOptions options
    ) {
        IAuthorizer authorizer = new Authorizer(
            accessToken: options.AccessToken ?? "",
            refreshToken: options.RefreshToken ?? "",
            clientId: options.ClientId ?? "",
            clientSecret: options.ClientSecret ?? ""
        );

        m_restService = new RestApiService( options );
    }
    
    public async Task<GetUsersResponse> GetUsers(
        IEnumerable<string> users
    ) {
        return await m_restService.GetUsers( users.ToArray() );
    }
    
    public async Task<GetVideosResponse> GetVideos(
        IEnumerable<string> users
    ) {
        return await m_restService.GetVideos( users.ToArray() );
    }
}
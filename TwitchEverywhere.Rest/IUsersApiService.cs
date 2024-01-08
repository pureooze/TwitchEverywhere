using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest;

public interface IUsersApiService {
    Task<GetUsersResponse> GetUsersById( 
        IHttpClientWrapper httpClient, 
        string[] userIds
    );
    
    Task<GetUsersResponse> GetUsersByLogin( 
        IHttpClientWrapper httpClient, 
        string[] logins
    );
    
    Task<GetUsersResponse> UpdateUser( IHttpClientWrapper httpClient, string description );
}
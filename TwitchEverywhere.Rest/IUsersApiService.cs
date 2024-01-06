using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest;

public interface IUsersApiService {
    Task<GetUsersResponse> GetUsers( IHttpClientWrapper httpClient, string[] userIds );
    Task<GetUsersResponse> UpdateUser( IHttpClientWrapper httpClient, string description );
}
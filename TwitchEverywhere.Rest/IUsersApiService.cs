using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest;

public interface IUsersApiService {
    Task<GetUsersResponse> GetUsers( HttpClient httpClient, string[] userIds );
    Task<GetUsersResponse> UpdateUser( HttpClient httpClient, string description );
}
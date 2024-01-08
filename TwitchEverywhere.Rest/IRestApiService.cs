using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest;

public interface IRestApiService {
    Task<GetUsersResponse> GetUsersById(
        string[] userIds
    );
    
    Task<GetUsersResponse> GetUsersByLogin(
        string[] logins
    );
    
    Task<GetUsersResponse> UpdateUser(
        string description
    );
    
    Task<GetVideosResponse> GetVideosForUsersById(
        string userId
    );
}
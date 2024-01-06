using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest;

public interface IRestApiService {
    Task<GetUsersResponse> GetUsers(
        string[] userIds
    );
    
    Task<GetUsersResponse> UpdateUser(
        string description
    );
    
    Task<GetVideosResponse> GetVideos(
        string userId
    );
}
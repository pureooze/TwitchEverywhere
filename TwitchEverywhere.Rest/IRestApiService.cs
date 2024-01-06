using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest;

public interface IRestApiService {
    Task<GetUsersResponse> GetUsers(
        string[] userIds
    );
    
    Task<GetVideosResponse> GetVideos(
        string userId
    );
}
using TwitchEverywhere.Core.Types;

namespace TwitchEverywhere.Rest;

public interface IRestApiService {
    Task<GetUsersResponse> GetUsers(
        string[] userIds
    );
}
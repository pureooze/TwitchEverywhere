using TwitchEverywhere.Types;

namespace TwitchEverywhere;

public interface IRestApiService {
    Task<GetUsersResponse> GetUsers(
        string[] userIds
    );
}
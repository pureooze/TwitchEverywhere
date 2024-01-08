using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest;

public interface IVideosApiService {
    Task<GetVideosResponse> GetVideosForUsersById( IHttpClientWrapper httpClient, string userId );
}
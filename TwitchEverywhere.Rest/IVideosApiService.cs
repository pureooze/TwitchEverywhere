using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest;

public interface IVideosApiService {
    Task<GetVideosResponse> GetVideos( IHttpClientWrapper httpClient, string userId );
}
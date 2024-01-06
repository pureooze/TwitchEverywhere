using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest;

public interface IVideosApiService {
    Task<GetVideosResponse> GetVideos( HttpClient httpClient, string userId );
}
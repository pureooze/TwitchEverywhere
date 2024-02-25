using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest;

public interface IStreamApiService {
    Task<GetStreamsResponse> GetStreams(
        IHttpClientWrapper httpClientWrapper,
        string[] logins
    );
}
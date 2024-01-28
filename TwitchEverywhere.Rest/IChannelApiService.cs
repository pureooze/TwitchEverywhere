using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest;

public interface IChannelApiService {
    Task<GetChannelInfoResponse> GetChannelInfo( IHttpClientWrapper httpClient, string broadcasterId );
}
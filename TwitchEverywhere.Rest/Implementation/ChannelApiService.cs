using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types.RestApi.Channel;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest.Implementation;

public class ChannelApiService( 
    TwitchConnectionOptions option 
) : IChannelApiService {

    async Task<GetChannelInfoResponse> IChannelApiService.GetChannelInfo(
        IHttpClientWrapper httpClientWrapper,
        string broadcasterId
    ) {
        using HttpRequestMessage request = new (
            method: HttpMethod.Get,
            requestUri: $"{Globals.HelixPrefix}/channels?broadcaster_id={broadcasterId}"
        );
        
        request.Headers.Add( 
            name: "Client-ID", value: option.ClientId
        );
        
        request.Headers.Add(
            name: "Authorization", value: "Bearer " + option.AccessToken 
        );
        
        using HttpResponseMessage response = await httpClientWrapper.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        JsonSerializerOptions options = new() {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            PropertyNameCaseInsensitive = true
        };
        
        GetChannelInfoApiResponse? serializedResponse = await response.Content.ReadFromJsonAsync<GetChannelInfoApiResponse>( options );
        
        return new GetChannelInfoResponse(
            ApiResponse: serializedResponse ?? new GetChannelInfoApiResponse( Array.Empty<ChannelInfoEntry>() ),
            StatusCode: response.StatusCode
        );
    }
}
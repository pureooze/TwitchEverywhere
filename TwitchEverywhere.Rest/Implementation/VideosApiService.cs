using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types.RestApi.Videos;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest.Implementation;

public class VideosApiService( 
    TwitchConnectionOptions option 
) : IVideosApiService {

    async Task<GetVideosResponse> IVideosApiService.GetVideosForUsersById(
        IHttpClientWrapper httpClientWrapper,
        string userId
    ) {
        using HttpRequestMessage request = new (
            method: HttpMethod.Get,
            requestUri: $"{Globals.HelixPrefix}/videos?user_id={userId}"
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
        
        GetVideosApiResponse? serializedResponse = await response.Content.ReadFromJsonAsync<GetVideosApiResponse>( options );
        
        return new GetVideosResponse(
            ApiResponse: serializedResponse ?? new GetVideosApiResponse( Array.Empty<VideoEntry>() ),
            StatusCode: response.StatusCode
        );
    }
}
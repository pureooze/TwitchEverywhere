using System.Net.Http.Json;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types.RestApi.Streams;
using TwitchEverywhere.Core.Types.RestApi.Videos;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest.Implementation;

public class StreamApiService( 
    TwitchConnectionOptions option 
) : IStreamApiService {
    
    async Task<GetStreamsResponse> IStreamApiService.GetStreams(
        IHttpClientWrapper httpClientWrapper,
        string[] logins
    ) {
        using HttpRequestMessage request = new (
            method: HttpMethod.Get,
            requestUri: $"{Globals.HelixPrefix}/streams?user_login={string.Join("&user_login=", logins)}"
        );
        
        request.Headers.Add( 
            name: "Client-ID", value: option.ClientId
        );
        
        request.Headers.Add(
            name: "Authorization", value: "Bearer " + option.AccessToken 
        );
        
        using HttpResponseMessage response = await httpClientWrapper.SendAsync(
            request, 
            HttpCompletionOption.ResponseHeadersRead
        );
        
        GetStreamsApiResponse? serializedResponse = await response.Content.ReadFromJsonAsync<GetStreamsApiResponse>();
        
        return new GetStreamsResponse(
            ApiResponse: serializedResponse ?? new GetStreamsApiResponse( Array.Empty<StreamEntry>(), new Pagination("") ),
            StatusCode: response.StatusCode
        );
    }
}
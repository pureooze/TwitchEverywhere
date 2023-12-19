using System.Net.Http.Json;
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.RestApi;

namespace TwitchEverywhere.Implementation;

public class RestApiService( 
    TwitchConnectionOptions option 
) : IRestApiService {
    
    private readonly HttpClient m_httpClient = new(){ Timeout = TimeSpan.FromSeconds(30) };

    async Task<GetUsersResponse> IRestApiService.GetUsers(
        string[] userIds
    ) {

        using HttpRequestMessage request = new (
            method: HttpMethod.Get,
            requestUri: $"https://api.twitch.tv/helix/users?login={string.Join("&login=", userIds)}"
        );
        
        request.Headers.Add( 
            name: "Client-ID", value: option.ClientId
        );
        
        request.Headers.Add(
            name: "Authorization", value: "Bearer " + option.AccessToken 
        );
        
        using HttpResponseMessage response = await m_httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        GetUsersApiResponse? serializedResponse = await response.Content.ReadFromJsonAsync<GetUsersApiResponse>();
        
        return new GetUsersResponse(
            ApiResponse: serializedResponse ?? new GetUsersApiResponse( Array.Empty<UserEntry>() ),
            StatusCode: response.StatusCode
        );
    }
}
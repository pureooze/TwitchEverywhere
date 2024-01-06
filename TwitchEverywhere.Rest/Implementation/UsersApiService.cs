using System.Net.Http.Json;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types.RestApi.Users;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest.Implementation;

public class UsersApiService( 
    TwitchConnectionOptions option 
) : IUsersApiService {

    async Task<GetUsersResponse> IUsersApiService.GetUsers(
        HttpClient httpClient,
        string[] userIds
    ) {
        using HttpRequestMessage request = new (
            method: HttpMethod.Get,
            requestUri: $"{Globals.HelixPrefix}/users?login={string.Join("&login=", userIds)}"
        );
        
        request.Headers.Add( 
            name: "Client-ID", value: option.ClientId
        );
        
        request.Headers.Add(
            name: "Authorization", value: "Bearer " + option.AccessToken 
        );
        
        using HttpResponseMessage response = await httpClient.SendAsync(
            request, 
            HttpCompletionOption.ResponseHeadersRead
        );
        
        response.EnsureSuccessStatusCode();

        GetUsersApiResponse? serializedResponse = await response.Content.ReadFromJsonAsync<GetUsersApiResponse>();
        
        return new GetUsersResponse(
            ApiResponse: serializedResponse ?? new GetUsersApiResponse( Array.Empty<UserEntry>() ),
            StatusCode: response.StatusCode
        );
    }
    
    async Task<GetUsersResponse> IUsersApiService.UpdateUser(
        HttpClient httpClient,
        string description
    ) {
        using HttpRequestMessage request = new (
            method: HttpMethod.Put,
            requestUri: $"{Globals.HelixPrefix}/users?description={description}"
        );
        
        request.Headers.Add( 
            name: "Client-ID", value: option.ClientId
        );
        
        request.Headers.Add(
            name: "Authorization", value: "Bearer " + option.AccessToken 
        );
        
        using HttpResponseMessage response = await httpClient.SendAsync(
            request, 
            HttpCompletionOption.ResponseHeadersRead
        );
        
        response.EnsureSuccessStatusCode();

        GetUsersApiResponse? serializedResponse = await response.Content.ReadFromJsonAsync<GetUsersApiResponse>();
        
        return new GetUsersResponse(
            ApiResponse: serializedResponse ?? new GetUsersApiResponse( Array.Empty<UserEntry>() ),
            StatusCode: response.StatusCode
        );
    }
}
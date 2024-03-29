using System.Net;
using System.Net.Http.Json;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types.RestApi.Users;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;

namespace TwitchEverywhere.Rest.Implementation;

public class UsersApiService( 
    TwitchConnectionOptions option 
) : IUsersApiService {

    async Task<GetUsersResponse> IUsersApiService.GetUsersById(
        IHttpClientWrapper httpClientWrapper,
        string[] userIds
    ) {
        
        using HttpRequestMessage request = new (
            method: HttpMethod.Get,
            requestUri: $"{Globals.HelixPrefix}/users?id={string.Join("&id=", userIds)}"
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
        
        GetUsersApiResponse? serializedResponse = await response.Content.ReadFromJsonAsync<GetUsersApiResponse>();
        
        return new GetUsersResponse(
            ApiResponse: serializedResponse ?? new GetUsersApiResponse( Array.Empty<UserEntry>() ),
            StatusCode: response.StatusCode
        );
    }
    
    async Task<GetUsersResponse> IUsersApiService.GetUsersByLogin(
        IHttpClientWrapper httpClientWrapper,
        string[] logins
    ) {
        
        using HttpRequestMessage request = new (
            method: HttpMethod.Get,
            requestUri: $"{Globals.HelixPrefix}/users?login={string.Join("&login=", logins)}"
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
        
        GetUsersApiResponse? serializedResponse = await response.Content.ReadFromJsonAsync<GetUsersApiResponse>();
        
        return new GetUsersResponse(
            ApiResponse: serializedResponse ?? new GetUsersApiResponse( Array.Empty<UserEntry>() ),
            StatusCode: response.StatusCode
        );
    }
    
    async Task<GetUsersResponse> IUsersApiService.UpdateUser(
        IHttpClientWrapper httpClientWrapper,
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
        
        using HttpResponseMessage response = await httpClientWrapper.SendAsync(
            request, 
            HttpCompletionOption.ResponseHeadersRead
        );
        
        GetUsersApiResponse? serializedResponse = await response.Content.ReadFromJsonAsync<GetUsersApiResponse>();
        
        return new GetUsersResponse(
            ApiResponse: serializedResponse ?? new GetUsersApiResponse( Array.Empty<UserEntry>() ),
            StatusCode: response.StatusCode
        );
    }
    
    async Task<GetUserBlockListResponse> IUsersApiService.GetUserBlockList(
        IHttpClientWrapper httpClientWrapper,
        string broadcasterId,
        int? first,
        string? after
    ) {
        
        string queryParams = $"?broadcaster_id={broadcasterId}";
        
        if( first != null ) {
            queryParams += $"&first={first.Value}";
        }
        
        if( after != null ) {
            queryParams += $"&after={after}";
        }
        
        using HttpRequestMessage request = new (
            method: HttpMethod.Get,
            requestUri: $"{Globals.HelixPrefix}/users/blocks{queryParams}"
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
        
        GetUserBlockListApiResponse? serializedResponse = await response.Content.ReadFromJsonAsync<GetUserBlockListApiResponse>();
        
        return new GetUserBlockListResponse(
            ApiResponse: serializedResponse 
                         ?? new GetUserBlockListApiResponse( 
                             Data: Array.Empty<BlockListEntry>(), 
                             Pagination: null 
                            ),
            StatusCode: response.StatusCode
        );
    }
    async Task<HttpStatusCode> IUsersApiService.BlockUser(
        IHttpClientWrapper httpClientWrapper,
        string targetUserId,
        string? sourceContext,
        string? reason
    ) {
        
        string queryParams = $"?target_user_id={targetUserId}";
        
        if( sourceContext != null ) {
            queryParams += $"&source_context={sourceContext}";
        }
        
        if( reason != null ) {
            queryParams += $"&reason={reason}";
        }
        
        using HttpRequestMessage request = new (
            method: HttpMethod.Put,
            requestUri: $"{Globals.HelixPrefix}/users/blocks{queryParams}"
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
        
        return response.StatusCode;
    }
}
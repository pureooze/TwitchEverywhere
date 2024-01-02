using System.Net.Http.Json;
using System.Text;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;

namespace TwitchEverywhere.Irc.Implementation; 

internal class Authorizer(
    string accessToken,
    string refreshToken,
    string clientId,
    string clientSecret
) : IAuthorizer {
    private const string CLIENT_GRANT_TYPE = "refresh_token";

    public async Task<string> GetToken() {
        TwitchRefreshTokenResponse? response = await RefreshToken();

        if( response?.AccessToken == null || response.RefreshToken == null ) {
            throw new InvalidTokenResponseException();
        }
        
        accessToken = response.AccessToken;
        refreshToken = response.RefreshToken;
        
        return accessToken;
    }

    private async Task<TwitchRefreshTokenResponse?> RefreshToken() {
        HttpClient httpClient = new ();
        HttpRequestMessage request = new (
            method: HttpMethod.Post,
            requestUri: new Uri( "https://id.twitch.tv/oauth2/token" )
        );

        string content = $"client_id={clientId}&client_secret={clientSecret}&grant_type={CLIENT_GRANT_TYPE}&refresh_token={refreshToken}";
        
        request.Content = new StringContent(
            content: content,
            encoding: Encoding.UTF8,
            mediaType: "application/x-www-form-urlencoded"
        ); 
        
        using HttpResponseMessage response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<TwitchRefreshTokenResponse>();
    }
}
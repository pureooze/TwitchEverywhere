using System.Data;
using System.Net.Http.Json;
using System.Text;

namespace TwitchEverywhere.Implementation; 

internal class Authorizer : IAuthorizer {
    private static string ACCESS_TOKEN = "";
    private static string REFRESH_TOKEN = "";
    private const string CLIENT_ID = "";
    private const string CLIENT_SECRET = "";
    private const string CLIENT_GRANT_TYPE = "refresh_token";

    public async Task<string> GetToken() {
        TwitchRefreshTokenResponse? response = await RefreshToken();

        if( response?.AccessToken == null || response.RefreshToken == null ) {
            throw new InvalidTokenResponseException();
        }
        
        ACCESS_TOKEN = response.AccessToken;
        REFRESH_TOKEN = response.RefreshToken;
        
        return ACCESS_TOKEN;
    }

    private async Task<TwitchRefreshTokenResponse?> RefreshToken() {
        HttpClient httpClient = new ();
        HttpRequestMessage request = new (
            method: HttpMethod.Post,
            requestUri: new Uri( "https://id.twitch.tv/oauth2/token" )
        );

        string content = $"client_id={CLIENT_ID}&client_secret={CLIENT_SECRET}&grant_type={CLIENT_GRANT_TYPE}&refresh_token={REFRESH_TOKEN}";
        
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
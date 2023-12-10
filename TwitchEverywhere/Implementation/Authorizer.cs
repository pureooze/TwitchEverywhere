using System.Net.Http.Json;
using System.Text;

namespace TwitchEverywhere.Implementation; 

internal class Authorizer : IAuthorizer {
    private string m_accessToken;
    private string m_refreshToken;
    private readonly string m_clientId;
    private readonly string m_clientSecret;
    private const string CLIENT_GRANT_TYPE = "refresh_token";

    public Authorizer( 
        string accessToken,
        string refreshToken,
        string clientId,
        string clientSecret
    ) {
        m_accessToken = accessToken;
        m_refreshToken = refreshToken;
        m_clientId = clientId;
        m_clientSecret = clientSecret;
    }
    
    public async Task<string> GetToken() {
        TwitchRefreshTokenResponse? response = await RefreshToken();

        if( response?.AccessToken == null || response.RefreshToken == null ) {
            throw new InvalidTokenResponseException();
        }
        
        m_accessToken = response.AccessToken;
        m_refreshToken = response.RefreshToken;
        
        return m_accessToken;
    }

    private async Task<TwitchRefreshTokenResponse?> RefreshToken() {
        HttpClient httpClient = new ();
        HttpRequestMessage request = new (
            method: HttpMethod.Post,
            requestUri: new Uri( "https://id.twitch.tv/oauth2/token" )
        );

        string content = $"client_id={m_clientId}&client_secret={m_clientSecret}&grant_type={CLIENT_GRANT_TYPE}&refresh_token={m_refreshToken}";
        
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
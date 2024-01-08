namespace TwitchEverywhere.Rest.Implementation;

public class HttpClientWrapper : IHttpClientWrapper {
    private readonly HttpClient m_httpClient = new(){ Timeout = TimeSpan.FromSeconds(30) };
    
    async Task<HttpResponseMessage> IHttpClientWrapper.SendAsync(
        HttpRequestMessage request,
        HttpCompletionOption completionOption
    ) {
        return await m_httpClient.SendAsync(
            request, 
            completionOption
        );
    }
}
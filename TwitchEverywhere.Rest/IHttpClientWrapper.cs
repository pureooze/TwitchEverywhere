namespace TwitchEverywhere.Rest;

public interface IHttpClientWrapper {
    
    Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead
    );
}
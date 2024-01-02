namespace TwitchEverywhere.Core; 

public interface IAuthorizer {
    Task<string> GetToken();
}
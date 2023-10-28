namespace TwitchEverywhere; 

public interface IAuthorizer {
    Task<string> GetToken();
}
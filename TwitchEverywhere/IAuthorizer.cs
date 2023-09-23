namespace TwitchEverywhere.Implementation; 

public interface IAuthorizer {
    Task<string> GetToken();
}
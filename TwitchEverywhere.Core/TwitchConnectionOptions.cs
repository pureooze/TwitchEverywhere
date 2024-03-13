namespace TwitchEverywhere.Core; 

public sealed record TwitchConnectionOptions(
    string? AccessToken,
    string? RefreshToken,
    string? ClientId,
    string? ClientSecret,
    string? ClientName
);
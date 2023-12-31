namespace TwitchEverywhere.Core; 

public sealed record TwitchConnectionOptions(
    string Channel,
    string? AccessToken,
    string? RefreshToken,
    string? ClientId,
    string? ClientSecret,
    string? ClientName
);
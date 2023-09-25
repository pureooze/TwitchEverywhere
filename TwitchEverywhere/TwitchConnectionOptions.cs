namespace TwitchEverywhere; 

public sealed record TwitchConnectionOptions(
    string Channel,
    string? AccessToken,
    string? RefreshToken,
    string? ClientId,
    string? ClientSecret,
    int BufferSize
);
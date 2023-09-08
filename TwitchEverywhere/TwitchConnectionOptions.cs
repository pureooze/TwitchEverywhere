namespace TwitchEverywhere; 

public sealed record TwitchConnectionOptions(
    string Channel,
    string[] Tags,
    string Message
);
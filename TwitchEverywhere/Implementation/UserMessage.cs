namespace TwitchEverywhere.Implementation; 

public record UserMessage(
    string DisplayName,
    string Message,
    DateTime Timestamp
);
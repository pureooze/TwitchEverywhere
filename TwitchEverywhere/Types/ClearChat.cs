namespace TwitchEverywhere.Types; 

public record ClearChat(
    long? Duration,
    string RoomId,
    string? UserId,
    DateTime Timestamp
);
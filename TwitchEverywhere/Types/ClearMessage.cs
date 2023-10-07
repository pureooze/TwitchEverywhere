namespace TwitchEverywhere.Types; 

public record ClearMessage(
    long? Duration,
    string RoomId,
    string? UserId,
    DateTime Timestamp
);
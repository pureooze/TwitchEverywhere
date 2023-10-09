namespace TwitchEverywhere.Types; 

public record ClearMsg(
    string Login,
    string RoomId,
    string TargetMessageId,
    DateTime Timestamp
);
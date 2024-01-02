namespace TwitchEverywhere.Core.Types.Messages.Interfaces;

public interface IRoomStateMsg : IMessage {
    bool EmoteOnly { get; }
    int FollowersOnly { get; }
    bool R9K { get; }
    string RoomId { get; }
    int Slow { get; }
    bool SubsOnly { get; }
}
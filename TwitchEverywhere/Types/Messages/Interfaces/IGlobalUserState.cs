using System.Collections.Immutable;

namespace TwitchEverywhere.Types.Messages.Interfaces {
    public interface IGlobalUserState {
        MessageType MessageType { get; }
        IImmutableList<Badge> Badges { get; }
        IImmutableList<Badge> BadgeInfo { get; }
        string Color { get; }
        string DisplayName { get; }
        IImmutableList<string> EmoteSets { get; }
        bool Turbo { get; }
        string UserId { get; }
        UserType UserType { get; }
    }
}
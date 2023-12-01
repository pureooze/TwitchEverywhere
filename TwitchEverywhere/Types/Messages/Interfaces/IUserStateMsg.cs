using System.Collections.Immutable;

namespace TwitchEverywhere.Types.Messages.Interfaces;

public interface IUserStateMsg {
    IImmutableList<Badge> BadgeInfo { get; }

    IImmutableList<Badge> Badges { get; }

    string Color { get; }

    string DisplayName { get; }

    IImmutableList<string> EmoteSets { get; }

    string Id { get; }

    bool Mod { get; }

    bool Subscriber { get; }

    bool Turbo { get; }

    UserType UserType { get; }
}
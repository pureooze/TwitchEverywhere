using System.Collections.Immutable;

namespace TwitchEverywhere.Core.Types.Messages.Interfaces;

public interface IUserStateMsg : IMessage {
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
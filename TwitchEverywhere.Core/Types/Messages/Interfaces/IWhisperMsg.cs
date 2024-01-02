using System.Collections.Immutable;

namespace TwitchEverywhere.Core.Types.Messages.Interfaces;

public interface IWhisperMsg : IMessage {
    IImmutableList<Badge> Badges { get; }
    string Color { get; }
    string DisplayName { get; }
    IImmutableList<Emote>? Emotes { get; }
    string MsgId { get; }
    string ThreadId { get; }
    bool Turbo { get; }
    string UserId { get; }
    UserType UserType { get; }
    string FromUser { get; }
    string ToUser { get; }
    string Text { get; }
}
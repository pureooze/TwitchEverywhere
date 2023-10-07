using System.Collections.Immutable;

namespace TwitchEverywhere.Types; 

public record PrivMessage( 
    string Text,
    DateTime Timestamp,
    TimeSpan SinceStartOfStream,
    string DisplayName,
    IImmutableList<Badge> Badges,
    MessageType MessageType
);
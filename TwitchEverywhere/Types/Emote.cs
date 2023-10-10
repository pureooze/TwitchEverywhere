namespace TwitchEverywhere.Types; 

public record Emote(
    string EmoteId,
    long Start,
    long End
);
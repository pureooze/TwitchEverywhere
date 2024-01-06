using System.Text.Json.Serialization;

namespace TwitchEverywhere.Core.Types.RestApi.Videos;

public record MutedSegment( 
    [property: JsonPropertyName("duration")] int Duration,
    [property: JsonPropertyName("offset")] int Offset
);
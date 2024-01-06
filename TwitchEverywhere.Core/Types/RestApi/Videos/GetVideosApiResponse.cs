using System.Text.Json.Serialization;

namespace TwitchEverywhere.Core.Types.RestApi.Videos;

public record GetVideosApiResponse(
    [property: JsonPropertyName("data")] VideoEntry[] Data
);


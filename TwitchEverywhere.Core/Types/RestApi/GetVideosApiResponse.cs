using System.Text.Json.Serialization;

namespace TwitchEverywhere.Core.Types.RestApi;

public record GetVideosApiResponse(
    [property: JsonPropertyName("data")] VideoEntry[] Data
);


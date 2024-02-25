using System.Text.Json.Serialization;
using TwitchEverywhere.Core.Types.RestApi.Videos;

namespace TwitchEverywhere.Core.Types.RestApi.Streams;

public record GetStreamsApiResponse( 
    [property: JsonPropertyName("data")] StreamEntry[] Data,
    [property: JsonPropertyName("pagination")] Pagination Pagination
);
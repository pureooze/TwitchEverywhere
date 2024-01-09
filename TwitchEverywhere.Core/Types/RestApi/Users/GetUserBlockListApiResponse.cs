using System.Text.Json.Serialization;
using TwitchEverywhere.Core.Types.RestApi.Videos;

namespace TwitchEverywhere.Core.Types.RestApi.Users;

public record GetUserBlockListApiResponse( 
    [property: JsonPropertyName("data")] BlockListEntry[] Data,
    [property: JsonPropertyName("pagination")] Pagination? Pagination
);
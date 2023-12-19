using System.Text.Json.Serialization;

namespace TwitchEverywhere.Types.RestApi;

public record GetUsersApiResponse(
    [property: JsonPropertyName("data")] UserEntry[] Data
);


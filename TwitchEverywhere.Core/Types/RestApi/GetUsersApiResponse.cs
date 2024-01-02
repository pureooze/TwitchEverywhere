using System.Text.Json.Serialization;

namespace TwitchEverywhere.Core.Types.RestApi;

public record GetUsersApiResponse(
    [property: JsonPropertyName("data")] UserEntry[] Data
);


using System.Text.Json.Serialization;

namespace TwitchEverywhere.Core.Types.RestApi.Users;

public record GetUsersApiResponse(
    [property: JsonPropertyName("data")] UserEntry[] Data
);


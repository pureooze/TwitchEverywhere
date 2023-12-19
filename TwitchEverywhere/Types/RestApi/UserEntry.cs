using System.Text.Json.Serialization;

namespace TwitchEverywhere.Types.RestApi;

public record UserEntry(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("login")] string Login,
    [property: JsonPropertyName("display_name")] string DisplayName,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("broadcaster_type")] string BroadcasterType,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("profile_image_url")] string ProfileImageUrl,
    [property: JsonPropertyName("offline_image_url")] string OfflineImageUrl,
    [property: JsonPropertyName("view_count")] long ViewCount,
    [property: JsonPropertyName("created_at")] DateTime CreatedAt
);
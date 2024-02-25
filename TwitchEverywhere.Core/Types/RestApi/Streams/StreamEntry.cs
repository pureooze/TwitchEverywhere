using System.Text.Json.Serialization;

namespace TwitchEverywhere.Core.Types.RestApi.Streams;

public record StreamEntry(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("user_name")] string UserName,
    [property: JsonPropertyName("game_id")] string GameId,
    [property: JsonPropertyName("game_name")] string GameName,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("tags")] string[] Tags,
    [property: JsonPropertyName("viewer_count")] int ViewerCount,
    [property: JsonPropertyName("started_at")] DateTime StartedAt,
    [property: JsonPropertyName("language")] string Language,
    [property: JsonPropertyName("thumbnail_url")] string ThumbnailUrl,
    [property: JsonPropertyName("tag_ids")] string[] TagIds,
    [property: JsonPropertyName("is_mature")] bool IsMature
);
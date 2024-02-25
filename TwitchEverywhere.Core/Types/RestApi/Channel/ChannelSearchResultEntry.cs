using System.Text.Json.Serialization;

namespace TwitchEverywhere.Core.Types.RestApi.Channel;

public record ChannelSearchResultEntry(
    [property: JsonPropertyName("broadcaster_language")] string BroadcasterLanguage,
    [property: JsonPropertyName("broadcaster_login")] string BroadcasterLogin,
    [property: JsonPropertyName("display_name")] string DisplayName,
    [property: JsonPropertyName("game_id")] string GameId,
    [property: JsonPropertyName("game_name")] string GameName,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("is_live")] bool IsLive,
    [property: JsonPropertyName("tag_ids")] List<string> TagIds,
    [property: JsonPropertyName("tags")] List<string> Tags,
    [property: JsonPropertyName("thumbnail_url")] string ThumbnailUrl,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("started_at")] string StartedAt
);
using System.Text.Json.Serialization;

namespace TwitchEverywhere.Core.Types.RestApi.Channel;

public record ChannelInfoEntry(
    [property: JsonPropertyName("broadcaster_id")] string BroadcasterId,
    [property: JsonPropertyName("broadcaster_login")] string BroadcasterLogin,
    [property: JsonPropertyName("broadcaster_name")] string BroadcasterName,
    [property: JsonPropertyName("broadcaster_language")] string BroadcasterLanguage,
    [property: JsonPropertyName("game_id")] string GameId,
    [property: JsonPropertyName("game_name")] string GameName,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("delay")] int Delay,
    [property: JsonPropertyName("tags")] List<string> Tags,
    [property: JsonPropertyName("content_classification_labels")] List<string> ContentClassificationLabels,
    [property: JsonPropertyName("is_branded_content")] bool IsBrandedContent
);
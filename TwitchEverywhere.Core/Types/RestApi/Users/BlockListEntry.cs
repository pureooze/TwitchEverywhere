using System.Text.Json.Serialization;

namespace TwitchEverywhere.Core.Types.RestApi.Users;

public record BlockListEntry(
    [property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("user_login")] string UserLogin,
    [property: JsonPropertyName("display_name")] string DisplayName
);
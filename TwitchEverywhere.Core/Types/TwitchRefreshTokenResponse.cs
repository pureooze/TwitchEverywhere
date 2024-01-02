using System.Text.Json.Serialization;

namespace TwitchEverywhere.Core.Types;

public record TwitchRefreshTokenResponse (
    [property: JsonPropertyName( "access_token" )] string AccessToken,
    [property: JsonPropertyName( "expires_in" )] long ExpiresIn,
    [property: JsonPropertyName( "token_type" )] string TokenType,
    [property: JsonPropertyName( "refresh_token" )] string RefreshToken
);
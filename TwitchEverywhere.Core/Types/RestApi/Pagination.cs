using System.Text.Json.Serialization;

namespace TwitchEverywhere.Core.Types.RestApi;

public record Pagination( 
    [property: JsonPropertyName("cursor")] string Cursor
);
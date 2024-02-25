using System.Text.Json.Serialization;

namespace TwitchEverywhere.Core.Types.RestApi.Channel;

public record GetChannelSearchApiResponse(
    [property: JsonPropertyName("data")] ChannelSearchResultEntry[] Data
);


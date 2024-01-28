using System.Text.Json.Serialization;

namespace TwitchEverywhere.Core.Types.RestApi.Channel;

public record GetChannelInfoApiResponse(
    [property: JsonPropertyName("data")] ChannelInfoEntry[] Data
);


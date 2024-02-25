using System.Net;
using TwitchEverywhere.Core.Types.RestApi.Channel;

namespace TwitchEverywhere.Core.Types.RestApi.Wrappers;

public record GetChannelSearchResponse(
    GetChannelSearchApiResponse ApiResponse,
    HttpStatusCode StatusCode
);
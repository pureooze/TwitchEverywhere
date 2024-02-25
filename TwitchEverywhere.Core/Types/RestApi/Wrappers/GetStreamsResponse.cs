using System.Net;
using TwitchEverywhere.Core.Types.RestApi.Streams;

namespace TwitchEverywhere.Core.Types.RestApi.Wrappers;

public record GetStreamsResponse(
    GetStreamsApiResponse ApiResponse,
    HttpStatusCode StatusCode
);
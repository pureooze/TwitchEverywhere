using System.Net;
using TwitchEverywhere.Core.Types.RestApi;

namespace TwitchEverywhere.Core.Types;

public record GetVideosResponse(
    GetVideosApiResponse ApiResponse,
    HttpStatusCode StatusCode
);


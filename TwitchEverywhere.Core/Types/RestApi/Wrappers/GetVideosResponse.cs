using System.Net;
using TwitchEverywhere.Core.Types.RestApi.Videos;

namespace TwitchEverywhere.Core.Types.RestApi.Wrappers;

public record GetVideosResponse(
    GetVideosApiResponse ApiResponse,
    HttpStatusCode StatusCode
);


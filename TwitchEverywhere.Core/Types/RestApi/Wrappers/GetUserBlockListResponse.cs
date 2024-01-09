using System.Net;
using TwitchEverywhere.Core.Types.RestApi.Users;

namespace TwitchEverywhere.Core.Types.RestApi.Wrappers;

public record GetUserBlockListResponse(
    GetUserBlockListApiResponse ApiResponse,
    HttpStatusCode StatusCode
);


using System.Net;
using TwitchEverywhere.Core.Types.RestApi;

namespace TwitchEverywhere.Core.Types;

public record GetUsersResponse(
    GetUsersApiResponse ApiResponse,
    HttpStatusCode StatusCode
);


using System.Net;
using TwitchEverywhere.Types.RestApi;

namespace TwitchEverywhere.Types;

public record GetUsersResponse(
    GetUsersApiResponse ApiResponse,
    HttpStatusCode StatusCode
);


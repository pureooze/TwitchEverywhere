using System.Net;
using TwitchEverywhere.Core.Types.RestApi.Users;

namespace TwitchEverywhere.Core.Types.RestApi.Wrappers;

public record GetUsersResponse(
    GetUsersApiResponse ApiResponse,
    HttpStatusCode StatusCode
);


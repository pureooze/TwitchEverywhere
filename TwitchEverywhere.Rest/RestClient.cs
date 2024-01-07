using System.Net;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types.RestApi.Users;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;
using TwitchEverywhere.Rest.Implementation;

namespace TwitchEverywhere.Rest;

public class RestClient( TwitchConnectionOptions options ) {
    private readonly IRestApiService m_restService = new RestApiService( options );

    /// <summary>
    /// <para>
    /// Gets information about one or more users.
    /// You may look up users using their user ID but the sum total of the number of users you may look up is 100.
    /// </para>
    /// <para><b>Optional scope:</b> user:read:email</para>
    /// </summary>
    /// <param name="ids">The IDs of the users to get.</param>
    /// <returns></returns>
    public async Task<GetUsersResponse> GetUsersById(
        IEnumerable<string> ids
    ) {
        
        return await m_restService.GetUsersById( 
            userIds: ids.ToArray()
        );
    }
    
    /// <summary>
    /// <para>
    /// Gets information about one or more users.
    /// You may look up users using their login name but the sum total of the number of users you may look up is 100.
    /// </para>
    /// <para><b>Optional scope:</b> user:read:email</para>
    /// </summary>
    /// <param name="logins">The login names of the users to get.</param>
    /// <returns></returns>
    public async Task<GetUsersResponse> GetUsersByLogin(
        IEnumerable<string> logins
    ) {

        return await m_restService.GetUsersByLogin( 
            logins: logins.ToArray()
        );
    }
    
    /// <summary>
    /// <para>
    /// Updates the specified user’s <paramref name="description"/>. The user ID in the OAuth token identifies the user whose information you want to update.
    /// To include the user’s verified email address, the user access token must also include the user:read:email scope.
    /// </para>
    /// <para><b>Required scope:</b> user:edit</para>
    /// <para><b>Optional scope:</b> user:read:email</para>
    /// </summary>
    /// <param name="description">The string to update the channel’s description to. The description is limited to a maximum of 300 characters.</param>
    /// <returns></returns>
    public async Task<GetUsersResponse> UpdateUser(
        string description
    ) {
        return await m_restService.UpdateUser( description );
    }
    
    /// <summary>
    /// <para>
    /// Gets information about one or more published videos by user ID.
    /// </para>
    /// </summary>
    /// <param name="userId">The ID of the user whose list of videos you want to get.</param>
    /// <returns></returns>
    public async Task<GetVideosResponse> GetVideosForUsersById(
        string userId
    ) {
        return await m_restService.GetVideosForUsersById( userId );
    }
}
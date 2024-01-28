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
    /// Updates the specified user’s <paramref name="description"/>. The user ID in the OAuth token identifies the user whose information you want to update.
    /// To include the user’s verified email address, the user access token must also include the user:read:email scope.
    /// </para>
    /// <para><b>Required scope:</b> user:read:blocked_users</para>
    /// </summary>
    /// <param name="broadcasterId">The ID of the broadcaster whose list of blocked users you want to get.</param>
    /// <param name="first">The maximum number of items to return per page in the response. The minimum page size is 1 item per page and the maximum is 100. The default is 20.</param>
    /// <param name="after">The cursor used to get the next page of results.</param>
    /// <returns></returns>
    public async Task<GetUserBlockListResponse> GetUserBlockList(
        string broadcasterId,
        int? first = 20,
        string? after = null
    ) {
        if( first < 1 ) {
            throw new ArgumentException("The minimum page size is 1.");
        }
        
        if( first > 100 ) {
            throw new ArgumentException("The maximum page size is 100.");
        }
        
        return await m_restService.GetUserBlockList( broadcasterId, first, after );
    }
    
    /// <summary>
    /// <para>
    /// Updates the specified user’s <paramref name="description"/>. The user ID in the OAuth token identifies the user whose information you want to update.
    /// To include the user’s verified email address, the user access token must also include the user:read:email scope.
    /// </para>
    /// <para><b>Required scope:</b> user:manage:blocked_users</para>
    /// </summary>
    /// <param name="targetUserId">The ID of the user to block.</param>
    /// <param name="sourceContext">The location where the harassment took place that is causing the broadcaster to block the user.</param>
    /// <param name="reason">The reason that the broadcaster is blocking the user.</param>
    /// <returns></returns>
    public async Task<HttpStatusCode> BlockUser(
        string targetUserId,
        SourceContext? sourceContext = null,
        Reason? reason = null
    ) {
        return await m_restService.BlockUser( targetUserId, sourceContext, reason );
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

    /// <summary>
    /// <para>
    /// Gets information about one channel.
    /// </para>
    /// </summary>
    /// <param name="broadcasterId"></param>
    /// <returns></returns>
    public async Task<GetChannelInfoResponse> GetChannelInfo(
        string broadcasterId
    ) {
        return await m_restService.GetChannelInfo( broadcasterId );
    }
}
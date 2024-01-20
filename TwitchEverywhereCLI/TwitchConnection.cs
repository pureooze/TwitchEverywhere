using System.Net;
using System.Text;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.ImmediateLoadedMessages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;
using TwitchEverywhere.Core.Types.RestApi.Users;
using TwitchEverywhere.Core.Types.RestApi.Videos;
using TwitchEverywhere.Core.Types.RestApi.Wrappers;
using TwitchEverywhere.Irc;
using TwitchEverywhere.Rest;
using TwitchEverywhereCLI.Implementation;

namespace TwitchEverywhereCLI; 

internal class TwitchConnection( 
    TwitchConnectionOptions options 
) {

    private readonly MessageBuffer m_messageBuffer = new( buffer: new StringBuilder() );
    private readonly MessageBuffer m_clearChat = new( buffer: new StringBuilder() );
    private readonly MessageBuffer m_clearMsg = new( buffer: new StringBuilder() );
    private readonly ICompressor m_compressor = new BrotliCompressor();
    private readonly IrcClient m_ircClient = new( options );
    private readonly RestClient m_restClient = new( options );
    private const int BUFFER_SIZE = 3;
    private DateTime m_startTimestamp = DateTime.UtcNow;

    public async Task ConnectToIrcClient() {
        await m_ircClient.ConnectToChannel( MessageCallback );
    }

    public async Task ConnectToRestClient() {
        GetUsersResponse users = await m_restClient.GetUsersByLogin(
            logins: [ "pureooze" ] 
        );
        
        if (users.StatusCode != HttpStatusCode.OK) {
            Console.WriteLine( "Error in GetUsers request with status code: " + users.StatusCode );
            return;
        }
        
        foreach (UserEntry userEntry in users.ApiResponse.Data) {
            Console.WriteLine( userEntry );
            
            GetUsersResponse updatedUser = await m_restClient.UpdateUser(
                description: "Did it work"
            );
            
            if (updatedUser.StatusCode != HttpStatusCode.OK) {
                Console.WriteLine( "Error in UpdateUser request with status code: " + updatedUser.StatusCode );
                return;
            }
            
            GetVideosResponse response = await m_restClient.GetVideosForUsersById( 
                userId: userEntry.Id
            );
            
            if (response.StatusCode != HttpStatusCode.OK) {
                Console.WriteLine( "Error in GetVideos request with status code: " + response.StatusCode );
                return;
            }
         
            foreach (VideoEntry videoEntry in response.ApiResponse.Data) {
                Console.WriteLine( videoEntry );
            }
        }
        
    }
    
    private async Task SaveBufferToFile( string fileName, StringBuilder buffer, DateTime startTimestamp ) {
        if( buffer.Length == 0 ) {
            return;
        }
    
        string rawData = buffer.ToString();
        byte[] byteBuffer = Encoding.UTF8.GetBytes( rawData );
    
        byte[] compressedData = await m_compressor.Compress( byteBuffer );
    
        string path = $"{fileName}-{startTimestamp.ToUniversalTime():yyyy-M-d_H-mm-ss}.json";
        SaveBinaryDataToFile( path, compressedData );
    }

    private static void SaveBinaryDataToFile(
        string path,
        byte[] compressedData
    ) {
        using FileStream fileStream = new (path, FileMode.Create);
        fileStream.Write( compressedData, 0, compressedData.Length );
    }

    private async void MessageCallback(
        IMessage message
    ) {
        switch( message.MessageType ) {
            case MessageType.PrivMsg:
                IPrivMsg lazyLoadedPrivMsg = (IPrivMsg) message;
                // PrivMessageCallback( lazyLoadedPrivMsg );
                Console.WriteLine( $"PrivMsg: {lazyLoadedPrivMsg.DisplayName}, {lazyLoadedPrivMsg.Text}" );

                // PrivMsg reply = new(
                //     channel: "pureooze",
                //     replyParentMsgId: lazyLoadedPrivMsg.Id,
                //     text: lazyLoadedPrivMsg.Text + "? hmm maybe..."
                // );
                //
                // bool sendMessage = await m_ircClient.SendMessage( reply, MessageType.PrivMsg );
                //
                // Console.WriteLine( sendMessage ? $"Sent message SUCCEEDED!" : $"Sent message FAILED!" );
                break;
            case MessageType.ClearChat:
                IClearChatMsg lazyLoadedClearChatMsgMsg = (IClearChatMsg) message;
                // ClearChatCallback( lazyLoadedClearChatMsgMsg );
                Console.WriteLine( $"ClearChat: {lazyLoadedClearChatMsgMsg.Text}" );
                break;
            case MessageType.ClearMsg:
                IClearMsg lazyLoadedClearMsg = (IClearMsg) message;
                // ClearMsgCallback( lazyLoadedClearMsg );
                Console.WriteLine( $"ClearChat: {lazyLoadedClearMsg.Login}, {lazyLoadedClearMsg.Timestamp}, {lazyLoadedClearMsg.RoomId}" );
                break;
            case MessageType.GlobalUserState:
                IGlobalUserStateMsg lazyLoadedGlobalUserStateMsgMsg = (IGlobalUserStateMsg) message;
                Console.WriteLine( $"GlobalUserState: {lazyLoadedGlobalUserStateMsgMsg.UserId}, {lazyLoadedGlobalUserStateMsgMsg.UserType}, {lazyLoadedGlobalUserStateMsgMsg.DisplayName}" );
                break;
            case MessageType.Notice:
                INoticeMsg lazyLoadedNoticeMsg = (INoticeMsg) message;
                // NoticeMsgCallback( lazyLoadedNoticeMsg );
                Console.WriteLine( $"NoticeMsg: {{ TargetUserId: {lazyLoadedNoticeMsg.TargetUserId}, MsgId: {lazyLoadedNoticeMsg.MsgId} }}" );
                Console.WriteLine( $"NoticeMsg: {lazyLoadedNoticeMsg.RawMessage}" );
                break;
            case MessageType.RoomState:
                IRoomStateMsg lazyLoadedRoomStateMsg = (IRoomStateMsg) message;
                Console.WriteLine( $"RoomStateMsg: {lazyLoadedRoomStateMsg.RoomId}, {lazyLoadedRoomStateMsg.R9K}, {lazyLoadedRoomStateMsg.Slow}" );
                break;
            case MessageType.UserNotice:
                IUserNoticeMsg lazyLoadedUserNoticeMsgMsg = (IUserNoticeMsg) message;
                Console.WriteLine( $"UserNotice: {lazyLoadedUserNoticeMsgMsg}" );
                break;
            case MessageType.UserState:
                IUserStateMsg lazyLoadedUserStateMsg = (IUserStateMsg) message;
                Console.WriteLine( $"UserStateMsg: {lazyLoadedUserStateMsg.DisplayName}, {lazyLoadedUserStateMsg.UserType}, {lazyLoadedUserStateMsg.Badges}" );
                break;
            case MessageType.Whisper:
                IWhisperMsg lazyLoadedWhisperMsg = (IWhisperMsg) message;
                Console.WriteLine( $"WhisperMsg: {lazyLoadedWhisperMsg}" );
                break;
            case MessageType.Join:
                IJoinMsg lazyLoadedJoinMsg = (IJoinMsg) message;
                Console.WriteLine( $"{lazyLoadedJoinMsg.User} joining {lazyLoadedJoinMsg.Channel}" );
                break;
            case MessageType.Part:
                IPartMsg lazyLoadedPartMsg = (IPartMsg) message;
                Console.WriteLine( $"{lazyLoadedPartMsg.User} leaving {lazyLoadedPartMsg.Channel}" );
                break;
            case MessageType.Unknown:
                IUnknownMsg lazyLoadedUnknownMsg = (IUnknownMsg) message;
                Console.WriteLine( $"UnknownMessage: {lazyLoadedUnknownMsg}" );
                break;
            case MessageType.HostTarget:
                IHostTargetMsg hostTargetMsg = (IHostTargetMsg) message;
                Console.WriteLine( $"HostTargetMsg: {hostTargetMsg.HostingChannel}, {hostTargetMsg.NumberOfViewers}" );
                break;
            case MessageType.Reconnect:
                IReconnectMsg lazyLoadedReconnectMsg = (IReconnectMsg) message;
                Console.WriteLine( $"ReconnectMsg: {lazyLoadedReconnectMsg}" );
                break;
            case MessageType.CapReq:
                ICapReq lazyLoadedCapReqMsg = (ICapReq) message;
                Console.WriteLine( $"CapReq: {lazyLoadedCapReqMsg.RawMessage}" );
                break;
            case MessageType.JoinCount:
                IJoinCountMsg lazyLoadedJoinCountMsg = (IJoinCountMsg) message;
                Console.WriteLine( $"JoinCount: {lazyLoadedJoinCountMsg.RawMessage}" );
                break;
            case MessageType.JoinEnd:
                IJoinEndMsg lazyLoadedIJoinEndMsg = (IJoinEndMsg) message;
                Console.WriteLine( $"JoinEnd: {lazyLoadedIJoinEndMsg.RawMessage}" );
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private async void PrivMessageCallback(
        IPrivMsg lazyLoadedPrivMsg
    ) {
        if( m_messageBuffer.Count == BUFFER_SIZE ) {
            await WriteToStore( m_messageBuffer, MessageType.PrivMsg );
        }
        
        Console.WriteLine( $"{lazyLoadedPrivMsg.DisplayName}: {lazyLoadedPrivMsg.Text}" );
        m_messageBuffer.AddToBuffer( lazyLoadedPrivMsg.Text );
    }
    
    private async void ClearChatCallback(
        IClearChatMsg lazyLoadedClearChatMsg
    ) {
        if( m_clearChat.Count == BUFFER_SIZE ) {
            await WriteToStore( m_clearChat, MessageType.ClearChat );
        }
        
        Console.WriteLine( $"ClearChat: On {lazyLoadedClearChatMsg.Timestamp} the user {lazyLoadedClearChatMsg.TargetUserId} was muted/banned for {lazyLoadedClearChatMsg.Duration} seconds {lazyLoadedClearChatMsg.Text}" );
        
        if( lazyLoadedClearChatMsg.TargetUserId != null ) {
            m_clearChat.AddToBuffer( lazyLoadedClearChatMsg.TargetUserId );
        }
    }
    
    private async void ClearMsgCallback(
        IClearMsg lazyLoadedClearMsg
    ) {
        if( m_clearMsg.Count == BUFFER_SIZE ) {
            await WriteToStore( m_clearMsg, MessageType.ClearMsg );
        }
        
        Console.WriteLine( $"ClearMsg: On {lazyLoadedClearMsg.Timestamp} the user {lazyLoadedClearMsg.Login} had a message deleted for message {lazyLoadedClearMsg.TargetMessageId}" );
        
        m_clearMsg.AddToBuffer( lazyLoadedClearMsg.TargetMessageId );
    }
    
    private void NoticeMsgCallback(
        INoticeMsg immediateLoadedNoticeMsg
    ) {      
        Console.WriteLine( $"NoticeMsg: On {immediateLoadedNoticeMsg.MsgId} the user {immediateLoadedNoticeMsg.TargetUserId}" );
    }

    private async Task WriteToStore(
        MessageBuffer messageBuffer, MessageType type 
    ) {
        StringBuilder tempBuffer = new( messageBuffer.ReadAsString() );
        DateTime tempStartTimestamp = new( m_startTimestamp.Ticks );
        messageBuffer.Clear();
        m_startTimestamp = DateTime.UtcNow;
        await SaveBufferToFile( $"{type}", tempBuffer, tempStartTimestamp );
    }
}
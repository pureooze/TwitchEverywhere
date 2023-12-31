using System.Net;
using System.Text;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.ImmediateLoadedMessages;
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
                IPrivMsg lazyLoadedPrivMsg = (LazyLoadedPrivMsg) message;
                PrivMessageCallback( lazyLoadedPrivMsg );
                Console.WriteLine( $"PrivMsg: {lazyLoadedPrivMsg.DisplayName}, {lazyLoadedPrivMsg.Text}" );

                ImmediateLoadedPrivMsg reply = new(
                    channel: "pureooze",
                    replyParentMsgId: lazyLoadedPrivMsg.Id,
                    text: lazyLoadedPrivMsg.Text + "? hmm maybe..."
                );
                
                bool sendMessage = await m_ircClient.SendMessage( reply, MessageType.PrivMsg );

                Console.WriteLine( sendMessage ? $"Sent message SUCCEEDED!" : $"Sent message FAILED!" );
                break;
            case MessageType.ClearChat:
                IClearChat lazyLoadedClearChatMsg = (LazyLoadedClearChat) message;
                ClearChatCallback( lazyLoadedClearChatMsg );
                Console.WriteLine( $"ClearChat: {lazyLoadedClearChatMsg.Text}" );
                break;
            case MessageType.ClearMsg:
                IClearMsg lazyLoadedClearMsg = (LazyLoadedClearMsg) message;
                ClearMsgCallback( lazyLoadedClearMsg );
                Console.WriteLine( $"ClearChat: {lazyLoadedClearMsg.Login}, {lazyLoadedClearMsg.Timestamp}, {lazyLoadedClearMsg.RoomId}" );
                break;
            case MessageType.GlobalUserState:
                IGlobalUserState lazyLoadedGlobalUserStateMsg = (LazyLoadedGlobalUserState) message;
                Console.WriteLine( $"GlobalUserState: {lazyLoadedGlobalUserStateMsg.UserId}, {lazyLoadedGlobalUserStateMsg.UserType}, {lazyLoadedGlobalUserStateMsg.DisplayName}" );
                break;
            case MessageType.Notice:
                INoticeMsg lazyLoadedNoticeMsg = (LazyLoadedNoticeMsg) message;
                NoticeMsgCallback( lazyLoadedNoticeMsg );
                Console.WriteLine( $"NoticeMsg: {{ TargetUserId: {lazyLoadedNoticeMsg.TargetUserId}, MsgId: {lazyLoadedNoticeMsg.MsgId} }}" );
                Console.WriteLine( $"NoticeMsg: {lazyLoadedNoticeMsg.RawMessage}" );
                break;
            case MessageType.RoomState:
                IRoomStateMsg lazyLoadedRoomStateMsg = (LazyLoadedRoomStateMsg) message;
                Console.WriteLine( $"RoomStateMsg: {lazyLoadedRoomStateMsg.RoomId}, {lazyLoadedRoomStateMsg.R9K}, {lazyLoadedRoomStateMsg.Slow}" );
                break;
            case MessageType.UserNotice:
                IUserNotice lazyLoadedUserNoticeMsg = (LazyLoadedUserNotice) message;
                Console.WriteLine( $"UserNotice: {lazyLoadedUserNoticeMsg}" );
                break;
            case MessageType.UserState:
                IUserStateMsg lazyLoadedUserStateMsg = (LazyLoadedUserStateMsg) message;
                Console.WriteLine( $"UserStateMsg: {lazyLoadedUserStateMsg.DisplayName}, {lazyLoadedUserStateMsg.UserType}, {lazyLoadedUserStateMsg.Badges}" );
                break;
            case MessageType.Whisper:
                IWhisperMsg lazyLoadedWhisperMsg = (LazyLoadedWhisperMsg) message;
                Console.WriteLine( $"WhisperMsg: {lazyLoadedWhisperMsg}" );
                break;
            case MessageType.Join:
                IJoinMsg lazyLoadedJoinMsg = (LazyLoadedJoinMsg) message;
                Console.WriteLine( $"{lazyLoadedJoinMsg.User} joining {lazyLoadedJoinMsg.Channel}" );
                break;
            case MessageType.Part:
                IPartMsg lazyLoadedPartMsg = (LazyLoadedPartMsg) message;
                Console.WriteLine( $"{lazyLoadedPartMsg.User} leaving {lazyLoadedPartMsg.Channel}" );
                break;
            case MessageType.Unknown:
                IUnknownMessage lazyLoadedUnknownMsg = (LazyLoadedUnknownMessage) message;
                Console.WriteLine( $"UnknownMessage: {lazyLoadedUnknownMsg}" );
                break;
            case MessageType.HostTarget:
                IHostTargetMsg hostTargetMsg = (LazyLoadedHostTargetMsg) message;
                Console.WriteLine( $"HostTargetMsg: {hostTargetMsg.HostingChannel}, {hostTargetMsg.NumberOfViewers}" );
                break;
            case MessageType.Reconnect:
                IReconnectMsg lazyLoadedReconnectMsg = (LazyLoadedReconnectMsg) message;
                Console.WriteLine( $"ReconnectMsg: {lazyLoadedReconnectMsg}" );
                break;
            case MessageType.CapReq:
                ICapReq lazyLoadedCapReqMsg = (LazyLoadedCapReq) message;
                Console.WriteLine( $"CapReq: {lazyLoadedCapReqMsg.RawMessage}" );
                break;
            case MessageType.JoinCount:
                IJoinCountMsg lazyLoadedJoinCountMsg = (LazyLoadedJoinCountMsg) message;
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
        IClearChat lazyLoadedClearChat
    ) {
        if( m_clearChat.Count == BUFFER_SIZE ) {
            await WriteToStore( m_clearChat, MessageType.ClearChat );
        }
        
        Console.WriteLine( $"ClearChat: On {lazyLoadedClearChat.Timestamp} the user {lazyLoadedClearChat.TargetUserId} was muted/banned for {lazyLoadedClearChat.Duration} seconds {lazyLoadedClearChat.Text}" );
        
        if( lazyLoadedClearChat.TargetUserId != null ) {
            m_clearChat.AddToBuffer( lazyLoadedClearChat.TargetUserId );
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
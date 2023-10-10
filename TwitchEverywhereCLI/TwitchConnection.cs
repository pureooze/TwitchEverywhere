using System.Text;
using TwitchEverywhere;
using TwitchEverywhere.Types;
using TwitchEverywhereCLI.Implementation;

namespace TwitchEverywhereCLI; 

internal class TwitchConnection {

    private readonly MessageBuffer m_messageBuffer = new( buffer: new StringBuilder() );
    private readonly MessageBuffer m_clearChat = new( buffer: new StringBuilder() );
    private readonly MessageBuffer m_clearMsg = new( buffer: new StringBuilder() );
    private readonly ICompressor m_compressor = new BrotliCompressor();
    private DateTime m_startTimestamp = DateTime.UtcNow;

    private const int BUFFER_SIZE = 3;

    public async Task Connect( TwitchConnectionOptions options ) {
        TwitchEverywhere.TwitchEverywhere twitchEverywhere = new( options );
        await twitchEverywhere.ConnectToChannel( MessageCallback );
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

    private void MessageCallback(
        Message message
    ) {
        switch( message.MessageType ) {
            case MessageType.PrivMsg: {
                PrivMsg privMsg = (PrivMsg) message;
                PrivMessageCallback( privMsg );
                break;
            }
            case MessageType.ClearChat: {
                ClearChat clearChatMsg = (ClearChat) message;
                ClearChatCallback( clearChatMsg );
                break;
            }
            case MessageType.ClearMsg: {
                ClearMsg clearMsg = (ClearMsg) message;
                ClearMsgCallback( clearMsg );
                break;
            }
            case MessageType.GlobalUserState:
                break;
            case MessageType.Notice:
                break;
            case MessageType.RoomState:
                break;
            case MessageType.UserNotice:
                break;
            case MessageType.UserState:
                break;
            case MessageType.Whisper:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private async void PrivMessageCallback(
        PrivMsg privMsg
    ) {
        if( m_messageBuffer.Count == BUFFER_SIZE ) {
            await WriteToStore( m_messageBuffer, MessageType.PrivMsg );
        }
        
        Console.WriteLine( $"Message: {privMsg}" );
        m_messageBuffer.AddToBuffer( privMsg.Text );
    }

    private async void ClearChatCallback(
        ClearChat clearChat
    ) {
        if( m_clearChat.Count == BUFFER_SIZE ) {
            await WriteToStore( m_clearChat, MessageType.ClearChat );
        }
        
        Console.WriteLine( $"ClearChat: On {clearChat.Timestamp} the user {clearChat.UserId} was muted/banned for {clearChat.Duration} seconds" );
        
        if( clearChat.UserId != null ) {
            m_clearChat.AddToBuffer( clearChat.UserId );
        }
    }
    
    private async void ClearMsgCallback(
        ClearMsg clearMsg
    ) {
        if( m_clearMsg.Count == BUFFER_SIZE ) {
            await WriteToStore( m_clearMsg, MessageType.ClearMsg );
        }
        
        Console.WriteLine( $"ClearMsg: On {clearMsg.Timestamp} the user {clearMsg.Login} had a message deleted for message {clearMsg.TargetMessageId}" );
        
        m_clearMsg.AddToBuffer( clearMsg.TargetMessageId );
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
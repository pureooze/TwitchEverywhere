using System.Text;
using TwitchEverywhere;
using TwitchEverywhere.Types;
using TwitchEverywhereCLI.Implementation;

namespace TwitchEverywhereCLI; 

internal class TwitchConnection {

    private readonly MessageBuffer m_messageBuffer = new( buffer: new StringBuilder() );
    private readonly MessageBuffer m_clearChat = new( buffer: new StringBuilder() );
    private readonly ICompressor m_compressor = new BrotliCompressor();
    private DateTime m_startTimestamp = DateTime.UtcNow;

    private const int BUFFER_SIZE = 3;

    public async Task Connect( TwitchConnectionOptions options ) {
        TwitchEverywhere.TwitchEverywhere twitchEverywhere = new( options );
        await twitchEverywhere.ConnectToChannel( PrivMessageCallback, ClearMessageCallback );
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
    
    private async void PrivMessageCallback(
        PrivMessage privMessage
    ) {
        if( m_messageBuffer.Count == BUFFER_SIZE ) {
            await WriteToStore( m_messageBuffer, MessageType.PrivMsg );
        }
        
        Console.WriteLine( $"Message: {privMessage.Text}" );
        m_messageBuffer.AddToBuffer( privMessage.Text );
    }

    private async void ClearMessageCallback(
        ClearMessage clearMessage
    ) {
        if( m_clearChat.Count == BUFFER_SIZE ) {
            await WriteToStore( m_messageBuffer, MessageType.ClearChat );
        }
        
        Console.WriteLine( $"Clear: On {clearMessage.Timestamp} the user {clearMessage.UserId} was muted/banned for {clearMessage.Duration} seconds" );
        
        if( clearMessage.UserId != null ) {
            m_clearChat.AddToBuffer( clearMessage.UserId );
        }
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
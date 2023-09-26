using System.Text;
using TwitchEverywhere;
using TwitchEverywhereCLI.Implementation;

namespace TwitchEverywhereCLI; 

internal class TwitchConnection {

    private readonly MessageBuffer m_messageBuffer = new( buffer: new StringBuilder() );
    private readonly ICompressor m_compressor = new BrotliCompressor();
    private DateTime m_startTimestamp = DateTime.UtcNow;

    private const int BUFFER_SIZE = 5;

    public async Task Connect( TwitchConnectionOptions options ) {
        TwitchEverywhere.TwitchEverywhere twitchEverywhere = new( options );
        await twitchEverywhere.ConnectToChannel( MessageCallback );
    }

    private async Task WriteMessagesToStore( StringBuilder buffer, DateTime startTimestamp ) {
        if( buffer.Length == 0 ) {
            return;
        }

        string rawData = buffer.ToString();
        byte[] byteBuffer = Encoding.UTF8.GetBytes( rawData );

        byte[] compressedData = await m_compressor.Compress( byteBuffer );
    
        string path = $"{startTimestamp.ToUniversalTime():yyyy-M-d_H-mm-ss}.csv";
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
        string message
    ) {
        Console.WriteLine( message );
        
        if( m_messageBuffer.Count == BUFFER_SIZE ) {
            StringBuilder tempBuffer = new( m_messageBuffer.ReadAsString() );
            DateTime tempStartTimestamp = new( m_startTimestamp.Ticks );
            m_messageBuffer.Clear();
            m_startTimestamp = DateTime.UtcNow;
            await WriteMessagesToStore( tempBuffer, tempStartTimestamp );
        }
        
        m_messageBuffer.AddToBuffer( message );
    }
}
using System.IO.Compression;

namespace TwitchEverywhere.Implementation; 

internal class BrotliCompressor : ICompressor {
    async Task<byte[]> ICompressor.Compress(
        byte[] byteBuffer
    ) {
        using MemoryStream outputStream = new();
        await using BrotliStream brotliStream = new( outputStream, CompressionLevel.SmallestSize );
        await brotliStream.WriteAsync(byteBuffer, 0, byteBuffer.Length, default);
        brotliStream.Close();
        return outputStream.ToArray();
    }
}
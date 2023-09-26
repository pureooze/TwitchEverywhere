namespace TwitchEverywhere; 

public interface ICompressor {
    Task<byte[]> Compress(
        byte[] byteBuffer
    );
}
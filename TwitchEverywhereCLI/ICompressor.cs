namespace TwitchEverywhereCLI; 

public interface ICompressor {
    Task<byte[]> Compress(
        byte[] byteBuffer
    );
}
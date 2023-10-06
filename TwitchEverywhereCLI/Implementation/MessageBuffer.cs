using System.Text;

namespace TwitchEverywhereCLI.Implementation; 

internal sealed class MessageBuffer {
    public MessageBuffer(
        StringBuilder buffer
    ) {
        Buffer = buffer;
        Count = 0;
    }

    public void AddToBuffer(
        string message
    ) {
        if( this.Count >= 0 ) {
            Buffer.Append( "," );
        }
        Buffer.Append( message );
        Count += 1;
    }

    public void Clear() {
        Buffer.Clear();
        Count = 0;
    }

    private StringBuilder Buffer { get; }
    public int Count { get; private set; }

    public string ReadAsString() {
        return Buffer.ToString();
    }
};
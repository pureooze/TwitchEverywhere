namespace TwitchEverywhere.Types.Messages; 

public class UnknownMessage : Message {

    public UnknownMessage(
        string message
    ) {
        Message = message;
    }
    
    public override MessageType MessageType => MessageType.Unknown;
    
    public string Message { get; }
}
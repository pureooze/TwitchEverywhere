using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedUnknownMessage : Message, IUnknownMessage {

    public LazyLoadedUnknownMessage(
        string message
    ) {
        Message = message;
    }
    
    public override MessageType MessageType => MessageType.Unknown;
    
    public string Message { get; }
}
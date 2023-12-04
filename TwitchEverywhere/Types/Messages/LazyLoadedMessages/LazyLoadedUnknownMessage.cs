using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedUnknownMessage : Message, IUnknownMessage {
    private readonly string m_message;

    public LazyLoadedUnknownMessage(
        string message
    ) {
        m_message = message;
    }
    
    public override MessageType MessageType => MessageType.Unknown;
    public override string RawMessage => m_message;
}
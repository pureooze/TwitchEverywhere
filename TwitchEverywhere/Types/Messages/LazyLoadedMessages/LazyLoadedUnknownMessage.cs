using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedUnknownMessage : IUnknownMessage {
    private readonly string m_message;

    public LazyLoadedUnknownMessage(
        string channel,
        string message
    ) {
        Channel = channel;
        m_message = message;
    }
    
    public MessageType MessageType => MessageType.Unknown;
    public string RawMessage => m_message;
    
    public string Channel { get; }
}
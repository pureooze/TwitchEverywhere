using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedUnknownMessage : Message, IUnknownMessage {
    private readonly string m_message;

    public ImmediateLoadedUnknownMessage(
        string message
    ) {
        m_message = message;
    }
    
    public override MessageType MessageType => MessageType.Unknown;

    public override string RawMessage => m_message;
}
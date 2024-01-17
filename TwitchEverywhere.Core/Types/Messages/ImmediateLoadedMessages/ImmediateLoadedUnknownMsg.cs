using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedUnknownMsg : IUnknownMsg {
    private readonly string m_message;

    public ImmediateLoadedUnknownMsg(
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
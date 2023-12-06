using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedReconnectMsg : IReconnectMsg {
    private readonly string m_message;

    public ImmediateLoadedReconnectMsg(
        string channel,
        string message
    ) {
        Channel = channel;
        m_message = message;
    }
    
    public MessageType MessageType => MessageType.Reconnect;
    
    public string RawMessage => m_message;
    
    public string Channel { get; }
}
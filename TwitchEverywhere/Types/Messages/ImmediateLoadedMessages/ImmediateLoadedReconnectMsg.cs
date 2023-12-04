using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedReconnectMsg : Message, IReconnectMsg {
    private readonly string m_message;

    public ImmediateLoadedReconnectMsg(
        string message
    ) {
        m_message = message;
    }
    
    public override MessageType MessageType => MessageType.Reconnect;
    
    public override string RawMessage => m_message;
}
using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages; 

public class ImmediateLoadedJoinMsg : IJoinMsg {
    private readonly string m_channel;
    private readonly string m_user;

    public ImmediateLoadedJoinMsg(
        string channel,
        string user
    ) {
        m_user = user;
        m_channel = channel;
    }

    public MessageType MessageType => MessageType.Join;
    public string RawMessage => GetRawMessage();
    public string Channel => m_channel;
    string IJoinMsg.User => m_user;
    
    private string GetRawMessage() {
        string message = string.Empty;
        
        message += $":{m_user} ";
        message += $"JOIN #{m_channel}\n";

        return message;
    }
}
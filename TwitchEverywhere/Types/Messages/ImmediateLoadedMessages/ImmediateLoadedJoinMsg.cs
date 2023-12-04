using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages; 

public class ImmediateLoadedJoinMsg : Message, IJoinMsg {
    private readonly string m_channel;
    private readonly string m_user;

    public ImmediateLoadedJoinMsg(
        string channel,
        string user
    ) {
        m_user = user;
        m_channel = channel;
    }

    public override MessageType MessageType => MessageType.Join;
    public override string RawMessage => GetRawMessage();
    string IJoinMsg.User => m_user;
    string IJoinMsg.Channel => m_channel;
    
    private string GetRawMessage() {
        string message = string.Empty;
        
        message += $":{m_user} ";
        message += $"JOIN #{m_channel}\n";

        return message;
    }
}
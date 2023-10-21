using TwitchEverywhere.Implementation.MessagePlugins;

namespace TwitchEverywhere.Types.Messages; 

public class PartMsg : Message {
    private readonly string m_message;

    public PartMsg(
        string message,
        string channel
    ) {
        m_message = message;
        Channel = channel;
    }
    
    public override MessageType MessageType => MessageType.Part;
    
    public string User => MessagePluginUtils.UserJoinPattern.Match( m_message ).Value;
    
    public string Channel { get; }
}
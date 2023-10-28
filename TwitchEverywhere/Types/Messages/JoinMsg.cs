using TwitchEverywhere.Implementation.MessagePlugins;

namespace TwitchEverywhere.Types.Messages; 

public class JoinMsg : Message {
    private readonly string m_message;

    public JoinMsg(
        string message,
        string channel
    ) {
        m_message = message;
        Channel = channel;
    }
    
    public override MessageType MessageType => MessageType.Join;

    public string User => MessagePluginUtils.UserJoinPattern.Match( m_message ).Value;

    public string Channel { get; }
}
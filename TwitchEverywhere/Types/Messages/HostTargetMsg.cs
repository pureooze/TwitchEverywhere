using TwitchEverywhere.Implementation.MessagePlugins;

namespace TwitchEverywhere.Types.Messages; 

public class HostTargetMsg : Message {
    private readonly string m_message;

    public HostTargetMsg(
        string message,
        string channel
    ) {
        m_message = message;
        HostingChannel = channel;
    }
    
    public override MessageType MessageType => MessageType.HostTarget;

    public string HostingChannel { get; }

    public string Channel => MessagePluginUtils.HostTargetPattern.Match( m_message ).Value[1..];
    
    public int NumberOfViewers => int.Parse( MessagePluginUtils.HostViewerCountPattern.Match( m_message ).Value );

    public bool IsHostingChannel => !String.Equals( Channel, "-" );
}
using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedPartMsg : Message, IPartMsg {
    private readonly string m_message;

    public LazyLoadedPartMsg(
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
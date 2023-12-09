using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;
using MessagePluginUtils = TwitchEverywhere.Implementation.MessagePluginUtils;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedPartMsg : IPartMsg {
    private readonly string m_message;

    public LazyLoadedPartMsg(
        string message,
        string channel
    ) {
        m_message = message;
        Channel = channel;
    }
    
    public MessageType MessageType => MessageType.Part;
    
    public string RawMessage => m_message;

    public string User => MessagePluginUtils.UserJoinPattern().Match( m_message ).Groups[1].Value;

    public string Channel { get; }
}
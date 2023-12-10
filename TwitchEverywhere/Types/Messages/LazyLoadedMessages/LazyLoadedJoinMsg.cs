using TwitchEverywhere.Types.Messages.Interfaces;
using MessagePluginUtils = TwitchEverywhere.Implementation.MessagePluginUtils;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedJoinMsg : IJoinMsg {
    private readonly string m_message;

    public LazyLoadedJoinMsg(
        string message,
        string channel
    ) {
        m_message = message;
        Channel = channel;
    }
    
    public MessageType MessageType => MessageType.Join;
    
    public string RawMessage => m_message;

    public string User => MessagePluginUtils.UserJoinPattern().Match( m_message ).Groups[1].Value;

    public string Channel { get; }
}
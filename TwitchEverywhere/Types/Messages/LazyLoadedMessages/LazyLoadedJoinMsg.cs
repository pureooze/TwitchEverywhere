using TwitchEverywhere.Types.Messages.Interfaces;
using MessagePluginUtils = TwitchEverywhere.Implementation.MessagePluginUtils;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedJoinMsg(
    string message,
    string channel
) : IJoinMsg {

    public MessageType MessageType => MessageType.Join;
    
    public string RawMessage => message;

    public string User => MessagePluginUtils.UserJoinPattern().Match( message ).Groups[1].Value;

    public string Channel { get; } = channel;
}
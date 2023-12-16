using TwitchEverywhere.Types.Messages.Interfaces;
using MessagePluginUtils = TwitchEverywhere.Implementation.MessagePluginUtils;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedPartMsg(
    string message,
    string channel
) : IPartMsg {

    public MessageType MessageType => MessageType.Part;
    
    public string RawMessage => message;

    public string User => MessagePluginUtils.UserJoinPattern().Match( message ).Groups[1].Value;

    public string Channel { get; } = channel;
}
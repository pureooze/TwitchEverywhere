using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedPartMsg(
    string message,
    string channel
) : IPartMsg {

    public MessageType MessageType => MessageType.Part;
    
    public string RawMessage => message;

    public string User => MessagePluginUtils.UserJoinPattern().Match( message ).Groups[1].Value;

    public string Channel { get; } = channel;
}
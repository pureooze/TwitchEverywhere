using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedRoomStateMsg(
    string channel,
    string message
) : IRoomStateMsg {

    public MessageType MessageType => MessageType.RoomState;
    
    public string RawMessage => message;
    
    public string Channel { get; } = channel;

    public bool EmoteOnly => MessagePluginUtils.GetValueIsPresentOrBoolean( message, MessagePluginUtils.EmoteOnlyPattern() );

    public int FollowersOnly => MessagePluginUtils.GetIntValueFromResponse( message, MessagePluginUtils.FollowersOnlyPattern() );
    
    public bool R9K => MessagePluginUtils.GetValueIsPresentOrBoolean( message, MessagePluginUtils.R9KPattern() );
    
    public string RoomId => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.RoomIdPattern() );
    
    public int Slow => MessagePluginUtils.GetIntValueFromResponse( message, MessagePluginUtils.SlowPattern() );
    
    public bool SubsOnly => MessagePluginUtils.GetValueIsPresentOrBoolean( message, MessagePluginUtils.SubsOnlyPattern() );
}
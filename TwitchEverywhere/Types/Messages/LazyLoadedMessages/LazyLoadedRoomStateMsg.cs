using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedRoomStateMsg : Message, IRoomStateMsg {
    private readonly string m_message;
    
    public LazyLoadedRoomStateMsg(
        string message
    ) {
        m_message = message;

    }

    public override MessageType MessageType => MessageType.RoomState;
    
    public override string RawMessage => m_message;
    
    public bool EmoteOnly => MessagePluginUtils.GetValueIsPresentOrBoolean( m_message, MessagePluginUtils.EmoteOnlyPattern );

    public int FollowersOnly => MessagePluginUtils.GetIntValueFromResponse( m_message, MessagePluginUtils.FollowersOnlyPattern );
    
    public bool R9K => MessagePluginUtils.GetValueIsPresentOrBoolean( m_message, MessagePluginUtils.R9KPattern );
    
    public string RoomId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.RoomIdPattern );
    
    public int Slow => MessagePluginUtils.GetIntValueFromResponse( m_message, MessagePluginUtils.SlowPattern );
    
    public bool SubsOnly => MessagePluginUtils.GetValueIsPresentOrBoolean( m_message, MessagePluginUtils.SubsOnlyPattern );
}
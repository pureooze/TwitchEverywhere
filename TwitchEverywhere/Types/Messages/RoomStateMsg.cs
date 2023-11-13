using TwitchEverywhere.Implementation.MessagePlugins;

namespace TwitchEverywhere.Types.Messages; 

public class RoomStateMsg : Message {
    private readonly string m_message;
    
    public RoomStateMsg(
        string message
    ) {
        m_message = message;

    }

    public override MessageType MessageType => MessageType.RoomState;
    
    public bool EmoteOnly => MessagePluginUtils.GetValueIsPresentOrBoolean( m_message, MessagePluginUtils.EmoteOnlyPattern );

    public int FollowersOnly => MessagePluginUtils.GetIntValueFromResponse( m_message, MessagePluginUtils.FollowersOnlyPattern );
    
    public bool R9K => MessagePluginUtils.GetValueIsPresentOrBoolean( m_message, MessagePluginUtils.R9KPattern );
    
    public string RoomId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.RoomIdPattern );
    
    public int Slow => MessagePluginUtils.GetIntValueFromResponse( m_message, MessagePluginUtils.SlowPattern );
    
    public bool SubsOnly => MessagePluginUtils.GetValueIsPresentOrBoolean( m_message, MessagePluginUtils.SubsOnlyPattern );
}
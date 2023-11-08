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
    
    public bool EmoteOnly => int.Parse( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.EmoteOnlyPattern ) ) == 1;

    public int FollowersOnly => int.Parse( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.FollowersOnlyPattern ) );
    
    public bool R9K => int.Parse( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.R9KPattern ) ) == 1;
    
    public string RoomId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.RoomIdPattern );
    
    public int Slow => int.Parse( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.SlowPattern ) );
    
    public bool SubsOnly => int.Parse( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.SubsOnlyPattern ) ) == 1;
}
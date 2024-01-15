using System.Text;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedRoomStateMsg( RawMessage response ) : IRoomStateMsg {
    private string m_tags;
    
    public MessageType MessageType => MessageType.RoomState;
    
    public string RawMessage => Encoding.UTF8.GetString( response.Data.Span );
    
    public string Channel => MessagePluginUtils.GetChannelFromMessage( response );

    public bool EmoteOnly {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueIsPresentOrBoolean( m_tags, MessagePluginUtils.EmoteOnlyPattern() );
        }
    }

    public int FollowersOnly {
        get {
            InitializeTags();
            return MessagePluginUtils.GetIntValueFromResponse( m_tags, MessagePluginUtils.FollowersOnlyPattern() );
        }
    }

    public bool R9K {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueIsPresentOrBoolean( m_tags, MessagePluginUtils.R9KPattern() );
        }
    }

    public string RoomId {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.RoomIdPattern() );
        }
    }

    public int Slow {
        get {
            InitializeTags();
            return MessagePluginUtils.GetIntValueFromResponse( m_tags, MessagePluginUtils.SlowPattern() );
        }
    }

    public bool SubsOnly {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueIsPresentOrBoolean( m_tags, MessagePluginUtils.SubsOnlyPattern() );
        }
    }

    private void InitializeTags() {

        if( !string.IsNullOrEmpty( m_tags ) ) {
            return;
        }

        m_tags = MessagePluginUtils.GetTagsFromMessage( response );
    }
}
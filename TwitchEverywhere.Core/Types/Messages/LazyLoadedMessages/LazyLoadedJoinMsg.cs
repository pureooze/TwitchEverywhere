using System.Text;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedJoinMsg (RawMessage response) : IJoinMsg {
    private readonly string m_message = Encoding.UTF8.GetString( response.Data.Span );
    private readonly string m_channel = "";
    private string m_tags;
    
    public MessageType MessageType => MessageType.Join;
    
    public string RawMessage => m_message;
    
    public string Channel => m_channel;

    public string User {
        get {
            InitializeTags();
            return MessagePluginUtils.UserJoinPattern().Match( m_tags ).Groups[1].Value;
        }
    }

    private void InitializeTags() {

        if( !string.IsNullOrEmpty( m_tags ) ) {
            return;
        }

        m_tags = MessagePluginUtils.GetTagsFromMessage( response );
    }
}
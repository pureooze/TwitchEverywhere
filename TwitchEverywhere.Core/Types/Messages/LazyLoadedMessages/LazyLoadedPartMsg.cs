using System.Text;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedPartMsg( RawMessage response ) : IPartMsg {
    private readonly string m_message = Encoding.UTF8.GetString( response.Data.Span );

    public MessageType MessageType => MessageType.Part;
    
    public string RawMessage => m_message;

    public string User => MessagePluginUtils.UserJoinPattern().Match( m_message ).Groups[1].Value;

    public string Channel => MessagePluginUtils.GetChannelFromMessage( response );
}
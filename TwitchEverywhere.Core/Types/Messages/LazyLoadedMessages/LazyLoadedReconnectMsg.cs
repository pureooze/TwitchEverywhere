using System.Text;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedReconnectMsg( RawMessage response ) : IReconnectMsg {

    public MessageType MessageType => MessageType.Reconnect;
    
    public string RawMessage => Encoding.UTF8.GetString( response.Data.Span );

    public string Channel => MessagePluginUtils.GetChannelFromMessage( response );
}
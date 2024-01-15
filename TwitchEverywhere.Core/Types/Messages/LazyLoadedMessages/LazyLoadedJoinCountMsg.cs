using System.Text;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

public class LazyLoadedJoinCountMsg( RawMessage response ) : IJoinCountMsg {
    private readonly string m_message = Encoding.UTF8.GetString( response.Data.Span );
    
    MessageType IMessage.MessageType => MessageType.JoinCount;

    string IMessage.RawMessage => m_message;

    string IMessage.Channel => MessagePluginUtils.GetChannelFromMessage( response );
}
using System.Text;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class UnknownMsg (RawMessage message) : IUnknownMsg {
    
    MessageType IMessage.MessageType => MessageType.Unknown;

    string IMessage.RawMessage => Encoding.UTF8.GetString( message.Data.Span );

    string IMessage.Channel => string.Empty;
}
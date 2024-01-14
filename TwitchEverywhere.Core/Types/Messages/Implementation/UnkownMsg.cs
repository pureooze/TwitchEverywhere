using System.Text;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class UnknownMsg (RawMessage message) : IUnknownMessage {
    private MessageType m_messageType;
    private string m_rawMessage;
    private string m_channel;
    
    MessageType IMessage.MessageType => MessageType.Unknown;

    string IMessage.RawMessage => Encoding.UTF8.GetString( message.Data.Span );

    string IMessage.Channel => "";
}
using System.Text;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

public class LazyLoadedCapReq(RawMessage response) : ICapReq {
    private readonly string m_message = Encoding.UTF8.GetString( response.Data.Span );
    private readonly string m_channel = "";

    MessageType IMessage.MessageType => MessageType.CapReq;

    string IMessage.RawMessage => m_message;

    string IMessage.Channel => m_channel;
}
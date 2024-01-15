using System.Text;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

public class LazyLoadedJoinEndMsg(
    RawMessage response
) : IJoinEndMsg {
    private readonly string m_channel = "";

    MessageType IMessage.MessageType => MessageType.JoinEnd;

    string IMessage.RawMessage => Encoding.UTF8.GetString( response.Data.Span );

    string IMessage.Channel => m_channel;
}
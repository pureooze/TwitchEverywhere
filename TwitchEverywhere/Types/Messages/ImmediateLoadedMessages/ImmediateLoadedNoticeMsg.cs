using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages; 

public class ImmediateLoadedNoticeMsg : Message, INoticeMsg {
    private readonly string m_message;

    public ImmediateLoadedNoticeMsg(
        string message,
        NoticeMsgIdType msgId,
        string targetUserId
    ) {
        m_message = message;
        MsgId = msgId;
        TargetUserId = targetUserId;
    }

    public override MessageType MessageType => MessageType.Notice;
    
    public override string RawMessage => m_message;

    public NoticeMsgIdType MsgId { get; }

    public string TargetUserId { get; }
    
}
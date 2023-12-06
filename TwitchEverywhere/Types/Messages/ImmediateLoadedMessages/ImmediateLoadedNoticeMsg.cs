using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages; 

public class ImmediateLoadedNoticeMsg : INoticeMsg {

    public ImmediateLoadedNoticeMsg(
        string channel,
        string message,
        NoticeMsgIdType msgId,
        string targetUserId
    ) {
        Channel = channel;
        RawMessage = message;
        MsgId = msgId;
        TargetUserId = targetUserId;
    }

    public MessageType MessageType => MessageType.Notice;
    
    public string RawMessage { get; }

    public string Channel { get; }

    public NoticeMsgIdType MsgId { get; }

    public string TargetUserId { get; }
    
}
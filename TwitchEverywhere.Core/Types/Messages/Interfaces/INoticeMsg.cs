namespace TwitchEverywhere.Core.Types.Messages.Interfaces; 

public interface INoticeMsg : IMessage {
    NoticeMsgIdType MsgId { get; }
    
    string TargetUserId { get; }
    
    string RawMessage { get; }
}
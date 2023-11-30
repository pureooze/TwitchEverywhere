namespace TwitchEverywhere.Types.Messages.Interfaces; 

public interface INoticeMsg {
    MessageType MessageType { get; }

    NoticeMsgIdType MsgId { get; }
    
    string TargetUserId { get; }
    
    string RawMessage { get; }
}
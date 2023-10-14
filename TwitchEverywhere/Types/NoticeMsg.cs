namespace TwitchEverywhere.Types; 

public record NoticeMsg(
    NoticeMsgIdType MsgId,
    string TargetUserId
) : Message ( MessageType.Notice );
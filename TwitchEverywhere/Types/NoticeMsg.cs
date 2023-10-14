namespace TwitchEverywhere.Types; 

public record NoticeMsg(
    string MsgId,
    string TargetUserId
) : Message ( MessageType.Notice );
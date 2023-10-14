namespace TwitchEverywhere.Types; 

public record Notice(
    string MsgId,
    string TargetUserId
) : Message ( MessageType.Notice );
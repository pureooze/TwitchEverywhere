namespace TwitchEverywhere.Types; 

public enum MessageType {
    ClearChat,
    ClearMsg,
    GlobalUserState,
    Notice,
    PrivMsg,
    RoomState,
    UserNotice,
    UserState,
    Whisper,
    Join,
    Part,
    HostTarget,
    Reconnect,
    CapReq,
    JoinCount,
    JoinEnd,
    Unknown
}
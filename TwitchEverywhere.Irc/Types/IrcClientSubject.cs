using System.Reactive.Subjects;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Irc.Types;

public sealed record IrcClientSubject(
        Subject<ICapReq> CapReqSubject,
        Subject<IClearChatMsg> ClearChatSubject,
        Subject<IClearMsg> ClearMsgSubject,
        Subject<IGlobalUserStateMsg> GlobalUserStateSubject,
        Subject<IHostTargetMsg> HostTargetSubject,
        Subject<IJoinCountMsg> JoinCountSubject,
        Subject<IJoinEndMsg> JoinEndSubject,
        Subject<IJoinMsg> JoinSubject,
        Subject<INoticeMsg> NoticeSubject,
        Subject<IPartMsg> PartSubject,
        Subject<IPrivMsg> PrivMsgSubject,
        Subject<IReconnectMsg> ReconnectSubject,
        Subject<IRoomStateMsg> RoomStateSubject,
        Subject<IUnknownMsg> UnknownSubject,
        Subject<IUserNoticeMsg> UserNoticeSubject,
        Subject<IUserStateMsg> UserStateSubject,
        Subject<IWhisperMsg> WhisperSubject
);
using System.Reactive.Subjects;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Irc.Types;

public sealed record IrcClientObservable(
        Subject<ICapReq> CapReqSubject,
        IObservable<ICapReq> CapReqObservable,
        IObservable<IClearChatMsg> ClearChatObservable,
        IObservable<IClearMsg> ClearMsgObservable,
        IObservable<IGlobalUserStateMsg> GlobalUserStateObservable,
        IObservable<IHostTargetMsg> HostTargetObservable,
        IObservable<IJoinCountMsg> JoinCountObservable,
        IObservable<IJoinEndMsg> JoinEndObservable,
        Subject<IJoinMsg> JoinSubject,
        IObservable<IJoinMsg> JoinObservable,
        IObservable<INoticeMsg> NoticeObservable,
        IObservable<IPartMsg> PartObservable,
        IObservable<IPrivMsg> PrivMsgObservable,
        IObservable<IReconnectMsg> ReconnectObservable,
        IObservable<IRoomStateMsg> RoomStateObservable,
        IObservable<IUnknownMsg> UnknownObservable,
        IObservable<IUserNoticeMsg> UserNoticeObservable,
        IObservable<IUserStateMsg> UserStateObservable,
        IObservable<IWhisperMsg> WhisperObservable
);
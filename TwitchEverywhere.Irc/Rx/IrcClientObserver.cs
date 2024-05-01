using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Irc.Rx;

public sealed record IrcClientObserver(
    List<IObserver<ICapReq>>? CapReqObservables = null,
    List<IObserver<IClearChatMsg>>? ClearChatObservables = null,
    List<IObserver<IClearMsg>>? ClearMsgObservables = null,
    List<IObserver<IGlobalUserStateMsg>>? GlobalUserStateObservables = null,
    List<IObserver<IHostTargetMsg>>? HostTargetObservables = null,
    List<IObserver<IJoinCountMsg>>? JoinCountObservables = null,
    List<IObserver<IJoinEndMsg>>? JoinEndObservables = null,
    List<IObserver<IJoinMsg>>? JoinObservables = null,
    List<IObserver<INoticeMsg>>? NoticeObservables = null,
    List<IObserver<IPartMsg>>? PartObservables = null,
    List<IObserver<IPrivMsg>>? PrivMsgObservables = null,
    List<IObserver<IReconnectMsg>>? ReconnectObservables = null,
    List<IObserver<IRoomStateMsg>>? RoomStateObservables = null,
    List<IObserver<IUnknownMsg>>? UnknownObservables = null,
    List<IObserver<IUserNoticeMsg>>? UserNoticeObservables = null,
    List<IObserver<IUserStateMsg>>? UserStateObservables = null,
    List<IObserver<IWhisperMsg>>? WhisperObservables = null
);
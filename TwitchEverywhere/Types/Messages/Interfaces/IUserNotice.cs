using System.Collections.Immutable;

namespace TwitchEverywhere.Types.Messages.Interfaces;

public interface IUserNotice : IMessage {
    IImmutableList<Badge> BadgeInfo { get; }
    IImmutableList<Badge> Badges { get; }
    string Color { get; }
    string DisplayName { get; }
    IImmutableList<Emote> Emotes { get; }
    string Id { get; }
    string Login { get; }
    bool Mod { get; }
    MsgIdType MsgId { get; }
    string RoomId { get; }
    bool Subscriber { get; }
    string SystemMsg { get; }
    DateTime Timestamp { get; }
    bool Turbo { get; }
    string UserId { get; }
    UserType UserType { get; }
    int? MsgParamCumulativeMonths { get; }
    string MsgParamDisplayName { get; }
    string MsgParamLogin { get; }
    int? MsgParamMonths { get; }
    int? MsgParamPromoGiftTotal { get; }
    string MsgParamPromoName { get; }
    string MsgParamRecipientDisplayName { get; }
    string MsgParamRecipientId { get; }
    string MsgParamRecipientUserName { get; }
    string MsgParamSenderLogin { get; }
    string MsgParamSenderName { get; }
    bool? MsgParamShouldShareStreak { get; }
    int? MsgParamStreakMonths { get; }
    MsgSubPlanType MsgParamSubPlan { get; }
    string MsgParamSubPlanName { get; }
    int? MsgParamViewerCount { get; }
    string MsgParamRitualName { get; }
    string MsgParamThreshold { get; }
    int? MsgParamGiftMonths { get; }
}
using System.Collections.Immutable;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedUserNoticeMsg : Message, IUserNotice {
    private readonly IImmutableList<Badge> m_badgeInfo;
    private readonly IImmutableList<Badge> m_badges;
    private readonly string m_color;
    private readonly string m_displayName;
    private readonly IImmutableList<Emote> m_emotes;
    private readonly string m_id;
    private readonly string m_login;
    private readonly bool m_mod;
    private readonly MsgIdType m_msgId;
    private readonly string m_roomId;
    private readonly bool m_subscriber;
    private readonly string m_systemMsg;
    private readonly DateTime m_timestamp;
    private readonly bool m_turbo;
    private readonly string m_userId;
    private readonly UserType m_userType;
    private readonly int? m_msgParamCumulativeMonths;
    private readonly string m_msgParamDisplayName;
    private readonly string m_msgParamLogin;
    private readonly int? m_msgParamMonths;
    private readonly int? m_msgParamPromoGiftTotal;
    private readonly string m_msgParamPromoName;
    private readonly string m_msgParamRecipientDisplayName;
    private readonly string m_msgParamRecipientId;
    private readonly string m_msgParamRecipientUserName;
    private readonly string m_msgParamSenderLogin;
    private readonly string m_msgParamSenderName;
    private readonly bool? m_msgParamShouldShareStreak;
    private readonly int? m_msgParamStreakMonths;
    private readonly MsgSubPlanType m_msgParamSubPlan;
    private readonly string m_msgParamSubPlanName;
    private readonly int? m_msgParamViewerCount;
    private readonly string m_msgParamRitualName;
    private readonly string m_msgParamThreshold;
    private readonly int? m_msgParamGiftMonths;

    public ImmediateLoadedUserNoticeMsg(
        IImmutableList<Badge> badgeInfo,
        IImmutableList<Badge> badges,
        string color,
        string displayName,
        IImmutableList<Emote> emotes,
        string id,
        string login,
        bool mod,
        MsgIdType msgId,
        string roomId,
        bool subscriber,
        string systemMsg,
        DateTime timestamp,
        bool turbo,
        string userId,
        UserType userType,
        int? msgParamCumulativeMonths,
        string msgParamDisplayName,
        string msgParamLogin,
        int? msgParamMonths,
        int? msgParamPromoGiftTotal,
        string msgParamPromoName,
        string msgParamRecipientDisplayName,
        string msgParamRecipientId,
        string msgParamRecipientUserName,
        string msgParamSenderLogin,
        string msgParamSenderName,
        bool? msgParamShouldShareStreak,
        int? msgParamStreakMonths,
        MsgSubPlanType msgParamSubPlan,
        string msgParamSubPlanName,
        int? msgParamViewerCount,
        string msgParamRitualName,
        string msgParamThreshold,
        int? msgParamGiftMonths
    ) {
        m_badgeInfo = badgeInfo;
        m_badges = badges;
        m_color = color;
        m_displayName = displayName;
        m_emotes = emotes;
        m_id = id;
        m_login = login;
        m_mod = mod;
        m_msgId = msgId;
        m_roomId = roomId;
        m_subscriber = subscriber;
        m_systemMsg = systemMsg;
        m_timestamp = timestamp;
        m_turbo = turbo;
        m_userId = userId;
        m_userType = userType;
        m_msgParamCumulativeMonths = msgParamCumulativeMonths;
        m_msgParamDisplayName = msgParamDisplayName;
        m_msgParamLogin = msgParamLogin;
        m_msgParamMonths = msgParamMonths;
        m_msgParamPromoGiftTotal = msgParamPromoGiftTotal;
        m_msgParamPromoName = msgParamPromoName;
        m_msgParamRecipientDisplayName = msgParamRecipientDisplayName;
        m_msgParamRecipientId = msgParamRecipientId;
        m_msgParamRecipientUserName = msgParamRecipientUserName;
        m_msgParamSenderLogin = msgParamSenderLogin;
        m_msgParamSenderName = msgParamSenderName;
        m_msgParamShouldShareStreak = msgParamShouldShareStreak;
        m_msgParamStreakMonths = msgParamStreakMonths;
        m_msgParamSubPlan = msgParamSubPlan;
        m_msgParamSubPlanName = msgParamSubPlanName;
        m_msgParamViewerCount = msgParamViewerCount;
        m_msgParamRitualName = msgParamRitualName;
        m_msgParamThreshold = msgParamThreshold;
        m_msgParamGiftMonths = msgParamGiftMonths;
    }
    public override MessageType MessageType => MessageType.UserNotice;

    IImmutableList<Badge> IUserNotice.BadgeInfo => m_badgeInfo;

    IImmutableList<Badge> IUserNotice.Badges => m_badges;

    string IUserNotice.Color => m_color;

    string IUserNotice.DisplayName => m_displayName;

    IImmutableList<Emote> IUserNotice.Emotes => m_emotes;

    string IUserNotice.Id => m_id;

    string IUserNotice.Login => m_login;

    bool IUserNotice.Mod => m_mod;

    MsgIdType IUserNotice.MsgId => m_msgId;

    string IUserNotice.RoomId => m_roomId;

    bool IUserNotice.Subscriber => m_subscriber;

    string IUserNotice.SystemMsg => m_systemMsg;

    DateTime IUserNotice.Timestamp => m_timestamp;

    bool IUserNotice.Turbo => m_turbo;

    string IUserNotice.UserId => m_userId;

    UserType IUserNotice.UserType => m_userType;

    int? IUserNotice.MsgParamCumulativeMonths => m_msgParamCumulativeMonths;

    string IUserNotice.MsgParamDisplayName => m_msgParamDisplayName;

    string IUserNotice.MsgParamLogin => m_msgParamLogin;

    int? IUserNotice.MsgParamMonths => m_msgParamMonths;

    int? IUserNotice.MsgParamPromoGiftTotal => m_msgParamPromoGiftTotal;

    string IUserNotice.MsgParamPromoName => m_msgParamPromoName;

    string IUserNotice.MsgParamRecipientDisplayName => m_msgParamRecipientDisplayName;

    string IUserNotice.MsgParamRecipientId => m_msgParamRecipientId;

    string IUserNotice.MsgParamRecipientUserName => m_msgParamRecipientUserName;

    string IUserNotice.MsgParamSenderLogin => m_msgParamSenderLogin;

    string IUserNotice.MsgParamSenderName => m_msgParamSenderName;

    bool? IUserNotice.MsgParamShouldShareStreak => m_msgParamShouldShareStreak;

    int? IUserNotice.MsgParamStreakMonths => m_msgParamStreakMonths;

    MsgSubPlanType IUserNotice.MsgParamSubPlan => m_msgParamSubPlan;

    string IUserNotice.MsgParamSubPlanName => m_msgParamSubPlanName;

    int? IUserNotice.MsgParamViewerCount => m_msgParamViewerCount;

    string IUserNotice.MsgParamRitualName => m_msgParamRitualName;

    string IUserNotice.MsgParamThreshold => m_msgParamThreshold;

    int? IUserNotice.MsgParamGiftMonths => m_msgParamGiftMonths;
}
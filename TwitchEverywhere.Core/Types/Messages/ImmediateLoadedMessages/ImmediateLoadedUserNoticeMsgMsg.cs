using System.Collections.Immutable;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedUserNoticeMsgMsg : IUserNoticeMsg {
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

    public ImmediateLoadedUserNoticeMsgMsg(
        string channel,
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
        Channel = channel;
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
    public MessageType MessageType => MessageType.UserNotice;
    
    public string RawMessage => "";
    
    public string Channel { get; }

    IImmutableList<Badge> IUserNoticeMsg.BadgeInfo => m_badgeInfo;

    IImmutableList<Badge> IUserNoticeMsg.Badges => m_badges;

    string IUserNoticeMsg.Color => m_color;

    string IUserNoticeMsg.DisplayName => m_displayName;

    IImmutableList<Emote> IUserNoticeMsg.Emotes => m_emotes;

    string IUserNoticeMsg.Id => m_id;

    string IUserNoticeMsg.Login => m_login;

    bool IUserNoticeMsg.Mod => m_mod;

    MsgIdType IUserNoticeMsg.MsgId => m_msgId;

    string IUserNoticeMsg.RoomId => m_roomId;

    bool IUserNoticeMsg.Subscriber => m_subscriber;

    string IUserNoticeMsg.SystemMsg => m_systemMsg;

    DateTime IUserNoticeMsg.Timestamp => m_timestamp;

    bool IUserNoticeMsg.Turbo => m_turbo;

    string IUserNoticeMsg.UserId => m_userId;

    UserType IUserNoticeMsg.UserType => m_userType;

    int? IUserNoticeMsg.MsgParamCumulativeMonths => m_msgParamCumulativeMonths;

    string IUserNoticeMsg.MsgParamDisplayName => m_msgParamDisplayName;

    string IUserNoticeMsg.MsgParamLogin => m_msgParamLogin;

    int? IUserNoticeMsg.MsgParamMonths => m_msgParamMonths;

    int? IUserNoticeMsg.MsgParamPromoGiftTotal => m_msgParamPromoGiftTotal;

    string IUserNoticeMsg.MsgParamPromoName => m_msgParamPromoName;

    string IUserNoticeMsg.MsgParamRecipientDisplayName => m_msgParamRecipientDisplayName;

    string IUserNoticeMsg.MsgParamRecipientId => m_msgParamRecipientId;

    string IUserNoticeMsg.MsgParamRecipientUserName => m_msgParamRecipientUserName;

    string IUserNoticeMsg.MsgParamSenderLogin => m_msgParamSenderLogin;

    string IUserNoticeMsg.MsgParamSenderName => m_msgParamSenderName;

    bool? IUserNoticeMsg.MsgParamShouldShareStreak => m_msgParamShouldShareStreak;

    int? IUserNoticeMsg.MsgParamStreakMonths => m_msgParamStreakMonths;

    MsgSubPlanType IUserNoticeMsg.MsgParamSubPlan => m_msgParamSubPlan;

    string IUserNoticeMsg.MsgParamSubPlanName => m_msgParamSubPlanName;

    int? IUserNoticeMsg.MsgParamViewerCount => m_msgParamViewerCount;

    string IUserNoticeMsg.MsgParamRitualName => m_msgParamRitualName;

    string IUserNoticeMsg.MsgParamThreshold => m_msgParamThreshold;

    int? IUserNoticeMsg.MsgParamGiftMonths => m_msgParamGiftMonths;
}
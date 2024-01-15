using System.Collections.Immutable;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class UserNoticeMsg : IUserNoticeMsg {
    private MessageType m_messageType;
    private string m_rawMessage;
    private string m_channel;
    private IImmutableList<Badge> m_badgeInfo;
    private IImmutableList<Badge> m_badges;
    private string m_color;
    private string m_displayName;
    private IImmutableList<Emote> m_emotes;
    private string m_id;
    private string m_login;
    private bool m_mod;
    private MsgIdType m_msgId;
    private string m_roomId;
    private bool m_subscriber;
    private string m_systemMsg;
    private DateTime m_timestamp;
    private bool m_turbo;
    private string m_userId;
    private UserType m_userType;
    private int? m_msgParamCumulativeMonths;
    private string m_msgParamDisplayName;
    private string m_msgParamLogin;
    private int? m_msgParamMonths;
    private int? m_msgParamPromoGiftTotal;
    private string m_msgParamPromoName;
    private string m_msgParamRecipientDisplayName;
    private string m_msgParamRecipientId;
    private string m_msgParamRecipientUserName;
    private string m_msgParamSenderLogin;
    private string m_msgParamSenderName;
    private bool? m_msgParamShouldShareStreak;
    private int? m_msgParamStreakMonths;
    private MsgSubPlanType m_msgParamSubPlan;
    private string m_msgParamSubPlanName;
    private int? m_msgParamViewerCount;
    private string m_msgParamRitualName;
    private string m_msgParamThreshold;
    private int? m_msgParamGiftMonths;
    private readonly IUserNoticeMsg m_inner;
    
    public UserNoticeMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedUserNoticeMsg( message );
    }
    
    MessageType IMessage.MessageType => MessageType.UserNotice;

    string IMessage.RawMessage => m_rawMessage;

    string IMessage.Channel => m_channel;

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
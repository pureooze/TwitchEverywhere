using System.Collections.Immutable;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedGlobalUserState : Message, IGlobalUserState {
    
    private readonly IImmutableList<Badge> m_badges;
    private readonly IImmutableList<Badge> m_badgeInfo;
    private readonly string m_color;
    private readonly string m_displayName;
    private readonly IImmutableList<string> m_emoteSets;
    private readonly bool m_turbo;
    private readonly string m_userId;
    private readonly UserType m_userType;

    public ImmediateLoadedGlobalUserState(
        IImmutableList<Badge> badges,
        IImmutableList<Badge> badgeInfo,
        string color,
        string displayName,
        IImmutableList<string> emoteSets,
        bool turbo,
        string userId,
        UserType userType
    ) {
        m_badges = badges;
        m_badgeInfo = badgeInfo;
        m_color = color;
        m_displayName = displayName;
        m_emoteSets = emoteSets;
        m_turbo = turbo;
        m_userId = userId;
        m_userType = userType;
    }

    public override MessageType MessageType => MessageType.GlobalUserState;
    
    IImmutableList<Badge> IGlobalUserState.Badges => m_badges;

    IImmutableList<Badge> IGlobalUserState.BadgeInfo => m_badgeInfo;

    string IGlobalUserState.Color => m_color;

    string IGlobalUserState.DisplayName => m_displayName;

    IImmutableList<string> IGlobalUserState.EmoteSets => m_emoteSets;

    bool IGlobalUserState.Turbo => m_turbo;

    string IGlobalUserState.UserId => m_userId;

    UserType IGlobalUserState.UserType => m_userType;
}
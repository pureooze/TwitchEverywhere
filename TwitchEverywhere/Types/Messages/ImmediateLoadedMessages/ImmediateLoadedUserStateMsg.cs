using System.Collections.Immutable;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedUserStateMsg : IUserStateMsg {
    private readonly IImmutableList<Badge> m_badgeInfo;
    private readonly IImmutableList<Badge> m_badges;
    private readonly string m_color;
    private readonly string m_displayName;
    private readonly IImmutableList<string> m_emoteSets;
    private readonly string m_id;
    private readonly bool m_mod;
    private readonly bool m_subscriber;
    private readonly bool m_turbo;
    private readonly UserType m_userType;

    public ImmediateLoadedUserStateMsg(
        string channel,
        IImmutableList<Badge>? badgeInfo = null,
        IImmutableList<Badge>? badges = null,
        string? color = null,
        string? displayName = null,
        IImmutableList<string>? emoteSets = null,
        string? id = null,
        bool mod = default,
        bool subscriber = default,
        bool turbo = default,
        UserType userType = default
    ) {
        Channel = channel;
        m_badgeInfo = badgeInfo ?? new ImmutableArray<Badge>();
        m_badges = badges ?? new ImmutableArray<Badge>();
        m_color = color ?? string.Empty;
        m_displayName = displayName ?? string.Empty;
        m_emoteSets = emoteSets ?? new ImmutableArray<string>();
        m_id = id ?? string.Empty;
        m_mod = mod;
        m_subscriber = subscriber;
        m_turbo = turbo;
        m_userType = userType;
    }

    public MessageType MessageType => MessageType.UserState;
    
    public string RawMessage => "";
    
    public string Channel { get; }

    IImmutableList<Badge> IUserStateMsg.BadgeInfo => m_badgeInfo;

    IImmutableList<Badge> IUserStateMsg.Badges => m_badges;

    string IUserStateMsg.Color => m_color;

    string IUserStateMsg.DisplayName => m_displayName;

    IImmutableList<string> IUserStateMsg.EmoteSets => m_emoteSets;

    string IUserStateMsg.Id => m_id;

    bool IUserStateMsg.Mod => m_mod;

    bool IUserStateMsg.Subscriber => m_subscriber;

    bool IUserStateMsg.Turbo => m_turbo;

    UserType IUserStateMsg.UserType => m_userType;
}
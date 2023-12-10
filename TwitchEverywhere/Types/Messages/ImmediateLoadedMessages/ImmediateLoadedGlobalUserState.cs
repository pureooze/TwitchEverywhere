using System.Collections.Immutable;
using TwitchEverywhere.Types.Messages.Interfaces;
using MessagePluginUtils = TwitchEverywhere.Implementation.MessagePluginUtils;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedGlobalUserState : IGlobalUserState {
    
    private readonly IImmutableList<Badge> m_badges;
    private readonly IImmutableList<Badge> m_badgeInfo;
    private readonly string m_color;
    private readonly string m_displayName;
    private readonly IImmutableList<string> m_emoteSets;
    private readonly bool m_turbo;
    private readonly string m_userId;
    private readonly UserType m_userType;
    private readonly string m_channel;

    public ImmediateLoadedGlobalUserState(
        string channel,
        IImmutableList<Badge>? badges = null,
        IImmutableList<Badge>? badgeInfo = null,
        string? color = null,
        string? displayName = null,
        IImmutableList<string>? emoteSets = null,
        bool? turbo = null,
        string? userId = null,
        UserType? userType = null
    ) {
        m_channel = channel;
        m_badges = badges ?? ImmutableArray<Badge>.Empty;
        m_badgeInfo = badgeInfo ?? ImmutableArray<Badge>.Empty;
        m_color = color ?? string.Empty;
        m_displayName = displayName ?? string.Empty;
        m_emoteSets = emoteSets ?? ImmutableArray<string>.Empty;
        m_turbo = turbo ?? default;
        m_userId = userId ?? string.Empty;
        m_userType = userType ?? UserType.Normal;
    }

    public MessageType MessageType => MessageType.GlobalUserState;
    
    public string RawMessage => GetRawMessage();
    
    public string Channel => m_channel;

    IImmutableList<Badge> IGlobalUserState.Badges => m_badges;

    IImmutableList<Badge> IGlobalUserState.BadgeInfo => m_badgeInfo;

    string IGlobalUserState.Color => m_color;

    string IGlobalUserState.DisplayName => m_displayName;

    IImmutableList<string> IGlobalUserState.EmoteSets => m_emoteSets;

    bool IGlobalUserState.Turbo => m_turbo;

    string IGlobalUserState.UserId => m_userId;

    UserType IGlobalUserState.UserType => m_userType;
    
    private string GetRawMessage() {
        string message = "@";

        if( m_badges.Any() ) {
            message += SerializeProperty( MessagePluginUtils.Properties.BadgeInfo, () => string.Join( ",", m_badgeInfo.Select( b => $"{b.Name}/{b.Version}" ) ) );
            message += SerializeProperty( MessagePluginUtils.Properties.Badges, () => string.Join( ",", m_badges.Select( b => $"{b.Name}/{b.Version}" ) ) );
        }

        if( !string.IsNullOrEmpty( m_color ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.Color, () => m_color );
        }
        
        if( !string.IsNullOrEmpty( m_displayName ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.DisplayName, () => m_displayName );
        }
        
        if( m_emoteSets.Any() ) {
            message += SerializeProperty( MessagePluginUtils.Properties.EmoteSets, () => string.Join( ",", m_emoteSets.Select( b => b ) ) );
        }
        
        message += SerializeProperty( MessagePluginUtils.Properties.Turbo, () => m_turbo ? "1" : "0" );
        
        if( !string.IsNullOrEmpty( m_userId ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.UserId, () => m_userId );
        }
        
        if( m_userType != UserType.Normal ) {
            message += SerializeProperty( MessagePluginUtils.Properties.UserType, () => MessagePluginUtils.GetUserTypeText( m_userType ) );
        }

        message = message.Substring(0, message.Length - 1);

        message += $" :tmi.twitch.tv {MessageType.ToString().ToUpper()}";
        
        
        return message;
    }
    
    private string SerializeProperty(
        MessagePluginUtils.Properties property,
        Func<string> serializer
    ) {

        return string.Format( MessagePluginUtils.GetPropertyAsString( property ), serializer() );
    }
}
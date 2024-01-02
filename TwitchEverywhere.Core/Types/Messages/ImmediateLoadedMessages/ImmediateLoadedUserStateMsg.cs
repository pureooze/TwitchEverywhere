using System.Collections.Immutable;
using TwitchEverywhere.Core.Types.Messages.Interfaces;


namespace TwitchEverywhere.Core.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedUserStateMsg : IUserStateMsg {
    private readonly string m_channel;
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
        m_channel = channel;
        m_badgeInfo = badgeInfo ?? ImmutableArray<Badge>.Empty;
        m_badges = badges ?? ImmutableArray<Badge>.Empty;
        m_color = color ?? string.Empty;
        m_displayName = displayName ?? string.Empty;
        m_emoteSets = emoteSets ?? ImmutableArray<string>.Empty;
        m_id = id ?? string.Empty;
        m_mod = mod;
        m_subscriber = subscriber;
        m_turbo = turbo;
        m_userType = userType;
    }

    public MessageType MessageType => MessageType.UserState;
    
    public string RawMessage => GetRawMessage();

    public string Channel => m_channel;

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
        
        message += SerializeProperty( MessagePluginUtils.Properties.Mod, () => m_mod ? "1" : "0" );
        
        message += SerializeProperty( MessagePluginUtils.Properties.Subscriber, () => m_subscriber ? "1" : "0" );
        
        message += SerializeProperty( MessagePluginUtils.Properties.Turbo, () => m_turbo ? "1" : "0" );
        
        if( m_userType != UserType.Normal ) {
            message += SerializeProperty( MessagePluginUtils.Properties.UserType, () => MessagePluginUtils.GetUserTypeText( m_userType ) );
        }
        
        message = message.Substring( 0, message.Length - 1 );

        message += $" :tmi.twitch.tv {MessageType.ToString().ToUpper()} #{m_channel}";

        return message;
    }

    private string SerializeProperty(
        MessagePluginUtils.Properties property,
        Func<string> serializer
    ) {

        return string.Format( MessagePluginUtils.GetPropertyAsString( property ), serializer() );
    }
}
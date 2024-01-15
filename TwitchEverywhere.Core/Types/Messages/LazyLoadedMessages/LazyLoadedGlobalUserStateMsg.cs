using System.Collections.Immutable;
using System.Text;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedGlobalUserStateMsg( RawMessage response ) : IGlobalUserStateMsg {
    private readonly string m_message = Encoding.UTF8.GetString( response.Data.Span );
    private string m_tags;
    
    public MessageType MessageType => MessageType.GlobalUserState;
    
    public string RawMessage => m_message;
    
    public string Channel => MessagePluginUtils.GetChannelFromMessage( response );

    public IImmutableList<Badge> Badges {
        get {
            InitializeTags();
            string badges = MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.BadgesPattern() );
            return GetBadges( badges );
        }
    }
    
    public IImmutableList<Badge> BadgeInfo {
        get {
            InitializeTags();
            string badges = MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.BadgeInfoPattern() );
            return GetBadges( badges );
        }
    }
    
    public string Color {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.ColorPattern() );
        }
    }

    public string DisplayName {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.DisplayNamePattern() );
        }
    }

    public IImmutableList<string> EmoteSets {
        get {
            InitializeTags();
            string emotesText = MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.EmoteSetsPattern() );
            return GetEmoteSetsFromText( emotesText );
        }
    }
    
    public bool Turbo {
        get {
            InitializeTags();
            return int.TryParse( MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.TurboPattern() ), out _ );
        }
    }

    public string UserId {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.UserIdPattern() );
        }
    }

    public UserType UserType {
        get {
            InitializeTags();
            return GetUserType( 
                MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.UserTypePattern() ) 
            );
        }
    }

    private void InitializeTags() {

        if( !string.IsNullOrEmpty( m_tags ) ) {
            return;
        }

        m_tags = MessagePluginUtils.GetTagsFromMessage( response );
    }
    
    private IImmutableList<string> GetEmoteSetsFromText(
        string emotesText
    ) {
        return string.IsNullOrEmpty( emotesText ) 
            ? new List<string>().ToImmutableList() 
            : emotesText.Split( "," ).ToImmutableList();

    }

    private static UserType GetUserType(
        string userTypeText
    ) {
        return userTypeText switch {
            "mod" => UserType.Mod,
            "admin" => UserType.Admin,
            "global_mod" => UserType.GlobalMod,
            "staff" => UserType.Staff,
            _ => UserType.Normal
        };
    }
    
    private static IImmutableList<Badge> GetBadges(
        string badges
    ) {
        string[] badgeList = badges.Split( ',' );

        if( string.IsNullOrEmpty( badges ) ) {
            return Array.Empty<Badge>().ToImmutableList();
        }

        List<Badge> parsedBadges = new();

        foreach (string badge in badgeList) {
            string[] badgeInfo = badge.Split( '/' );

            if( badgeInfo.Length == 2 ) {
                parsedBadges.Add( new Badge( Name: badgeInfo[0], Version: badgeInfo[1] ) );
            }
        }

        return parsedBadges.ToImmutableList();
    }
}
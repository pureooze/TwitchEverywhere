using System.Collections.Immutable;
using TwitchEverywhere.Implementation.MessagePlugins;

namespace TwitchEverywhere.Types.Messages; 

public class UserStateMsg : Message {
    private readonly string m_message;

    public UserStateMsg(
        string message
    ) {
        m_message = message;
    }
    
    public override MessageType MessageType => MessageType.UserState;
    
    public IImmutableList<Badge> BadgeInfo {
        get {
            string badges = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.BadgeInfoPattern );
            return GetBadges( badges );
        }
    }
    
    public IImmutableList<Badge> Badges {
        get {
            string badges = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.BadgesPattern );
            return GetBadges( badges );
        }
    }
    
    public string? Color => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ColorPattern );
    
    public string DisplayName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.DisplayNamePattern );
    
    public IImmutableList<string> EmoteSets {
        get {
            string emotesText = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.EmoteSetsPattern );
            return GetEmoteSetsFromText( emotesText );
        }
    }
    
    public string Id => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.IdPattern );
    
    public bool Mod => int.Parse( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ModPattern ) ) == 1;
    
    public bool Subscriber => int.Parse( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.SubscriberPattern ) ) == 1;
    
    public bool Turbo => int.Parse( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.TurboPattern ) ) == 1;
    
    public UserType UserType => GetUserType( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.UserTypePattern ) );
    
    private IImmutableList<string> GetEmoteSetsFromText(
        string emotesText
    ) {
        string[] sets = emotesText.Split( "," );
        return sets.ToImmutableList();
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

        for( int index = 0; index < badgeList.Length; index++ ) {
            string badge = badgeList[index];
            string[] badgeInfo = badge.Split( '/' );

            if( badgeInfo.Length == 2 ) {
                parsedBadges.Add( new Badge( Name: badgeInfo[0], Version: badgeInfo[1] ) );
            }
        }

        return parsedBadges.ToImmutableList();
    }
}
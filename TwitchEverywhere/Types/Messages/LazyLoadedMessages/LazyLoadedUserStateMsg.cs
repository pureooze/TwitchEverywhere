using System.Collections.Immutable;
using TwitchEverywhere.Types.Messages.Interfaces;
using MessagePluginUtils = TwitchEverywhere.Implementation.MessagePluginUtils;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedUserStateMsg(
    string channel,
    string message
) : IUserStateMsg {

    public MessageType MessageType => MessageType.UserState;
    
    public string RawMessage => message;
    
    public string Channel { get; } = channel;

    public IImmutableList<Badge> BadgeInfo {
        get {
            string badges = MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.BadgeInfoPattern() );
            return GetBadges( badges );
        }
    }
    
    public IImmutableList<Badge> Badges {
        get {
            string badges = MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.BadgesPattern() );
            return GetBadges( badges );
        }
    }
    
    public string Color => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.ColorPattern() );
    
    public string DisplayName => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.DisplayNamePattern() );
    
    public IImmutableList<string> EmoteSets {
        get {
            string emotesText = MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.EmoteSetsPattern() );
            return GetEmoteSetsFromText( emotesText );
        }
    }
    
    public string Id => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.IdPattern() );
    
    public bool Mod => MessagePluginUtils.GetValueIsPresentOrBoolean( message, MessagePluginUtils.ModPattern() );
    
    public bool Subscriber => MessagePluginUtils.GetValueIsPresentOrBoolean( message, MessagePluginUtils.SubscriberPattern() );
    
    public bool Turbo => MessagePluginUtils.GetValueIsPresentOrBoolean( message, MessagePluginUtils.TurboPattern() );
    
    public UserType UserType => GetUserType( MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.UserTypePattern() ) );
    
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

        foreach (string badge in badgeList) {
            string[] badgeInfo = badge.Split( '/' );

            if( badgeInfo.Length == 2 ) {
                parsedBadges.Add( new Badge( Name: badgeInfo[0], Version: badgeInfo[1] ) );
            }
        }

        return parsedBadges.ToImmutableList();
    }
}
using System.Collections.Immutable;
using System.Text;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedUserStateMsg(
    RawMessage response
) : IUserStateMsg {

    private readonly string m_message = Encoding.UTF8.GetString( response.Data.Span );
    private readonly string m_channel = "";
    private string m_tags;
    
    public MessageType MessageType => MessageType.UserState;
    
    public string RawMessage => m_message;
    
    public string Channel => m_channel;

    public IImmutableList<Badge> BadgeInfo {
        get {
            InitializeTags();
            return GetBadges(MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.BadgeInfoPattern()));
        }
    }
    
    public IImmutableList<Badge> Badges {
        get {
            InitializeTags();
            return GetBadges(MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.BadgesPattern()));
        }
    }
    
    public string Color {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.ColorPattern());
        }
    }
    
    public string DisplayName {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.DisplayNamePattern());
        }
    }
    
    public IImmutableList<string> EmoteSets {
        get {
            InitializeTags();
            return GetEmoteSetsFromText(
                MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.EmoteSetsPattern())
            );
        }
    }
    
    public string Id {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.IdPattern());
        }
    }
    
    public bool Mod {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueIsPresentOrBoolean(m_tags, MessagePluginUtils.ModPattern());
        }
    }
    
    public bool Subscriber {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueIsPresentOrBoolean(m_tags, MessagePluginUtils.SubscriberPattern());
        }
    }
    
    public bool Turbo {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueIsPresentOrBoolean(m_tags, MessagePluginUtils.TurboPattern());
        }
    }
    
    public UserType UserType {
        get {
            InitializeTags();
            return GetUserType(
                MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.UserTypePattern())
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
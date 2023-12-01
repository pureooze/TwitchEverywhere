using System.Collections.Immutable;
using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedWhisperMsg : Message, IWhisperMsg {
    private readonly string m_message;

    public LazyLoadedWhisperMsg(
        string message
    ) {
        m_message = message;

    }
    
    public override MessageType MessageType => MessageType.Whisper;
    
    public IImmutableList<Badge> Badges {
        get {
            string badges = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.BadgesPattern );
            return GetBadges( badges );
        }
    }
    
    public string Color => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ColorPattern );
    
    public string DisplayName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.DisplayNamePattern );
    
    public IImmutableList<Emote>? Emotes {
        get {
            string emotesText = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.EmotesPattern );
            return GetEmotesFromText( emotesText );
        }
    }
    
    public string MsgId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MessageIdPattern );
    
    public string ThreadId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ThreadIdPattern );
    
    public bool Turbo => MessagePluginUtils.GetValueIsPresentOrBoolean( m_message, MessagePluginUtils.TurboPattern );
    
    public string UserId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.UserIdPattern );

    public UserType UserType => GetUserType( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.UserTypePattern ) );

    public string FromUser => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.FromUserPattern );
    
    public string ToUser => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ToUserPattern );
    
    public string Text => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgTextPattern );
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
    
    private static IImmutableList<Emote>? GetEmotesFromText(
        string emotesText
    ) {

        if( string.IsNullOrEmpty( emotesText ) ) {
            return null;
        }

        List<Emote> emotes = new();
        string[] separatedRawEmotes = emotesText.Split( "/" );

        foreach (string rawEmote in separatedRawEmotes) {
            string[] separatedEmote = rawEmote.Split( ":" );
            string[] separatedEmoteLocationGroup = separatedEmote[1].Split( "," );

            foreach (string locationGroup in separatedEmoteLocationGroup) {
                string[] separatedEmoteLocation = locationGroup.Split( "-" );
                
                emotes.Add(
                    new Emote( 
                        separatedEmote[0], 
                        int.Parse( separatedEmoteLocation[0] ), 
                        int.Parse( separatedEmoteLocation[1] )
                    )
                );
            }
        }
        
        
        return emotes.ToImmutableList();
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
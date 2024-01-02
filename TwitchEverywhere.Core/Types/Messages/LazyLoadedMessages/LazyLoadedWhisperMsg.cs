using System.Collections.Immutable;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedWhisperMsg(
    string channel,
    string message
) : IWhisperMsg {

    public MessageType MessageType => MessageType.Whisper;
    
    public string RawMessage => message;
    
    public string Channel { get; } = channel;

    public IImmutableList<Badge> Badges {
        get {
            string badges = MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.BadgesPattern() );
            return GetBadges( badges );
        }
    }
    
    public string Color => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.ColorPattern() );
    
    public string DisplayName => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.DisplayNamePattern() );
    
    public IImmutableList<Emote>? Emotes {
        get {
            string emotesText = MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.EmotesPattern() );
            return GetEmotesFromText( emotesText );
        }
    }
    
    public string MsgId => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.MessageIdPattern() );
    
    public string ThreadId => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.ThreadIdPattern() );
    
    public bool Turbo => MessagePluginUtils.GetValueIsPresentOrBoolean( message, MessagePluginUtils.TurboPattern() );
    
    public string UserId => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.UserIdPattern() );

    public UserType UserType => GetUserType( MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.UserTypePattern() ) );

    public string FromUser => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.FromUserPattern() );
    
    public string ToUser => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.ToUserPattern() );
    
    public string Text => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.MsgTextPattern() );
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
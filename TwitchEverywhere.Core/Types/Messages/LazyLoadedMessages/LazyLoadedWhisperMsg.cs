using System.Collections.Immutable;
using System.Text;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

public class LazyLoadedWhisperMsg( RawMessage response ) : IWhisperMsg {

    private string m_tags;

    public MessageType MessageType => MessageType.Whisper;

    public string RawMessage => Encoding.UTF8.GetString( response.Data.Span );

    public string Channel => MessagePluginUtils.GetChannelFromMessage( response );

    public IImmutableList<Badge> Badges {
        get {
            InitializeTags();
            string badges = MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.BadgesPattern() );
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

    public IImmutableList<Emote>? Emotes {
        get {
            InitializeTags();
            string emotesText = MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.EmotesPattern() );
            return GetEmotesFromText( emotesText );
        }
    }

    public string MsgId {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.MessageIdPattern() );
        }
    }

    public string ThreadId {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.ThreadIdPattern() );
        }
    }

    public bool Turbo {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueIsPresentOrBoolean( m_tags, MessagePluginUtils.TurboPattern() );
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
            return GetUserType( MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.UserTypePattern() ) );
        }
    }

    public string FromUser {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse( RawMessage, MessagePluginUtils.FromUserPattern() );
        }
    }

    public string ToUser {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse( RawMessage, MessagePluginUtils.ToUserPattern() );
        }
    }

    public string Text {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse( RawMessage, MessagePluginUtils.MsgTextPattern() );
        }
    }

    private void InitializeTags() {

        if( !string.IsNullOrEmpty( m_tags ) ) {
            return;
        }

        m_tags = MessagePluginUtils.GetTagsFromMessage( response );
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
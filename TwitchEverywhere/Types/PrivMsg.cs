using System.Collections.Immutable;
using System.Text.RegularExpressions;
using TwitchEverywhere.Implementation.MessagePlugins;

namespace TwitchEverywhere.Types; 

public class PrivMsg : Message {
    public PrivMsg(
        string channel,
        string message,
        TimeSpan sinceStartOfStream
    ) {
        m_channel = channel;
        m_message = message;
        SinceStartOfStream = sinceStartOfStream;
    }

    public override MessageType MessageType => MessageType.PrivMsg;

    public IImmutableList<Badge> Badges {
        get {
            string badges = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.BadgesPattern );
            return GetBadges( badges );
        }
    }

    public string Bits {
        get {
            string[] bitsArray = MessagePluginUtils.BitsPattern
                .Match( m_message )
                .Value
                .Split( "=" );
            
            string bits = string.Empty;
            if( bits.Length > 1 ) {
                bits = bitsArray.ElementAt( 1 ).TrimEnd( ';' );
            }

            return bits;
        }
    }

    public string? Color => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ColorPattern );

    public string DisplayName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.DisplayNamePattern );

    public IImmutableList<Emote>? Emotes {
        get {
            string emotesText = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.EmotesPattern );
            return GetEmotesFromText( emotesText );
        }
    }

    public string Id => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.IdPattern );

    public bool Mod => int.Parse( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ModPattern ) ) == 1;

    public long? PinnedChatPaidAmount {
        get {
            string pinnedChatPaidAmount = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.PinnedChatPaidExponentPattern );
            return string.IsNullOrEmpty( pinnedChatPaidAmount ) ? null : long.Parse( pinnedChatPaidAmount );
        }
    }

    public string PinnedChatPaidCurrency => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.PinnedChatPaidCurrencyPattern );

    public long? PinnedChatPaidExponent {
        get {
            string pinnedChatPaidExponent = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.PinnedChatPaidExponentPattern );
            return string.IsNullOrEmpty( pinnedChatPaidExponent ) ? null : int.Parse( pinnedChatPaidExponent );
        }
    }

    public PinnedChatPaidLevel? PinnedChatPaidLevel {
        get {
            string pinnedChatPaidLevelText = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.PinnedChatPaidLevelPattern );
            return GetPinnedChatPaidLevelType( pinnedChatPaidLevelText );
        }
    }

    public bool PinnedChatPaidIsSystemMessage => !string.IsNullOrEmpty( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.PinnedChatPaidIsSystemMessagePattern ) );

    public string ReplyParentMsgId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ReplyParentMsgIdPattern );

    public string ReplyParentUserId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ReplyParentUserIdPattern );

    public string ReplyParentUserLogin => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ReplyParentUserLoginPattern );

    public string ReplyParentDisplayName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ReplyParentDisplayNamePattern );

    public string ReplyThreadParentMsg => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ReplyThreadParentMsgPattern );

    public string RoomId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.RoomIdPattern );

    public bool Subscriber => int.Parse( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.SubscriberPattern ) ) == 1;

    public DateTime Timestamp {
        get {
            long rawTimestamp = Convert.ToInt64(
                MessagePluginUtils.MessageTimestampPattern.Match( m_message ).Value
                    .Split( "=" )[1]
                    .TrimEnd( ';' )
            );

            return DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;
        }
    }

    public bool Turbo => int.Parse( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.TurboPattern ) ) == 1;

    public string UserId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.UserIdPattern );

    public UserType UserType => GetUserType( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.UserTypePattern ) );

    public bool Vip => !string.IsNullOrEmpty( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.VipPattern ) );

    public TimeSpan SinceStartOfStream { get; }

    public string Text => m_message.Split( $"PRIVMSG #{m_channel} :" )[1].Trim( '\r', '\n' );

    private readonly string m_channel;
    private readonly string m_message;

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

    private IImmutableList<Emote>? GetEmotesFromText(
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

        for( int index = 0; index < badgeList.Length; index++ ) {
            string badge = badgeList[index];
            string[] badgeInfo = badge.Split( '/' );

            if( badgeInfo.Length == 2 ) {
                parsedBadges.Add( new Badge( Name: badgeInfo[0], Version: badgeInfo[1] ) );
            }
        }

        return parsedBadges.ToImmutableList();
    }
    
    private static PinnedChatPaidLevel? GetPinnedChatPaidLevelType(
        string pinnedChatPaidLevelText
    ) {
        return pinnedChatPaidLevelText switch {
            "ONE" => Types.PinnedChatPaidLevel.One,
            "TWO" => Types.PinnedChatPaidLevel.Two,
            "THREE" => Types.PinnedChatPaidLevel.Three,
            "FOUR" => Types.PinnedChatPaidLevel.Four,
            "FIVE" => Types.PinnedChatPaidLevel.Five,
            "SIX" => Types.PinnedChatPaidLevel.Six,
            "SEVEN" => Types.PinnedChatPaidLevel.Seven,
            "EIGHT" => Types.PinnedChatPaidLevel.Eight,
            "NINE" => Types.PinnedChatPaidLevel.Nine,
            "TEN" => Types.PinnedChatPaidLevel.Ten,
            _ => null
        };
    }
}
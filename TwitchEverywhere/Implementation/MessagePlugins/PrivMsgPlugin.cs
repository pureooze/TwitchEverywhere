using System.Collections.Immutable;
using System.Text.RegularExpressions;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.Implementation.MessagePlugins; 

public class PrivMsgPlugin : IMessagePlugin {

    private readonly DateTime m_startTimestamp;

    public PrivMsgPlugin(
        IDateTimeService dateTimeService
    ) {
        m_startTimestamp = dateTimeService.GetStartTime();
    }

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $" PRIVMSG #{channel}" );
    }

    Message IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        string[] segments = response.Split( $"PRIVMSG #{channel} :" );
        
        string displayName = GetValueFromResponse( response, MessagePluginRegex.DisplayNamePattern );
        string badges = GetValueFromResponse( response, MessagePluginRegex.BadgesPattern );

        string emotesText = GetValueFromResponse( response, MessagePluginRegex.EmotesPattern );
        IImmutableList<Emote>? emotes = GetEmotesFromText( emotesText );
        
        string id = GetValueFromResponse( response, MessagePluginRegex.IdPattern );
        string pinnedChatPaidAmount = GetValueFromResponse( response, MessagePluginRegex.PinnedChatPaidAmountPattern );
        string pinnedChatPaidCurrency = GetValueFromResponse( response, MessagePluginRegex.PinnedChatPaidCurrencyPattern );
        string pinnedChatPaidExponent = GetValueFromResponse( response, MessagePluginRegex.PinnedChatPaidExponentPattern );

        string pinnedChatPaidLevelText = GetValueFromResponse( response, MessagePluginRegex.PinnedChatPaidLevelPattern );
        PinnedChatPaidLevel? pinnedChatPaidLevel = GetPinnedChatPaidLevelType( pinnedChatPaidLevelText );
        
        string pinnedChatPaidIsSystemMessage = GetValueFromResponse( response, MessagePluginRegex.PinnedChatPaidIsSystemMessagePattern );
        string replyParentMsgId = GetValueFromResponse( response, MessagePluginRegex.ReplyParentMsgIdPattern );
        string replyParentUserId = GetValueFromResponse( response, MessagePluginRegex.ReplyParentUserIdPattern );
        string replyParentUserLogin = GetValueFromResponse( response, MessagePluginRegex.ReplyParentUserLoginPattern );        
        string replyParentDisplayName = GetValueFromResponse( response, MessagePluginRegex.ReplyParentDisplayNamePattern );
        string replyThreadParentMsg = GetValueFromResponse( response, MessagePluginRegex.ReplyThreadParentMsgPattern );
        string roomId = GetValueFromResponse( response, MessagePluginRegex.RoomIdPattern );
        string subscriber = GetValueFromResponse( response, MessagePluginRegex.SubscriberPattern );
        string turbo = GetValueFromResponse( response, MessagePluginRegex.TurboPattern );
        string userId = GetValueFromResponse( response, MessagePluginRegex.UserIdPattern );
        
        string userTypeText = GetValueFromResponse( response, MessagePluginRegex.UserTypePattern );
        UserType userType = GetUserType( userTypeText );
        
        string vip = GetValueFromResponse( response, MessagePluginRegex.VipPattern );
        string isMod = GetValueFromResponse( response, MessagePluginRegex.ModPattern );
        string color = GetValueFromResponse( response, MessagePluginRegex.ColorPattern );
        
        string[] bitsArray = MessagePluginRegex.BitsPattern
            .Match( response )
            .Value
            .Split( "=" );

        string bits = string.Empty;
        if( bits.Length > 1 ) {
            bits = bitsArray.ElementAt( 1 ).TrimEnd( ';' );
            Console.WriteLine($"BITS: {bits}");
        }
        
        IImmutableList<Badge> parsedBadges = GetBadges( badges );

        long rawTimestamp = Convert.ToInt64(
            MessagePluginRegex.MessageTimestampPattern.Match( response ).Value
            .Split( "=" )[1]
            .TrimEnd( ';' )
        );

        DateTime messageTimestamp = DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;
        TimeSpan timeSinceStartOfStream = messageTimestamp - m_startTimestamp;
        
        string message = segments[1].Trim( '\r', '\n' );
        
        if( segments.Length <= 1 ) {
            throw new UnexpectedUserMessageException();
        }

        return new PrivMsg(
            Badges: parsedBadges,
            Bits: bits,
            Color: color,
            DisplayName: displayName,
            Emotes: emotes,
            Id: id,
            Mod: int.Parse( isMod ) == 1,
            PinnedChatPaidAmount: string.IsNullOrEmpty( pinnedChatPaidAmount ) ? null : long.Parse( pinnedChatPaidAmount ),
            PinnedChatPaidCurrency: pinnedChatPaidCurrency,
            PinnedChatPaidExponent: string.IsNullOrEmpty( pinnedChatPaidExponent ) ? null : int.Parse( pinnedChatPaidExponent ),
            PinnedChatPaidLevel: pinnedChatPaidLevel,
            PinnedChatPaidIsSystemMessage: !string.IsNullOrEmpty( pinnedChatPaidIsSystemMessage ),
            ReplyParentMsgId: replyParentMsgId,
            ReplyParentUserId: replyParentUserId,
            ReplyParentUserLogin: replyParentUserLogin,
            ReplyParentDisplayName: replyParentDisplayName,
            ReplyThreadParentMsg: replyThreadParentMsg,
            RoomId: roomId,
            Subscriber: int.Parse( subscriber ) == 1,
            Timestamp: messageTimestamp,
            Turbo: int.Parse( turbo ) == 1,
            UserId: userId,
            UserType: userType,
            Vip: !string.IsNullOrEmpty( vip ),
            SinceStartOfStream: timeSinceStartOfStream,
            Text: message
        );
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
    
    private static PinnedChatPaidLevel? GetPinnedChatPaidLevelType(
        string pinnedChatPaidLevelText
    ) {
        return pinnedChatPaidLevelText switch {
            "ONE" => PinnedChatPaidLevel.One,
            "TWO" => PinnedChatPaidLevel.Two,
            "THREE" => PinnedChatPaidLevel.Three,
            "FOUR" => PinnedChatPaidLevel.Four,
            "FIVE" => PinnedChatPaidLevel.Five,
            "SIX" => PinnedChatPaidLevel.Six,
            "SEVEN" => PinnedChatPaidLevel.Seven,
            "EIGHT" => PinnedChatPaidLevel.Eight,
            "NINE" => PinnedChatPaidLevel.Nine,
            "TEN" => PinnedChatPaidLevel.Ten,
            _ => null
        };
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
    
    private static string GetValueFromResponse(
        string response,
        Regex pattern
    ) {
        Match match = pattern
            .Match( response );

        string result = string.Empty;
        if( match.Success ) {
            result = match.Value.Split( "=" )[1].TrimEnd( ';' );
        }

        return result;
    }
}
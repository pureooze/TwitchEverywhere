using System.Collections.Immutable;
using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedUserNotice : Message, IUserNotice {
    private readonly string m_message;

    public LazyLoadedUserNotice(
        string message
    ) {
        m_message = message;
    }
    
    public override MessageType MessageType => MessageType.UserNotice;
    
    public override string RawMessage => m_message;
    
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
    
    public string Color => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.ColorPattern );
    
    public string DisplayName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.DisplayNamePattern );
    
    public IImmutableList<Emote> Emotes {
        get {
            string emotesText = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.EmotesPattern );
            return GetEmotesFromText( emotesText );
        }
    }
    
    public string Id => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.IdPattern );
    
    public string Login => MessagePluginUtils.LoginPattern
        .Match( m_message )
        .Value
        .Split( "=" )[1]
        .TrimEnd( ';' );
    
    public bool Mod => MessagePluginUtils.GetValueIsPresentOrBoolean( m_message, MessagePluginUtils.ModPattern );
    
    public MsgIdType MsgId => GetMessageIdType( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgIdPattern ) );
    
    public string RoomId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.RoomIdPattern );
    
    public bool Subscriber => MessagePluginUtils.GetValueIsPresentOrBoolean( m_message, MessagePluginUtils.SubscriberPattern );
    
    public string SystemMsg => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.SystemMessagePattern );
    
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
    
    public bool Turbo => MessagePluginUtils.GetValueIsPresentOrBoolean( m_message, MessagePluginUtils.TurboPattern );
    
    public string UserId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.UserIdPattern );
    
    public UserType UserType => GetUserType( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.UserTypePattern ) );
    
    public int? MsgParamCumulativeMonths {
        get {
            string value = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamCumulativeMonthsPattern );

            if( string.IsNullOrEmpty( value ) ) {
                return null;
            }
            
            return int.Parse( value );
        }
    }

    public string MsgParamDisplayName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamDisplayNamePattern );
    
    public string MsgParamLogin => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamLoginPattern );
    
    public int? MsgParamMonths {
        get {
            string value = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamMonthsPattern );
            
            if( string.IsNullOrEmpty( value ) ) {
                return null;
            }
            
            return int.Parse( value );
        }
    }

    public int? MsgParamPromoGiftTotal {
        get {
            string value = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamPromoGiftTotalPattern );
            
            if( string.IsNullOrEmpty( value ) ) {
                return null;
            }
            
            return int.Parse( value );
        }
    }

    public string MsgParamPromoName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamPromoNamePattern );
    
    public string MsgParamRecipientDisplayName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamRecipientDisplayNamePattern );
    
    public string MsgParamRecipientId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamRecipientIdPattern );
    
    public string MsgParamRecipientUserName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamRecipientUserNamePattern );
    
    public string MsgParamSenderLogin => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamSenderLoginPattern );
    
    public string MsgParamSenderName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamSenderNamePattern );
    
    public bool? MsgParamShouldShareStreak => int.TryParse( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamShouldShareStreakPattern ), out _ );

    public int? MsgParamStreakMonths {
        get {
            string value = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamStreakMonthsPattern );
            
            if( string.IsNullOrEmpty( value ) ) {
                return null;
            }
            
            return int.Parse( value );
        }
    }

    public MsgSubPlanType MsgParamSubPlan => GetMessageSubPlanType( MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamSubPlanPattern ) );
    
    public string MsgParamSubPlanName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamSubPlanNamePattern );
    
    public int? MsgParamViewerCount {
        get {
            string value = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamViewerCountPattern );
            
            if( string.IsNullOrEmpty( value ) ) {
                return null;
            }
            
            return int.Parse( value );
        }
    }

    public string MsgParamRitualName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamRitualNamePattern );
    
    public string MsgParamThreshold => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamThresholdPattern );
    
    public int? MsgParamGiftMonths {
        get {
            string value = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgParamGiftMonthsPattern );
            
            if( string.IsNullOrEmpty( value ) ) {
                return null;
            }
            
            return int.Parse( value );
        }
    }

    private static MsgSubPlanType GetMessageSubPlanType(
        string msgSubPlan
    ) {
        return msgSubPlan switch {
            "Prime" => MsgSubPlanType.Prime,
            "1000" => MsgSubPlanType.Tier1,
            "2000" => MsgSubPlanType.Tier2,
            "3000" => MsgSubPlanType.Tier3,
            _ => MsgSubPlanType.None
        };
    }
    
    private static MsgIdType GetMessageIdType(
        string msgId
    ) {
        return msgId switch {
            "sub" => MsgIdType.Sub,
            "resub" => MsgIdType.ReSub,
            "subgift" => MsgIdType.SubGift,
            "submysterygift" => MsgIdType.SubMysteryGift,
            "giftpaidupgrade" => MsgIdType.GiftPaidUpgrade,
            "rewardgift" => MsgIdType.RewardGift,
            "anongiftpaidupgrade" => MsgIdType.AnonGiftPaidUpgrade,
            "raid" => MsgIdType.Raid,
            "unraid" => MsgIdType.UnRaid,
            "ritual" => MsgIdType.Ritual,
            "bitsbadgetier" => MsgIdType.BitsBadgeTier,
            _ => throw new ArgumentOutOfRangeException( nameof(msgId), msgId, null )
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
    
    private static IImmutableList<Emote> GetEmotesFromText(
        string emotesText
    ) {

        List<Emote> emotes = new();
        
        if( string.IsNullOrEmpty( emotesText ) ) {
            return emotes.ToImmutableList();
        }

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
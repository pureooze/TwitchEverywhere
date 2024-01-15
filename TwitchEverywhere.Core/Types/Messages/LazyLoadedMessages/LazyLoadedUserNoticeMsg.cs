using System.Collections.Immutable;
using System.Text;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedUserNoticeMsg( 
    RawMessage response
) : IUserNoticeMsg {
    
    private readonly string m_message = Encoding.UTF8.GetString( response.Data.Span );
    private readonly string m_channel = "";
    private string m_tags;
    
    public MessageType MessageType => MessageType.UserNotice;
    
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
    
    public IImmutableList<Emote> Emotes {
        get {
            InitializeTags();
            return GetEmotesFromText(MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.EmotesPattern()));
        }
    }
    
    public string Id {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.IdPattern());
        }
    }
    
    public string Login {
        get {
            InitializeTags();
            return MessagePluginUtils.LoginPattern().Match(m_tags).Value.Split("=")[1].TrimEnd(';');
        }
    }
    
    public bool Mod {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueIsPresentOrBoolean(m_tags, MessagePluginUtils.ModPattern());
        }
    }
    
    public MsgIdType MsgId {
        get {
            InitializeTags();
            return GetMessageIdType(MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgIdPattern()));
        }
    }
    
    public string RoomId {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.RoomIdPattern());
        }
    }
    
    public bool Subscriber {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueIsPresentOrBoolean(m_tags, MessagePluginUtils.SubscriberPattern());
        }
    }
    
    public string SystemMsg {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.SystemMessagePattern());
        }
    }
    
    public DateTime Timestamp {
        get {
            InitializeTags();
            long rawTimestamp = Convert.ToInt64(MessagePluginUtils.MessageTimestampPattern().Match(m_tags).Value.Split("=")[1].TrimEnd(';'));
            return DateTimeOffset.FromUnixTimeMilliseconds(rawTimestamp).UtcDateTime;
        }
    }
    
    public bool Turbo {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueIsPresentOrBoolean(m_tags, MessagePluginUtils.TurboPattern());
        }
    }

    public string UserId {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.UserIdPattern());
        }
    }

    public UserType UserType {
        get {
            InitializeTags();
            return GetUserType(MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.UserTypePattern()));
        }
    }
    
    public int? MsgParamCumulativeMonths {
        get {
            InitializeTags();
            string value = MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamCumulativeMonthsPattern());
            return string.IsNullOrEmpty(value) ? null : int.Parse(value);
        }
    }

    public string MsgParamDisplayName {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamDisplayNamePattern());
        }
    }
    
    public string MsgParamLogin {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamLoginPattern());
        }
    }
    
    public int? MsgParamMonths {
        get {
            InitializeTags();
            string value = MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamMonthsPattern());
            return string.IsNullOrEmpty(value) ? null : int.Parse(value);
        }
    }

    public int? MsgParamPromoGiftTotal {
        get {
            InitializeTags();
            string value = MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamPromoGiftTotalPattern());
            return string.IsNullOrEmpty(value) ? null : int.Parse(value);
        }
    }

    public string MsgParamPromoName {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamPromoNamePattern());
        }
    }
    
    public string MsgParamRecipientDisplayName {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamRecipientDisplayNamePattern());
        }
    }
    
    public string MsgParamRecipientId {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamRecipientIdPattern());
        }
    }
    
    public string MsgParamRecipientUserName {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamRecipientUserNamePattern());
        }
    }
    
    public string MsgParamSenderLogin {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamSenderLoginPattern());
        }
    }
    
    public string MsgParamSenderName {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamSenderNamePattern());
        }
    }
    
    public bool? MsgParamShouldShareStreak {
        get {
            InitializeTags();
            return int.TryParse(MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamShouldShareStreakPattern()), out _);
        }
    }

    public int? MsgParamStreakMonths {
        get {
            InitializeTags();
            string value = MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamStreakMonthsPattern());
            return string.IsNullOrEmpty(value) ? null : int.Parse(value);
        }
    }

    public MsgSubPlanType MsgParamSubPlan {
        get {
            InitializeTags();
            return GetMessageSubPlanType(MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamSubPlanPattern()));
        }
    }
    
    public string MsgParamSubPlanName {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamSubPlanNamePattern());
        }
    }
    
    public int? MsgParamViewerCount {
        get {
            InitializeTags();
            string value = MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamViewerCountPattern());
            return string.IsNullOrEmpty(value) ? null : int.Parse(value);
        }
    }

    public string MsgParamRitualName {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamRitualNamePattern());
        }
    }
    
    public string MsgParamThreshold {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamThresholdPattern());
        }
    }
    
    public int? MsgParamGiftMonths {
        get {
            InitializeTags();
            string value = MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgParamGiftMonthsPattern());
            return string.IsNullOrEmpty(value) ? null : int.Parse(value);
        }
    }

    private void InitializeTags() {

        if( !string.IsNullOrEmpty( m_tags ) ) {
            return;
        }

        m_tags = MessagePluginUtils.GetTagsFromMessage( response );
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
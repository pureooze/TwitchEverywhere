using System.Collections.Immutable;
using Moq;
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.MessageTypeTests;

[TestFixture]
public class LazyLoadedUserNoticeTests {

    [Test]
    [TestCaseSource( nameof(UserNoticeMessages) )]
    public void UserNotice(
        string message,
        TestData expectedUserNoticeMessage
    ) {
        LazyLoadedUserNotice actualLazyLoadedUserNoticeMessage = new( channel: "channel", message: message );

        Assert.That( actualLazyLoadedUserNoticeMessage.MessageType, Is.EqualTo( MessageType.UserNotice ) );
        
        Assert.Multiple(() => {
            Assert.That(actualLazyLoadedUserNoticeMessage.BadgeInfo, Is.EqualTo(expectedUserNoticeMessage.BadgeInfo), "BadgeInfo was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.Badges, Is.EqualTo(expectedUserNoticeMessage.Badges), "Badges was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.Color, Is.EqualTo(expectedUserNoticeMessage.Color), "Color was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.DisplayName, Is.EqualTo(expectedUserNoticeMessage.DisplayName), "DisplayName was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.Emotes, Is.EqualTo(expectedUserNoticeMessage.Emotes), "Emotes was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.Id, Is.EqualTo(expectedUserNoticeMessage.Id), "Id was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.Login, Is.EqualTo(expectedUserNoticeMessage.Login), "Login was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.Mod, Is.EqualTo(expectedUserNoticeMessage.Mod), "Mod was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgId, Is.EqualTo(expectedUserNoticeMessage.MsgId), "MsgId was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.RoomId, Is.EqualTo(expectedUserNoticeMessage.RoomId), "RoomId was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.Subscriber, Is.EqualTo(expectedUserNoticeMessage.Subscriber), "Subscriber was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.SystemMsg, Is.EqualTo(expectedUserNoticeMessage.SystemMsg), "SystemMsg was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.Timestamp, Is.EqualTo(expectedUserNoticeMessage.Timestamp), "Timestamp was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.Turbo, Is.EqualTo(expectedUserNoticeMessage.Turbo), "Turbo was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.UserId, Is.EqualTo(expectedUserNoticeMessage.UserId), "UserId was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.UserType, Is.EqualTo(expectedUserNoticeMessage.UserType), "UserType was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamCumulativeMonths, Is.EqualTo(expectedUserNoticeMessage.MsgParamCumulativeMonths), "MsgParamCumulativeMonths was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamDisplayName, Is.EqualTo(expectedUserNoticeMessage.MsgParamDisplayName), "MsgParamDisplayName was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamLogin, Is.EqualTo(expectedUserNoticeMessage.MsgParamLogin), "MsgParamLogin was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamMonths, Is.EqualTo(expectedUserNoticeMessage.MsgParamMonths), "MsgParamMonths was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamPromoGiftTotal, Is.EqualTo(expectedUserNoticeMessage.MsgParamPromoGiftTotal), "MsgParamPromoGiftTotal was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamPromoName, Is.EqualTo(expectedUserNoticeMessage.MsgParamPromoName), "MsgParamPromoName was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamRecipientDisplayName, Is.EqualTo(expectedUserNoticeMessage.MsgParamRecipientDisplayName), "MsgParamRecipientDisplayName was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamRecipientId, Is.EqualTo(expectedUserNoticeMessage.MsgParamRecipientId), "MsgParamRecipientId was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamRecipientUserName, Is.EqualTo(expectedUserNoticeMessage.MsgParamRecipientUserName), "MsgParamRecipientUserName was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamSenderLogin, Is.EqualTo(expectedUserNoticeMessage.MsgParamSenderLogin), "MsgParamSenderLogin was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamSenderName, Is.EqualTo(expectedUserNoticeMessage.MsgParamSenderName), "MsgParamSenderName was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamShouldShareStreak, Is.EqualTo(expectedUserNoticeMessage.MsgParamShouldShareStreak), "MsgParamShouldShareStreak was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamStreakMonths, Is.EqualTo(expectedUserNoticeMessage.MsgParamStreakMonths), "MsgParamStreakMonths was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamSubPlan, Is.EqualTo(expectedUserNoticeMessage.MsgParamSubPlan), "MsgParamSubPlan was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamSubPlanName, Is.EqualTo(expectedUserNoticeMessage.MsgParamSubPlanName), "MsgParamSubPlanName was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamViewerCount, Is.EqualTo(expectedUserNoticeMessage.MsgParamViewerCount), "MsgParamViewerCount was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamRitualName, Is.EqualTo(expectedUserNoticeMessage.MsgParamRitualName), "MsgParamRitualName was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamThreshold, Is.EqualTo(expectedUserNoticeMessage.MsgParamThreshold), "MsgParamThreshold was not equal to expected value");
            Assert.That(actualLazyLoadedUserNoticeMessage.MsgParamGiftMonths, Is.EqualTo(expectedUserNoticeMessage.MsgParamGiftMonths), "MsgParamGiftMonths was not equal to expected value");
        });
    }

    internal static IEnumerable<TestCaseData> UserNoticeMessages() {
        yield return new TestCaseData(
            "@badge-info=;badges=staff/1,broadcaster/1,turbo/1;color=#008000;display-name=ronni;emotes=;id=db25007f-7a18-43eb-9379-80131e44d633;login=ronni;mod=0;msg-id=resub;msg-param-cumulative-months=6;msg-param-streak-months=2;msg-param-should-share-streak=1;msg-param-sub-plan=Prime;msg-param-sub-plan-name=Prime;room-id=12345678;subscriber=1;system-msg=ronni\\shas\\ssubscribed\\sfor\\s6\\smonths!;tmi-sent-ts=1507246572675;turbo=1;user-id=87654321;user-type=staff :tmi.twitch.tv USERNOTICE #channel :Great stream -- keep it up!",
            new TestData( 
                badgeInfo: new List<Badge>().ToImmutableList(),
                badges: new List<Badge> {
                    new( "staff", "1" ),
                    new( "broadcaster", "1" ),
                    new( "turbo", "1" )
                }.ToImmutableList(),
                color: "#008000",
                displayName: "ronni",
                emotes: new List<Emote>().ToImmutableList(),
                id: "db25007f-7a18-43eb-9379-80131e44d633",
                login: "ronni",
                mod: false,
                msgId: MsgIdType.ReSub,
                roomId: "12345678",
                subscriber: true,
                systemMsg: @"ronni\shas\ssubscribed\sfor\s6\smonths!",
                timestamp: new DateTime( 2017, 10, 05, 23, 36, 12, 675 ), 
                turbo: true,
                userId: "87654321",
                userType: UserType.Staff,
                msgParamCumulativeMonths: 6,
                msgParamDisplayName: "",
                msgParamLogin: "",
                msgParamMonths: null,
                msgParamPromoGiftTotal: null,
                msgParamPromoName: "",
                msgParamRecipientDisplayName: "",
                msgParamRecipientId: "",
                msgParamRecipientUserName: "",
                msgParamSenderLogin: "",
                msgParamSenderName: "",
                msgParamShouldShareStreak: true,
                msgParamStreakMonths: 2,
                msgParamSubPlan: MsgSubPlanType.Prime,
                msgParamSubPlanName: "Prime",
                msgParamViewerCount: null,
                msgParamRitualName: "",
                msgParamThreshold: "",
                msgParamGiftMonths: null
            )
        ).SetName( "Resubscribed" );
        
        yield return new TestCaseData(
            "@badge-info=;badges=staff/1,premium/1;color=#0000FF;display-name=TWW2;emotes=;id=e9176cd8-5e22-4684-ad40-ce53c2561c5e;login=tww2;mod=0;msg-id=subgift;msg-param-months=1;msg-param-recipient-display-name=Mr_Woodchuck;msg-param-recipient-id=55554444;msg-param-recipient-name=mr_woodchuck;msg-param-sub-plan-name=House\\sof\\sNyoro~n;msg-param-sub-plan=1000;room-id=19571752;subscriber=0;system-msg=TWW2\\sgifted\\sa\\sTier\\s1\\ssub\\sto\\sMr_Woodchuck!;tmi-sent-ts=1521159445153;turbo=0;user-id=87654321;user-type=staff :tmi.twitch.tv USERNOTICE #channel",
            new TestData( 
                badgeInfo: new List<Badge>().ToImmutableList(),
                badges: new List<Badge> {
                    new( "staff", "1" ),
                    new( "premium", "1" )
                }.ToImmutableList(),
                color: "#0000FF",
                displayName: "TWW2",
                emotes: new List<Emote>().ToImmutableList(),
                id: "e9176cd8-5e22-4684-ad40-ce53c2561c5e",
                login: "tww2",
                mod: false,
                msgId: MsgIdType.SubGift,
                roomId: "19571752",
                subscriber: false,
                systemMsg: @"TWW2\sgifted\sa\sTier\s1\ssub\sto\sMr_Woodchuck!",
                timestamp: new DateTime( 2018, 03, 16, 00, 17, 25, 153 ), 
                turbo: false,
                userId: "87654321",
                userType: UserType.Staff,
                msgParamCumulativeMonths: null,
                msgParamDisplayName: "",
                msgParamLogin: "",
                msgParamMonths: 1,
                msgParamPromoGiftTotal: null,
                msgParamPromoName: "",
                msgParamRecipientDisplayName: "Mr_Woodchuck",
                msgParamRecipientId: "55554444",
                msgParamRecipientUserName: "",
                msgParamSenderLogin: "",
                msgParamSenderName: "",
                msgParamShouldShareStreak: false,
                msgParamStreakMonths: null,
                msgParamSubPlan: MsgSubPlanType.Tier1,
                msgParamSubPlanName: @"House\sof\sNyoro~n",
                msgParamViewerCount: null,
                msgParamRitualName: "",
                msgParamThreshold: "",
                msgParamGiftMonths: null
            )
        ).SetName( "User gifted a subscription" );
        
        yield return new TestCaseData(
            @"@badge-info=;badges=turbo/1;color=#9ACD32;display-name=TestChannel;emotes=;id=3d830f12-795c-447d-af3c-ea05e40fbddb;login=testchannel;mod=0;msg-id=raid;msg-param-displayName=TestChannel;msg-param-login=testchannel;msg-param-viewerCount=15;room-id=33332222;subscriber=0;system-msg=15\\sraiders\\sfrom\\sTestChannel\\shave\\sjoined\\n!;tmi-sent-ts=1507246572675;turbo=1;user-id=123456;user-type= :tmi.twitch.tv USERNOTICE #channel",
            new TestData( 
                badgeInfo: new List<Badge>().ToImmutableList(),
                badges: new List<Badge> {
                    new( "turbo", "1" )
                }.ToImmutableList(),
                color: "#9ACD32",
                displayName: "TestChannel",
                emotes: new List<Emote>().ToImmutableList(),
                id: "3d830f12-795c-447d-af3c-ea05e40fbddb",
                login: "testchannel",
                mod: false,
                msgId: MsgIdType.Raid,
                roomId: "33332222",
                subscriber: false,
                systemMsg: @"15\\sraiders\\sfrom\\sTestChannel\\shave\\sjoined\\n!",
                timestamp: new DateTime( 2017, 10, 05, 23, 36, 12, 675 ), 
                turbo: true,
                userId: "123456",
                userType: UserType.Normal,
                msgParamCumulativeMonths: null,
                msgParamDisplayName: "TestChannel",
                msgParamLogin: "testchannel",
                msgParamMonths: null,
                msgParamPromoGiftTotal: null,
                msgParamPromoName: "",
                msgParamRecipientDisplayName: "",
                msgParamRecipientId: "",
                msgParamRecipientUserName: "",
                msgParamSenderLogin: "",
                msgParamSenderName: "",
                msgParamShouldShareStreak: false,
                msgParamStreakMonths: null,
                msgParamSubPlan: MsgSubPlanType.None,
                msgParamSubPlanName: @"",
                msgParamViewerCount: 15,
                msgParamRitualName: "",
                msgParamThreshold: "",
                msgParamGiftMonths: null
            )
        ).SetName( "Raiding a channel" );
        
        yield return new TestCaseData(
            @"@badge-info=;badges=;color=;display-name=SevenTest1;emotes=30259:0-6;id=37feed0f-b9c7-4c3a-b475-21c6c6d21c3d;login=seventest1;mod=0;msg-id=ritual;msg-param-ritual-name=new_chatter;room-id=87654321;subscriber=0;system-msg=Seventoes\\sis\\snew\\shere!;tmi-sent-ts=1508363903826;turbo=0;user-id=77776666;user-type= :tmi.twitch.tv USERNOTICE #channel :HeyGuys",
            new TestData( 
                badgeInfo: new List<Badge>().ToImmutableList(),
                badges: new List<Badge>().ToImmutableList(),
                color: "",
                displayName: "SevenTest1",
                emotes: new List<Emote> {
                    new( "30259", 0, 6)
                }.ToImmutableList(),
                id: "37feed0f-b9c7-4c3a-b475-21c6c6d21c3d",
                login: "seventest1",
                mod: false,
                msgId: MsgIdType.Ritual,
                roomId: "87654321",
                subscriber: false,
                systemMsg: @"Seventoes\\sis\\snew\\shere!",
                timestamp: new DateTime( 2017, 10, 18, 21, 58, 23, 826 ), 
                turbo: false,
                userId: "77776666",
                userType: UserType.Normal,
                msgParamCumulativeMonths: null,
                msgParamDisplayName: "",
                msgParamLogin: "",
                msgParamMonths: null,
                msgParamPromoGiftTotal: null,
                msgParamPromoName: "",
                msgParamRecipientDisplayName: "",
                msgParamRecipientId: "",
                msgParamRecipientUserName: "",
                msgParamSenderLogin: "",
                msgParamSenderName: "",
                msgParamShouldShareStreak: false,
                msgParamStreakMonths: null,
                msgParamSubPlan: MsgSubPlanType.None,
                msgParamSubPlanName: @"",
                msgParamViewerCount: null,
                msgParamRitualName: "new_chatter",
                msgParamThreshold: "",
                msgParamGiftMonths: null
            )
        ).SetName( "A new_chatter ritual" );
    }

    public class TestData {
        public IImmutableList<Badge> BadgeInfo { get; }
        public IImmutableList<Badge> Badges { get; }
        public string Color { get; }
        public string DisplayName { get; }
        public IImmutableList<Emote> Emotes { get; }
        public string Id { get; }
        public string Login { get; }
        public bool Mod { get; }
        public MsgIdType MsgId { get; }
        public string RoomId { get; }
        public bool Subscriber { get; }
        public string SystemMsg { get; }
        public DateTime Timestamp { get; }
        public bool Turbo { get; }
        public string UserId { get; }
        public UserType UserType { get; }
        public int? MsgParamCumulativeMonths { get; }
        public string MsgParamDisplayName { get; }
        public string MsgParamLogin { get; }
        public int? MsgParamMonths { get; }
        public int? MsgParamPromoGiftTotal { get; }
        public string MsgParamPromoName { get; }
        public string MsgParamRecipientDisplayName { get; }
        public string MsgParamRecipientId { get; }
        public string MsgParamRecipientUserName { get; }
        public string MsgParamSenderLogin { get; }
        public string MsgParamSenderName { get; }
        public bool? MsgParamShouldShareStreak { get; }
        public int? MsgParamStreakMonths { get; }
        public MsgSubPlanType MsgParamSubPlan { get; }
        public string MsgParamSubPlanName { get; }
        public int? MsgParamViewerCount { get; }
        public string MsgParamRitualName { get; }
        public string MsgParamThreshold { get; }
        public int? MsgParamGiftMonths { get; }

        public TestData(
            IImmutableList<Badge> badgeInfo,
            IImmutableList<Badge> badges,
            string color,
            string displayName,
            IImmutableList<Emote> emotes,
            string id,
            string login,
            bool mod,
            MsgIdType msgId,
            string roomId,
            bool subscriber,
            string systemMsg,
            DateTime timestamp,
            bool turbo,
            string userId,
            UserType userType,
            int? msgParamCumulativeMonths,
            string msgParamDisplayName,
            string msgParamLogin,
            int? msgParamMonths,
            int? msgParamPromoGiftTotal,
            string msgParamPromoName,
            string msgParamRecipientDisplayName,
            string msgParamRecipientId,
            string msgParamRecipientUserName,
            string msgParamSenderLogin,
            string msgParamSenderName,
            bool? msgParamShouldShareStreak,
            int? msgParamStreakMonths,
            MsgSubPlanType msgParamSubPlan,
            string msgParamSubPlanName,
            int? msgParamViewerCount,
            string msgParamRitualName,
            string msgParamThreshold,
            int? msgParamGiftMonths
        ) {
            BadgeInfo = badgeInfo;
            Badges = badges;
            Color = color;
            DisplayName = displayName;
            Emotes = emotes;
            Id = id;
            Login = login;
            Mod = mod;
            MsgId = msgId;
            RoomId = roomId;
            Subscriber = subscriber;
            SystemMsg = systemMsg;
            Timestamp = timestamp;
            Turbo = turbo;
            UserId = userId;
            UserType = userType;
            MsgParamCumulativeMonths = msgParamCumulativeMonths;
            MsgParamDisplayName = msgParamDisplayName;
            MsgParamLogin = msgParamLogin;
            MsgParamMonths = msgParamMonths;
            MsgParamPromoGiftTotal = msgParamPromoGiftTotal;
            MsgParamPromoName = msgParamPromoName;
            MsgParamRecipientDisplayName = msgParamRecipientDisplayName;
            MsgParamRecipientId = msgParamRecipientId;
            MsgParamRecipientUserName = msgParamRecipientUserName;
            MsgParamSenderLogin = msgParamSenderLogin;
            MsgParamSenderName = msgParamSenderName;
            MsgParamShouldShareStreak = msgParamShouldShareStreak;
            MsgParamStreakMonths = msgParamStreakMonths;
            MsgParamSubPlan = msgParamSubPlan;
            MsgParamSubPlanName = msgParamSubPlanName;
            MsgParamViewerCount = msgParamViewerCount;
            MsgParamRitualName = msgParamRitualName;
            MsgParamThreshold = msgParamThreshold;
            MsgParamGiftMonths = msgParamGiftMonths;
        }
    }

}
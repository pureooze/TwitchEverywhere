using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using TwitchEverywhere.Implementation.MessagePlugins;

namespace TwitchEverywhere.Benchmark; 

[MemoryDiagnoser]
[NativeMemoryProfiler]
public class RegexBenchmark {
    
    [Params(100)]
    public int Iterations;

    private readonly string m_message = "@emote-only=0;msg-id=delete_message_success;target-user-id=12345678;followers-only=0;bits=1;r9k=0;slow=0;login=ronni;target-msg-id=abc-123-def;ban-duration=350;subs-only=0;room-id=240866033;tmi-sent-ts=1642715756806;msg-param-cumulative-months=6;msg-param-streak-months=2;msg-param-should-share-streak=1;msg-param-sub-plan=Prime;msg-param-sub-plan-name=Prime;room-id=12345678;subscriber=1;system-msg=ronni\\shas\\ssubscribed\\sfor\\s6\\smonths!;badge-info=subscriber/8;badges=subscriber/6;subscriber=0;color=#0D4200;display-name=dallas;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;turbo=0;user-id=12345678;id=b34ccfc7-4977-403a-8a94-33c6bac34fb8;mod=0;user-type=admin :ronni!ronni@ronni.tmi.twitch.tv JOIN #channel";
    
    [Benchmark]
    public string BadgeInfoPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.BadgeInfoPattern);
        
        return value;
    }
    
    [Benchmark]
    public string BadgesPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.BadgesPattern);
        
        return value;
    }
    
    [Benchmark]
    public string BanDurationPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.BanDurationPattern);
        
        return value;
    }
    
    [Benchmark]
    public string BitsPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.BitsPattern);
        
        return value;
    }
    
    [Benchmark]
    public string ColorPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.ColorPattern);
        
        return value;
    }
    
    [Benchmark]
    public string DisplayNamePattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.DisplayNamePattern);
        
        return value;
    }
    
    [Benchmark]
    public string EmoteOnlyPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.EmoteOnlyPattern);
        
        return value;
    }
    
    [Benchmark]
    public string EmoteSetsPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.EmoteSetsPattern);
        
        return value;
    }
    
    [Benchmark]
    public string EmotesPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.EmotesPattern);
        
        return value;
    }
    
    [Benchmark]
    public string FollowersOnlyPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.FollowersOnlyPattern);
        
        return value;
    }
    
    [Benchmark]
    public string IdPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.IdPattern);
        
        return value;
    }
    
    [Benchmark]
    public string LoginPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.LoginPattern);
        
        return value;
    }
    
    [Benchmark]
    public string MessageIdPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MessageIdPattern);
        
        return value;
    }
    
    [Benchmark]
    public string MessageTimestampPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MessageTimestampPattern);
        
        return value;
    }
    
    [Benchmark]
    public string ModPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.ModPattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgIdPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgIdPattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamCumulativeMonthsPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamCumulativeMonthsPattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamDisplayNamePattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamDisplayNamePattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamGiftMonthsPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamGiftMonthsPattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamLoginPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamLoginPattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamMonthsPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamMonthsPattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamPromoGiftTotalPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamPromoGiftTotalPattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamPromoNamePattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamPromoNamePattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamRecipientDisplayNamePattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamRecipientDisplayNamePattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamRecipientIdPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamRecipientIdPattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamRecipientUserNamePattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamRecipientUserNamePattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamRitualNamePattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamRitualNamePattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamSenderLoginPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamSenderLoginPattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamSenderNamePattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamSenderNamePattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamShouldShareStreakPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamShouldShareStreakPattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamStreakMonthsPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamStreakMonthsPattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamSubPlanNamePattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamSubPlanNamePattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamSubPlanPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamSubPlanPattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamThresholdPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamThresholdPattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgParamViewerCountPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgParamViewerCountPattern);
        
        return value;
    }
    
    [Benchmark]
    public string PinnedChatPaidAmountPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.PinnedChatPaidAmountPattern);
        
        return value;
    }
    
    [Benchmark]
    public string PinnedChatPaidCurrencyPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.PinnedChatPaidCurrencyPattern);
        
        return value;
    }
    
    [Benchmark]
    public string PinnedChatPaidExponentPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.PinnedChatPaidExponentPattern);
        
        return value;
    }
    
    [Benchmark]
    public string PinnedChatPaidIsSystemMessagePattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.PinnedChatPaidIsSystemMessagePattern);
        
        return value;
    }
    
    [Benchmark]
    public string PinnedChatPaidLevelPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.PinnedChatPaidLevelPattern);
        
        return value;
    }
    
    [Benchmark]
    public string R9KPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.R9KPattern);
        
        return value;
    }
    
    [Benchmark]
    public string ReplyParentDisplayNamePattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.ReplyParentDisplayNamePattern);
        
        return value;
    }
    
    [Benchmark]
    public string ReplyParentMsgIdPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.ReplyParentMsgIdPattern);
        
        return value;
    }
    
    [Benchmark]
    public string ReplyParentUserIdPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.ReplyParentUserIdPattern);
        
        return value;
    }
    
    [Benchmark]
    public string ReplyParentUserLoginPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.ReplyParentUserLoginPattern);
        
        return value;
    }
    
    [Benchmark]
    public string ReplyThreadParentMsgPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.ReplyThreadParentMsgPattern);
        
        return value;
    }
    
    [Benchmark]
    public string RoomIdPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.RoomIdPattern);
        
        return value;
    }
    
    [Benchmark]
    public string SlowPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.SlowPattern);
        
        return value;
    }
    
    [Benchmark]
    public string SubsOnlyPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.SubsOnlyPattern);
        
        return value;
    }
    
    [Benchmark]
    public string SubscriberPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.SubscriberPattern);
        
        return value;
    }
    
    [Benchmark]
    public string SystemMessagePattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.SystemMessagePattern);
        
        return value;
    }
    
    [Benchmark]
    public string TargetMessageIdPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.TargetMessageIdPattern);
        
        return value;
    }
    
    [Benchmark]
    public string TargetUserIdPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.TargetUserIdPattern);
        
        return value;
    }
    
    [Benchmark]
    public string ThreadIdPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.ThreadIdPattern);
        
        return value;
    }
    
    [Benchmark]
    public string TurboPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.TurboPattern);
        
        return value;
    }
    
    [Benchmark]
    public string UserIdPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.UserIdPattern);
        
        return value;
    }
    
    [Benchmark]
    public string UserTypePattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.UserTypePattern);
        
        return value;
    }
    
    [Benchmark]
    public string VipPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.VipPattern);
        
        return value;
    }
    
    [Benchmark]
    public string UserJoinPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.UserJoinPattern);
        
        return value;
    }
    
    [Benchmark]
    public string HostViewerCountPattern() {
        const string message = ":tmi.twitch.tv HOSTTARGET #abc :xyz 10";
        string value = MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.HostViewerCountPattern );
        
        return value;
    }
    
    [Benchmark]
    public string HostTargetPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.HostTargetPattern);
        
        return value;
    }
    
    [Benchmark]
    public string FromUserPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.FromUserPattern);
        
        return value;
    }
    
    [Benchmark]
    public string ToUserPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.ToUserPattern);
        
        return value;
    }
    
    [Benchmark]
    public string MsgTextPattern() {
        string value = MessagePluginUtils.GetValueFromResponse(m_message, MessagePluginUtils.MsgTextPattern);
        
        return value;
    }
}
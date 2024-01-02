using TwitchEverywhere.Core.Types.Messages.Interfaces;


namespace TwitchEverywhere.Core.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedNoticeMsg : INoticeMsg {
    private readonly string m_channel;
    private readonly NoticeMsgIdType m_msgId;
    private readonly string m_message;

    public ImmediateLoadedNoticeMsg(
        string channel,
        string? message = null,
        NoticeMsgIdType? msgId = null,
        string? targetUserId = null
    ) {
        m_channel = channel;
        m_message = message ?? string.Empty;
        m_msgId = msgId ?? default;
        TargetUserId = targetUserId ?? string.Empty;
    }

    public MessageType MessageType => MessageType.Notice;

    public string RawMessage => GetRawMessage();

    public string Channel => m_channel;

    public NoticeMsgIdType MsgId => m_msgId;

    public string TargetUserId { get; }

    private string GetRawMessage() {
        string message = "@";

        message += SerializeProperty( MessagePluginUtils.Properties.MessageId, () => GetTargetMessageId( m_msgId ) );

        if( !string.IsNullOrEmpty( TargetUserId ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.TargetUserId, () => TargetUserId );
        }

        message = message.Substring( 0, message.Length - 1 );

        message += $" :tmi.twitch.tv {MessageType.ToString().ToUpper()} #{m_channel} :{m_message}";

        return message;
    }

    private string SerializeProperty(
        MessagePluginUtils.Properties property,
        Func<string> serializer
    ) {

        return string.Format( MessagePluginUtils.GetPropertyAsString( property ), serializer() );
    }

    private static string GetTargetMessageId(
        NoticeMsgIdType noticeMsgIdType
    ) {
        return noticeMsgIdType switch {
            NoticeMsgIdType.AlreadyBanned => "already_banned",
            NoticeMsgIdType.AlreadyEmoteOnlyOff => "already_emote_only_off",
            NoticeMsgIdType.AlreadyEmoteOnlyOn => "already_emote_only_on",
            NoticeMsgIdType.AlreadyFollowersOff => "already_followers_off",
            NoticeMsgIdType.AlreadyFollowersOn => "already_followers_on",
            NoticeMsgIdType.AlreadyR9KOff => "already_r9k_off",
            NoticeMsgIdType.AlreadyR9KOn => "already_r9k_on",
            NoticeMsgIdType.AlreadySlowOff => "already_slow_off",
            NoticeMsgIdType.AlreadySlowOn => "already_slow_on",
            NoticeMsgIdType.AlreadySubsOff => "already_subs_off",
            NoticeMsgIdType.AlreadySubsOn => "already_subs_on",
            NoticeMsgIdType.AutohostReceive => "autohost_receive",
            NoticeMsgIdType.BadBanAdmin => "bad_ban_admin",
            NoticeMsgIdType.BadBanAnon => "bad_ban_anon",
            NoticeMsgIdType.BadBanBroadcaster => "bad_ban_broadcaster",
            NoticeMsgIdType.BadBanMod => "bad_ban_mod",
            NoticeMsgIdType.BadBanSelf => "bad_ban_self",
            NoticeMsgIdType.BadBanStaff => "bad_ban_staff",
            NoticeMsgIdType.BadCommercialError => "bad_commercial_error",
            NoticeMsgIdType.BadDeleteMessageBroadcaster => "bad_delete_message_broadcaster",
            NoticeMsgIdType.BadDeleteMessageMod => "bad_delete_message_mod",
            NoticeMsgIdType.BadHostError => "bad_host_error",
            NoticeMsgIdType.BadHostHosting => "bad_host_hosting",
            NoticeMsgIdType.BadHostRateExceeded => "bad_host_rate_exceeded",
            NoticeMsgIdType.BadHostRejected => "bad_host_rejected",
            NoticeMsgIdType.BadHostSelf => "bad_host_self",
            NoticeMsgIdType.BadModBanned => "bad_mod_banned",
            NoticeMsgIdType.BadModMod => "bad_mod_mod",
            NoticeMsgIdType.BadSlowDuration => "bad_slow_duration",
            NoticeMsgIdType.BadTimeoutAdmin => "bad_timeout_admin",
            NoticeMsgIdType.BadTimeoutAnon => "bad_timeout_anon",
            NoticeMsgIdType.BadTimeoutBroadcaster => "bad_timeout_broadcaster",
            NoticeMsgIdType.BadTimeoutDuration => "bad_timeout_duration",
            NoticeMsgIdType.BadTimeoutMod => "bad_timeout_mod",
            NoticeMsgIdType.BadTimeoutSelf => "bad_timeout_self",
            NoticeMsgIdType.BadTimeoutStaff => "bad_timeout_staff",
            NoticeMsgIdType.BadUnbanNoBan => "bad_unban_no_ban",
            NoticeMsgIdType.BadUnhostError => "bad_unhost_error",
            NoticeMsgIdType.BadUnmodMod => "bad_unmod_mod",
            NoticeMsgIdType.BadVipGranteeBanned => "bad_vip_grantee_banned",
            NoticeMsgIdType.BadVipGranteeAlreadyVip => "bad_vip_grantee_already_vip",
            NoticeMsgIdType.BadVipMaxVipsReached => "bad_vip_max_vips_reached",
            NoticeMsgIdType.BadVipAchievementIncomplete => "bad_vip_achievement_incomplete",
            NoticeMsgIdType.BadUnvipGranteeNotVip => "bad_unvip_grantee_not_vip",
            NoticeMsgIdType.BanSuccess => "ban_success",
            NoticeMsgIdType.CmdsAvailable => "cmds_available",
            NoticeMsgIdType.ColorChanged => "color_changed",
            NoticeMsgIdType.CommercialSuccess => "commercial_success",
            NoticeMsgIdType.DeleteMessageSuccess => "delete_message_success",
            NoticeMsgIdType.DeleteStaffMessageSuccess => "delete_staff_message_success",
            NoticeMsgIdType.EmoteOnlyOff => "emote_only_off",
            NoticeMsgIdType.EmoteOnlyOn => "emote_only_on",
            NoticeMsgIdType.FollowersOff => "followers_off",
            NoticeMsgIdType.FollowersOn => "followers_on",
            NoticeMsgIdType.FollowersOnZero => "followers_on_zero",
            NoticeMsgIdType.HostOff => "host_off",
            NoticeMsgIdType.HostOn => "host_on",
            NoticeMsgIdType.HostReceive => "host_receive",
            NoticeMsgIdType.HostReceiveNoCount => "host_receive_no_count",
            NoticeMsgIdType.HostTargetWentOffline => "host_target_went_offline",
            NoticeMsgIdType.HostsRemaining => "hosts_remaining",
            NoticeMsgIdType.InvalidUser => "invalid_user",
            NoticeMsgIdType.ModSuccess => "mod_success",
            NoticeMsgIdType.MsgBanned => "msg_banned",
            NoticeMsgIdType.MsgBadCharacters => "msg_bad_characters",
            NoticeMsgIdType.MsgChannelBlocked => "msg_channel_blocked",
            NoticeMsgIdType.MsgChannelSuspended => "msg_channel_suspended",
            NoticeMsgIdType.MsgDuplicate => "msg_duplicate",
            NoticeMsgIdType.MsgEmoteOnly => "msg_emoteonly",
            NoticeMsgIdType.MsgFollowersOnly => "msg_followersonly",
            NoticeMsgIdType.MsgFollowersOnlyFollowed => "msg_followersonly_followed",
            NoticeMsgIdType.MsgFollowersOnlyZero => "msg_followersonly_zero",
            NoticeMsgIdType.MsgR9K => "msg_r9k",
            NoticeMsgIdType.MsgRateLimit => "msg_ratelimit",
            NoticeMsgIdType.MsgRejected => "msg_rejected",
            NoticeMsgIdType.MsgRejectedMandatory => "msg_rejected_mandatory",
            NoticeMsgIdType.MsgRequiresVerifiedPhoneNumber => "msg_requires_verified_phone_number",
            NoticeMsgIdType.MsgSlowMode => "msg_slowmode",
            NoticeMsgIdType.MsgSubsOnly => "msg_subsonly",
            NoticeMsgIdType.MsgSuspended => "msg_suspended",
            NoticeMsgIdType.MsgTimedOut => "msg_timedout",
            NoticeMsgIdType.MsgVerifiedEmail => "msg_verified_email",
            NoticeMsgIdType.NoHelp => "no_help",
            NoticeMsgIdType.NoMods => "no_mods",
            NoticeMsgIdType.NoVips => "no_vips",
            NoticeMsgIdType.NotHosting => "not_hosting",
            NoticeMsgIdType.NoPermission => "no_permission",
            NoticeMsgIdType.R9KOff => "r9k_off",
            NoticeMsgIdType.R9KOn => "r9k_on",
            NoticeMsgIdType.RaidErrorAlreadyRaiding => "raid_error_already_raiding",
            NoticeMsgIdType.RaidErrorForbidden => "raid_error_forbidden",
            NoticeMsgIdType.RaidErrorSelf => "raid_error_self",
            NoticeMsgIdType.RaidErrorTooManyViewers => "raid_error_too_many_viewers",
            NoticeMsgIdType.RaidErrorUnexpected => "raid_error_unexpected",
            NoticeMsgIdType.RaidNoticeMature => "raid_notice_mature",
            NoticeMsgIdType.RaidNoticeRestrictedChat => "raid_notice_restricted_chat",
            NoticeMsgIdType.RoomMods => "room_mods",
            NoticeMsgIdType.SlowOff => "slow_off",
            NoticeMsgIdType.SlowOn => "slow_on",
            NoticeMsgIdType.SubsOff => "subs_off",
            NoticeMsgIdType.SubsOn => "subs_on",
            NoticeMsgIdType.TimeoutNoTimeout => "timeout_no_timeout",
            NoticeMsgIdType.TimeoutSuccess => "timeout_success",
            NoticeMsgIdType.TosBan => "tos_ban",
            NoticeMsgIdType.TurboOnlyColor => "turbo_only_color",
            NoticeMsgIdType.UnavailableCommand => "unavailable_command",
            NoticeMsgIdType.UnbanSuccess => "unban_success",
            NoticeMsgIdType.UnmodSuccess => "unmod_success",
            NoticeMsgIdType.UnraidErrorNoActiveRaid => "unraid_error_no_active_raid",
            NoticeMsgIdType.UnraidErrorUnexpected => "unraid_error_unexpected",
            NoticeMsgIdType.UnraidSuccess => "unraid_success",
            NoticeMsgIdType.UnrecognizedCmd => "unrecognized_cmd",
            NoticeMsgIdType.UntimeoutBanned => "untimeout_banned",
            NoticeMsgIdType.UntimeoutSuccess => "untimeout_success",
            NoticeMsgIdType.UnvipSuccess => "unvip_success",
            NoticeMsgIdType.UsageBan => "usage_ban",
            NoticeMsgIdType.UsageClear => "usage_clear",
            NoticeMsgIdType.UsageColor => "usage_color",
            NoticeMsgIdType.UsageCommercial => "usage_commercial",
            NoticeMsgIdType.UsageDisconnect => "usage_disconnect",
            NoticeMsgIdType.UsageDelete => "usage_delete",
            NoticeMsgIdType.UsageEmoteOnlyOff => "usage_emote_only_off",
            NoticeMsgIdType.UsageEmoteOnlyOn => "usage_emote_only_on",
            NoticeMsgIdType.UsageFollowersOff => "usage_followers_off",
            NoticeMsgIdType.UsageFollowersOn => "usage_followers_on",
            NoticeMsgIdType.UsageHelp => "usage_help",
            NoticeMsgIdType.UsageHost => "usage_host",
            NoticeMsgIdType.UsageMarker => "usage_marker",
            NoticeMsgIdType.UsageMe => "usage_me",
            NoticeMsgIdType.UsageMod => "usage_mod",
            NoticeMsgIdType.UsageMods => "usage_mods",
            NoticeMsgIdType.UsageR9KOff => "usage_r9k_off",
            NoticeMsgIdType.UsageR9KOn => "usage_r9k_on",
            NoticeMsgIdType.UsageRaid => "usage_raid",
            NoticeMsgIdType.UsageSlowOff => "usage_slow_off",
            NoticeMsgIdType.UsageSlowOn => "usage_slow_on",
            NoticeMsgIdType.UsageSubsOff => "usage_subs_off",
            NoticeMsgIdType.UsageSubsOn => "usage_subs_on",
            NoticeMsgIdType.UsageTimeout => "usage_timeout",
            NoticeMsgIdType.UsageUnban => "usage_unban",
            NoticeMsgIdType.UsageUnhost => "usage_unhost",
            NoticeMsgIdType.UsageUnmod => "usage_unmod",
            NoticeMsgIdType.UsageUnraid => "usage_unraid",
            NoticeMsgIdType.UsageUntimeout => "usage_untimeout",
            NoticeMsgIdType.UsageUnvip => "usage_unvip",
            NoticeMsgIdType.UsageUser => "usage_user",
            NoticeMsgIdType.UsageVip => "usage_vip",
            NoticeMsgIdType.UsageVips => "usage_vips",
            NoticeMsgIdType.UsageWhisper => "usage_whisper",
            NoticeMsgIdType.VipSuccess => "vip_success",
            NoticeMsgIdType.VipsSuccess => "vips_success",
            NoticeMsgIdType.WhisperBanned => "whisper_banned",
            NoticeMsgIdType.WhisperBannedRecipient => "whisper_banned_recipient",
            NoticeMsgIdType.WhisperInvalidLogin => "whisper_invalid_login",
            NoticeMsgIdType.WhisperInvalidSelf => "whisper_invalid_self",
            NoticeMsgIdType.WhisperLimitPerMin => "whisper_limit_per_min",
            NoticeMsgIdType.WhisperLimitPerSec => "whisper_limit_per_sec",
            NoticeMsgIdType.WhisperRestricted => "whisper_restricted",
            NoticeMsgIdType.WhisperRestrictedRecipient => "whisper_restricted_recipient",
            _ => "undefined"
        };
    }
}
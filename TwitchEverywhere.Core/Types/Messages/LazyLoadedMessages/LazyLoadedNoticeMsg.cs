using System.Text;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedNoticeMsg( RawMessage response ) : INoticeMsg {
    private string m_tags;

    public MessageType MessageType => MessageType.Notice;
    
    public string RawMessage { get; } = Encoding.UTF8.GetString( response.Data.Span );

    public string Channel => "";

    public NoticeMsgIdType MsgId {
        get {
            InitializeTags();
            return GetNoticeMsgIdType(MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgIdPattern()));
        }
    }

    public string TargetUserId {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.TargetUserIdPattern());
        }
    }
    
    private void InitializeTags() {

        if( !string.IsNullOrEmpty( m_tags ) ) {
            return;
        }

        m_tags = MessagePluginUtils.GetTagsFromMessage( response );
    }
    
    private static NoticeMsgIdType GetNoticeMsgIdType(
        string targetMessageId
    ) {
        return targetMessageId switch {
            "already_banned" => NoticeMsgIdType.AlreadyBanned,
            "already_emote_only_off" => NoticeMsgIdType.AlreadyEmoteOnlyOff,
            "already_emote_only_on" => NoticeMsgIdType.AlreadyEmoteOnlyOn,
            "already_followers_off" => NoticeMsgIdType.AlreadyFollowersOff,
            "already_followers_on" => NoticeMsgIdType.AlreadyFollowersOn,
            "already_r9k_off" => NoticeMsgIdType.AlreadyR9KOff,
            "already_r9k_on" => NoticeMsgIdType.AlreadyR9KOn,
            "already_slow_off" => NoticeMsgIdType.AlreadySlowOff,
            "already_slow_on" => NoticeMsgIdType.AlreadySlowOn,
            "already_subs_off" => NoticeMsgIdType.AlreadySubsOff,
            "already_subs_on" => NoticeMsgIdType.AlreadySubsOn,
            "autohost_receive" => NoticeMsgIdType.AutohostReceive,
            "bad_ban_admin" => NoticeMsgIdType.BadBanAdmin,
            "bad_ban_anon" => NoticeMsgIdType.BadBanAnon,
            "bad_ban_broadcaster" => NoticeMsgIdType.BadBanBroadcaster,
            "bad_ban_mod" => NoticeMsgIdType.BadBanMod,
            "bad_ban_self" => NoticeMsgIdType.BadBanSelf,
            "bad_ban_staff" => NoticeMsgIdType.BadBanStaff,
            "bad_commercial_error" => NoticeMsgIdType.BadCommercialError,
            "bad_delete_message_broadcaster" => NoticeMsgIdType.BadDeleteMessageBroadcaster,
            "bad_delete_message_mod" => NoticeMsgIdType.BadDeleteMessageMod,
            "bad_host_error" => NoticeMsgIdType.BadHostError,
            "bad_host_hosting" => NoticeMsgIdType.BadHostHosting,
            "bad_host_rate_exceeded" => NoticeMsgIdType.BadHostRateExceeded,
            "bad_host_rejected" => NoticeMsgIdType.BadHostRejected,
            "bad_host_self" => NoticeMsgIdType.BadHostSelf,
            "bad_mod_banned" => NoticeMsgIdType.BadModBanned,
            "bad_mod_mod" => NoticeMsgIdType.BadModMod,
            "bad_slow_duration" => NoticeMsgIdType.BadSlowDuration,
            "bad_timeout_admin" => NoticeMsgIdType.BadTimeoutAdmin,
            "bad_timeout_anon" => NoticeMsgIdType.BadTimeoutAnon,
            "bad_timeout_broadcaster" => NoticeMsgIdType.BadTimeoutBroadcaster,
            "bad_timeout_duration" => NoticeMsgIdType.BadTimeoutDuration,
            "bad_timeout_mod" => NoticeMsgIdType.BadTimeoutMod,
            "bad_timeout_self" => NoticeMsgIdType.BadTimeoutSelf,
            "bad_timeout_staff" => NoticeMsgIdType.BadTimeoutStaff,
            "bad_unban_no_ban" => NoticeMsgIdType.BadUnbanNoBan,
            "bad_unhost_error" => NoticeMsgIdType.BadUnhostError,
            "bad_unmod_mod" => NoticeMsgIdType.BadUnmodMod,
            "bad_vip_grantee_banned" => NoticeMsgIdType.BadVipGranteeBanned,
            "bad_vip_grantee_already_vip" => NoticeMsgIdType.BadVipGranteeAlreadyVip,
            "bad_vip_max_vips_reached" => NoticeMsgIdType.BadVipMaxVipsReached,
            "bad_vip_achievement_incomplete" => NoticeMsgIdType.BadVipAchievementIncomplete,
            "bad_unvip_grantee_not_vip" => NoticeMsgIdType.BadUnvipGranteeNotVip,
            "ban_success" => NoticeMsgIdType.BanSuccess,
            "cmds_available" => NoticeMsgIdType.CmdsAvailable,
            "color_changed" => NoticeMsgIdType.ColorChanged,
            "commercial_success" => NoticeMsgIdType.CommercialSuccess,
            "delete_message_success" => NoticeMsgIdType.DeleteMessageSuccess,
            "delete_staff_message_success" => NoticeMsgIdType.DeleteStaffMessageSuccess,
            "emote_only_off" => NoticeMsgIdType.EmoteOnlyOff,
            "emote_only_on" => NoticeMsgIdType.EmoteOnlyOn,
            "followers_off" => NoticeMsgIdType.FollowersOff,
            "followers_on" => NoticeMsgIdType.FollowersOn,
            "followers_on_zero" => NoticeMsgIdType.FollowersOnZero,
            "host_off" => NoticeMsgIdType.HostOff,
            "host_on" => NoticeMsgIdType.HostOn,
            "host_receive" => NoticeMsgIdType.HostReceive,
            "host_receive_no_count" => NoticeMsgIdType.HostReceiveNoCount,
            "host_target_went_offline" => NoticeMsgIdType.HostTargetWentOffline,
            "hosts_remaining" => NoticeMsgIdType.HostsRemaining,
            "invalid_user" => NoticeMsgIdType.InvalidUser,
            "mod_success" => NoticeMsgIdType.ModSuccess,
            "msg_banned" => NoticeMsgIdType.MsgBanned,
            "msg_bad_characters" => NoticeMsgIdType.MsgBadCharacters,
            "msg_channel_blocked" => NoticeMsgIdType.MsgChannelBlocked,
            "msg_channel_suspended" => NoticeMsgIdType.MsgChannelSuspended,
            "msg_duplicate" => NoticeMsgIdType.MsgDuplicate,
            "msg_emoteonly" => NoticeMsgIdType.MsgEmoteOnly,
            "msg_followersonly" => NoticeMsgIdType.MsgFollowersOnly,
            "msg_followersonly_followed" => NoticeMsgIdType.MsgFollowersOnlyFollowed,
            "msg_followersonly_zero" => NoticeMsgIdType.MsgFollowersOnlyZero,
            "msg_r9k" => NoticeMsgIdType.MsgR9K,
            "msg_ratelimit" => NoticeMsgIdType.MsgRateLimit,
            "msg_rejected" => NoticeMsgIdType.MsgRejected,
            "msg_rejected_mandatory" => NoticeMsgIdType.MsgRejectedMandatory,
            "msg_requires_verified_phone_number" => NoticeMsgIdType.MsgRequiresVerifiedPhoneNumber,
            "msg_slowmode" => NoticeMsgIdType.MsgSlowMode,
            "msg_subsonly" => NoticeMsgIdType.MsgSubsOnly,
            "msg_suspended" => NoticeMsgIdType.MsgSuspended,
            "msg_timedout" => NoticeMsgIdType.MsgTimedOut,
            "msg_verified_email" => NoticeMsgIdType.MsgVerifiedEmail,
            "no_help" => NoticeMsgIdType.NoHelp,
            "no_mods" => NoticeMsgIdType.NoMods,
            "no_vips" => NoticeMsgIdType.NoVips,
            "not_hosting" => NoticeMsgIdType.NotHosting,
            "no_permission" => NoticeMsgIdType.NoPermission,
            "r9k_off" => NoticeMsgIdType.R9KOff,
            "r9k_on" => NoticeMsgIdType.R9KOn,
            "raid_error_already_raiding" => NoticeMsgIdType.RaidErrorAlreadyRaiding,
            "raid_error_forbidden" => NoticeMsgIdType.RaidErrorForbidden,
            "raid_error_self" => NoticeMsgIdType.RaidErrorSelf,
            "raid_error_too_many_viewers" => NoticeMsgIdType.RaidErrorTooManyViewers,
            "raid_error_unexpected" => NoticeMsgIdType.RaidErrorUnexpected,
            "raid_notice_mature" => NoticeMsgIdType.RaidNoticeMature,
            "raid_notice_restricted_chat" => NoticeMsgIdType.RaidNoticeRestrictedChat,
            "room_mods" => NoticeMsgIdType.RoomMods,
            "slow_off" => NoticeMsgIdType.SlowOff,
            "slow_on" => NoticeMsgIdType.SlowOn,
            "subs_off" => NoticeMsgIdType.SubsOff,
            "subs_on" => NoticeMsgIdType.SubsOn,
            "timeout_no_timeout" => NoticeMsgIdType.TimeoutNoTimeout,
            "timeout_success" => NoticeMsgIdType.TimeoutSuccess,
            "tos_ban" => NoticeMsgIdType.TosBan,
            "turbo_only_color" => NoticeMsgIdType.TurboOnlyColor,
            "unavailable_command" => NoticeMsgIdType.UnavailableCommand,
            "unban_success" => NoticeMsgIdType.UnbanSuccess,
            "unmod_success" => NoticeMsgIdType.UnmodSuccess,
            "unraid_error_no_active_raid" => NoticeMsgIdType.UnraidErrorNoActiveRaid,
            "unraid_error_unexpected" => NoticeMsgIdType.UnraidErrorUnexpected,
            "unraid_success" => NoticeMsgIdType.UnraidSuccess,
            "unrecognized_cmd" => NoticeMsgIdType.UnrecognizedCmd,
            "untimeout_banned" => NoticeMsgIdType.UntimeoutBanned,
            "untimeout_success" => NoticeMsgIdType.UntimeoutSuccess,
            "unvip_success" => NoticeMsgIdType.UnvipSuccess,
            "usage_ban" => NoticeMsgIdType.UsageBan,
            "usage_clear" => NoticeMsgIdType.UsageClear,
            "usage_color" => NoticeMsgIdType.UsageColor,
            "usage_commercial" => NoticeMsgIdType.UsageCommercial,
            "usage_disconnect" => NoticeMsgIdType.UsageDisconnect,
            "usage_delete" => NoticeMsgIdType.UsageDelete,
            "usage_emote_only_off" => NoticeMsgIdType.UsageEmoteOnlyOff,
            "usage_emote_only_on" => NoticeMsgIdType.UsageEmoteOnlyOn,
            "usage_followers_off" => NoticeMsgIdType.UsageFollowersOff,
            "usage_followers_on" => NoticeMsgIdType.UsageFollowersOn,
            "usage_help" => NoticeMsgIdType.UsageHelp,
            "usage_host" => NoticeMsgIdType.UsageHost,
            "usage_marker" => NoticeMsgIdType.UsageMarker,
            "usage_me" => NoticeMsgIdType.UsageMe,
            "usage_mod" => NoticeMsgIdType.UsageMod,
            "usage_mods" => NoticeMsgIdType.UsageMods,
            "usage_r9k_off" => NoticeMsgIdType.UsageR9KOff,
            "usage_r9k_on" => NoticeMsgIdType.UsageR9KOn,
            "usage_raid" => NoticeMsgIdType.UsageRaid,
            "usage_slow_off" => NoticeMsgIdType.UsageSlowOff,
            "usage_slow_on" => NoticeMsgIdType.UsageSlowOn,
            "usage_subs_off" => NoticeMsgIdType.UsageSubsOff,
            "usage_subs_on" => NoticeMsgIdType.UsageSubsOn,
            "usage_timeout" => NoticeMsgIdType.UsageTimeout,
            "usage_unban" => NoticeMsgIdType.UsageUnban,
            "usage_unhost" => NoticeMsgIdType.UsageUnhost,
            "usage_unmod" => NoticeMsgIdType.UsageUnmod,
            "usage_unraid" => NoticeMsgIdType.UsageUnraid,
            "usage_untimeout" => NoticeMsgIdType.UsageUntimeout,
            "usage_unvip" => NoticeMsgIdType.UsageUnvip,
            "usage_user" => NoticeMsgIdType.UsageUser,
            "usage_vip" => NoticeMsgIdType.UsageVip,
            "usage_vips" => NoticeMsgIdType.UsageVips,
            "usage_whisper" => NoticeMsgIdType.UsageWhisper,
            "vip_success" => NoticeMsgIdType.VipSuccess,
            "vips_success" => NoticeMsgIdType.VipsSuccess,
            "whisper_banned" => NoticeMsgIdType.WhisperBanned,
            "whisper_banned_recipient" => NoticeMsgIdType.WhisperBannedRecipient,
            "whisper_invalid_login" => NoticeMsgIdType.WhisperInvalidLogin,
            "whisper_invalid_self" => NoticeMsgIdType.WhisperInvalidSelf,
            "whisper_limit_per_min" => NoticeMsgIdType.WhisperLimitPerMin,
            "whisper_limit_per_sec" => NoticeMsgIdType.WhisperLimitPerSec,
            "whisper_restricted" => NoticeMsgIdType.WhisperRestricted,
            "whisper_restricted_recipient" => NoticeMsgIdType.WhisperRestrictedRecipient,
            _ => NoticeMsgIdType.Undefined
        };
    }
}
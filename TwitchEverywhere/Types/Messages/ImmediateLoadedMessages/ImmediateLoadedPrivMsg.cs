using System.Collections.Immutable;
using TwitchEverywhere.Types.Messages.Interfaces;
using MessagePluginUtils = TwitchEverywhere.Implementation.MessagePluginUtils;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedPrivMsg : IPrivMsg {
    private readonly string m_channel;
    private IImmutableList<Badge> m_badges;
    private IImmutableList<Badge> m_badgeInfo;
    private string m_bits;
    private string m_color;
    private string m_displayName;
    private IImmutableList<Emote>? m_emotes;
    private string m_id;
    private bool m_mod;
    private long? m_pinnedChatPaidAmount;
    private string m_pinnedChatPaidCurrency;
    private long? m_pinnedChatPaidExponent;
    private PinnedChatPaidLevel? m_pinnedChatPaidLevel;
    private bool m_pinnedChatPaidIsSystemMessage;
    private string m_replyParentMsgId;
    private string m_replyParentUserId;
    private string m_replyParentUserLogin;
    private string m_replyParentDisplayName;
    private string m_replyThreadParentMsg;
    private string m_roomId;
    private bool m_subscriber;
    private DateTime m_timestamp;
    private bool m_turbo;
    private string m_userId;
    private UserType m_userType;
    private bool m_vip;
    private string m_text;

    public ImmediateLoadedPrivMsg(
        string channel,
        IImmutableList<Badge>? badges = null,
        IImmutableList<Badge>? badgeInfo = null,
        string? bits = null,
        string? color = null,
        string? displayName = null,
        IImmutableList<Emote>? emotes = null,
        string? id = null,
        bool mod = default,
        long? pinnedChatPaidAmount = default,
        string? pinnedChatPaidCurrency = null,
        long? pinnedChatPaidExponent = default,
        PinnedChatPaidLevel? pinnedChatPaidLevel = default,
        bool pinnedChatPaidIsSystemMessage = default,
        string? replyParentMsgId = null,
        string? replyParentUserId = null,
        string? replyParentUserLogin = null,
        string? replyParentDisplayName = null,
        string? replyThreadParentMsg = null,
        string? roomId = null,
        bool subscriber = default,
        DateTime timestamp = default,
        bool turbo = default,
        string? userId = null,
        UserType userType = default,
        bool vip = default,
        string? text = null
    ) {
        m_badges = badges ?? ImmutableArray<Badge>.Empty;
        m_badgeInfo = badgeInfo ?? ImmutableArray<Badge>.Empty;
        m_bits = bits ?? string.Empty;
        m_color = color ?? string.Empty;
        m_displayName = displayName ?? string.Empty;
        m_channel = channel;
        m_emotes = emotes ?? ImmutableArray<Emote>.Empty;
        m_id = id ?? string.Empty;
        m_mod = mod;
        m_pinnedChatPaidAmount = pinnedChatPaidAmount;
        m_pinnedChatPaidCurrency = pinnedChatPaidCurrency ?? string.Empty;
        m_pinnedChatPaidExponent = pinnedChatPaidExponent;
        m_pinnedChatPaidLevel = pinnedChatPaidLevel;
        m_pinnedChatPaidIsSystemMessage = pinnedChatPaidIsSystemMessage;
        m_replyParentMsgId = replyParentMsgId ?? string.Empty;
        m_replyParentUserId = replyParentUserId ?? string.Empty;
        m_replyParentUserLogin = replyParentUserLogin ?? string.Empty;
        m_replyParentDisplayName = replyParentDisplayName ?? string.Empty;
        m_replyThreadParentMsg = replyThreadParentMsg ?? string.Empty;
        m_roomId = roomId ?? string.Empty;
        m_subscriber = subscriber;
        m_timestamp = timestamp;
        m_turbo = turbo;
        m_userId = userId ?? string.Empty;
        m_userType = userType;
        m_vip = vip;
        m_text = text ?? string.Empty;
    }

    public MessageType MessageType => MessageType.PrivMsg;

    public string RawMessage => GetRawMessageString();
    
    public string Channel => m_channel;

    IImmutableList<Badge> IPrivMsg.Badges => m_badges;

    IImmutableList<Badge> IPrivMsg.BadgeInfo => m_badgeInfo;

    string IPrivMsg.Bits => m_bits;

    string IPrivMsg.Color => m_color;

    string IPrivMsg.DisplayName => m_displayName;

    IImmutableList<Emote>? IPrivMsg.Emotes => m_emotes;

    string IPrivMsg.Id => m_id;

    bool IPrivMsg.Mod => m_mod;

    long? IPrivMsg.PinnedChatPaidAmount => m_pinnedChatPaidAmount;

    string IPrivMsg.PinnedChatPaidCurrency => m_pinnedChatPaidCurrency;

    long? IPrivMsg.PinnedChatPaidExponent => m_pinnedChatPaidExponent;

    PinnedChatPaidLevel? IPrivMsg.PinnedChatPaidLevel => m_pinnedChatPaidLevel;

    bool IPrivMsg.PinnedChatPaidIsSystemMessage => m_pinnedChatPaidIsSystemMessage;

    string IPrivMsg.ReplyParentMsgId => m_replyParentMsgId;

    string IPrivMsg.ReplyParentUserId => m_replyParentUserId;

    string IPrivMsg.ReplyParentUserLogin => m_replyParentUserLogin;

    string IPrivMsg.ReplyParentDisplayName => m_replyParentDisplayName;

    string IPrivMsg.ReplyThreadParentMsg => m_replyThreadParentMsg;

    string IPrivMsg.RoomId => m_roomId;

    bool IPrivMsg.Subscriber => m_subscriber;

    DateTime IPrivMsg.Timestamp => m_timestamp;

    bool IPrivMsg.Turbo => m_turbo;

    string IPrivMsg.UserId => m_userId;

    UserType IPrivMsg.UserType => m_userType;

    bool IPrivMsg.Vip => m_vip;

    string IPrivMsg.Text => m_text;

    private string GetRawMessageString() {
        string message = "@";
        
        if( m_badges.Any() ) {
            message += SerializeProperty( MessagePluginUtils.Properties.BadgeInfo, () => string.Join( ",", m_badgeInfo.Select( b => $"{b.Name}/{b.Version}" ) ) );
            message += SerializeProperty( MessagePluginUtils.Properties.Badges, () => string.Join( ",", m_badges.Select( b => $"{b.Name}/{b.Version}" ) ) );
        }
        
        if( !string.IsNullOrEmpty( m_bits ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.Bits, () => m_bits );
        }
        
        if( !string.IsNullOrEmpty( m_color ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.Color, () => m_color );
        }
        
        if( !string.IsNullOrEmpty( m_displayName ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.DisplayName, () => m_displayName );
        }
        
        message += SerializeProperty( MessagePluginUtils.Properties.Mod, () => m_mod ? "1" : "0" );
        
        if( m_emotes != null && m_emotes.Any() ) {
            message += SerializeProperty( MessagePluginUtils.Properties.Emotes, () => {
                var groupedEmotes = m_emotes
                    .GroupBy( e => e.EmoteId )
                    .Select(
                        g => new {
                            EmoteId = g.Key,
                            Positions = string.Join( ",", g.Select( e => $"{e.Start}-{e.End}" ) )
                        }
                    );

                return string.Join( "/", groupedEmotes.Select( e => $"{e.EmoteId}:{e.Positions}" ));
            } );
        }
        
        if( !string.IsNullOrEmpty( m_id ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.Id, () => m_id );
        }
        
        if( m_pinnedChatPaidAmount.HasValue ) {
            message += SerializeProperty( MessagePluginUtils.Properties.PinnedChatPaidAmount, () => m_pinnedChatPaidAmount?.ToString() ?? string.Empty );
            message += SerializeProperty( MessagePluginUtils.Properties.PinnedChatPaidCanonicalAmount, () => m_pinnedChatPaidAmount?.ToString() ?? string.Empty );
            message += SerializeProperty( MessagePluginUtils.Properties.PinnedChatPaidCurrency, () => m_pinnedChatPaidCurrency );
            message += SerializeProperty( MessagePluginUtils.Properties.PinnedChatPaidExponent, () => m_pinnedChatPaidExponent?.ToString() ?? string.Empty );
            message += SerializeProperty( MessagePluginUtils.Properties.PinnedChatPaidIsSystemMessage, () => m_pinnedChatPaidIsSystemMessage ? "1" : "0" );
            message += SerializeProperty( MessagePluginUtils.Properties.PinnedChatPaidLevel, () => m_pinnedChatPaidLevel?.ToString().ToUpper() ?? string.Empty );
        }

        if( !string.IsNullOrEmpty( m_replyParentMsgId ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.ReplyParentMsgId, () => m_replyParentMsgId );
        }
        
        if( !string.IsNullOrEmpty( m_replyParentUserId ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.ReplyParentUserId, () => m_replyParentUserId );
        }
        
        if( !string.IsNullOrEmpty( m_replyParentUserLogin ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.ReplyParentUserLogin, () => m_replyParentUserLogin );
        }
        
        if( !string.IsNullOrEmpty( m_replyParentDisplayName ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.ReplyParentDisplayName, () => m_replyParentDisplayName );
        }
        
        if( !string.IsNullOrEmpty( m_replyThreadParentMsg ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.ReplyThreadParentMsg, () => m_replyThreadParentMsg );
        }
        
        if( !string.IsNullOrEmpty( m_roomId ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.RoomId, () => m_roomId );
        }

        message += SerializeProperty( MessagePluginUtils.Properties.Subscriber, () => m_subscriber ? "1" : "0" );

        message += SerializeProperty( MessagePluginUtils.Properties.Turbo, () => m_turbo ? "1" : "0" );
        
        if( !string.IsNullOrEmpty( m_userId ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.UserId, () => m_userId );
        }

        if( m_userType != UserType.Normal ) {
            message += SerializeProperty( MessagePluginUtils.Properties.UserType, () => MessagePluginUtils.GetUserTypeText( m_userType ) );
        }
        
        message += SerializeProperty( MessagePluginUtils.Properties.MessageTimestamp, () => new DateTimeOffset(m_timestamp).ToUnixTimeMilliseconds().ToString() );

        if( m_vip ) {
            message += SerializeProperty( MessagePluginUtils.Properties.Vip, () => "1");
        }

        message = message.Substring(0, message.Length - 1);

        if( !string.IsNullOrEmpty( m_text ) ) {
            message += $" :{m_channel}!{m_channel}@{m_channel}.tmi.twitch.tv {MessageType.ToString().ToUpper()} #{m_channel} :{m_text}";
        }
        
        return message;
    }

    private string SerializeProperty(
        MessagePluginUtils.Properties property,
        Func<string> serializer
    ) {

        return string.Format( MessagePluginUtils.GetPropertyAsString( property ), serializer() );
    }
}
using System.Collections.Immutable;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Core.Types.Messages.Implementation;

public class PrivMsg : IPrivMsg {
    private readonly IPrivMsg m_inner;
    
    private string m_rawMessage;
    private string m_channel;
    private IImmutableList<Badge>? m_badges;
    private IImmutableList<Badge>? m_badgeInfo;
    private string m_bits;
    private string m_color;
    private string m_displayName;
    private IImmutableList<Emote>? m_emotes;
    private string m_id;
    private bool? m_mod;
    private long? m_pinnedChatPaidAmount;
    private string m_pinnedChatPaidCurrency;
    private long? m_pinnedChatPaidExponent;
    private PinnedChatPaidLevel? m_pinnedChatPaidLevel;
    private bool? m_pinnedChatPaidIsSystemMessage;
    private string m_replyParentMsgId;
    private string m_replyParentUserId;
    private string m_replyParentUserLogin;
    private string m_replyParentDisplayName;
    private string m_replyThreadParentMsg;
    private string m_roomId;
    private bool? m_subscriber;
    private DateTime? m_timestamp;
    private bool? m_turbo;
    private string m_userId;
    private UserType? m_userType;
    private bool? m_vip;
    private string m_text = string.Empty;

    public PrivMsg(
        RawMessage message
    ) {
        m_inner = new LazyLoadedPrivMsg( message );
    }
    
    public PrivMsg(
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
        DateTime? timestamp = null,
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
        m_timestamp = timestamp ?? null;
        m_turbo = turbo;
        m_userId = userId ?? string.Empty;
        m_userType = userType;
        m_vip = vip;
        m_text = text ?? string.Empty;
        m_rawMessage = GetRawMessageString();
    }

    MessageType IMessage.MessageType => MessageType.PrivMsg;

    string IMessage.RawMessage {
        get {
            if ( string.IsNullOrEmpty( m_rawMessage ) ) {
                m_rawMessage = m_inner.RawMessage;
            }

            return m_rawMessage;
        }
    }
    
    string IMessage.Channel {
        get {
            if ( string.IsNullOrEmpty( m_channel ) ) {
                m_channel = m_inner.Channel;
            }

            return m_channel;
        }
    }

    IImmutableList<Badge> IPrivMsg.Badges {
        get {
            m_badges ??= m_inner.Badges;

            return m_badges;
        }
    }

    IImmutableList<Badge> IPrivMsg.BadgeInfo {
        get {
            m_badgeInfo ??= m_inner.BadgeInfo;

            return m_badgeInfo;
        }
    }

    string IPrivMsg.Bits {
        get {
            if ( string.IsNullOrEmpty( m_bits ) ) {
                m_bits = m_inner.Bits;
            }

            return m_bits;
        }
    }

    string IPrivMsg.Color {
        get {
            if ( string.IsNullOrEmpty( m_color ) ) {
                m_color = m_inner.Color;
            }

            return m_color;
        }
    }

    string IPrivMsg.DisplayName {
        get {
            if ( string.IsNullOrEmpty( m_displayName ) ) {
                m_displayName = m_inner.DisplayName;
            }

            return m_displayName;
        }
    }

    IImmutableList<Emote>? IPrivMsg.Emotes {
        get {
            m_emotes ??= m_inner.Emotes;

            return m_emotes;
        }
    }

    string IPrivMsg.Id {
        get {
            if ( string.IsNullOrEmpty( m_id ) ) {
                m_id = m_inner.Id;
            }

            return m_id;
        }
    }

    bool IPrivMsg.Mod {
        get {
            m_mod ??= m_inner.Mod;

            return m_mod.Value;
        }
    }

    long? IPrivMsg.PinnedChatPaidAmount {
        get {
            m_pinnedChatPaidAmount ??= m_inner.PinnedChatPaidAmount;

            return m_pinnedChatPaidAmount;
        }
    }

    string IPrivMsg.PinnedChatPaidCurrency {
        get {
            if ( string.IsNullOrEmpty( m_pinnedChatPaidCurrency ) ) {
                m_pinnedChatPaidCurrency = m_inner.PinnedChatPaidCurrency;
            }

            return m_pinnedChatPaidCurrency;
        }
    }

    long? IPrivMsg.PinnedChatPaidExponent {
        get {
            m_pinnedChatPaidExponent ??= m_inner.PinnedChatPaidExponent;

            return m_pinnedChatPaidExponent;
        }
    }

    PinnedChatPaidLevel? IPrivMsg.PinnedChatPaidLevel {
        get {
            m_pinnedChatPaidLevel ??= m_inner.PinnedChatPaidLevel;

            return m_pinnedChatPaidLevel;
        }
    }

    bool IPrivMsg.PinnedChatPaidIsSystemMessage {
        get {
            m_pinnedChatPaidIsSystemMessage ??= m_inner.PinnedChatPaidIsSystemMessage;

            return m_pinnedChatPaidIsSystemMessage.Value;
        }
    }

    string IPrivMsg.ReplyParentMsgId {
        get {
            if ( string.IsNullOrEmpty( m_replyParentMsgId ) ) {
                m_replyParentMsgId = m_inner.ReplyParentMsgId;
            }

            return m_replyParentMsgId;
        }
    }

    string IPrivMsg.ReplyParentUserId {
        get {
            if ( string.IsNullOrEmpty( m_replyParentUserId ) ) {
                m_replyParentUserId = m_inner.ReplyParentUserId;
            }

            return m_replyParentUserId;
        }
    }

    string IPrivMsg.ReplyParentUserLogin {
        get {
            if ( string.IsNullOrEmpty( m_replyParentUserLogin ) ) {
                m_replyParentUserLogin = m_inner.ReplyParentUserLogin;
            }

            return m_replyParentUserLogin;
        }
    }

    string IPrivMsg.ReplyParentDisplayName {
        get {
            if ( string.IsNullOrEmpty( m_replyParentDisplayName ) ) {
                m_replyParentDisplayName = m_inner.ReplyParentDisplayName;
            }

            return m_replyParentDisplayName;
        }
    }

    string IPrivMsg.ReplyThreadParentMsg {
        get {
            if ( string.IsNullOrEmpty( m_replyThreadParentMsg ) ) {
                m_replyThreadParentMsg = m_inner.ReplyThreadParentMsg;
            }

            return m_replyThreadParentMsg;
        }
    }

    string IPrivMsg.RoomId {
        get {
            if ( string.IsNullOrEmpty( m_roomId ) ) {
                m_roomId = m_inner.RoomId;
            }

            return m_roomId;
        }
    }

    bool IPrivMsg.Subscriber {
        get {
            m_subscriber ??= m_inner.Subscriber;

            return m_subscriber.Value;
        }
    }

    DateTime? IPrivMsg.Timestamp {
        get {
            m_timestamp ??= m_inner.Timestamp;

            return m_timestamp;
        }
    }

    bool IPrivMsg.Turbo {
        get {
            m_turbo ??= m_inner.Turbo;

            return m_turbo.Value;
        }
    }

    string IPrivMsg.UserId {
        get {
            if ( string.IsNullOrEmpty( m_userId ) ) {
                m_userId = m_inner.UserId;
            }

            return m_userId;
        }
    }

    UserType IPrivMsg.UserType {
        get {
            m_userType ??= m_inner.UserType;

            return m_userType.Value;
        }
    }

    bool IPrivMsg.Vip {
        get {
            m_vip ??= m_inner.Vip;

            return m_vip.Value;
        }
    }

    string IPrivMsg.Text {
        get {
            if ( string.IsNullOrEmpty( m_text ) ) {
                m_text = m_inner.Text;
            }

            return m_text;
        }
    }
    
    private string GetRawMessageString() {
        string message = "@";
        
        if( m_badges != null && m_badges.Any() ) {
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
        
        message += SerializeProperty( MessagePluginUtils.Properties.Mod, () => m_mod != null && m_mod.Value ? "1" : "0" );
        
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
            message += SerializeProperty( MessagePluginUtils.Properties.PinnedChatPaidIsSystemMessage, () => m_pinnedChatPaidIsSystemMessage != null && m_pinnedChatPaidIsSystemMessage.Value ? "1" : "0" );
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

        message += SerializeProperty( MessagePluginUtils.Properties.Subscriber, () => m_subscriber != null && m_subscriber.Value ? "1" : "0" );

        message += SerializeProperty( MessagePluginUtils.Properties.Turbo, () => m_turbo != null && m_turbo.Value ? "1" : "0" );
        
        if( !string.IsNullOrEmpty( m_userId ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.UserId, () => m_userId );
        }

        if( m_userType != null && m_userType != UserType.Normal ) {
            message += SerializeProperty( MessagePluginUtils.Properties.UserType, () => MessagePluginUtils.GetUserTypeText( m_userType.Value ) );
        }

        if( m_timestamp != null ) {
            message += SerializeProperty( MessagePluginUtils.Properties.MessageTimestamp, () => new DateTimeOffset( m_timestamp.Value ).ToUnixTimeMilliseconds().ToString() );
        }

        if( m_vip != null && m_vip.Value ) {
            message += SerializeProperty( MessagePluginUtils.Properties.Vip, () => "1");
        }

        message = message.Substring(0, message.Length - 1);

        if( !string.IsNullOrEmpty( m_text ) ) {
            message += $" :{m_channel}!{m_channel}@{m_channel}.tmi.twitch.tv {((IMessage)this).MessageType.ToString().ToUpper()} #{m_channel} :{m_text}";
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
using System.Collections.Immutable;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedPrivMsg : Message, IPrivMsg {
    private readonly string m_channel;
    private IImmutableList<Badge> m_badges;
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
        m_badges = badges ?? new ImmutableArray<Badge>();
        m_bits = bits ?? string.Empty;
        m_color = color ?? string.Empty;
        m_displayName = displayName ?? string.Empty;
        m_channel = channel;
        m_emotes = emotes;
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

    public override MessageType MessageType => MessageType.PrivMsg;


    IImmutableList<Badge> IPrivMsg.Badges => m_badges;

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

    string IPrivMsg.RawMessage => GetRawMessageString();
    
    private string GetRawMessageString() {
        string rawMessage = string.Empty;
        
        rawMessage += $"badges={m_badges}";
        rawMessage += $"bits={m_bits}";
        rawMessage += $"color={m_color}";
        rawMessage += $"display-name={m_displayName}";
        rawMessage += $"emote-only={m_emotes}";
        rawMessage += $"id={m_id}";
        rawMessage += $"mod={m_mod}";
        rawMessage += $"pinned-chat-paid-amount={m_pinnedChatPaidAmount}";
        rawMessage += $"pinned-chat-paid-currency={m_pinnedChatPaidCurrency}";
        rawMessage += $"pinned-chat-paid-exponent={m_pinnedChatPaidExponent}";
        rawMessage += $"pinned-chat-paid-level={m_pinnedChatPaidLevel}";
        rawMessage += $"pinned-chat-paid-is-system-message={m_pinnedChatPaidIsSystemMessage}";
        rawMessage += $"reply-parent-msg-id={m_replyParentMsgId}";
        rawMessage += $"reply-parent-user-id={m_replyParentUserId}";
        rawMessage += $"reply-parent-user-login={m_replyParentUserLogin}";
        rawMessage += $"reply-parent-display-name={m_replyParentDisplayName}";
        rawMessage += $"reply-thread-parent-msg-id={m_replyThreadParentMsg}";
        rawMessage += $"room-id={m_roomId}";
        rawMessage += $"subscriber={m_subscriber}";
        rawMessage += $"tmi-sent-ts={m_timestamp}";
        rawMessage += $"turbo={m_turbo}";
        rawMessage += $"user-id={m_userId}";
        rawMessage += $"user-type={m_userType}";
        rawMessage += $"vip={m_vip}";

        rawMessage += $" PRIVMSG #{m_channel} :";
        
        rawMessage += $"{m_text}";

        return rawMessage;
    }
}
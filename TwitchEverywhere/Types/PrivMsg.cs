using System.Collections.Immutable;

namespace TwitchEverywhere.Types; 

public record PrivMsg( 
    IImmutableList<Badge> Badges,
    string Bits,
    string? Color,
    string DisplayName,
    string Emotes,
    string Id,
    bool Mod,
    long? PinnedChatPaidAmount,
    string PinnedChatPaidCurrency,
    string PinnedChatPaidExponent,
    PinnedChatPaidLevel? PinnedChatPaidLevel,
    string PinnedChatPaidIsSystemMessage,
    string ReplyParentMsgId,
    string ReplyParentUserId,
    string ReplyParentUserLogin,
    string ReplyParentDisplayName,
    string ReplyThreadParentMsg,
    string RoomId,
    string Subscriber,
    DateTime Timestamp,
    string Turbo,
    string UserId,
    string UserType,
    string Vip,
    TimeSpan SinceStartOfStream,
    string Text,
    MessageType MessageType
);
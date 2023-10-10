using System.Collections.Immutable;

namespace TwitchEverywhere.Types; 

public record PrivMsg( 
    IImmutableList<Badge> Badges,
    string Bits,
    string? Color,
    string DisplayName,
    IImmutableList<Emote>? Emotes,
    string Id,
    bool Mod,
    long? PinnedChatPaidAmount,
    string PinnedChatPaidCurrency,
    long? PinnedChatPaidExponent,
    PinnedChatPaidLevel? PinnedChatPaidLevel,
    bool PinnedChatPaidIsSystemMessage,
    string ReplyParentMsgId,
    string ReplyParentUserId,
    string ReplyParentUserLogin,
    string ReplyParentDisplayName,
    string ReplyThreadParentMsg,
    string RoomId,
    bool Subscriber,
    DateTime Timestamp,
    bool Turbo,
    string UserId,
    UserType UserType,
    bool Vip,
    TimeSpan SinceStartOfStream,
    string Text,
    MessageType MessageType
) : Message( MessageType );
using System.Collections.Immutable;

namespace TwitchEverywhere.Types.Messages.Interfaces; 

public interface IPrivMsg : IMessage {
    IImmutableList<Badge> Badges { get; }
    IImmutableList<Badge> BadgeInfo { get; }
    string Bits { get; }
    string Color { get; }
    string DisplayName { get; }
    IImmutableList<Emote>? Emotes { get; }
    string Id { get; }
    bool Mod { get; }
    long? PinnedChatPaidAmount { get; }
    string PinnedChatPaidCurrency { get; }
    long? PinnedChatPaidExponent { get; }
    PinnedChatPaidLevel? PinnedChatPaidLevel { get; }
    bool PinnedChatPaidIsSystemMessage { get; }
    string ReplyParentMsgId { get; }
    string ReplyParentUserId { get; }
    string ReplyParentUserLogin { get; }
    string ReplyParentDisplayName { get; }
    string ReplyThreadParentMsg { get; }
    string RoomId { get; }
    bool Subscriber { get; }
    DateTime? Timestamp { get; }
    bool Turbo { get; }
    string UserId { get; }
    UserType UserType { get; }
    bool Vip { get; }
    string Text { get; }
}
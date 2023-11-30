using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedUnknownMessage : Message, IUnknownMessage {
    public override MessageType MessageType => MessageType.Unknown;

    string IUnknownMessage.Message => string.Empty;
}
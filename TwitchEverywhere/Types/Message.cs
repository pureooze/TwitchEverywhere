namespace TwitchEverywhere.Types;

public record Message {
    protected Message(
        MessageType messageType
    ) {
        MessageType = messageType;
    }

    public MessageType MessageType;
};
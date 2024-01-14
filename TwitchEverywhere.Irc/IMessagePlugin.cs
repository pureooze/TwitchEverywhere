using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;

namespace TwitchEverywhere.Irc; 

public interface IMessagePlugin {
    
    bool CanHandle(
        ReadOnlyMemory<byte> response,
        MessageType messageType
    );

    IMessage GetMessageData(
        RawMessage response
    );
}
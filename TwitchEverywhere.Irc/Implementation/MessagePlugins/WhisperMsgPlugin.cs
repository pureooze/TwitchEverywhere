using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class WhisperMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.Whisper;
    }
 
    IMessage IMessagePlugin.GetMessageData(
        RawMessage response
    ) {
        throw new NotImplementedException();
    }
}
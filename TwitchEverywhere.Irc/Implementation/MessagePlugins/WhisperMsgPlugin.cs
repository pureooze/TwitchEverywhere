using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class WhisperMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $" WHISPER " );
    }
    IMessage IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        return new LazyLoadedWhisperMsg(
            channel: channel,
            message: response 
        );
    }
}
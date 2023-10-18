using TwitchEverywhere.Types;

namespace TwitchEverywhere.Implementation.MessagePlugins; 

public class WhisperMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $" WHISPER " );
    }
    Message IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        return new WhisperMsg( message: response );
    }
}
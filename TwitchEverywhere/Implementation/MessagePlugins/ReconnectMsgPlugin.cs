using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;

namespace TwitchEverywhere.Implementation.MessagePlugins; 

public class ReconnectMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $" RECONNECT" );
    }

    Message IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        return new ReconnectMsg();
    }
}
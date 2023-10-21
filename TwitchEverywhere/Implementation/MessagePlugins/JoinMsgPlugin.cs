using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;

namespace TwitchEverywhere.Implementation.MessagePlugins; 

public class JoinMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $" JOIN #{channel}" );
    }
    Message IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        return new JoinMsg( response, channel );
    }
}
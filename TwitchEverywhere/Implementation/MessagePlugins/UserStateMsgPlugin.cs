using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;

namespace TwitchEverywhere.Implementation.MessagePlugins; 

public class UserStateMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $" USERSTATE #{channel}" );
    }

    Message IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        return new UserStateMsg( response );
    }
}
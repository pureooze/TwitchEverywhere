using TwitchEverywhere.Types;

namespace TwitchEverywhere.Implementation.MessagePlugins; 

public class GlobalUserStatePlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $" GLOBALUSERSTATE" );
    }
    Message IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        return new GlobalUserState( message: response );
    }
}
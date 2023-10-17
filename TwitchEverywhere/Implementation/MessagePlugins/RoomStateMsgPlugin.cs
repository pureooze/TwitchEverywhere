using TwitchEverywhere.Types;

namespace TwitchEverywhere.Implementation.MessagePlugins; 

public class RoomStateMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $" ROOMSTATE #{channel}" );
    }
    
    Message IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        return new RoomStateMsg( response );
    }
}
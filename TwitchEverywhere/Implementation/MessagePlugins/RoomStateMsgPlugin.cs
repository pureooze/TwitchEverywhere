using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;
using TwitchEverywhere.Types.Messages.LazyLoadedMessages;

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
        return new LazyLoadedRoomStateMsg( response );
    }
}
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class RoomStateMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $" ROOMSTATE #{channel}" );
    }
    
    IMessage IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        return new LazyLoadedRoomStateMsg(
            channel: channel,
            message: response
        );
    }
}
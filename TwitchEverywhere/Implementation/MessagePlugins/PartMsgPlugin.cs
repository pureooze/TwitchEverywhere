using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;

namespace TwitchEverywhere.Implementation.MessagePlugins; 

public class PartMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $" PART #{channel}" );
    }
    Message IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        return new LazyLoadedPartMsg( 
            message: response, 
            channel: channel 
        );
    }
}
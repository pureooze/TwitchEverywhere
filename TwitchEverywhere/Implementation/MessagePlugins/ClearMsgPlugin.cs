using System.Text.RegularExpressions;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.Implementation.MessagePlugins; 

public class ClearMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $" CLEARMSG #{channel}" );
    }

    Message IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        return new ClearMsg(
            channel: channel,
            message: response
        );
    }
}
using System.Text.RegularExpressions;
using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;

namespace TwitchEverywhere.Implementation.MessagePlugins; 

public class NoticeMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $" NOTICE #{channel}" );
    }

    Message IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        return new NoticeMsg(
            channel: channel,
            message: response
        );
    }
}
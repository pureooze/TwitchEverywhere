using TwitchEverywhere.Types;
using TwitchEverywhere.Types.Messages;
using TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

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
        return new ImmediateLoadedNoticeMsg(
            message: response
        );
    }
}
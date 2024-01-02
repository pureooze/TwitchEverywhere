using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class CapReqPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $":tmi.twitch.tv CAP * ACK" );
    }

    IMessage IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        return new LazyLoadedCapReq(
            channel,
            response
        );
    }
}
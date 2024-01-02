using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class JoinCountMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $"tmi.twitch.tv 353" );
    }

    IMessage IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        return new LazyLoadedJoinCountMsg(
            channel,
            response
        );
    }
}
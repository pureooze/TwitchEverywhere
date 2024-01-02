using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class JoinMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $" JOIN #{channel}" );
    }
    IMessage IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        return new LazyLoadedJoinMsg( response, channel );
    }
}
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Irc.MessagePlugins;
using TwitchEverywhere.Irc.Rx;

namespace TwitchEverywhere.Irc; 

public class MessageProcessor : IMessageProcessor {
    private readonly IEnumerable<IMessagePlugin> m_messagePlugins = new IMessagePlugin[] {
        // Membership
        new JoinMsgPlugin(),
        new PartMsgPlugin(),
        new CapReqPlugin(),
        new JoinCountMsgPlugin(),
        new JoinEndMsgPlugin(),
            
        // IRC Commands
        new PrivMsgPlugin(),
        new ClearChatPlugin(),
        new ClearMsgPlugin(),
        new NoticeMsgPlugin(),
        new GlobalUserStatePlugin(),
        new RoomStateMsgPlugin(),
        new WhisperMsgPlugin(),
        new UserNoticePlugin(),
        new UserStateMsgPlugin(),
        new HostTargetMsgPlugin(),
        new ReconnectMsgPlugin(),
            
        // Nothing worked, just give the raw message
        new UnknownMsgPlugin()
    };

    void IMessageProcessor.ProcessMessageRx(
        RawMessage response,
        string channel,
        IrcClientObserver observer
    ) {
        foreach (IMessagePlugin messagePlugin in m_messagePlugins) {
            if( !messagePlugin.CanHandle( response.Type ) ) {
                continue;
            }

            messagePlugin.ProcessMessage( observer, response );
            break;
        }
    }
}
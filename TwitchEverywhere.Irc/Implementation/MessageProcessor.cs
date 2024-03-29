using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Irc.Implementation.MessagePlugins;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc.Implementation; 

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
        IrcClientSubject subjects
    ) {
        foreach (IMessagePlugin messagePlugin in m_messagePlugins) {
            if( !messagePlugin.CanHandle( response.Type ) ) {
                continue;
            }

            messagePlugin.ProcessMessage( subjects, response );
            break;
        }
    }
}
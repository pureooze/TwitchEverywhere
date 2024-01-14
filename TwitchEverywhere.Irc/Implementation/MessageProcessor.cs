using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Irc.Implementation.MessagePlugins;

namespace TwitchEverywhere.Irc.Implementation; 

public class MessageProcessor : IMessageProcessor {
    private readonly IEnumerable<IMessagePlugin> m_messagePlugins = new IMessagePlugin[] {
        // Membership
        new JoinMsgPlugin(),
        // new PartMsgPlugin(),
        // new CapReqPlugin(),
        // new JoinCountMsgPlugin(),
        // new JoinEndMsgPlugin(),
            
        // IRC Commands
        new PrivMsgPlugin(),
        new ClearChatPlugin(),
        // new ClearMsgPlugin(),
        // new NoticeMsgPlugin(),
        // new GlobalUserStatePlugin(),
        // new RoomStateMsgPlugin(),
        // new WhisperMsgPlugin(),
        // new UserNoticePlugin(),
        // new UserStateMsgPlugin(),
        // new HostTargetMsgPlugin(),
        // new ReconnectMsgPlugin(),
            
        // Nothing worked, just give the raw message
        // new UnknownMsgPlugin()
    };

    public void ProcessMessage(
        RawMessage response,
        string channel,
        Action<IMessage> callback
    ) {
        foreach (IMessagePlugin messagePlugin in m_messagePlugins) {
            if( !messagePlugin.CanHandle( response.Data, response.Type ) ) {
                continue;
            }

            IMessage message = messagePlugin.GetMessageData( response );
            callback( message );
            break;
        }
    }
}
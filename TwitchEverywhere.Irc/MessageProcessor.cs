using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Irc.MessagePlugins;


namespace TwitchEverywhere.Irc; 

public class MessageProcessor : IMessageProcessor {
    private readonly IEnumerable<IMessagePlugin> _messagePlugins = [
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
    ];

    void IMessageProcessor.ProcessMessage(
        RawMessage response,
        string channel,
        Action<IMessage> callback
    ) {
        foreach (IMessagePlugin messagePlugin in _messagePlugins) {
            if( !messagePlugin.CanHandle( response.Type ) ) {
                continue;
            }

            IMessage message = messagePlugin.GetMessageData( response );
            callback( message );
            break;
        }
    }
}
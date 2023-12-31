using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Irc.Implementation.MessagePlugins;

namespace TwitchEverywhere.Irc.Implementation; 

public class MessageProcessor : IMessageProcessor {
    private readonly IEnumerable<IMessagePlugin> m_messagePlugins;

    public MessageProcessor(
        IDateTimeService dateTimeService
    ) {
        m_messagePlugins = new IMessagePlugin[] {
            // Membership
            new JoinMsgPlugin(),
            new PartMsgPlugin(),
            new CapReqPlugin(),
            new JoinCountMsgPlugin(),
            new JoinEndMsgPlugin(),
            
            // IRC Commands
            new PrivMsgPlugin( dateTimeService: dateTimeService ),
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
    }

    void IMessageProcessor.ProcessMessage(
        string response,
        string channel,
        Action<IMessage> callback
    ) {

        foreach (IMessagePlugin messagePlugin in m_messagePlugins) {
            if( !messagePlugin.CanHandle( response, channel ) ) {
                continue;
            }

            IMessage message = messagePlugin.GetMessageData( response, channel );
            callback( message );
            break;
        }
    }
}
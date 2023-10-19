using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.Implementation; 

public class MessageProcessor : IMessageProcessor {
    private readonly IEnumerable<IMessagePlugin> m_messagePlugins;

    public MessageProcessor(
        IDateTimeService dateTimeService
    ) {
        // TODO: use Dependency Injection
        m_messagePlugins = new IMessagePlugin[] {
            new PrivMsgPlugin( dateTimeService: dateTimeService ),
            new ClearChatPlugin(),
            new ClearMsgPlugin(),
            new NoticeMsgPlugin(),
            new GlobalUserStatePlugin(),
            new RoomStateMsgPlugin(),
            new WhisperMsgPlugin(),
            new UserNoticePlugin(),
            new UnknownMsgPlugin()
        };
    }

    void IMessageProcessor.ProcessMessage(
        string response,
        string channel,
        Action<Message> callback
    ) {

        foreach (IMessagePlugin messagePlugin in m_messagePlugins) {
            if( !messagePlugin.CanHandle( response, channel ) ) {
                continue;
            }

            Message message = messagePlugin.GetMessageData( response, channel );
            callback( message );
            break;
        }
    }
}
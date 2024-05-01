using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Irc.Rx;

namespace TwitchEverywhere.Irc;

public sealed class IrcClient(
    TwitchConnectionOptions options
) {

    private readonly MessageProcessor m_messageProcessor = new();
    private TwitchConnectorRx m_twitchConnectorRx;
    
    public void ConnectToChannelRx(
        string channel,
        IrcClientObserver observer
    ) {
        m_twitchConnectorRx = new TwitchConnectorRx(
            messageProcessor: m_messageProcessor,
            channel: channel, 
            options: options, 
            ircClientObserver: observer
        );
    }
    
    public async Task SendMessage(
        IMessage message
    ) {
        await m_twitchConnectorRx.SendMessage(message);
    }
    
    public Task<bool> Disconnect() {
        return m_twitchConnectorRx.Disconnect();
    }
}
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;

namespace TwitchEverywhere.Irc;

public sealed class IrcClient {

    private readonly MessageProcessor m_messageProcessor = new();
    private TwitchConnector _twitchConnector;
    private readonly TwitchConnectionOptions _options;
    private readonly string _channel;

    public IrcClient(
        TwitchConnectionOptions options,
        string channel,
        Action<IMessage> messageCallback
    ) {
        _options = options;
        _channel = channel;
        _twitchConnector = new TwitchConnector(
            messageProcessor: m_messageProcessor,
            options: _options,
            channel: _channel,
            messageCallback: messageCallback
        );
    }
    
    public async Task SendMessage(
        IMessage message,
        MessageType messageType
    ) {
        await _twitchConnector.SendMessage(
            message,
            messageType,
            _channel,
            _options
        );
    }
    
    public Task<bool> Disconnect(
        string channel
    ) {
        return _twitchConnector.Disconnect(channel);
    }
}
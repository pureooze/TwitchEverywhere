using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;
using MessagePluginUtils = TwitchEverywhere.Implementation.MessagePluginUtils;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages; 

public class ImmediateLoadedPartMsg : IPartMsg {
    private readonly string m_message;
    private readonly string m_channel;
    private readonly string m_user;

    public ImmediateLoadedPartMsg(
        string channel,
        string? message = null,
        string? user = null
    ) {
        m_channel = channel;
        m_message = message ?? string.Empty;
        m_user = user ?? string.Empty;
    }


    public MessageType MessageType => MessageType.Part;
    public string RawMessage => GetRawMessage();
    public string Channel => m_channel;

    string IPartMsg.User => m_user;

    private string GetRawMessage() {
        return $":{m_user}!{m_user}@{m_user}.tmi.twitch.tv {MessageType.ToString().ToUpper()} #{m_channel}";
    }

    private string SerializeProperty(
        MessagePluginUtils.Properties property,
        Func<string> serializer
    ) {

        return string.Format( MessagePluginUtils.GetPropertyAsString( property ), serializer() );
    }

}
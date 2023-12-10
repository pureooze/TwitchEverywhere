using TwitchEverywhere.Types.Messages.Interfaces;
using MessagePluginUtils = TwitchEverywhere.Implementation.MessagePluginUtils;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedClearMsg : IClearMsg {
    private readonly string m_channel;
    private readonly string m_login;
    private readonly string m_roomId;
    private readonly string m_targetMessageId;
    private readonly DateTime m_timestamp;
    private readonly string m_text;

    public ImmediateLoadedClearMsg(
        string channel,
        string? login = null,
        string? roomId = null,
        string? targetMessageId = null,
        DateTime timestamp = default,
        string? text = null
    ) {
        m_channel = channel;
        m_login = login ?? string.Empty;
        m_roomId = roomId ?? string.Empty;
        m_targetMessageId = targetMessageId ?? string.Empty;
        m_timestamp = timestamp;
        m_text = text ?? string.Empty;
    }
    public MessageType MessageType => MessageType.ClearMsg;
    public string RawMessage => GetRawMessage();

    string IMessage.Channel => m_channel;
    
    string IClearMsg.Login => m_login;

    string IClearMsg.RoomId => m_roomId;

    string IClearMsg.TargetMessageId => m_targetMessageId;

    DateTime IClearMsg.Timestamp => m_timestamp;
    
    string IClearMsg.Text => m_text;

    private string GetRawMessage() {
        string message = "@";

        if( !string.IsNullOrEmpty( m_login ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.Login, () => m_login );
        }
        
        if( !string.IsNullOrEmpty( m_roomId ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.RoomId, () => m_roomId );
        }
        
        if( !string.IsNullOrEmpty( m_targetMessageId ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.TargetMessageId, () => m_targetMessageId );
        }

        message += SerializeProperty( MessagePluginUtils.Properties.MessageTimestamp, () => new DateTimeOffset(m_timestamp).ToUnixTimeMilliseconds().ToString());

        message = message.Substring(0, message.Length - 1);
        
        message += $" :tmi.twitch.tv {MessageType.ToString().ToUpper()} #{m_channel} :{m_text}";
        
        return message;
    }
    
    private string SerializeProperty(
        MessagePluginUtils.Properties property,
        Func<string> serializer
    ) {

        return string.Format( MessagePluginUtils.GetPropertyAsString( property ), serializer() );
    }
}
using TwitchEverywhere.Implementation.MessagePlugins;

namespace TwitchEverywhere.Types;

public class ClearChat : Message {
    private readonly string m_channel;
    private readonly string m_message;
    
    public ClearChat(
        string channel,
        string message
    ) {
        m_channel = channel;
        m_message = message;
    }

    public override MessageType MessageType => MessageType.ClearChat;
    
    public long? Duration {
        get {
            string duration = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.BanDurationPattern );
            return string.IsNullOrEmpty( duration ) ? null : long.Parse( duration );
        }
    }

    public string RoomId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.RoomIdPattern );

    public string UserId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.TargetUserIdPattern );
    
    public DateTime Timestamp {
        get {
            long rawTimestamp = Convert.ToInt64(
                MessagePluginUtils.MessageTimestampPattern.Match( m_message ).Value
                    .Split( "=" )[1]
                    .TrimEnd( ';' )
            );

            return DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;
        }
    }

    public string Text => m_message.Split( $"CLEARCHAT #{m_channel}" )[1].Trim( '\r', '\n' );
}
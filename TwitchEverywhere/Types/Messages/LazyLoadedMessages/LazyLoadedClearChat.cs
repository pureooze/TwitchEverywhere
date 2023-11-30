using System.Text.RegularExpressions;
using TwitchEverywhere.Implementation.MessagePlugins;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages;

public class LazyLoadedClearChat : Message {
    private readonly string m_channel;
    private readonly string m_message;
    
    public LazyLoadedClearChat(
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

    public string Text => MessagePluginUtils.GetLastSplitValuesFromResponse( m_message, new Regex($"CLEARCHAT #{m_channel} :") ).Trim('\n');
}
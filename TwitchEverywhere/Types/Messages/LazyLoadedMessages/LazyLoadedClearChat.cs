using System.Text.RegularExpressions;
using TwitchEverywhere.Types.Messages.Interfaces;
using MessagePluginUtils = TwitchEverywhere.Implementation.MessagePluginUtils;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages;

public class LazyLoadedClearChat : IClearChat {
    private readonly string m_channel;
    private readonly string m_message;
    
    public LazyLoadedClearChat(
        string channel,
        string message
    ) {
        m_channel = channel;
        m_message = message;
    }

    public MessageType MessageType => MessageType.ClearChat;
    
    public string RawMessage => m_message;

    public string Channel => m_channel;

    public long? Duration {
        get {
            string duration = MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.BanDurationPattern() );
            return string.IsNullOrEmpty( duration ) ? null : long.Parse( duration );
        }
    }

    public string RoomId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.RoomIdPattern() );

    public string TargetUserId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.TargetUserIdPattern() );
    
    public DateTime Timestamp {
        get {
            long rawTimestamp = Convert.ToInt64(
                MessagePluginUtils.MessageTimestampPattern().Match( m_message ).Value
                    .Split( "=" )[1]
                    .TrimEnd( ';' )
            );

            return DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;
        }
    }

    public string Text => MessagePluginUtils.GetLastSplitValuesFromResponse( m_message, new Regex($"CLEARCHAT #{m_channel} :") ).Trim('\n');

    public string TargetUserName => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.MsgTextPattern() );
}
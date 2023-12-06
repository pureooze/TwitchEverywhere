using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedClearMsg : IClearMsg {
    private readonly string m_message;
    
    public LazyLoadedClearMsg(
        string channel,
        string message
    ) {
        Channel = channel;
        m_message = message;
    }
    
    public MessageType MessageType => MessageType.ClearMsg;
    
    public string RawMessage => m_message;

    public string Channel { get; }

    public string Login => MessagePluginUtils.GetLastSplitValuesFromResponse( m_message, MessagePluginUtils.LoginPattern );

    public string RoomId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.RoomIdPattern );
    
    public string TargetMessageId => MessagePluginUtils.GetLastSplitValuesFromResponse( m_message, MessagePluginUtils.TargetMessageIdPattern );
    
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
}
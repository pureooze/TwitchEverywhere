using TwitchEverywhere.Implementation.MessagePlugins;

namespace TwitchEverywhere.Types.Messages; 

public class ClearMsg : Message {
    private readonly string m_channel;
    private readonly string m_message;
    
    public ClearMsg(
        string channel,
        string message
    ) {
        m_channel = channel;
        m_message = message;
    }
    
    public override MessageType MessageType => MessageType.ClearMsg;

    public string Login => MessagePluginUtils.LoginPattern
        .Match( m_message )
        .Value
        .Split( "=" )[1]
        .TrimEnd( ';' );

    public string RoomId => MessagePluginUtils.GetValueFromResponse( m_message, MessagePluginUtils.RoomIdPattern );
    
    public string TargetMessageId => MessagePluginUtils.TargetMessageIdPattern
        .Match( m_message )
        .Value
        .Split( "=" )[1]
        .TrimEnd( ';' );
    
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
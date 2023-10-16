using TwitchEverywhere.Types;

namespace TwitchEverywhere.Implementation.MessagePlugins; 

public class PrivMsgPlugin : IMessagePlugin {

    private readonly DateTime m_startTimestamp;

    public PrivMsgPlugin(
        IDateTimeService dateTimeService
    ) {
        m_startTimestamp = dateTimeService.GetStartTime();
    }

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $" PRIVMSG #{channel}" );
    }

    Message IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        string[] segments = response.Split( $"PRIVMSG #{channel} :" );

        long rawTimestamp = Convert.ToInt64(
            MessagePluginUtils.MessageTimestampPattern.Match( response ).Value
            .Split( "=" )[1]
            .TrimEnd( ';' )
        );

        DateTime messageTimestamp = DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;
        TimeSpan timeSinceStartOfStream = messageTimestamp - m_startTimestamp;
        
        if( segments.Length <= 1 ) {
            throw new UnexpectedUserMessageException();
        }

        return new PrivMsg(
            channel,
            response,
            timeSinceStartOfStream
        );
    }
}
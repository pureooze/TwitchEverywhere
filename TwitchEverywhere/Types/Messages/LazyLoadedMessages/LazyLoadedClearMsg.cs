using System.Text.RegularExpressions;
using TwitchEverywhere.Types.Messages.Interfaces;
using MessagePluginUtils = TwitchEverywhere.Implementation.MessagePluginUtils;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedClearMsg(
    string channel,
    string message
) : IClearMsg {

    public MessageType MessageType => MessageType.ClearMsg;
    
    public string RawMessage => message;

    public string Channel { get; } = channel;

    public string Login => MessagePluginUtils.GetLastSplitValuesFromResponse( message, MessagePluginUtils.LoginPattern() );

    public string RoomId => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.RoomIdPattern() );
    
    public string TargetMessageId => MessagePluginUtils.GetLastSplitValuesFromResponse( message, MessagePluginUtils.TargetMessageIdPattern() );
    
    public DateTime Timestamp {
        get {
            long rawTimestamp = Convert.ToInt64(
                MessagePluginUtils.MessageTimestampPattern().Match( message ).Value
                    .Split( "=" )[1]
                    .TrimEnd( ';' )
            );

            return DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;
        }
    }

    string IClearMsg.Text => MessagePluginUtils.GetLastSplitValuesFromResponse( message, new Regex($"CLEARMSG #{Channel} :") ).Trim('\n');
}
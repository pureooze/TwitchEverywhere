using System.Text.RegularExpressions;
using TwitchEverywhere.Types.Messages.Interfaces;
using MessagePluginUtils = TwitchEverywhere.Implementation.MessagePluginUtils;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages;

public class LazyLoadedClearChat(
    string channel,
    string message
) : IClearChat {

    public MessageType MessageType => MessageType.ClearChat;
    
    public string RawMessage => message;

    public string Channel => channel;

    public long? Duration {
        get {
            string duration = MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.BanDurationPattern() );
            return string.IsNullOrEmpty( duration ) ? null : long.Parse( duration );
        }
    }

    public string RoomId => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.RoomIdPattern() );

    public string TargetUserId => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.TargetUserIdPattern() );
    
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

    public string Text => MessagePluginUtils.GetLastSplitValuesFromResponse( message, new Regex($"CLEARCHAT #{channel} :") ).Trim('\n');

    public string TargetUserName => MessagePluginUtils.GetValueFromResponse( message, MessagePluginUtils.MsgTextPattern() );
}
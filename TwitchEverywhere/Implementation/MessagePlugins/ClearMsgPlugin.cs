using System.Text.RegularExpressions;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.Implementation.MessagePlugins; 

public class ClearMsgPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $" CLEARMSG #{channel}" );
    }

    Message IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        string login = MessagePluginRegex.LoginPattern
            .Match( response )
            .Value
            .Split( "=" )[1]
            .TrimEnd( ';' );
        
        string targetMessageId = MessagePluginRegex.TargetMessageIdPattern
            .Match( response )
            .Value
            .Split( "=" )[1]
            .TrimEnd( ';' );
        
        long rawTimestamp = Convert.ToInt64(
            MessagePluginRegex.MessageTimestampPattern.Match( response ).Value
                .Split( "=" )[1]
        );
        
        string roomId = GetValueFromResponse( response, MessagePluginRegex.RoomIdPattern );

        DateTime messageTimestamp = DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;
        
        return new ClearMsg(
            Login: login,
            RoomId: roomId,
            TargetMessageId: targetMessageId,
            Timestamp: messageTimestamp
        );
    }
    
    private static string GetValueFromResponse(
        string response,
        Regex pattern
    ) {
        Match match = pattern
            .Match( response );

        string result = string.Empty;
        if( match.Success ) {
            result = match.Value.Split( "=" )[1].TrimEnd( ';' );
        }

        return result;
    }

    
}
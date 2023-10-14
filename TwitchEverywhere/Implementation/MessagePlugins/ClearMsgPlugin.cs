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
        string login = LoginPattern
            .Match( response )
            .Value
            .Split( "=" )[1]
            .TrimEnd( ';' );
        
        string targetMessageId = TargetMessageIdPattern
            .Match( response )
            .Value
            .Split( "=" )[1]
            .TrimEnd( ';' );
        
        long rawTimestamp = Convert.ToInt64(
            MessageTimestampPattern.Match( response ).Value
                .Split( "=" )[1]
        );
        
        string roomId = GetValueFromResponse( response, RoomIdPattern );

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

    private readonly static Regex RoomIdPattern = new("room-id=([^;]*);");
    private readonly static Regex LoginPattern = new("login([^;]*)");
    private readonly static Regex TargetMessageIdPattern = new("target-msg-id([^;]*)");
    private readonly static Regex MessageTimestampPattern = new("tmi-sent-ts=([0-9]+)");
}
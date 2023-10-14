using System.Text.RegularExpressions;
using TwitchEverywhere.Types;

namespace TwitchEverywhere.Implementation.MessagePlugins; 

public class ClearChatPlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        string response,
        string channel
    ) {
        return response.Contains( $" CLEARCHAT #{channel}" );
    }

    Message IMessagePlugin.GetMessageData(
        string response,
        string channel
    ) {
        string[] segments = response.Split( $"CLEARCHAT #{channel}" );

        string duration = GetValueFromResponse( response, BanDurationPattern );
        string roomId = GetValueFromResponse( response, RoomIdPattern );
        string targetUserId = GetValueFromResponse( response, TargetUserIdPattern );
        
        long rawTimestamp = Convert.ToInt64(
            MessageTimestampPattern.Match( response ).Value
                .Split( "=" )[1]
        );

        DateTime messageTimestamp = DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;

        return new ClearChat(
            Duration: string.IsNullOrEmpty( duration ) ? null : long.Parse( duration ),
            RoomId: roomId,
            UserId: targetUserId,
            Timestamp: messageTimestamp,
            Text: segments[1]
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
    
    private readonly static Regex BanDurationPattern = new("ban-duration=([^;]*)");
    private readonly static Regex RoomIdPattern = new("room-id=([^;]*);");
    private readonly static Regex MessageTimestampPattern = new("tmi-sent-ts=([0-9]+)");
    private readonly static Regex TargetUserIdPattern = new("target-user-id=([^ ;]*)");
}
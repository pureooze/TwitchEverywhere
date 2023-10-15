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
        return new ClearChat(
            channel: channel,
            message: response
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
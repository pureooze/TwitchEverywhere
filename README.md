# TwitchEverywhere

`TwitchEverywhere` is a .NET Core (6 and 7) library that allows connecting to a Twitch chat and subscribing to messages in that chat.

The goal of this library is to provide a lightweight, strongly typed API for clients so they can avoid parsing raw strings as much as possible.
Additionally, Twitch requires an authenticated connection to the IRC server which can get a bit complicated to setup and maintain. 
Fortunately `TwitchEverywhere` can do that for you! 😀

If you are planning on using `TwitchEverywhere` on AWS I suggest using .NET 6 for compatibility reasons.

## How To Use It
You will need to provide the following values as parameters to the `TwitchConnectionOptions` record:
```csharp
TwitchConnectionOptions options = new(
    Channel: channel,
    AccessToken: accessToken,
    RefreshToken: refreshToken,
    ClientId: clientId,
    ClientSecret: clientSecret,
    ClientName: clientName
);
```

Next define a `MessageCallback` method that will handle any messages that `TwitchEverywhere` sends to your application.
The input will be of type `TwitchEverywhere.Types.Message` and return type `void`.
```csharp
/* 
This example only handles MessageType.PrivMsg
but you should handle other types here too
*/
private void MessageCallback( Message message ) {
    switch( message.MessageType ) {
        case MessageType.PrivMsg: {
            PrivMsg privMsg = (PrivMsg) message;
            PrivMessageCallback( privMsg );
            break;
        }
        default:
            // This is just an example, uou can handle this case however you wish
            throw new ArgumentOutOfRangeException();
    }
}
```

Then initialize `TwitchEverywhere` and pass in the options to the constructor.
Finally call the `TwitchEverywhere.ConnectToChannel` method and pass in your callback as a parameter.
```csharp
TwitchEverywhere.TwitchEverywhere twitchEverywhere = new( options );
await twitchEverywhere.ConnectToChannel( MessageCallback );
```

Now whenever `TwitchEverywhere` receives a message it will pass it to your callback! 🎉

## Sample CLI App
There is a sample CLI application that is included as an example in this repo and you can use it to connect with Twitch – give it a try!

In order to connect you need to create an `appsettings.json` file in the root of the `TwitchEverywhereCLI` project with the following parameters:

```json
{
  "AccessToken": "your_twitch_access_token",
  "RefreshToken": "your_twitch_refresh_token",
  "ClientId": "your_client_id",
  "ClientSecret": "your_client_secret",
  "ClientName": "your_client_name_all_lowercase",
  "Channel": "channel_you_want_to_connect_to"
}
```

## Supported Functionality

### PRIVMSG
[PRIVMSG Twitch API](https://dev.twitch.tv/docs/irc/tags/#privmsg-tags)

[PrivMsg Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/PrivMsg.cs)

| Field                         | Support |
|-------------------------------|---------|
| Badges                        | ✅       |
| Bits                          | ✅       |
| Color                         | ✅       |
| DisplayName                   | ✅       |
| Emotes                        | ✅       |
| Id                            | ✅       |
| Mod                           | ✅       |
| PinnedChatPaidAmount          | ✅       |
| PinnedChatPaidCurrency        | ✅       |
| PinnedChatPaidExponent        | ✅       |
| PinnedChatPaidLevel           | ✅       |
| PinnedChatPaidIsSystemMessage | ✅       |
| ReplyParentMsgId              | ✅       |
| ReplyParentUserId             | ✅       |
| ReplyParentUserLogin          | ✅       |
| ReplyParentDisplayName        | ✅       |
| ReplyThreadParentMsg          | ✅       |
| RoomId                        | ✅       |
| Subscriber                    | ✅       |
| Timestamp                     | ✅       |
| Turbo                         | ✅       |
| UserId                        | ✅       |
| UserType                      | ✅       |
| Vip                           | ✅       |

### CLEARCHAT
WIP

### CLEARMSG
WIP

### GlobalUserState
WIP

### Notice
WIP

### RoomState
WIP

### UserNotice
WIP

### UserState
WIP

### Whisper
WIP

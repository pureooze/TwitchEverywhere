# TwitchEverywhere

`TwitchEverywhere` is a .NET Core (6 and 7) library that allows connecting to a Twitch chat and subscribing to messages in that chat.

The goal of this library is to provide strongly typed interfaces for clients so they can avoid parsing raw strings as much as possible.
Additionally, Twitch requires an authenticated connection to the IRC server which can get a bit complicated to setup and maintain. 
Fortunately `TwitchEverywhere` can do that for you! 😀

If you are planning on using `TwitchEverywhere` on AWS I suggest using .NET 6 for compatibility reasons.

## How To Use The CLI
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

| Field                         | Support                      |
|-------------------------------|------------------------------|
| Badges                        | ✅                            |
| Bits                          | ✅                            |
| Color                         | ✅                            |
| DisplayName                   | ✅                            |
| Emotes                        | WIP (currently a raw string) |
| Id                            | ✅                            |
| Mod                           | ✅                            |
| PinnedChatPaidAmount          | ✅                            |
| PinnedChatPaidCurrency        | ✅                            |
| PinnedChatPaidExponent        | ✅                            |
| PinnedChatPaidLevel           | ✅                            |
| PinnedChatPaidIsSystemMessage | ✅                            |
| ReplyParentMsgId              | ✅                            |
| ReplyParentUserId             | ✅                            |
| ReplyParentUserLogin          | ✅                            |
| ReplyParentDisplayName        | ✅                            |
| ReplyThreadParentMsg          | ✅                            |
| RoomId                        | ✅                            |
| Subscriber                    | ✅                            |
| Timestamp                     | ✅                            |
| Turbo                         | ✅                            |
| UserId                        | ✅                            |
| UserType                      | ✅                            |
| Vip                           | ✅                            |

### CLEARCHAT
WIP

### CLEARMSG
WIP

### GlobalUserState

### Notice
WIP

### PrivMsg
WIP

### RoomState
WIP

### UserNotice
WIP

### UserState
WIP

### Whisper
WIP

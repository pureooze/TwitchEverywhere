# TwitchEverywhere.Irc
`TwitchEverywhere.Irc` is a .NET library that allows connecting to a Twitch chat using IRC and subscribing to messages in that chat.

The goal of this library is to provide a lightweight, strongly typed API for clients so they can avoid parsing raw strings as much as possible.
`TwitchEverywhere.Irc` uses Rx.NET to provide a reactive API for handling messages from Twitch.

The library will support the latest LTS version of .NET and non LTS versions IF it is newer than the LTS version.
So for example before .NET 8, the library supported .NET 6 (LTS) and 7 but once .NET 8 was released support for 6 and 7 was dropped.

<!-- TOC -->
* [How To Use It](#how-to-use-it)
* [Sample CLI App](#sample-cli-app)
* [Performance](#performance)
* [Supported Functionality](#supported-functionality)
  * [IRC Commands](#irc-commands)
    * [CLEARCHAT](#clearchat)
    * [CLEARMSG](#clearmsg)
    * [GLOBALUSERSTATE](#globaluserstate)
    * [HOSTTARGET](#hosttarget)
    * [NOTICE](#notice)
    * [RECONNECT](#reconnect)
    * [ROOMSTATE](#roomstate)
    * [USERNOTICE](#usernotice)
    * [USERSTATE](#userstate)
    * [WHISPER](#whisper)
  * [IRC Membership](#irc-membership)
    * [JOIN](#join)
    * [PART](#part)
  * [IRC Tags](#irc-tags)
    * [PRIVMSG Tags](#privmsg-tags)
    * [CLEARCHAT Tags](#clearchat-tags)
    * [CLEARMSG Tags](#clearmsg-tags)
    * [GLOBALUSERSTATE Tags](#globaluserstate-tags)
    * [NOTICE Tags](#notice-tags)
    * [ROOMSTATE Tags](#roomstate-tags)
    * [USERNOTICE Tags](#usernotice-tags)
    * [USERSTATE Tags](#userstate-tags)
    * [WHISPER Tags](#whisper-tags)
<!-- TOC -->

## How To Use It
You will need to provide the following values as parameters to the `TwitchEverywhere.Core.TwitchConnectionOptions` record:
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

Next create an instance of the IrcClient and pass in the options to the constructor.
```csharp
internal class TwitchConnection(
    TwitchConnectionOptions options
) {
    private readonly IrcClient m_ircClient = new( options );
    
    // implementation
}
```

Then define a method that will consume the observables returned from `TwitchEverywhere`.
Each message type has its own observable that you can subscribe to and it will return a strong type.
```csharp

// This example only handles MessageType.PrivMsg but you can handle other types here too
public async Task ConnectToIrcClientRx() {
    
    // TaskCompletionSource is only needed for the CLI app to prevent it from exiting early
    TaskCompletionSource<bool> tcs = new();

    IrcClientObservable observable = m_ircClient.ConnectToChannelRx();

    IDisposable privObservable = observable.PrivMsgObservable.Subscribe( 
        msg => PrivMessageCallback(msg)
    );

    try {
        await tcs.Task;
    } finally{
        joinObservable.Dispose();
        privObservable.Dispose();
    }
}
```

Now whenever `TwitchEverywhere` receives a message it will pass it to the appropriate observable 🎉

## Sample CLI App
There is a [sample CLI application here](https://github.com/pureooze/TwitchEverywhere/tree/main/TwitchEverywhereCLI) that is included as an example in this repo and you can use it to connect with Twitch – give it a try!

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

## Performance
See the README in the `TwitchEverywhere.Benchmark` project [here](https://github.com/pureooze/TwitchEverywhere/tree/main/TwitchEverywhere.Benchmark)


## Supported Functionality

### IRC Commands
See the related Twitch documentation [here](https://dev.twitch.tv/docs/irc/commands/)

| Name              | API Link                                                                               | Data Type Link                                                                                                                    |
|-------------------|----------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------|
| `CLEARCHAT`       | [CLEARCHAT Twitch API](https://dev.twitch.tv/docs/irc/commands/#clearchat)             | [ClearChat Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/ClearChat.cs)             |
| `CLEARMSG`        | [CLEARMSG Twitch API](https://dev.twitch.tv/docs/irc/commands/#clearmsg)               | [ClearMsg Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/ClearMsg.cs)               |
| `GLOBALUSERSTATE` | [GLOBALUSERSTATE Twitch API](https://dev.twitch.tv/docs/irc/commands/#globaluserstate) | [GlobalUserState Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/GlobalUserState.cs) |
| `HOSTTARGET`      | [HOSTTARGET Twitch API](https://dev.twitch.tv/docs/irc/commands/#hosttarget)           | [HostTargetMsg Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/HostTargetMsg.cs)     |
| `NOTICE`          | [NOTICE Twitch API](https://dev.twitch.tv/docs/irc/commands/#notice)                   | [NoticeMsg Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/NoticeMsg.cs)             |
| `RECONNECT`       | [RECONNECT Twitch API](https://dev.twitch.tv/docs/irc/commands/#reconnect)             | [ReconnectMsg Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/ReconnectMsg.cs)       |
| `ROOMSTATE`       | [ROOMSTATE Twitch API](https://dev.twitch.tv/docs/irc/commands/#roomstate)             | [RoomStateMsg Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/RoomStateMsg.cs)       |
| `USERNOTICE`      | [USERNOTICE Twitch API](https://dev.twitch.tv/docs/irc/commands/#usernotice)           | [UserNotice Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/UserNotice.cs)           |
| `USERSTATE`       | [USERSTATE Twitch API](https://dev.twitch.tv/docs/irc/commands/#userstate)             | [UserState Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/UserStateMsg.cs)          |
| `WHISPER`         | [WHISPER Twitch API](https://dev.twitch.tv/docs/irc/commands/#whisper)                 | [WHISPER Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/WhisperMsg.cs)              |

#### CLEARCHAT
See [CLEARCHAT](#clearchat-tags) tags

#### CLEARMSG
See [CLEARMSG](#clearmsg-tags) tags

#### GLOBALUSERSTATE
See [GLOBALUSERSTATE](#globaluserstate-tags) tags

#### HOSTTARGET
| Field              | Support |
|--------------------|---------|
| `HostingChannel`   | ✅       |
| `Channel`          | ✅       |
| `NumberOfViewers`  | ✅       |
| `IsHostingChannel` | ✅       |

#### NOTICE
See [NOTICE](#notice-tags) tags

#### RECONNECT
No fields for `RECONNECT`, use the presence of this message as a signal to reconnect to Twitch. See the [Twitch API docs](https://dev.twitch.tv/docs/irc/commands/#reconnect) for more infomation.

#### ROOMSTATE
See [ROOMSTATE](#roomstate-tags) tags

#### USERNOTICE
See [USERNOTICE](#usernotice-tags) tags

#### USERSTATE
See [USERSTATE](#userstate-tags) tags

#### WHISPER
See [WHISPER](#whisper-tags) tags


### IRC Membership
See the related Twitch documentation [here](https://dev.twitch.tv/docs/irc/membership/).

| Name   | API Link                                                           | Data Type Link                                                                                                 |
|--------|--------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------|
| `JOIN` | [JOIN Twitch API](https://dev.twitch.tv/docs/irc/membership/#join) | [Join Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/JoinMsg.cs) |
| `PART` | [PART Twitch API](https://dev.twitch.tv/docs/irc/membership/#join) | [Part Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/PartMsg.cs) |


#### JOIN
| Field     | Support |
|-----------|---------|
| `Channel` | ✅       |
| `User`    | ✅       |

#### PART
| Field     | Support |
|-----------|---------|
| `Channel` | ✅       |
| `User`    | ✅       |

### IRC Tags
See the related Twitch documentation [here](https://dev.twitch.tv/docs/irc/tags/).

| Name              | Support | API Link                                                                                | Data Type Link                                                                                                                    |
|-------------------|---------|-----------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------|
| `PRIVMSG`         | ✅       | [PRIVMSG Twitch API](https://dev.twitch.tv/docs/irc/tags/#privmsg-tags)                 | [PrivMsg Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/PrivMsg.cs)                 |
| `CLEARCHAT`       | ✅       | [CLEARCHAT Twitch API](https://dev.twitch.tv/docs/irc/tags/#clearchat-tags)             | [ClearChat Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/ClearChat.cs)             |
| `CLEARMSG`        | ✅       | [CLEARMSG Twitch API](https://dev.twitch.tv/docs/irc/tags/#clearmsg-tags)               | [ClearMSG Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/ClearMsg.cs)               |
| `GLOBALUSERSTATE` | ✅       | [GLOBALUSERSTATE Twitch API](https://dev.twitch.tv/docs/irc/tags/#globaluserstate-tags) | [GlobalUserState Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/GlobalUserState.cs) |
| `NOTICE`          | ✅       | [NOTICE Twitch API](https://dev.twitch.tv/docs/irc/tags/#notice-tags)                   | [NoticeMsg Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/NoticeMsg.cs)             |
| `ROOMSTATE`       | ✅       | [ROOMSTATE Twitch API](https://dev.twitch.tv/docs/irc/tags/#roomstate-tags)             | [RoomState Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/RoomStateMsg.cs)          |
| `USERNOTICE`      | ✅       | [USERNOTICE Twitch API](https://dev.twitch.tv/docs/irc/tags/#usernotice-tags)           | [UserNotice Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/UserNotice.cs)           |
| `USERSTATE`       | ✅       | [USERSTATE Twitch API](https://dev.twitch.tv/docs/irc/tags/#userstate-tags)             | [UserState Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/UserStateMsg.cs)          |
| `WHISPER`         | ✅       | [WHISPER Twitch API](https://dev.twitch.tv/docs/irc/tags/#whisper-tags)                 | [UserNotice Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/Messages/WhisperMsg.cs)           |



#### PRIVMSG Tags
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

#### CLEARCHAT Tags
| Field        | Support |
|--------------|---------|
| BanDuration  | ✅       |
| RoomId       | ✅       |
| TargetUserId | ✅       |
| Timestamp    | ✅       |

#### CLEARMSG Tags
| Field           | Support |
|-----------------|---------|
| Login           | ✅       |
| RoomId          | ✅       |
| TargetMessageId | ✅       |
| Timestamp       | ✅       |

#### GLOBALUSERSTATE Tags
| Field       | Support |
|-------------|---------|
| BadgeInfo   | ✅       |
| Badges      | ✅       |
| Color       | ✅       |
| DisplayName | ✅       |
| EmoteSets   | ✅       |
| Turbo       | ✅       |
| UserId      | ✅       |
| UserType    | ✅       |

#### NOTICE Tags
| Field        | Support |
|--------------|---------|
| MsgId        | ✅       |
| TargetUserId | ✅       |

#### ROOMSTATE Tags
| Field         | Support |
|---------------|---------|
| EmoteOnly     | ✅       |
| FollowersOnly | ✅       |
| R9K           | ✅       |
| RoomId        | ✅       |
| Slow          | ✅       |
| SubsOnly      | ✅       |

#### USERNOTICE Tags
| Field         | Support |
|--------------- |---------|
| BadgeInfo     | ✅      |
| Badges        | ✅      |
| Color         | ✅      |
| DisplayName   | ✅      |
| Emotes        | ✅      |
| Id            | ✅      |
| Login         | ✅      |
| Mod           | ✅      |
| MsgId         | ✅      |
| RoomId        | ✅      |
| Subscriber    | ✅      |
| SystemMsg     | ✅      |
| TmiSentTs     | ✅      |
| Turbo         | ✅      |
| UserId        | ✅      |
| UserType       | ✅      |

#### USERSTATE Tags
| Field       | Support |
|-------------|---------|
| BadgeInfo   | ✅      |
| Badges      | ✅      |
| Color       | ✅      |
| DisplayName | ✅      |
| EmoteSets   | ✅      |
| Id          | ✅      |
| Mod         | ✅      |
| Subscriber  | ✅      |
| Turbo       | ✅      |
| UserType    | ✅      |

#### WHISPER Tags
| Field       | Support |
|-------------|---------|
| Badges      | ✅       |
| Color       | ✅       |
| DisplayName | ✅       |
| Emotes      | ✅       |
| MessageId   | ✅       |
| ThreadId    | ✅       |
| Turbo       | ✅       |
| UserId      | ✅       |
| UserType    | ✅       |

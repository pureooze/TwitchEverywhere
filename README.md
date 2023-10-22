# TwitchEverywhere

<!-- TOC -->
* [How To Use It](#how-to-use-it)
* [Sample CLI App](#sample-cli-app)
* [Performance](#performance)
* [Supported Functionality](#supported-functionality)
  * [Membership](#membership)
    * [JOIN](#join)
    * [PART](#part)
  * [IRC Tags](#irc-tags)
    * [PRIVMSG](#privmsg)
    * [CLEARCHAT](#clearchat)
    * [CLEARMSG](#clearmsg)
    * [GlobalUserState](#globaluserstate)
    * [Notice](#notice)
    * [RoomState](#roomstate)
    * [UserNotice](#usernotice)
    * [UserState](#userstate)
    * [Whisper](#whisper)
<!-- TOC -->

`TwitchEverywhere` is a .NET (6 and 7) library that allows connecting to a Twitch chat and subscribing to messages in that chat.

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

// This example only handles MessageType.PrivMsg but you should handle other types here too
private void MessageCallback( Message message ) {
    switch( message.MessageType ) {
        case MessageType.PrivMsg: {
            PrivMsg privMsg = (PrivMsg) message;
            PrivMessageCallback( privMsg );
            break;
        }
        default:
            // This is just an example, you can handle this case however you wish
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

## Performance
The benchmarks in the [TwitchEverywhere.Benchmark](https://github.com/pureooze/TwitchEverywhere/tree/main/TwitchEverywhere.Benchmark) project use the [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet/tree/master) library. You can read more about the methodology that BenchmarkDotNet uses [here](https://github.com/dotnet/BenchmarkDotNet/tree/master#features).

We send 500 messages of each type to `TwitchEverywhere` and run it several times to determine an average. The results are below:

| Method                   | Iterations |      Mean |      Error |     StdDev |    Allocated |
|--------------------------|------------|----------:|-----------:|-----------:|-------------:|
| `UserNoticeMessage`      | `500`      | `6.291 s` | `0.0400 s` | `0.0375 s` | `1740.00 KB` |
| `PrivMsg`                | `500`      | `7.548 s` | `0.1517 s` | `0.4473 s` | `1693.96 KB` |
| `UserStateMsg`           | `500`      | `7.909 s` | `0.0120 s` | `0.0112 s` | `1160.00 KB` |
| `WhisperMessage`         | `500`      | `7.636 s` | `0.1504 s` | `0.3003 s` | `1080.00 KB` |
| `GlobalUserStateMessage` | `500`      | `7.370 s` | `0.1589 s` | `0.4686 s` |  `951.70 KB` |
| `RoomStateMessage`       | `500`      | `7.891 s` | `0.0147 s` | `0.0130 s` |  `774.07 KB` |
| `ReconnectMsg`           | `500`      | `7.726 s` | `0.1534 s` | `0.4041 s` |  `754.67 KB` |
| `ClearChat`              | `500`      | `6.242 s` | `0.0269 s` | `0.0225 s` |  `718.94 KB` |
| `ClearMsg`               | `500`      | `7.232 s` | `0.1001 s` | `0.0936 s` |  `687.69 KB` |
| `NoticeMsg`              | `500`      | `6.249 s` | `0.0268 s` | `0.0251 s` |  `628.95 KB` |
| `HostTargetMsg`          | `500`      | `7.885 s` | `0.0234 s` | `0.0207 s` |  `624.19 KB` |
| `PartMsg`                | `500`      | `7.996 s` | `0.1551 s` | `0.2548 s` |  `585.02 KB` |
| `JoinMsg`                | `500`      | `7.968 s` | `0.0348 s` | `0.0326 s` |  `557.64 KB` |

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



#### PRIVMSG
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

#### CLEARCHAT
| Field        | Support |
|--------------|---------|
| BanDuration  | ✅       |
| RoomId       | ✅       |
| TargetUserId | ✅       |
| Timestamp    | ✅       |

#### CLEARMSG
| Field           | Support |
|-----------------|---------|
| Login           | ✅       |
| RoomId          | ✅       |
| TargetMessageId | ✅       |
| Timestamp       | ✅       |

#### GlobalUserState
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

#### Notice
| Field        | Support |
|--------------|---------|
| MsgId        | ✅       |
| TargetUserId | ✅       |

#### RoomState
| Field         | Support |
|---------------|---------|
| EmoteOnly     | ✅       |
| FollowersOnly | ✅       |
| R9K           | ✅       |
| RoomId        | ✅       |
| Slow          | ✅       |
| SubsOnly      | ✅       |

#### UserNotice
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

#### UserState
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

#### Whisper
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

# TwitchEverywhere

<!-- TOC -->
* [How To Use It](#how-to-use-it)
* [Sample CLI App](#sample-cli-app)
* [Performance](#performance)
* [Supported Functionality](#supported-functionality)
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

| Method                 | Iterations |    Mean |    Error |   StdDev |  Allocated |
|------------------------|------------|--------:|---------:|---------:|-----------:|
| PrivMsg                | 500        | 7.548 s | 0.1517 s | 0.4473 s | 1693.96 KB |
| ClearMsg               | 500        | 7.232 s | 0.1001 s | 0.0936 s |  687.69 KB |
| ClearChat              | 500        | 6.242 s | 0.0269 s | 0.0225 s |  718.94 KB |
| NoticeMsg              | 500        | 6.249 s | 0.0268 s | 0.0251 s |  628.95 KB |
| GlobalUserStateMessage | 500        | 7.370 s | 0.1589 s | 0.4686 s |   951.7 KB |



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
[CLEARCHAT Twitch API](https://dev.twitch.tv/docs/irc/tags/#clearchat-tags)

[ClearChat Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/ClearChat.cs)

| Field        | Support |
|--------------|---------|
| BanDuration  | ✅       |
| RoomId       | ✅       |
| TargetUserId | ✅       |
| Timestamp    | ✅       |

### CLEARMSG
[CLEARMSG Twitch API](https://dev.twitch.tv/docs/irc/tags/#clearmsg-tags)

[ClearMSG Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/ClearMsg.cs)

| Field           | Support |
|-----------------|---------|
| Login           | ✅       |
| RoomId          | ✅       |
| TargetMessageId | ✅       |
| Timestamp       | ✅       |

### GlobalUserState
[GLOBALUSERSTATE Twitch API](https://dev.twitch.tv/docs/irc/tags/#globaluserstate-tags)

[GlobalUserState Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/GlobalUserState.cs)

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

### Notice
[NOTICE Twitch API](https://dev.twitch.tv/docs/irc/tags/#notice-tags)

[NoticeMsg Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/NoticeMsg.cs)

| Field        | Support |
|--------------|---------|
| MsgId        | ✅       |
| TargetUserId | ✅       |

### RoomState
[ROOMSTATE Twitch API](https://dev.twitch.tv/docs/irc/tags/#roomstate-tags)

[RoomState Type](https://github.com/pureooze/TwitchEverywhere/blob/main/TwitchEverywhere/Types/RoomStateMsg.cs)

| Field         | Support |
|---------------|---------|
| EmoteOnly     | ✅       |
| FollowersOnly | ✅       |
| R9K           | ✅       |
| RoomId        | ✅       |
| Slow          | ✅       |
| SubsOnly      | ✅       |

### UserNotice
WIP

### UserState
WIP

### Whisper
WIP

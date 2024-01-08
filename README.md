# TwitchEverywhere



`TwitchEverywhere` is a .NET library that allows connecting to a Twitch chat and subscribing to messages in that chat.

The goal of this library is to provide a lightweight, strongly typed API for clients so they can avoid parsing raw strings as much as possible.
Additionally, Twitch requires an authenticated connection which can get a bit complicated to setup and maintain. 
Fortunately `TwitchEverywhere` can do that for you! 😀

The library will support the latest LTS version of .NET and non LTS versions IF it is newer than the LTS version.
So for example before .NET 8, the library supported .NET 6 (LTS) and 7 but once .NET 8 was released support for 6 and 7 was dropped.

## How To Use It
Depending on what you need to do, you will have to use the appropriate library:

| Library Name            | Description                                   | Nuget Package                                                    | Twitch Reference                          |
|-------------------------|-----------------------------------------------|------------------------------------------------------------------|-------------------------------------------|
| `TwitchEverywhere.Irc`  | For connecting to a twitch chat using IRC     | https://www.nuget.org/packages/VodOnDemand.TwitchEverywhere.Irc  | https://dev.twitch.tv/docs/irc/           |
| `TwitchEverywhere.Rest` | For making REST calls to the Twitch Helix API | https://www.nuget.org/packages/VodOnDemand.TwitchEverywhere.Rest | https://dev.twitch.tv/docs/api/reference/ |


# TwitchEverywhere

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

`TwitchEverywhere` is a .NET library that allows connecting to a Twitch chat and subscribing to messages in that chat.

The goal of this library is to provide a lightweight, strongly typed API for clients so they can avoid parsing raw strings as much as possible.
Additionally, Twitch requires an authenticated connection which can get a bit complicated to setup and maintain. 
Fortunately `TwitchEverywhere` can do that for you! 😀

The library will support the latest LTS version of .NET and non LTS versions IF it is newer than the LTS version.
So for example before .NET 8, the library supported .NET 6 (LTS) and 7 but once .NET 8 was released support for 6 and 7 was dropped.

## How To Use It
Depending on what you need to do, you will have to use the appropriate library:

| Library Name            | Description                                   | Twitch Reference                          |
|-------------------------|-----------------------------------------------|-------------------------------------------|
| `TwitchEverywhere.Irc`  | For connecting to a twitch chat using IRC     | https://dev.twitch.tv/docs/irc/           |
| `TwitchEverywhere.Rest` | For making REST calls to the Twitch Helix API | https://dev.twitch.tv/docs/api/reference/ |


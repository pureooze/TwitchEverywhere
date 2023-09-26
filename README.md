# TwitchEverywhere

`TwitchEverywhere` is a .NET Core 7 library that allows connecting to a Twitch chat and subscribing to messages in that chat.

Twitch requires an authenticated connection to the IRC server which can get a bit complicated to setup and maintain. Fortunately `TwitchEverywhere` can do that for you! 😀

## How To Use The CLI
There is a sample CLI application that is included as an example in this repo and you can use it to connect with Twitch – give it a try!

In order to connect you need to create an `appsettings.json` file in the root of the `TwitchEverywhereCLI` project with the following parameters:

```json
{
  "AccessToken": "your_twitch_access_token",
  "RefreshToken": "your_twitch_refresh_token",
  "ClientId": "your_client_id",
  "ClientSecret": "your_client_secret",
  "Channel": "channel_you_want_to_connect_to"
}
```

# TwitchEverywhere.Irc
`TwitchEverywhere.Rest` is a .NET library that allows connecting to the Twitch Helix API and making REST calls to it.

The goal of this library is to provide a lightweight, strongly typed API for clients so they can avoid parsing raw strings as much as possible.

The library will support the latest LTS version of .NET and non LTS versions IF it is newer than the LTS version.
So for example before .NET 8, the library supported .NET 6 (LTS) and 7 but once .NET 8 was released support for 6 and 7 was dropped.

<!-- TOC -->
* [How To Use It](#how-to-use-it)
* [Sample CLI App](#sample-cli-app)
* [Performance](#performance)
* [Supported Functionality](#supported-functionality)
  * [Twitch Helix API](#twitch-helix-api)
    * [Users](#users)
    * [Videos](#videos)
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

Then initialize `RestClient` and pass in the options to the constructor.
Finally call the appropriate API:
```csharp
RestClient restClient = new( options );
GetUsersResponse users = await m_restClient.GetUsersByLogin(
    logins: [ "userLogin" ] 
);

if (users.StatusCode != HttpStatusCode.OK) {
    Console.WriteLine( "Error in GetUsers request with status code: " + users.StatusCode );
    return;
}
```

And that's it, you have successfully made a call to the Helix API! 🎉

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
Note that currently there are no benchmarks for `TwitchEverywhere.Rest` but they will be added in the future.

## Supported Functionality

### Twitch Helix API

#### Users
| Endpoint    | Supported | API Link                                              |
|-------------|-----------|-------------------------------------------------------|
| Get Users   | ✅         | https://dev.twitch.tv/docs/api/reference/#get-users   |
| Update User | ✅         | https://dev.twitch.tv/docs/api/reference/#update-user |

#### Videos
| Endpoint    | Supported | API Link                                              |
|-------------|-----------|-------------------------------------------------------|
| Get Videos  | ✅         | https://dev.twitch.tv/docs/api/reference/#get-videos  |
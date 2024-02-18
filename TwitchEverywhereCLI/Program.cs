// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using TwitchEverywhere.Core;
using TwitchEverywhere.Irc;
using TwitchConnection = TwitchEverywhereCLI.TwitchConnection;

Console.WriteLine( "Hello, World!" );

bool ircClient = false;

foreach (string arg in args) {
    if( arg == "-i" ) {
        ircClient = true;
    }
}

IConfigurationBuilder builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfigurationRoot config = builder.Build();
string? accessToken = config["AccessToken"];
string? refreshToken = config["RefreshToken"];
string? clientId = config["ClientId"];
string? clientSecret = config["ClientSecret"];
string? clientName = config["ClientName"];
string? channel = config["Channel"] ?? "";

TwitchConnectionOptions options = new(
    Channel: channel,
    AccessToken: accessToken,
    RefreshToken: refreshToken,
    ClientId: clientId,
    ClientSecret: clientSecret,
    ClientName: clientName
);

TwitchConnection twitchConnection = new( options );

if( ircClient ) {
    twitchConnection.ConnectToIrcClientRx();
} else {
    await twitchConnection.ConnectToRestClient();
}
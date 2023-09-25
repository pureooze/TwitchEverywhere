// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using TwitchEverywhere;

Console.WriteLine( "Hello, World!" );

IConfigurationBuilder builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfigurationRoot config = builder.Build();
string? accessToken = config["AccessToken"];
string? refreshToken = config["RefreshToken"];
string? clientId = config["ClientId"];
string? clientSecret = config["ClientSecret"];
int bufferSize = int.Parse( config["BufferSize"] ?? "50" );

TwitchConnectionOptions options = new(
    Channel: "jaaski",
    AccessToken: accessToken,
    RefreshToken: refreshToken,
    ClientId: clientId,
    ClientSecret: clientSecret,
    BufferSize: bufferSize
);

TwitchEverywhere.TwitchEverywhere twitchEverywhere = new( options );

await twitchEverywhere.TryConnectToChannel( message => {
    Console.WriteLine( message );
    
    
});
// See https://aka.ms/new-console-template for more information

using TwitchEverywhere;

Console.WriteLine( "Hello, World!" );

string[] channels = { "test" };
TwitchEverywhere.TwitchEverywhere twitchEverywhere = new( channels );
TwitchConnectionOptions options = new(
    Channel: "cohhcarnage",
    Tags: new[] { "hello" },
    Message: "hello, world!" 
);

await twitchEverywhere.TryConnectToChannel( 
    options: options
);
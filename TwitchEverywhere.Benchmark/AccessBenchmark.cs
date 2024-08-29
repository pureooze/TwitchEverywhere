using System.Text;
using BenchmarkDotNet.Attributes;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Interfaces;
using TwitchEverywhere.Irc;

namespace TwitchEverywhere.Benchmark;

public class AccessBenchmark {
    private readonly TwitchConnectionOptions m_options = new(
        "access_token",
        "refresh_token",
        "client_id",
        "client_secret",
        "client_name"
    );

    private IMessageProcessor processor = new MessageProcessor();
    private IPrivMsg privMsg;

    [GlobalSetup]
    public void AddData() {
        string message =
            "@badge-info=;badges=turbo/1;color=#0D4200;display-name=ronni;emotes=25:0-4,12-16/1902:6-10;id=b34ccfc7-4977-403a-8a94-33c6bac34fb8;mod=0;room-id=1337;subscriber=0;tmi-sent-ts=1507246572675;turbo=1;user-id=1337;user-type=global_mod :ronni!ronni@ronni.tmi.twitch.tv PRIVMSG #channel :Kappa Keepo Kappa";

        byte[] byteArray = Encoding.UTF8.GetBytes( message );
        processor.ProcessMessage( new RawMessage( byteArray ), "", null );
    }

    [Params( 3 )] public int Iterations;

    [Benchmark]
    public string Create() {
        string text = string.Empty;
        for( int i = 0; i < Iterations; i++ ) {
            text = privMsg.Id;
        }

        return text;
    }

    private void BenchmarkCallback(
        IMessage message
    ) {
        switch( message.MessageType ) {
            case MessageType.PrivMsg:
                privMsg = (IPrivMsg)message;
                break;
        }
    }
}
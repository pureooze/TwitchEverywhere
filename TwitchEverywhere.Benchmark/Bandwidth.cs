using System.Diagnostics;
using System.Text;
using BenchmarkDotNet.Attributes;
using TwitchEverywhere.Core;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Irc;
using TwitchEverywhere.Irc.Implementation;

namespace TwitchEverywhere.Benchmark;

[MemoryDiagnoser(false)]
public class Bandwidth {
    
    private readonly TwitchConnectionOptions m_options = new(
        "channel",
        "access_token",
        "refresh_token",
        "client_id",
        "client_secret",
        "client_name"
    );
    
    private byte[][] utf8Lines = null!;
    private string[] lines = null!;
    private IMessageProcessor processor = new MessageProcessor();

    [Params(1)]
    public int LineCount { get; set; }
    
    [GlobalSetup]
    public void AddData()
    {
        utf8Lines = File.ReadAllText("data.txt").Split('\0').Take(LineCount).Select(Encoding.UTF8.GetBytes).ToArray();
        
        Console.WriteLine(utf8Lines.Length);
    }
    
    [Benchmark]
    public void Create()
    {
        foreach (byte[] line in utf8Lines.AsSpan()) {
            processor.ProcessMessage( new RawMessage(line), "", _ => { } );
        }
    }
}
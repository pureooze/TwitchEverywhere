# Benchmarks
**Disclaimer:** I am not an expert in profiling and the document is an attempt by me to try and provide some data to give you an idea of how this library performs.
Its likely I will make a mistake so if you notice one please make an issue or a PR and we can get it fixed. Thanks for your understanding 🙂.

<!-- TOC -->
* [General](#general)
* [Memory Usage Profiling](#memory-usage-profiling)
<!-- TOC -->

## General
The benchmarks in the [TwitchEverywhere.Benchmark](https://github.com/pureooze/TwitchEverywhere/tree/main/TwitchEverywhere.Benchmark) project use the [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet/tree/master) library. You can read more about the methodology that BenchmarkDotNet uses [here](https://github.com/dotnet/BenchmarkDotNet/tree/master#features).

We send 500 messages of each type to `TwitchEverywhere` and run it several times to determine an average. The results are below:

| Method                   | Iterations |      Mean |      Error |     StdDev |    Allocated |
|--------------------------|------------|----------:|-----------:|-----------:|-------------:|
| `UserNoticeMessage`      | `500`      | `6.291 s` | `0.0400 s` | `0.0375 s` | `1740.00 KB` |
| `PrivMsg`                | `500`      | `7.548 s` | `0.1517 s` | `0.4473 s` | `1693.96 KB` |
| `UserStateMsg`           | `500`      | `7.909 s` | `0.0120 s` | `0.0112 s` | `1160.00 KB` |
| `WhisperMessage`         | `500`      | `7.636 s` | `0.1504 s` | `0.3003 s` | `1080.00 KB` |
| `GlobalUserStateMessage` | `500`      | `7.370 s` | `0.1589 s` | `0.4686 s` |  `951.70 KB` |
| `RoomStateMessage`       | `500`      | `7.891 s` | `0.0147 s` | `0.0130 s` |  `774.07 KB` |
| `ReconnectMsg`           | `500`      | `7.726 s` | `0.1534 s` | `0.4041 s` |  `754.67 KB` |
| `ClearChat`              | `500`      | `6.242 s` | `0.0269 s` | `0.0225 s` |  `718.94 KB` |
| `ClearMsg`               | `500`      | `7.232 s` | `0.1001 s` | `0.0936 s` |  `687.69 KB` |
| `NoticeMsg`              | `500`      | `6.249 s` | `0.0268 s` | `0.0251 s` |  `628.95 KB` |
| `HostTargetMsg`          | `500`      | `7.885 s` | `0.0234 s` | `0.0207 s` |  `624.19 KB` |
| `PartMsg`                | `500`      | `7.996 s` | `0.1551 s` | `0.2548 s` |  `585.02 KB` |
| `JoinMsg`                | `500`      | `7.968 s` | `0.0348 s` | `0.0326 s` |  `557.64 KB` |

## Memory Usage Profiling
Using `dotMemory Profiler` we can test how much memory `TwitchEverywhere` uses. 
The sample CLI app in the `TwitchEverywhereCLI` project is an example of a very minimal app that stores some messages in a memory buffer and occasionally writes to disk, so we can use it to for the benchmark.
The benchmark was also run against a Twitch chat with around 100,000 viewers so the chat was quite active. It was run for approximately 20 minutes and the results are here:

![](C:\Users\uzi\RiderProjects\TwitchEverywhere\TwitchEverywhere.Benchmark\images\MemoryUsage.PNG)

We can see that total memory usage does not exceed approximately 28MB but there is some Garbage Collection (GC) happening at 6m and 16m.
This could be happening for a few reasons, there might be something in `TwitchEverywhere` itself or it could be an issue with the CLI app.

Lets see what happens if we change the CLI app so it only logs messages to `STDOUT` and then stops referencing the objects.
The same test as above is run with the updated code (~100K viewers, 20 minutes):
![](C:\Users\uzi\RiderProjects\TwitchEverywhere\TwitchEverywhere.Benchmark\images\MemoryUsage-Optimize.PNG)

Some observations about this change we can make:
* `Gen0` garbage collection gets deferred by ~2 minutes
  * This causes `Gen1` and `Gen2` to be deferred as well (not surprising 😉)
* Fewer spikes in memory usage, probably because we dont store in a buffer or write to disk
  * If you look closely, there are some very small spikes but they are not negligible
* High traffic can cause issues with GC so clients should be careful with buffers/IO streams especially when trying to run at scale
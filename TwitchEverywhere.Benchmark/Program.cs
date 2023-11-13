// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using TwitchEverywhere.Benchmark;

BenchmarkRunner.Run<MsgBenchmark>();
BenchmarkRunner.Run<RegexBenchmark>();

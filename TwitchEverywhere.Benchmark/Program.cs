// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using TwitchEverywhere.Benchmark;

BenchmarkRunner.Run<RegexBenchmark>();
// BenchmarkRunner.Run<Bandwidth>();
// BenchmarkRunner.Run<AccessBenchmark>();

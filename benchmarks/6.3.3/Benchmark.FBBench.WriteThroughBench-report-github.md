``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.819)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET 7.0  
IterationCount=5  LaunchCount=7  WarmupCount=3  

```
|           Method |       Mean |   Error |   StdDev |     Median |        P25 |        P95 |   Gen0 | Allocated |
|----------------- |-----------:|--------:|---------:|-----------:|-----------:|-----------:|-------:|----------:|
|   RefStructSmall |   174.3 ns | 0.70 ns |  1.09 ns |   174.4 ns |   174.1 ns |   175.6 ns | 0.0086 |     144 B |
|   RefStructLarge | 1,141.0 ns | 6.62 ns | 10.30 ns | 1,147.1 ns | 1,129.8 ns | 1,153.1 ns | 0.0095 |     176 B |
| ValueStructSmall |   221.5 ns | 0.47 ns |  0.76 ns |   221.4 ns |   221.0 ns |   223.2 ns | 0.0062 |     104 B |
| ValueStructLarge | 1,220.0 ns | 6.21 ns |  9.67 ns | 1,213.9 ns | 1,211.0 ns | 1,232.1 ns | 0.0057 |     104 B |

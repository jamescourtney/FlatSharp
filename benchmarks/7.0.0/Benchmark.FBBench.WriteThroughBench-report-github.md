``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.819)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET 7.0  
IterationCount=5  LaunchCount=7  WarmupCount=3  

```
|           Method |       Mean |   Error |   StdDev |        P25 |        P95 |   Gen0 | Allocated |
|----------------- |-----------:|--------:|---------:|-----------:|-----------:|-------:|----------:|
|   RefStructSmall |   180.3 ns | 1.69 ns |  2.58 ns |   178.3 ns |   184.9 ns | 0.0086 |     144 B |
|   RefStructLarge | 1,156.9 ns | 6.27 ns |  9.76 ns | 1,145.5 ns | 1,167.2 ns | 0.0095 |     176 B |
| ValueStructSmall |   201.7 ns | 9.58 ns | 15.47 ns |   193.6 ns |   239.1 ns | 0.0062 |     104 B |
| ValueStructLarge | 1,227.7 ns | 7.77 ns | 12.32 ns | 1,217.2 ns | 1,246.2 ns | 0.0057 |     104 B |

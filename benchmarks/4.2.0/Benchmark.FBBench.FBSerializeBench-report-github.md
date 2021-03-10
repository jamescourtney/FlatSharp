``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.17763.1757 (1809/October2018Update/Redstone5)
AMD EPYC 7452, 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.103
  [Host]   : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT
  ShortRun : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET Core 5.0  
IterationCount=7  LaunchCount=7  WarmupCount=5  

```
|                                    Method | VectorLength |        Mean |     Error |    StdDev |      Median |        P25 |         P50 |         P67 |         P80 |         P90 |         P95 |
|------------------------------------------ |------------- |------------:|----------:|----------:|------------:|-----------:|------------:|------------:|------------:|------------:|------------:|
|                      FlatSharp_GetMaxSize |           30 |    394.9 ns |   4.08 ns |   8.15 ns |    389.9 ns |   388.2 ns |    389.9 ns |    399.3 ns |    405.0 ns |    406.5 ns |    407.6 ns |
|                       FlatSharp_Serialize |           30 |  3,156.7 ns | 101.26 ns | 197.50 ns |  3,074.0 ns | 2,976.9 ns |  3,074.0 ns |  3,322.2 ns |  3,405.7 ns |  3,436.1 ns |  3,440.7 ns |
|            FlatSharp_Serialize_NonVirtual |           30 |  3,067.8 ns | 146.78 ns | 293.14 ns |  3,045.8 ns | 3,014.4 ns |  3,045.8 ns |  3,064.4 ns |  3,147.9 ns |  3,626.9 ns |  3,634.7 ns |
|   FlatSharp_Serialize_StringVector_Sorted |           30 |  3,579.9 ns |  65.06 ns | 123.78 ns |  3,646.6 ns | 3,453.4 ns |  3,646.6 ns |  3,657.5 ns |  3,670.5 ns |  3,677.1 ns |  3,681.4 ns |
| FlatSharp_Serialize_StringVector_Unsorted |           30 |  1,707.8 ns |  68.17 ns | 136.13 ns |  1,690.4 ns | 1,615.7 ns |  1,690.4 ns |  1,703.7 ns |  1,712.7 ns |  2,015.6 ns |  2,019.9 ns |
|      FlatSharp_Serialize_IntVector_Sorted |           30 |  2,247.2 ns |  52.68 ns | 103.99 ns |  2,253.4 ns | 2,192.1 ns |  2,253.4 ns |  2,335.9 ns |  2,344.1 ns |  2,351.2 ns |  2,354.3 ns |
|    FlatSharp_Serialize_IntVector_Unsorted |           30 |    771.5 ns |   3.97 ns |   7.65 ns |    769.2 ns |   766.1 ns |    769.2 ns |    774.2 ns |    781.3 ns |    783.6 ns |    783.8 ns |
|      FlatSharp_Serialize_ValueTableVector |           30 | 10,147.7 ns |  80.02 ns | 152.25 ns | 10,134.7 ns | 9,996.2 ns | 10,134.7 ns | 10,198.1 ns | 10,236.9 ns | 10,440.8 ns | 10,453.7 ns |

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.17763.1757 (1809/October2018Update/Redstone5)
AMD EPYC 7452, 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.103
  [Host]   : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT
  ShortRun : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET Core 5.0  
IterationCount=7  LaunchCount=7  WarmupCount=5  

```
|                                    Method | VectorLength |       Mean |     Error |    StdDev |        P25 |        P50 |        P67 |        P80 |        P90 |        P95 |
|------------------------------------------ |------------- |-----------:|----------:|----------:|-----------:|-----------:|-----------:|-----------:|-----------:|-----------:|
|                      FlatSharp_GetMaxSize |           30 |   391.0 ns |   3.37 ns |   6.50 ns |   391.4 ns |   393.0 ns |   394.2 ns |   394.7 ns |   395.4 ns |   395.6 ns |
|                       FlatSharp_Serialize |           30 | 4,768.4 ns | 112.21 ns | 218.85 ns | 4,680.9 ns | 4,804.5 ns | 4,933.3 ns | 4,943.0 ns | 4,954.4 ns | 4,958.9 ns |
|            FlatSharp_Serialize_NonVirtual |           30 |         NA |        NA |        NA |         NA |         NA |         NA |         NA |         NA |         NA |
|   FlatSharp_Serialize_StringVector_Sorted |           30 | 4,005.0 ns | 109.02 ns | 212.64 ns | 3,815.6 ns | 3,941.2 ns | 4,050.4 ns | 4,303.3 ns | 4,319.9 ns | 4,326.4 ns |
| FlatSharp_Serialize_StringVector_Unsorted |           30 | 2,537.4 ns |  89.48 ns | 178.70 ns | 2,347.7 ns | 2,583.4 ns | 2,660.9 ns | 2,739.8 ns | 2,754.0 ns | 2,761.9 ns |
|      FlatSharp_Serialize_IntVector_Sorted |           30 | 2,794.9 ns |  53.88 ns | 107.60 ns | 2,708.4 ns | 2,804.3 ns | 2,833.5 ns | 2,887.6 ns | 2,956.4 ns | 2,967.3 ns |
|    FlatSharp_Serialize_IntVector_Unsorted |           30 | 1,357.2 ns |  62.11 ns | 121.14 ns | 1,289.5 ns | 1,316.1 ns | 1,340.6 ns | 1,371.8 ns | 1,436.7 ns | 1,669.3 ns |
|      FlatSharp_Serialize_ValueTableVector |           30 |         NA |        NA |        NA |         NA |         NA |         NA |         NA |         NA |         NA |

Benchmarks with issues:
  FBSerializeBench.FlatSharp_Serialize_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [VectorLength=30]
  FBSerializeBench.FlatSharp_Serialize_ValueTableVector: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [VectorLength=30]

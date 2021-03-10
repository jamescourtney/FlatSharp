``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.17763.1757 (1809/October2018Update/Redstone5)
AMD EPYC 7452, 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.103
  [Host]   : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT
  ShortRun : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET Core 5.0  
IterationCount=7  LaunchCount=7  WarmupCount=5  

```
|                                    Method | VectorLength |       Mean |    Error |    StdDev |     Median |        P25 |        P50 |        P67 |        P80 |        P90 |        P95 |
|------------------------------------------ |------------- |-----------:|---------:|----------:|-----------:|-----------:|-----------:|-----------:|-----------:|-----------:|-----------:|
|                      FlatSharp_GetMaxSize |           30 |   379.6 ns |  5.99 ns |  11.39 ns |   372.6 ns |   370.4 ns |   372.6 ns |   384.9 ns |   391.2 ns |   397.6 ns |   399.8 ns |
|                       FlatSharp_Serialize |           30 | 4,844.1 ns | 52.09 ns | 102.82 ns | 4,808.6 ns | 4,775.5 ns | 4,808.6 ns | 4,831.1 ns | 4,958.4 ns | 5,018.7 ns | 5,034.6 ns |
|            FlatSharp_Serialize_NonVirtual |           30 |         NA |       NA |        NA |         NA |         NA |         NA |         NA |         NA |         NA |         NA |
|   FlatSharp_Serialize_StringVector_Sorted |           30 | 3,915.6 ns | 30.69 ns |  60.57 ns | 3,934.1 ns | 3,897.1 ns | 3,934.1 ns | 3,949.7 ns | 3,961.0 ns | 3,970.8 ns | 3,973.5 ns |
| FlatSharp_Serialize_StringVector_Unsorted |           30 | 2,246.1 ns | 81.76 ns | 163.29 ns | 2,174.2 ns | 2,122.9 ns | 2,174.2 ns | 2,238.2 ns | 2,434.8 ns | 2,539.4 ns | 2,541.8 ns |
|      FlatSharp_Serialize_IntVector_Sorted |           30 | 2,581.3 ns | 43.80 ns |  87.47 ns | 2,556.4 ns | 2,525.8 ns | 2,556.4 ns | 2,567.4 ns | 2,703.9 ns | 2,713.5 ns | 2,717.3 ns |
|    FlatSharp_Serialize_IntVector_Unsorted |           30 | 1,205.6 ns |  3.57 ns |   6.88 ns | 1,205.2 ns | 1,201.0 ns | 1,205.2 ns | 1,207.9 ns | 1,211.7 ns | 1,214.0 ns | 1,214.6 ns |
|      FlatSharp_Serialize_ValueTableVector |           30 |         NA |       NA |        NA |         NA |         NA |         NA |         NA |         NA |         NA |         NA |

Benchmarks with issues:
  FBSerializeBench.FlatSharp_Serialize_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [VectorLength=30]
  FBSerializeBench.FlatSharp_Serialize_ValueTableVector: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [VectorLength=30]

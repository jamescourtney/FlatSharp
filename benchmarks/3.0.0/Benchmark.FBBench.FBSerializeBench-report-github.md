``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.17763.1757 (1809/October2018Update/Redstone5)
AMD EPYC 7452, 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.103
  [Host]   : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT
  ShortRun : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET Core 5.0  
IterationCount=7  LaunchCount=7  WarmupCount=5  

```
|                                    Method | VectorLength |       Mean |     Error |    StdDev |     Median |        P25 |        P50 |        P67 |        P80 |        P90 |        P95 |
|------------------------------------------ |------------- |-----------:|----------:|----------:|-----------:|-----------:|-----------:|-----------:|-----------:|-----------:|-----------:|
|                      FlatSharp_GetMaxSize |           30 |   392.4 ns |   6.00 ns |  11.56 ns |   392.6 ns |   384.2 ns |   392.6 ns |   402.0 ns |   403.1 ns |   404.3 ns |   404.7 ns |
|                       FlatSharp_Serialize |           30 | 4,877.3 ns |  86.15 ns | 172.05 ns | 4,852.7 ns | 4,774.3 ns | 4,852.7 ns | 4,952.6 ns | 5,045.9 ns | 5,118.8 ns | 5,130.8 ns |
|            FlatSharp_Serialize_NonVirtual |           30 |         NA |        NA |        NA |         NA |         NA |         NA |         NA |         NA |         NA |         NA |
|   FlatSharp_Serialize_StringVector_Sorted |           30 | 3,935.2 ns | 109.09 ns | 210.18 ns | 3,858.2 ns | 3,799.8 ns | 3,858.2 ns | 3,919.7 ns | 3,992.4 ns | 4,360.8 ns | 4,387.9 ns |
| FlatSharp_Serialize_StringVector_Unsorted |           30 | 2,328.7 ns | 138.42 ns | 266.69 ns | 2,190.7 ns | 2,176.8 ns | 2,190.7 ns | 2,236.3 ns | 2,405.2 ns | 2,921.2 ns | 2,928.8 ns |
|      FlatSharp_Serialize_IntVector_Sorted |           30 | 2,672.2 ns |  80.85 ns | 161.46 ns | 2,633.1 ns | 2,580.2 ns | 2,633.1 ns | 2,642.9 ns | 2,721.9 ns | 3,020.1 ns | 3,042.0 ns |
|    FlatSharp_Serialize_IntVector_Unsorted |           30 | 1,210.8 ns |   4.63 ns |   9.13 ns | 1,212.0 ns | 1,202.0 ns | 1,212.0 ns | 1,215.7 ns | 1,219.5 ns | 1,223.1 ns | 1,224.6 ns |
|      FlatSharp_Serialize_ValueTableVector |           30 |         NA |        NA |        NA |         NA |         NA |         NA |         NA |         NA |         NA |         NA |

Benchmarks with issues:
  FBSerializeBench.FlatSharp_Serialize_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [VectorLength=30]
  FBSerializeBench.FlatSharp_Serialize_ValueTableVector: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [VectorLength=30]

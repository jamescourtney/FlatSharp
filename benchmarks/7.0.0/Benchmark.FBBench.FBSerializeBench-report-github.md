``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.819)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET 7.0  
IterationCount=5  LaunchCount=7  WarmupCount=3  

```
|                                    Method | VectorLength |        Mean |     Error |      StdDev |      Median |         P25 |         P95 |   Gen0 | Allocated |
|------------------------------------------ |------------- |------------:|----------:|------------:|------------:|------------:|------------:|-------:|----------:|
|              Google_FlatBuffers_Serialize |           30 |  5,261.5 ns |  35.96 ns |    57.03 ns |  5,253.9 ns |  5,218.8 ns |  5,350.2 ns | 0.0076 |     144 B |
|    Google_FlatBuffers_Serialize_ObjectApi |           30 |  5,674.7 ns |  62.34 ns |    97.06 ns |  5,629.6 ns |  5,619.9 ns |  5,900.9 ns | 0.0076 |     144 B |
|    Google_Flatbuffers_StringVector_Sorted |           30 | 19,569.7 ns | 729.64 ns | 1,178.23 ns | 19,786.3 ns | 18,744.8 ns | 21,279.8 ns | 1.4343 |   24424 B |
|  Google_Flatbuffers_StringVector_Unsorted |           30 |  3,223.3 ns |  20.51 ns |    33.12 ns |  3,213.5 ns |  3,203.5 ns |  3,268.3 ns | 0.0076 |     144 B |
|       Google_Flatbuffers_IntVector_Sorted |           30 |  7,908.2 ns | 302.61 ns |   471.13 ns |  7,780.7 ns |  7,497.3 ns |  8,541.3 ns | 0.0076 |     232 B |
|     Google_Flatbuffers_IntVector_Unsorted |           30 |  1,909.3 ns |  24.99 ns |    38.90 ns |  1,934.7 ns |  1,866.6 ns |  1,958.0 ns | 0.0076 |     144 B |
|                            PBDN_Serialize |           30 |  8,212.4 ns | 108.93 ns |   166.34 ns |  8,179.4 ns |  8,087.0 ns |  8,576.9 ns |      - |      32 B |
|                 PBDN_Serialize_NonVirtual |           30 |  7,960.1 ns |  89.37 ns |   144.31 ns |  7,992.9 ns |  7,863.5 ns |  8,197.4 ns |      - |      32 B |
|              MsgPack_Serialize_NonVirtual |           30 |  2,506.4 ns |  52.81 ns |    80.64 ns |  2,480.0 ns |  2,436.2 ns |  2,661.8 ns | 0.1488 |    2528 B |
|                      FlatSharp_GetMaxSize |           30 |    138.0 ns |   1.05 ns |     1.60 ns |    138.1 ns |    137.1 ns |    141.8 ns |      - |         - |
|                       FlatSharp_Serialize |           30 |  1,212.6 ns |   5.00 ns |     7.79 ns |  1,211.4 ns |  1,207.6 ns |  1,228.2 ns |      - |         - |
|          FlatSharp_Serialize_ValueStructs |           30 |  1,229.4 ns |   3.88 ns |     6.15 ns |  1,232.1 ns |  1,224.8 ns |  1,236.1 ns |      - |         - |
|   FlatSharp_Serialize_StringVector_Sorted |           30 |  1,784.7 ns |  51.68 ns |    75.75 ns |  1,761.9 ns |  1,742.0 ns |  1,917.9 ns | 0.0038 |      88 B |
| FlatSharp_Serialize_StringVector_Unsorted |           30 |    819.7 ns |   7.70 ns |    11.52 ns |    818.6 ns |    810.4 ns |    838.6 ns |      - |         - |
|      FlatSharp_Serialize_IntVector_Sorted |           30 |    977.0 ns |   9.31 ns |    14.77 ns |    980.3 ns |    973.7 ns |    995.1 ns | 0.0038 |      88 B |
|    FlatSharp_Serialize_IntVector_Unsorted |           30 |    396.3 ns |   2.25 ns |     3.50 ns |    396.5 ns |    395.4 ns |    401.1 ns |      - |         - |
|      FlatSharp_Serialize_ValueTableVector |           30 |          NA |        NA |          NA |          NA |          NA |          NA |      - |         - |

Benchmarks with issues:
  FBSerializeBench.FlatSharp_Serialize_ValueTableVector: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET 7.0, IterationCount=5, LaunchCount=7, WarmupCount=3) [VectorLength=30]

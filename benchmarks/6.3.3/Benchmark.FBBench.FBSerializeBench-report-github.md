``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.819)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET 7.0  
IterationCount=5  LaunchCount=7  WarmupCount=3  

```
|                                    Method | VectorLength |       Mean |    Error |   StdDev |        P25 |        P95 |   Gen0 | Allocated |
|------------------------------------------ |------------- |-----------:|---------:|---------:|-----------:|-----------:|-------:|----------:|
|                      FlatSharp_GetMaxSize |           30 |   138.8 ns |  1.62 ns |  2.57 ns |   137.8 ns |   143.7 ns |      - |         - |
|                       FlatSharp_Serialize |           30 | 1,219.5 ns | 11.01 ns | 16.81 ns | 1,206.5 ns | 1,240.6 ns |      - |         - |
|          FlatSharp_Serialize_ValueStructs |           30 | 1,237.3 ns |  4.99 ns |  7.62 ns | 1,230.6 ns | 1,246.7 ns |      - |         - |
|   FlatSharp_Serialize_StringVector_Sorted |           30 | 1,760.1 ns | 33.91 ns | 55.71 ns | 1,732.9 ns | 1,860.9 ns | 0.0038 |      88 B |
| FlatSharp_Serialize_StringVector_Unsorted |           30 |   827.8 ns |  6.18 ns |  9.63 ns |   824.0 ns |   846.9 ns |      - |         - |
|      FlatSharp_Serialize_IntVector_Sorted |           30 |   977.2 ns | 16.61 ns | 26.35 ns |   949.8 ns | 1,010.2 ns | 0.0048 |      88 B |
|    FlatSharp_Serialize_IntVector_Unsorted |           30 |   395.3 ns |  1.88 ns |  3.04 ns |   393.9 ns |   400.7 ns |      - |         - |
|      FlatSharp_Serialize_ValueTableVector |           30 |   796.4 ns | 15.92 ns | 25.25 ns |   782.9 ns |   853.8 ns |      - |         - |

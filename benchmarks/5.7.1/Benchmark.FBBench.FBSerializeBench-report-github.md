``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1237 (20H2/October2020Update)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=5.0.201
  [Host]   : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT
  ShortRun : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET 5.0  
IterationCount=5  LaunchCount=7  WarmupCount=3  

```
|                                      Method | VectorLength |       Mean |     Error |    StdDev |     Median |        P25 |        P95 |  Gen 0 | Allocated |
|-------------------------------------------- |------------- |-----------:|----------:|----------:|-----------:|-----------:|-----------:|-------:|----------:|
|                        FlatSharp_GetMaxSize |           30 |   170.3 ns |   3.73 ns |   6.12 ns |   170.3 ns |   163.8 ns |   177.7 ns |      - |         - |
|                         FlatSharp_Serialize |           30 | 1,784.9 ns |  45.10 ns |  72.83 ns | 1,747.9 ns | 1,738.5 ns | 1,920.3 ns |      - |      24 B |
|              FlatSharp_Serialize_NonVirtual |           30 | 1,200.3 ns |  42.08 ns |  65.51 ns | 1,167.4 ns | 1,148.0 ns | 1,300.3 ns |      - |      24 B |
|            FlatSharp_Serialize_ValueStructs |           30 | 1,353.2 ns |  24.13 ns |  38.27 ns | 1,351.9 ns | 1,323.5 ns | 1,435.2 ns |      - |      24 B |
| FlatSharp_Serialize_ValueStructs_NonVirtual |           30 | 1,232.3 ns |  12.36 ns |  19.60 ns | 1,234.4 ns | 1,210.7 ns | 1,259.7 ns |      - |      24 B |
|     FlatSharp_Serialize_StringVector_Sorted |           30 | 1,802.7 ns |  18.43 ns |  30.29 ns | 1,811.7 ns | 1,790.4 ns | 1,836.1 ns | 0.0057 |     112 B |
|   FlatSharp_Serialize_StringVector_Unsorted |           30 |   888.3 ns |   3.34 ns |   5.10 ns |   890.9 ns |   882.4 ns |   894.3 ns | 0.0010 |      24 B |
|        FlatSharp_Serialize_IntVector_Sorted |           30 | 1,282.3 ns |  38.60 ns |  60.10 ns | 1,284.4 ns | 1,214.0 ns | 1,400.4 ns | 0.0057 |     112 B |
|      FlatSharp_Serialize_IntVector_Unsorted |           30 |   431.2 ns |  45.10 ns |  70.21 ns |   402.3 ns |   400.6 ns |   592.3 ns | 0.0014 |      24 B |
|        FlatSharp_Serialize_ValueTableVector |           30 | 6,474.6 ns | 151.47 ns | 240.25 ns | 6,379.0 ns | 6,310.1 ns | 6,973.7 ns |      - |      24 B |

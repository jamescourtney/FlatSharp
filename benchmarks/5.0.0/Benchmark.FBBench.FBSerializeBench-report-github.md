``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1237 (20H2/October2020Update)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=5.0.201
  [Host]   : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT
  ShortRun : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET 5.0  
IterationCount=5  LaunchCount=7  WarmupCount=3  

```
|                                    Method | VectorLength |       Mean |     Error |    StdDev |     Median |        P25 |        P95 |  Gen 0 | Allocated |
|------------------------------------------ |------------- |-----------:|----------:|----------:|-----------:|-----------:|-----------:|-------:|----------:|
|                      FlatSharp_GetMaxSize |           30 |   222.3 ns |   2.19 ns |   3.47 ns |   223.5 ns |   223.0 ns |   224.8 ns |      - |         - |
|                       FlatSharp_Serialize |           30 | 2,019.3 ns |  51.65 ns |  83.40 ns | 2,017.1 ns | 1,943.0 ns | 2,137.4 ns |      - |      24 B |
|            FlatSharp_Serialize_NonVirtual |           30 | 1,266.9 ns |  28.81 ns |  45.70 ns | 1,244.9 ns | 1,237.8 ns | 1,368.4 ns |      - |      24 B |
|   FlatSharp_Serialize_StringVector_Sorted |           30 | 1,805.3 ns |  23.95 ns |  37.28 ns | 1,803.1 ns | 1,771.8 ns | 1,857.3 ns | 0.0057 |     112 B |
| FlatSharp_Serialize_StringVector_Unsorted |           30 |   918.0 ns |   3.68 ns |   5.63 ns |   918.2 ns |   914.7 ns |   927.7 ns | 0.0010 |      24 B |
|      FlatSharp_Serialize_IntVector_Sorted |           30 | 1,299.5 ns |  26.31 ns |  42.48 ns | 1,309.1 ns | 1,273.6 ns | 1,375.7 ns | 0.0057 |     112 B |
|    FlatSharp_Serialize_IntVector_Unsorted |           30 |   442.9 ns |  26.45 ns |  41.95 ns |   427.1 ns |   420.2 ns |   540.2 ns | 0.0010 |      24 B |
|      FlatSharp_Serialize_ValueTableVector |           30 | 6,362.9 ns | 101.40 ns | 157.87 ns | 6,312.3 ns | 6,289.2 ns | 6,771.1 ns |      - |      24 B |

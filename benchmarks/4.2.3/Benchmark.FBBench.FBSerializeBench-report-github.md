``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1237 (20H2/October2020Update)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=5.0.201
  [Host]   : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT
  ShortRun : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET 5.0  
IterationCount=5  LaunchCount=7  WarmupCount=3  

```
|                                    Method | VectorLength |       Mean |    Error |    StdDev |        P25 |        P95 |  Gen 0 | Allocated |
|------------------------------------------ |------------- |-----------:|---------:|----------:|-----------:|-----------:|-------:|----------:|
|                      FlatSharp_GetMaxSize |           30 |   214.4 ns |  1.10 ns |   1.75 ns |   213.9 ns |   218.9 ns |      - |         - |
|                       FlatSharp_Serialize |           30 | 1,823.8 ns | 87.36 ns | 138.56 ns | 1,729.5 ns | 2,133.9 ns |      - |      24 B |
|            FlatSharp_Serialize_NonVirtual |           30 | 1,433.9 ns | 19.57 ns |  31.60 ns | 1,405.9 ns | 1,478.8 ns |      - |      24 B |
|   FlatSharp_Serialize_StringVector_Sorted |           30 | 1,829.3 ns | 20.97 ns |  32.66 ns | 1,796.6 ns | 1,882.2 ns | 0.0057 |     112 B |
| FlatSharp_Serialize_StringVector_Unsorted |           30 |   911.1 ns |  5.74 ns |   8.77 ns |   903.5 ns |   926.6 ns | 0.0010 |      24 B |
|      FlatSharp_Serialize_IntVector_Sorted |           30 | 1,255.5 ns | 16.96 ns |  26.90 ns | 1,237.0 ns | 1,287.3 ns | 0.0057 |     112 B |
|    FlatSharp_Serialize_IntVector_Unsorted |           30 |   428.4 ns |  1.54 ns |   2.45 ns |   427.2 ns |   435.3 ns | 0.0014 |      24 B |
|      FlatSharp_Serialize_ValueTableVector |           30 | 6,581.1 ns | 83.97 ns | 137.96 ns | 6,445.4 ns | 6,852.2 ns |      - |      24 B |

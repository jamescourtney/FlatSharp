``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1237 (20H2/October2020Update)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=5.0.201
  [Host]   : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT
  ShortRun : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET 5.0  
IterationCount=5  LaunchCount=7  WarmupCount=3  

```
|                                      Method | VectorLength |        Mean |     Error |    StdDev |      Median |         P25 |         P95 |  Gen 0 | Allocated |
|-------------------------------------------- |------------- |------------:|----------:|----------:|------------:|------------:|------------:|-------:|----------:|
|                Google_FlatBuffers_Serialize |           30 |  6,157.3 ns | 132.19 ns | 217.20 ns |  6,077.0 ns |  6,006.7 ns |  6,593.9 ns | 0.0076 |     144 B |
|      Google_FlatBuffers_Serialize_ObjectApi |           30 |  6,489.7 ns |  62.46 ns |  95.38 ns |  6,453.7 ns |  6,432.9 ns |  6,713.9 ns | 0.0076 |     144 B |
|      Google_Flatbuffers_StringVector_Sorted |           30 |  8,498.2 ns | 240.72 ns | 388.72 ns |  8,481.0 ns |  8,166.3 ns |  9,116.0 ns |      - |     232 B |
|    Google_Flatbuffers_StringVector_Unsorted |           30 |  3,416.4 ns |  33.09 ns |  52.48 ns |  3,399.5 ns |  3,393.3 ns |  3,533.1 ns | 0.0076 |     144 B |
|         Google_Flatbuffers_IntVector_Sorted |           30 |  5,774.3 ns | 260.86 ns | 421.24 ns |  5,937.6 ns |  5,303.1 ns |  6,306.4 ns | 0.0076 |     232 B |
|       Google_Flatbuffers_IntVector_Unsorted |           30 |  1,953.2 ns |  12.34 ns |  20.28 ns |  1,941.1 ns |  1,937.0 ns |  1,980.4 ns | 0.0076 |     144 B |
|                              PBDN_Serialize |           30 | 10,596.3 ns | 387.38 ns | 625.54 ns | 10,314.8 ns | 10,175.3 ns | 11,963.9 ns |      - |      32 B |
|                   PBDN_Serialize_NonVirtual |           30 |  8,517.8 ns |  68.02 ns | 105.90 ns |  8,502.3 ns |  8,458.6 ns |  8,664.2 ns |      - |      32 B |
|                MsgPack_Serialize_NonVirtual |           30 |  2,613.4 ns |  40.50 ns |  64.23 ns |  2,586.5 ns |  2,546.3 ns |  2,709.5 ns | 0.1488 |   2,528 B |
|                        FlatSharp_GetMaxSize |           30 |    166.2 ns |   5.78 ns |   9.33 ns |    158.7 ns |    158.4 ns |    178.0 ns |      - |         - |
|                         FlatSharp_Serialize |           30 |  1,799.3 ns | 108.73 ns | 175.58 ns |  1,704.4 ns |  1,680.3 ns |  2,143.3 ns |      - |         - |
|              FlatSharp_Serialize_NonVirtual |           30 |  1,127.7 ns |  25.08 ns |  39.78 ns |  1,114.7 ns |  1,100.2 ns |  1,212.3 ns |      - |         - |
|            FlatSharp_Serialize_ValueStructs |           30 |  1,430.4 ns |  49.15 ns |  75.06 ns |  1,435.8 ns |  1,362.4 ns |  1,531.9 ns |      - |         - |
| FlatSharp_Serialize_ValueStructs_NonVirtual |           30 |  1,190.4 ns |   9.56 ns |  15.44 ns |  1,193.3 ns |  1,175.3 ns |  1,218.1 ns |      - |         - |
|     FlatSharp_Serialize_StringVector_Sorted |           30 |  1,814.8 ns |  44.90 ns |  71.21 ns |  1,803.5 ns |  1,770.2 ns |  1,945.7 ns | 0.0038 |      88 B |
|   FlatSharp_Serialize_StringVector_Unsorted |           30 |    852.2 ns |   3.73 ns |   6.03 ns |    850.0 ns |    849.1 ns |    867.9 ns |      - |         - |
|        FlatSharp_Serialize_IntVector_Sorted |           30 |  1,319.5 ns |  52.86 ns |  83.84 ns |  1,282.5 ns |  1,263.0 ns |  1,473.2 ns | 0.0038 |      88 B |
|      FlatSharp_Serialize_IntVector_Unsorted |           30 |    408.6 ns |   2.05 ns |   3.26 ns |    408.4 ns |    405.6 ns |    412.9 ns |      - |         - |
|        FlatSharp_Serialize_ValueTableVector |           30 |  6,512.7 ns | 152.08 ns | 236.76 ns |  6,432.2 ns |  6,341.2 ns |  7,018.6 ns |      - |         - |

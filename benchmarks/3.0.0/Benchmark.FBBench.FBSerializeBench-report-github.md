``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.18363
AMD Ryzen 9 3900X 12-Core Processor (Max: 3.80GHz), 1 CPU, 24 logical and 12 physical cores
.NET Core SDK=3.1.200
  [Host]   : .NET Core 2.1.16 (CoreCLR 4.6.28516.03, CoreFX 4.6.28516.10), 64bit RyuJIT
  ShortRun : .NET Core 2.1.16 (CoreCLR 4.6.28516.03, CoreFX 4.6.28516.10), 64bit RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                                    Method | VectorLength |         Mean |        Error |      StdDev |
|------------------------------------------ |------------- |-------------:|-------------:|------------:|
|              **Google_FlatBuffers_Serialize** |            **3** |  **1,207.54 ns** |    **15.939 ns** |   **0.9006 ns** |
|    Google_FlatBuffers_Serialize_ObjectApi |            3 |  1,304.85 ns |    68.591 ns |   3.8755 ns |
|    Google_Flatbuffers_StringVector_Sorted |            3 |    835.69 ns |    20.266 ns |   1.1451 ns |
|  Google_Flatbuffers_StringVector_Unsorted |            3 |    618.97 ns |    13.900 ns |   0.7854 ns |
|       Google_Flatbuffers_IntVector_Sorted |            3 |    505.20 ns |    39.553 ns |   2.2348 ns |
|     Google_Flatbuffers_IntVector_Unsorted |            3 |    351.51 ns |    40.918 ns |   2.3120 ns |
|                      FlatSharp_GetMaxSize |            3 |     59.72 ns |     7.326 ns |   0.4139 ns |
|                       FlatSharp_Serialize |            3 |    593.95 ns |    14.366 ns |   0.8117 ns |
|                            PBDN_Serialize |            3 |  1,312.14 ns |    79.789 ns |   4.5082 ns |
|   FlatSharp_Serialize_StringVector_Sorted |            3 |    470.08 ns |    44.074 ns |   2.4903 ns |
| FlatSharp_Serialize_StringVector_Unsorted |            3 |    360.37 ns |    33.831 ns |   1.9115 ns |
|      FlatSharp_Serialize_IntVector_Sorted |            3 |    315.05 ns |    20.314 ns |   1.1478 ns |
|    FlatSharp_Serialize_IntVector_Unsorted |            3 |    207.48 ns |     6.115 ns |   0.3455 ns |
|              **Google_FlatBuffers_Serialize** |           **30** | **10,026.14 ns** | **1,264.833 ns** |  **71.4654 ns** |
|    Google_FlatBuffers_Serialize_ObjectApi |           30 | 10,244.86 ns |   318.526 ns |  17.9973 ns |
|    Google_Flatbuffers_StringVector_Sorted |           30 | 11,263.71 ns |    61.884 ns |   3.4966 ns |
|  Google_Flatbuffers_StringVector_Unsorted |           30 |  5,390.77 ns |   356.174 ns |  20.1245 ns |
|       Google_Flatbuffers_IntVector_Sorted |           30 |  7,957.70 ns |   642.139 ns |  36.2820 ns |
|     Google_Flatbuffers_IntVector_Unsorted |           30 |  2,443.95 ns |   303.820 ns |  17.1664 ns |
|                      FlatSharp_GetMaxSize |           30 |    257.17 ns |    23.423 ns |   1.3234 ns |
|                       FlatSharp_Serialize |           30 |  4,166.81 ns |   499.667 ns |  28.2321 ns |
|                            PBDN_Serialize |           30 |  8,734.09 ns | 2,936.292 ns | 165.9059 ns |
|   FlatSharp_Serialize_StringVector_Sorted |           30 |  3,348.19 ns |   219.791 ns |  12.4186 ns |
| FlatSharp_Serialize_StringVector_Unsorted |           30 |  2,291.05 ns |   144.783 ns |   8.1805 ns |
|      FlatSharp_Serialize_IntVector_Sorted |           30 |  2,141.52 ns |    40.313 ns |   2.2777 ns |
|    FlatSharp_Serialize_IntVector_Unsorted |           30 |  1,003.58 ns |   137.569 ns |   7.7729 ns |

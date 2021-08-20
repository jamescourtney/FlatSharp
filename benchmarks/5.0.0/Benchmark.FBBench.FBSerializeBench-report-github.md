``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.17763.1757 (1809/October2018Update/Redstone5)
AMD EPYC 7452, 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.103
  [Host]   : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT
  ShortRun : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET Core 5.0  
IterationCount=7  LaunchCount=7  WarmupCount=5  

```
|                                    Method | VectorLength |        Mean |     Error |    StdDev |      Median |         P25 |         P50 |         P67 |         P80 |         P90 |         P95 |
|------------------------------------------ |------------- |------------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|------------:|
|              Google_FlatBuffers_Serialize |           30 | 13,960.5 ns | 223.92 ns | 420.58 ns | 14,036.7 ns | 13,813.1 ns | 14,036.7 ns | 14,105.0 ns | 14,154.8 ns | 14,233.1 ns | 14,409.4 ns |
|    Google_FlatBuffers_Serialize_ObjectApi |           30 | 14,106.0 ns | 283.02 ns | 558.65 ns | 14,085.2 ns | 13,525.8 ns | 14,085.2 ns | 14,622.9 ns | 14,684.8 ns | 14,745.7 ns | 14,781.9 ns |
|    Google_Flatbuffers_StringVector_Sorted |           30 | 16,286.1 ns | 439.41 ns | 867.34 ns | 16,175.6 ns | 15,550.9 ns | 16,175.6 ns | 16,648.8 ns | 17,021.2 ns | 17,583.1 ns | 17,989.4 ns |
|  Google_Flatbuffers_StringVector_Unsorted |           30 |  6,936.1 ns |  94.86 ns | 187.24 ns |  6,894.4 ns |  6,812.1 ns |  6,894.4 ns |  6,958.9 ns |  7,140.2 ns |  7,208.9 ns |  7,276.4 ns |
|       Google_Flatbuffers_IntVector_Sorted |           30 |  9,631.8 ns | 368.26 ns | 718.26 ns |  9,513.8 ns |  9,128.7 ns |  9,513.8 ns |  9,643.4 ns |  9,924.1 ns | 10,288.0 ns | 10,569.0 ns |
|     Google_Flatbuffers_IntVector_Unsorted |           30 |  3,283.8 ns |  16.90 ns |  33.74 ns |  3,290.0 ns |  3,267.8 ns |  3,290.0 ns |  3,301.9 ns |  3,312.8 ns |  3,317.5 ns |  3,323.0 ns |
|                            PBDN_Serialize |           30 | 11,440.5 ns | 109.33 ns | 215.81 ns | 11,448.3 ns | 11,343.1 ns | 11,448.3 ns | 11,515.9 ns | 11,601.0 ns | 11,712.3 ns | 11,800.4 ns |
|                 PBDN_Serialize_NonVirtual |           30 | 10,550.0 ns | 108.13 ns | 208.34 ns | 10,580.5 ns | 10,367.3 ns | 10,580.5 ns | 10,661.6 ns | 10,708.8 ns | 10,812.1 ns | 10,855.3 ns |
|              MsgPack_Serialize_NonVirtual |           30 |  6,174.3 ns |  49.04 ns |  96.80 ns |  6,148.2 ns |  6,113.0 ns |  6,148.2 ns |  6,201.3 ns |  6,272.6 ns |  6,306.1 ns |  6,344.3 ns |
|                      FlatSharp_GetMaxSize |           30 |    404.4 ns |   4.76 ns |   9.27 ns |    409.9 ns |    392.6 ns |    409.9 ns |    411.3 ns |    412.4 ns |    413.5 ns |    413.9 ns |
|                       FlatSharp_Serialize |           30 |  2,906.5 ns |  49.12 ns |  98.09 ns |  2,870.6 ns |  2,834.9 ns |  2,870.6 ns |  2,895.0 ns |  3,029.4 ns |  3,065.5 ns |  3,071.6 ns |
|            FlatSharp_Serialize_NonVirtual |           30 |  2,492.5 ns |  80.75 ns | 153.64 ns |  2,578.3 ns |  2,366.5 ns |  2,578.3 ns |  2,583.9 ns |  2,590.9 ns |  2,599.5 ns |  2,603.1 ns |
|   FlatSharp_Serialize_StringVector_Sorted |           30 |  3,321.0 ns |  61.00 ns | 118.98 ns |  3,298.4 ns |  3,208.7 ns |  3,298.4 ns |  3,379.0 ns |  3,420.0 ns |  3,484.6 ns |  3,488.8 ns |
| FlatSharp_Serialize_StringVector_Unsorted |           30 |  1,635.3 ns |  19.02 ns |  37.55 ns |  1,630.4 ns |  1,601.8 ns |  1,630.4 ns |  1,666.3 ns |  1,671.8 ns |  1,677.0 ns |  1,687.0 ns |
|      FlatSharp_Serialize_IntVector_Sorted |           30 |  2,293.2 ns |  46.36 ns |  91.51 ns |  2,273.7 ns |  2,203.9 ns |  2,273.7 ns |  2,357.2 ns |  2,402.7 ns |  2,412.7 ns |  2,426.4 ns |
|    FlatSharp_Serialize_IntVector_Unsorted |           30 |    767.7 ns |  11.71 ns |  22.56 ns |    764.2 ns |    749.1 ns |    764.2 ns |    772.2 ns |    779.2 ns |    806.4 ns |    817.3 ns |
|      FlatSharp_Serialize_ValueTableVector |           30 |  9,805.5 ns |  41.34 ns |  80.62 ns |  9,786.1 ns |  9,758.2 ns |  9,786.1 ns |  9,797.8 ns |  9,819.1 ns |  9,985.7 ns | 10,000.6 ns |

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.14393.3930 (1607/AnniversaryUpdate/Redstone1), VM=Hyper-V
Intel Xeon CPU E5-2667 v3 3.20GHz, 1 CPU, 8 logical and 8 physical cores
  [Host]                  : .NET Framework 4.8 (4.8.4240.0), X64 RyuJIT
  MediumRun-.NET 4.7      : .NET Framework 4.8 (4.8.4240.0), X64 RyuJIT
  MediumRun-.NET Core 2.1 : .NET Core 2.1.22 (CoreCLR 4.6.29220.03, CoreFX 4.6.29220.01), X64 RyuJIT
  MediumRun-.NET Core 3.1 : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT
  MediumRun-.NET Core 5.0 : .NET Core 5.0.0 (CoreCLR 5.0.20.45114, CoreFX 5.0.20.45114), X64 RyuJIT

IterationCount=15  LaunchCount=2  WarmupCount=10  

```
|                                    Method |                     Job |       Runtime | VectorLength |         Mean |      Error |       StdDev |       Median |
|------------------------------------------ |------------------------ |-------------- |------------- |-------------:|-----------:|-------------:|-------------:|
|              **Google_FlatBuffers_Serialize** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |            **3** |  **1,733.15 ns** |  **12.198 ns** |    **18.258 ns** |  **1,734.27 ns** |
|    Google_FlatBuffers_Serialize_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,811.86 ns |  18.157 ns |    26.614 ns |  1,809.95 ns |
|    Google_Flatbuffers_StringVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,739.58 ns |  10.358 ns |    15.503 ns |  1,738.95 ns |
|  Google_Flatbuffers_StringVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |    970.08 ns |  10.001 ns |    14.969 ns |    968.14 ns |
|       Google_Flatbuffers_IntVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,017.45 ns |   6.265 ns |     9.184 ns |  1,015.06 ns |
|     Google_Flatbuffers_IntVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |    550.95 ns |   1.937 ns |     2.716 ns |    550.31 ns |
|                      FlatSharp_GetMaxSize |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |     74.67 ns |   0.773 ns |     1.134 ns |     74.84 ns |
|                       FlatSharp_Serialize |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,814.19 ns |  17.552 ns |    25.727 ns |  1,816.23 ns |
|                            PBDN_Serialize |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,833.93 ns |  13.120 ns |    19.638 ns |  1,831.40 ns |
|   FlatSharp_Serialize_StringVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,362.79 ns |   9.099 ns |    13.336 ns |  1,362.22 ns |
| FlatSharp_Serialize_StringVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,077.86 ns |   5.473 ns |     8.022 ns |  1,078.90 ns |
|      FlatSharp_Serialize_IntVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |    908.46 ns |   8.702 ns |    12.199 ns |    910.79 ns |
|    FlatSharp_Serialize_IntVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |    622.67 ns |   2.279 ns |     3.268 ns |    622.66 ns |
|              Google_FlatBuffers_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |  1,663.20 ns |   7.696 ns |    11.281 ns |  1,660.16 ns |
|    Google_FlatBuffers_Serialize_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |  1,746.02 ns |   8.289 ns |    12.150 ns |  1,743.76 ns |
|    Google_Flatbuffers_StringVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |  1,207.12 ns |   4.053 ns |     5.812 ns |  1,206.59 ns |
|  Google_Flatbuffers_StringVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    923.14 ns |   3.578 ns |     5.131 ns |    921.81 ns |
|       Google_Flatbuffers_IntVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    758.90 ns |   4.697 ns |     6.884 ns |    758.73 ns |
|     Google_Flatbuffers_IntVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    546.30 ns |   4.821 ns |     6.914 ns |    545.21 ns |
|                      FlatSharp_GetMaxSize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |     75.59 ns |   0.342 ns |     0.490 ns |     75.52 ns |
|                       FlatSharp_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    784.86 ns |   5.312 ns |     7.619 ns |    784.60 ns |
|                            PBDN_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |  1,740.37 ns |  19.042 ns |    28.501 ns |  1,747.22 ns |
|   FlatSharp_Serialize_StringVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    622.48 ns |   2.708 ns |     3.969 ns |    621.76 ns |
| FlatSharp_Serialize_StringVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    473.73 ns |   3.054 ns |     4.477 ns |    473.60 ns |
|      FlatSharp_Serialize_IntVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    450.33 ns |   4.613 ns |     6.905 ns |    451.63 ns |
|    FlatSharp_Serialize_IntVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    288.89 ns |   1.202 ns |     1.725 ns |    288.73 ns |
|              Google_FlatBuffers_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |  1,594.87 ns |  11.823 ns |    16.956 ns |  1,597.10 ns |
|    Google_FlatBuffers_Serialize_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |  1,627.80 ns |   7.077 ns |     9.920 ns |  1,627.24 ns |
|    Google_Flatbuffers_StringVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |  1,187.27 ns |   7.377 ns |    11.041 ns |  1,185.98 ns |
|  Google_Flatbuffers_StringVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    867.92 ns |   4.423 ns |     6.344 ns |    868.49 ns |
|       Google_Flatbuffers_IntVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    774.65 ns |   3.137 ns |     4.499 ns |    773.82 ns |
|     Google_Flatbuffers_IntVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    556.91 ns |   2.986 ns |     4.469 ns |    557.02 ns |
|                      FlatSharp_GetMaxSize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |     71.32 ns |   0.445 ns |     0.624 ns |     71.43 ns |
|                       FlatSharp_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    677.81 ns |   3.536 ns |     5.071 ns |    677.07 ns |
|                            PBDN_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |  1,641.30 ns |  14.896 ns |    21.835 ns |  1,641.15 ns |
|   FlatSharp_Serialize_StringVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    555.06 ns |   4.127 ns |     6.177 ns |    555.38 ns |
| FlatSharp_Serialize_StringVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    380.59 ns |   1.508 ns |     2.211 ns |    380.44 ns |
|      FlatSharp_Serialize_IntVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    423.09 ns |   2.621 ns |     3.758 ns |    423.38 ns |
|    FlatSharp_Serialize_IntVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    281.12 ns |   2.823 ns |     4.137 ns |    280.97 ns |
|              Google_FlatBuffers_Serialize | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |  1,591.26 ns |  16.801 ns |    25.147 ns |  1,591.23 ns |
|    Google_FlatBuffers_Serialize_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |  1,653.83 ns |   9.643 ns |    14.433 ns |  1,651.71 ns |
|    Google_Flatbuffers_StringVector_Sorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |  1,136.70 ns |   5.646 ns |     8.450 ns |  1,135.68 ns |
|  Google_Flatbuffers_StringVector_Unsorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |    851.68 ns |   4.494 ns |     6.726 ns |    850.77 ns |
|       Google_Flatbuffers_IntVector_Sorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |    743.52 ns |   3.276 ns |     4.903 ns |    742.31 ns |
|     Google_Flatbuffers_IntVector_Unsorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |    549.80 ns |   3.915 ns |     5.488 ns |    550.99 ns |
|                      FlatSharp_GetMaxSize | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |     72.35 ns |   0.412 ns |     0.604 ns |     72.23 ns |
|                       FlatSharp_Serialize | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |    659.77 ns |   3.624 ns |     5.312 ns |    660.98 ns |
|                            PBDN_Serialize | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |  1,557.97 ns |   8.901 ns |    13.323 ns |  1,560.05 ns |
|   FlatSharp_Serialize_StringVector_Sorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |    481.37 ns |   1.776 ns |     2.547 ns |    480.89 ns |
| FlatSharp_Serialize_StringVector_Unsorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |    357.27 ns |   2.738 ns |     4.013 ns |    356.24 ns |
|      FlatSharp_Serialize_IntVector_Sorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |    352.27 ns |   1.607 ns |     2.305 ns |    352.38 ns |
|    FlatSharp_Serialize_IntVector_Unsorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |    236.23 ns |   1.084 ns |     1.623 ns |    236.26 ns |
|              **Google_FlatBuffers_Serialize** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |           **30** | **14,754.95 ns** |  **80.512 ns** |   **120.507 ns** | **14,720.56 ns** |
|    Google_FlatBuffers_Serialize_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 | 15,599.65 ns | 137.699 ns |   193.035 ns | 15,553.03 ns |
|    Google_Flatbuffers_StringVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 | 29,892.44 ns | 892.654 ns | 1,336.082 ns | 29,945.08 ns |
|  Google_Flatbuffers_StringVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  8,533.85 ns |  48.459 ns |    71.030 ns |  8,514.96 ns |
|       Google_Flatbuffers_IntVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 | 18,386.60 ns | 686.908 ns |   962.949 ns | 19,163.99 ns |
|     Google_Flatbuffers_IntVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  3,873.57 ns |  17.801 ns |    24.367 ns |  3,871.23 ns |
|                      FlatSharp_GetMaxSize |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |    336.94 ns |   2.006 ns |     2.940 ns |    336.35 ns |
|                       FlatSharp_Serialize |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 | 13,500.56 ns | 117.849 ns |   176.391 ns | 13,469.63 ns |
|                            PBDN_Serialize |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 | 11,786.95 ns | 161.065 ns |   225.790 ns | 11,732.84 ns |
|   FlatSharp_Serialize_StringVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 | 11,267.87 ns | 106.926 ns |   160.042 ns | 11,265.67 ns |
| FlatSharp_Serialize_StringVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  7,957.33 ns |  58.735 ns |    87.911 ns |  7,960.32 ns |
|      FlatSharp_Serialize_IntVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  7,728.28 ns | 185.172 ns |   277.157 ns |  7,694.07 ns |
|    FlatSharp_Serialize_IntVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  3,400.07 ns |  23.790 ns |    34.872 ns |  3,402.79 ns |
|              Google_FlatBuffers_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 | 14,284.31 ns |  79.349 ns |   118.766 ns | 14,295.95 ns |
|    Google_FlatBuffers_Serialize_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 | 15,236.83 ns |  76.692 ns |   112.414 ns | 15,226.18 ns |
|    Google_Flatbuffers_StringVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 | 19,654.88 ns | 592.584 ns |   849.866 ns | 19,620.59 ns |
|  Google_Flatbuffers_StringVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  7,964.96 ns |  40.296 ns |    59.066 ns |  7,957.01 ns |
|       Google_Flatbuffers_IntVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 | 12,103.84 ns | 220.135 ns |   315.711 ns | 12,032.54 ns |
|     Google_Flatbuffers_IntVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  3,879.69 ns |  16.493 ns |    23.653 ns |  3,879.65 ns |
|                      FlatSharp_GetMaxSize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |    345.08 ns |   3.143 ns |     4.406 ns |    345.59 ns |
|                       FlatSharp_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  5,569.15 ns |  33.352 ns |    49.920 ns |  5,580.34 ns |
|                            PBDN_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 | 11,375.99 ns |  62.451 ns |    91.540 ns | 11,376.17 ns |
|   FlatSharp_Serialize_StringVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  5,186.51 ns | 289.928 ns |   433.951 ns |  5,206.45 ns |
| FlatSharp_Serialize_StringVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  2,953.84 ns |  29.727 ns |    44.494 ns |  2,952.04 ns |
|      FlatSharp_Serialize_IntVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  3,083.46 ns |  57.417 ns |    85.939 ns |  3,065.57 ns |
|    FlatSharp_Serialize_IntVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  1,400.49 ns |  27.900 ns |    40.896 ns |  1,427.10 ns |
|              Google_FlatBuffers_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 | 13,599.57 ns |  61.151 ns |    89.635 ns | 13,578.39 ns |
|    Google_FlatBuffers_Serialize_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 | 14,542.44 ns | 100.792 ns |   150.861 ns | 14,564.26 ns |
|    Google_Flatbuffers_StringVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 | 19,455.90 ns | 141.631 ns |   211.986 ns | 19,450.93 ns |
|  Google_Flatbuffers_StringVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  7,629.89 ns |  98.180 ns |   146.951 ns |  7,635.53 ns |
|       Google_Flatbuffers_IntVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 | 11,339.71 ns | 181.497 ns |   271.656 ns | 11,394.15 ns |
|     Google_Flatbuffers_IntVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  3,869.88 ns |  28.220 ns |    41.365 ns |  3,870.77 ns |
|                      FlatSharp_GetMaxSize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |    363.33 ns |   5.270 ns |     7.725 ns |    359.19 ns |
|                       FlatSharp_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  4,965.94 ns |  61.164 ns |    89.653 ns |  5,000.76 ns |
|                            PBDN_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 | 11,141.84 ns |  59.222 ns |    84.935 ns | 11,135.11 ns |
|   FlatSharp_Serialize_StringVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  4,449.87 ns |  33.401 ns |    49.992 ns |  4,440.97 ns |
| FlatSharp_Serialize_StringVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  2,463.21 ns |  11.420 ns |    16.378 ns |  2,460.03 ns |
|      FlatSharp_Serialize_IntVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  3,078.05 ns |  64.511 ns |    92.519 ns |  3,035.00 ns |
|    FlatSharp_Serialize_IntVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  1,405.82 ns |  10.561 ns |    15.480 ns |  1,400.50 ns |
|              Google_FlatBuffers_Serialize | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 | 13,834.89 ns | 141.858 ns |   212.327 ns | 13,864.97 ns |
|    Google_FlatBuffers_Serialize_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 | 14,381.88 ns |  70.400 ns |   103.191 ns | 14,341.81 ns |
|    Google_Flatbuffers_StringVector_Sorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 | 18,277.48 ns | 277.629 ns |   406.946 ns | 18,505.38 ns |
|  Google_Flatbuffers_StringVector_Unsorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 |  7,112.69 ns |  49.370 ns |    72.365 ns |  7,127.36 ns |
|       Google_Flatbuffers_IntVector_Sorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 | 12,088.30 ns | 723.580 ns | 1,014.358 ns | 11,210.64 ns |
|     Google_Flatbuffers_IntVector_Unsorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 |  3,890.22 ns |  16.731 ns |    24.524 ns |  3,886.16 ns |
|                      FlatSharp_GetMaxSize | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 |    380.83 ns |   2.712 ns |     4.059 ns |    380.19 ns |
|                       FlatSharp_Serialize | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 |  4,963.64 ns |  33.785 ns |    48.454 ns |  4,963.53 ns |
|                            PBDN_Serialize | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 | 10,834.18 ns |  57.690 ns |    84.561 ns | 10,844.36 ns |
|   FlatSharp_Serialize_StringVector_Sorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 |  4,084.40 ns |  47.393 ns |    67.969 ns |  4,121.60 ns |
| FlatSharp_Serialize_StringVector_Unsorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 |  2,182.81 ns |  11.201 ns |    16.064 ns |  2,180.13 ns |
|      FlatSharp_Serialize_IntVector_Sorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 |  2,942.13 ns | 112.171 ns |   167.893 ns |  2,939.92 ns |
|    FlatSharp_Serialize_IntVector_Unsorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 |  1,236.47 ns |   6.366 ns |     9.529 ns |  1,235.84 ns |

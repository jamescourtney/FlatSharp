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
|                                    Method |                     Job |       Runtime | VectorLength |         Mean |      Error |     StdDev |       Median |
|------------------------------------------ |------------------------ |-------------- |------------- |-------------:|-----------:|-----------:|-------------:|
|              **Google_FlatBuffers_Serialize** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |            **3** |  **1,751.77 ns** |  **17.998 ns** |  **26.938 ns** |  **1,746.74 ns** |
|    Google_FlatBuffers_Serialize_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,809.75 ns |   8.182 ns |  12.246 ns |  1,809.46 ns |
|    Google_Flatbuffers_StringVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,727.42 ns |  10.418 ns |  15.270 ns |  1,724.84 ns |
|  Google_Flatbuffers_StringVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |    980.30 ns |   7.571 ns |  11.331 ns |    980.73 ns |
|       Google_Flatbuffers_IntVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |    998.72 ns |   9.656 ns |  13.848 ns |    996.75 ns |
|     Google_Flatbuffers_IntVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |    533.37 ns |   2.353 ns |   3.375 ns |    532.16 ns |
|                      FlatSharp_GetMaxSize |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |     74.01 ns |   0.500 ns |   0.718 ns |     73.94 ns |
|                       FlatSharp_Serialize |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,843.11 ns |   7.561 ns |  10.844 ns |  1,839.33 ns |
|                            PBDN_Serialize |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,774.54 ns |  11.229 ns |  16.459 ns |  1,771.33 ns |
|   FlatSharp_Serialize_StringVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,450.59 ns |   7.219 ns |  10.581 ns |  1,450.41 ns |
| FlatSharp_Serialize_StringVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,158.38 ns |   8.752 ns |  12.829 ns |  1,160.38 ns |
|      FlatSharp_Serialize_IntVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |    969.78 ns |   7.843 ns |  11.739 ns |    972.02 ns |
|    FlatSharp_Serialize_IntVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |    681.24 ns |   4.783 ns |   7.011 ns |    680.51 ns |
|              Google_FlatBuffers_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |  1,665.92 ns |   4.939 ns |   6.923 ns |  1,664.50 ns |
|    Google_FlatBuffers_Serialize_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |  1,761.28 ns |   6.179 ns |   8.862 ns |  1,760.87 ns |
|    Google_Flatbuffers_StringVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |  1,234.64 ns |   5.916 ns |   8.855 ns |  1,233.19 ns |
|  Google_Flatbuffers_StringVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    942.88 ns |   5.076 ns |   7.280 ns |    941.60 ns |
|       Google_Flatbuffers_IntVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    764.17 ns |   3.837 ns |   5.743 ns |    763.10 ns |
|     Google_Flatbuffers_IntVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    548.29 ns |   3.512 ns |   5.037 ns |    548.33 ns |
|                      FlatSharp_GetMaxSize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |     74.84 ns |   0.528 ns |   0.773 ns |     74.71 ns |
|                       FlatSharp_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    838.24 ns |  11.903 ns |  17.071 ns |    836.46 ns |
|                            PBDN_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |  1,730.18 ns |  12.849 ns |  19.232 ns |  1,724.15 ns |
|   FlatSharp_Serialize_StringVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    711.46 ns |   8.997 ns |  13.187 ns |    711.15 ns |
| FlatSharp_Serialize_StringVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    533.17 ns |   7.729 ns |  11.329 ns |    529.83 ns |
|      FlatSharp_Serialize_IntVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    496.69 ns |   3.592 ns |   5.152 ns |    495.76 ns |
|    FlatSharp_Serialize_IntVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    354.91 ns |   5.765 ns |   8.268 ns |    355.21 ns |
|              Google_FlatBuffers_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |  1,600.00 ns |  14.527 ns |  21.293 ns |  1,599.66 ns |
|    Google_FlatBuffers_Serialize_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |  1,646.07 ns |   8.846 ns |  13.241 ns |  1,648.12 ns |
|    Google_Flatbuffers_StringVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |  1,166.91 ns |   5.233 ns |   7.833 ns |  1,165.82 ns |
|  Google_Flatbuffers_StringVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    867.89 ns |   3.089 ns |   4.430 ns |    867.17 ns |
|       Google_Flatbuffers_IntVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    763.50 ns |   9.903 ns |  14.515 ns |    767.47 ns |
|     Google_Flatbuffers_IntVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    557.20 ns |   5.738 ns |   8.589 ns |    557.16 ns |
|                      FlatSharp_GetMaxSize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |     71.91 ns |   1.419 ns |   2.124 ns |     71.71 ns |
|                       FlatSharp_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    680.69 ns |   6.253 ns |   9.165 ns |    680.67 ns |
|                            PBDN_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |  1,631.04 ns |   9.066 ns |  13.288 ns |  1,630.04 ns |
|   FlatSharp_Serialize_StringVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    578.45 ns |   3.217 ns |   4.815 ns |    579.47 ns |
| FlatSharp_Serialize_StringVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    417.84 ns |   2.564 ns |   3.838 ns |    418.62 ns |
|      FlatSharp_Serialize_IntVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    440.65 ns |   3.938 ns |   5.647 ns |    439.87 ns |
|    FlatSharp_Serialize_IntVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    288.66 ns |   3.836 ns |   5.501 ns |    291.17 ns |
|              Google_FlatBuffers_Serialize | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |  1,612.75 ns |  11.534 ns |  16.907 ns |  1,615.54 ns |
|    Google_FlatBuffers_Serialize_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |  1,647.42 ns |   6.762 ns |  10.122 ns |  1,648.02 ns |
|    Google_Flatbuffers_StringVector_Sorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |  1,130.31 ns |   7.287 ns |  10.451 ns |  1,133.24 ns |
|  Google_Flatbuffers_StringVector_Unsorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |    844.70 ns |   2.339 ns |   3.355 ns |    844.32 ns |
|       Google_Flatbuffers_IntVector_Sorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |    772.09 ns |   8.125 ns |  11.910 ns |    769.30 ns |
|     Google_Flatbuffers_IntVector_Unsorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |    554.44 ns |   8.245 ns |  12.086 ns |    560.36 ns |
|                      FlatSharp_GetMaxSize | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |     82.14 ns |   8.601 ns |  12.058 ns |     92.97 ns |
|                       FlatSharp_Serialize | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |    652.70 ns |   3.430 ns |   4.808 ns |    651.63 ns |
|                            PBDN_Serialize | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |  1,554.25 ns |  12.552 ns |  18.002 ns |  1,549.49 ns |
|   FlatSharp_Serialize_StringVector_Sorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |    506.99 ns |   5.928 ns |   8.502 ns |    507.65 ns |
| FlatSharp_Serialize_StringVector_Unsorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |    380.88 ns |   2.658 ns |   3.979 ns |    381.04 ns |
|      FlatSharp_Serialize_IntVector_Sorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |    385.77 ns |   2.149 ns |   3.151 ns |    385.32 ns |
|    FlatSharp_Serialize_IntVector_Unsorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |            3 |    271.66 ns |   1.079 ns |   1.547 ns |    271.22 ns |
|              **Google_FlatBuffers_Serialize** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |           **30** | **15,117.19 ns** |  **74.997 ns** | **109.929 ns** | **15,092.88 ns** |
|    Google_FlatBuffers_Serialize_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 | 15,811.55 ns |  87.621 ns | 125.663 ns | 15,814.51 ns |
|    Google_Flatbuffers_StringVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 | 28,992.99 ns | 257.547 ns | 385.484 ns | 28,927.89 ns |
|  Google_Flatbuffers_StringVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  8,301.21 ns |  50.248 ns |  73.653 ns |  8,296.14 ns |
|       Google_Flatbuffers_IntVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 | 16,673.50 ns | 401.284 ns | 575.509 ns | 17,028.07 ns |
|     Google_Flatbuffers_IntVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  4,007.69 ns |  13.499 ns |  20.204 ns |  4,002.09 ns |
|                      FlatSharp_GetMaxSize |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |    350.04 ns |   9.637 ns |  14.424 ns |    348.86 ns |
|                       FlatSharp_Serialize |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 | 13,679.80 ns |  75.960 ns | 113.694 ns | 13,664.54 ns |
|                            PBDN_Serialize |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 | 11,579.37 ns |  84.229 ns | 123.461 ns | 11,578.18 ns |
|   FlatSharp_Serialize_StringVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 | 12,207.74 ns |  68.315 ns | 100.135 ns | 12,168.98 ns |
| FlatSharp_Serialize_StringVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  8,884.07 ns |  57.563 ns |  86.158 ns |  8,875.66 ns |
|      FlatSharp_Serialize_IntVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  8,411.64 ns | 170.391 ns | 255.033 ns |  8,420.46 ns |
|    FlatSharp_Serialize_IntVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  4,145.51 ns |  13.388 ns |  19.200 ns |  4,145.23 ns |
|              Google_FlatBuffers_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 | 14,360.91 ns |  74.199 ns | 111.057 ns | 14,342.37 ns |
|    Google_FlatBuffers_Serialize_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 | 14,820.31 ns |  57.450 ns |  85.988 ns | 14,830.68 ns |
|    Google_Flatbuffers_StringVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 | 20,303.46 ns |  70.319 ns | 103.072 ns | 20,320.14 ns |
|  Google_Flatbuffers_StringVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  8,056.26 ns |  47.189 ns |  69.170 ns |  8,037.35 ns |
|       Google_Flatbuffers_IntVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 | 13,287.40 ns |  70.665 ns | 103.580 ns | 13,266.12 ns |
|     Google_Flatbuffers_IntVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  3,909.06 ns |  22.430 ns |  32.877 ns |  3,898.89 ns |
|                      FlatSharp_GetMaxSize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |    341.43 ns |   1.470 ns |   2.155 ns |    341.93 ns |
|                       FlatSharp_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  5,845.61 ns |  68.630 ns |  96.209 ns |  5,870.01 ns |
|                            PBDN_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 | 11,398.20 ns |  56.994 ns |  81.739 ns | 11,382.22 ns |
|   FlatSharp_Serialize_StringVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  5,778.35 ns |  85.381 ns | 125.151 ns |  5,843.03 ns |
| FlatSharp_Serialize_StringVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  3,903.84 ns | 112.764 ns | 165.288 ns |  4,004.21 ns |
|      FlatSharp_Serialize_IntVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  3,582.27 ns |  24.266 ns |  35.569 ns |  3,580.41 ns |
|    FlatSharp_Serialize_IntVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  1,897.02 ns |   7.713 ns |  11.062 ns |  1,897.41 ns |
|              Google_FlatBuffers_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 | 13,490.13 ns |  51.737 ns |  77.437 ns | 13,475.24 ns |
|    Google_FlatBuffers_Serialize_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 | 14,242.40 ns |  49.730 ns |  71.321 ns | 14,241.35 ns |
|    Google_Flatbuffers_StringVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 | 18,945.15 ns | 504.831 ns | 755.607 ns | 19,006.08 ns |
|  Google_Flatbuffers_StringVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  7,580.54 ns |  48.980 ns |  71.794 ns |  7,588.77 ns |
|       Google_Flatbuffers_IntVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 | 11,184.22 ns | 119.510 ns | 178.876 ns | 11,204.27 ns |
|     Google_Flatbuffers_IntVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  3,998.73 ns |  20.766 ns |  29.782 ns |  4,008.81 ns |
|                      FlatSharp_GetMaxSize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |    371.54 ns |   1.617 ns |   2.370 ns |    371.77 ns |
|                       FlatSharp_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  4,918.79 ns |  41.440 ns |  62.026 ns |  4,927.21 ns |
|                            PBDN_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 | 11,345.56 ns |  52.101 ns |  76.369 ns | 11,344.46 ns |
|   FlatSharp_Serialize_StringVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  4,785.73 ns |  20.569 ns |  29.499 ns |  4,790.89 ns |
| FlatSharp_Serialize_StringVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  2,782.01 ns |   8.321 ns |  11.934 ns |  2,781.94 ns |
|      FlatSharp_Serialize_IntVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  3,311.96 ns |  16.361 ns |  23.982 ns |  3,315.47 ns |
|    FlatSharp_Serialize_IntVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  1,456.46 ns |   6.242 ns |   9.343 ns |  1,455.09 ns |
|              Google_FlatBuffers_Serialize | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 | 13,854.05 ns | 163.311 ns | 234.216 ns | 13,845.17 ns |
|    Google_FlatBuffers_Serialize_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 | 14,748.99 ns |  53.995 ns |  79.146 ns | 14,749.08 ns |
|    Google_Flatbuffers_StringVector_Sorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 | 18,564.23 ns | 126.452 ns | 177.268 ns | 18,640.26 ns |
|  Google_Flatbuffers_StringVector_Unsorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 |  7,356.53 ns |  25.010 ns |  35.869 ns |  7,351.62 ns |
|       Google_Flatbuffers_IntVector_Sorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 | 12,013.81 ns | 234.292 ns | 336.014 ns | 12,025.69 ns |
|     Google_Flatbuffers_IntVector_Unsorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 |  3,782.96 ns |  13.382 ns |  18.759 ns |  3,783.31 ns |
|                      FlatSharp_GetMaxSize | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 |    376.18 ns |   1.781 ns |   2.611 ns |    376.83 ns |
|                       FlatSharp_Serialize | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 |  4,707.09 ns |  29.797 ns |  43.676 ns |  4,721.84 ns |
|                            PBDN_Serialize | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 | 11,039.77 ns | 148.219 ns | 217.257 ns | 11,049.71 ns |
|   FlatSharp_Serialize_StringVector_Sorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 |  4,498.46 ns | 113.241 ns | 165.988 ns |  4,582.65 ns |
| FlatSharp_Serialize_StringVector_Unsorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 |  2,529.10 ns |  15.627 ns |  23.389 ns |  2,523.18 ns |
|      FlatSharp_Serialize_IntVector_Sorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 |  3,033.60 ns |  13.635 ns |  19.985 ns |  3,030.64 ns |
|    FlatSharp_Serialize_IntVector_Unsorted | MediumRun-.NET Core 5.0 | .NET Core 5.0 |           30 |  1,382.79 ns |  21.024 ns |  30.816 ns |  1,368.37 ns |

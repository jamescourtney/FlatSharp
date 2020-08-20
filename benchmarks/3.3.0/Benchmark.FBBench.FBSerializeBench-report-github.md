``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.450 (2004/?/20H1)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET Core SDK=3.1.401
  [Host]                  : .NET Core 2.1.21 (CoreCLR 4.6.29130.01, CoreFX 4.6.29130.02), X64 RyuJIT
  MediumRun-.NET 4.7      : .NET Framework 4.8 (4.8.4200.0), X64 RyuJIT
  MediumRun-.NET Core 2.1 : .NET Core 2.1.21 (CoreCLR 4.6.29130.01, CoreFX 4.6.29130.02), X64 RyuJIT
  MediumRun-.NET Core 3.1 : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

IterationCount=15  LaunchCount=2  WarmupCount=10  

```
|                                    Method |                     Job |       Runtime | VectorLength |         Mean |      Error |     StdDev |       Median |
|------------------------------------------ |------------------------ |-------------- |------------- |-------------:|-----------:|-----------:|-------------:|
|              **Google_FlatBuffers_Serialize** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |            **3** |  **1,207.91 ns** |   **4.261 ns** |   **6.111 ns** |  **1,206.02 ns** |
|    Google_FlatBuffers_Serialize_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,263.84 ns |   9.107 ns |  13.061 ns |  1,269.78 ns |
|    Google_Flatbuffers_StringVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |    906.58 ns |  10.411 ns |  14.931 ns |    918.32 ns |
|  Google_Flatbuffers_StringVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |    656.01 ns |   3.258 ns |   4.776 ns |    656.11 ns |
|       Google_Flatbuffers_IntVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |    560.54 ns |   3.069 ns |   4.401 ns |    560.03 ns |
|     Google_Flatbuffers_IntVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |    370.04 ns |   3.146 ns |   4.410 ns |    371.52 ns |
|                      FlatSharp_GetMaxSize |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |     60.02 ns |   0.781 ns |   1.121 ns |     60.35 ns |
|                       FlatSharp_Serialize |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,318.12 ns |   5.771 ns |   8.458 ns |  1,321.22 ns |
|                            PBDN_Serialize |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,310.96 ns |   7.924 ns |  11.109 ns |  1,307.25 ns |
|   FlatSharp_Serialize_StringVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |  1,037.13 ns |   1.917 ns |   2.809 ns |  1,037.95 ns |
| FlatSharp_Serialize_StringVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |    836.72 ns |   2.808 ns |   4.116 ns |    837.44 ns |
|      FlatSharp_Serialize_IntVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |    715.94 ns |   9.084 ns |  13.027 ns |    719.40 ns |
|    FlatSharp_Serialize_IntVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |            3 |    506.20 ns |   1.677 ns |   2.510 ns |    506.02 ns |
|              Google_FlatBuffers_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |  1,196.07 ns |   2.691 ns |   4.028 ns |  1,195.20 ns |
|    Google_FlatBuffers_Serialize_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |  1,270.54 ns |   3.797 ns |   5.683 ns |  1,269.98 ns |
|    Google_Flatbuffers_StringVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    847.09 ns |   1.350 ns |   1.936 ns |    847.23 ns |
|  Google_Flatbuffers_StringVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    641.03 ns |   5.595 ns |   8.374 ns |    638.83 ns |
|       Google_Flatbuffers_IntVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    475.59 ns |   1.457 ns |   2.135 ns |    475.80 ns |
|     Google_Flatbuffers_IntVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    330.25 ns |   2.598 ns |   3.889 ns |    331.04 ns |
|                      FlatSharp_GetMaxSize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |     60.71 ns |   0.363 ns |   0.532 ns |     60.82 ns |
|                       FlatSharp_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    604.07 ns |   2.227 ns |   3.194 ns |    603.68 ns |
|                            PBDN_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |  1,282.58 ns |   3.092 ns |   4.532 ns |  1,283.62 ns |
|   FlatSharp_Serialize_StringVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    508.58 ns |   1.094 ns |   1.638 ns |    508.42 ns |
| FlatSharp_Serialize_StringVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    414.53 ns |   1.845 ns |   2.762 ns |    414.38 ns |
|      FlatSharp_Serialize_IntVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    371.12 ns |   1.597 ns |   2.341 ns |    372.01 ns |
|    FlatSharp_Serialize_IntVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |            3 |    259.13 ns |   2.266 ns |   3.392 ns |    257.74 ns |
|              Google_FlatBuffers_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |  1,140.51 ns |   5.825 ns |   8.718 ns |  1,141.31 ns |
|    Google_FlatBuffers_Serialize_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |  1,151.43 ns |   4.517 ns |   6.478 ns |  1,149.29 ns |
|    Google_Flatbuffers_StringVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    811.41 ns |   3.121 ns |   4.671 ns |    809.83 ns |
|  Google_Flatbuffers_StringVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    612.56 ns |   6.095 ns |   9.122 ns |    612.41 ns |
|       Google_Flatbuffers_IntVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    521.59 ns |   9.579 ns |  14.041 ns |    513.07 ns |
|     Google_Flatbuffers_IntVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    369.79 ns |   6.422 ns |   9.612 ns |    369.30 ns |
|                      FlatSharp_GetMaxSize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |     61.44 ns |   0.430 ns |   0.630 ns |     61.71 ns |
|                       FlatSharp_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    504.91 ns |   7.310 ns |  10.484 ns |    502.62 ns |
|                            PBDN_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |  1,282.48 ns |  26.685 ns |  39.941 ns |  1,290.26 ns |
|   FlatSharp_Serialize_StringVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    408.39 ns |   4.631 ns |   6.931 ns |    406.47 ns |
| FlatSharp_Serialize_StringVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    307.47 ns |   4.375 ns |   6.548 ns |    306.99 ns |
|      FlatSharp_Serialize_IntVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    322.08 ns |   1.511 ns |   2.262 ns |    322.89 ns |
|    FlatSharp_Serialize_IntVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |            3 |    216.24 ns |   1.751 ns |   2.567 ns |    216.05 ns |
|              **Google_FlatBuffers_Serialize** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |           **30** |  **9,782.25 ns** | **112.628 ns** | **165.089 ns** |  **9,909.83 ns** |
|    Google_FlatBuffers_Serialize_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 | 10,960.57 ns |  27.330 ns |  40.907 ns | 10,947.13 ns |
|    Google_Flatbuffers_StringVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 | 12,621.94 ns | 223.491 ns | 320.524 ns | 12,880.91 ns |
|  Google_Flatbuffers_StringVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  5,342.52 ns |  54.057 ns |  80.909 ns |  5,345.82 ns |
|       Google_Flatbuffers_IntVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  8,566.73 ns | 162.732 ns | 243.570 ns |  8,565.39 ns |
|     Google_Flatbuffers_IntVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  2,406.43 ns |   6.682 ns |   9.795 ns |  2,405.14 ns |
|                      FlatSharp_GetMaxSize |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |    272.16 ns |   0.995 ns |   1.395 ns |    272.45 ns |
|                       FlatSharp_Serialize |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 | 10,159.99 ns |  62.827 ns |  90.104 ns | 10,228.66 ns |
|                            PBDN_Serialize |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  9,061.26 ns |  16.918 ns |  25.322 ns |  9,061.57 ns |
|   FlatSharp_Serialize_StringVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  8,525.96 ns |  29.635 ns |  44.357 ns |  8,530.36 ns |
| FlatSharp_Serialize_StringVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  6,366.34 ns |   5.670 ns |   7.949 ns |  6,366.30 ns |
|      FlatSharp_Serialize_IntVector_Sorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  6,288.28 ns |  58.202 ns |  81.591 ns |  6,217.72 ns |
|    FlatSharp_Serialize_IntVector_Unsorted |      MediumRun-.NET 4.7 |      .NET 4.7 |           30 |  3,126.53 ns |   4.542 ns |   6.657 ns |  3,124.16 ns |
|              Google_FlatBuffers_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  9,993.91 ns | 116.081 ns | 173.744 ns | 10,053.02 ns |
|    Google_FlatBuffers_Serialize_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 | 10,282.35 ns | 133.422 ns | 195.568 ns | 10,152.03 ns |
|    Google_Flatbuffers_StringVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 | 11,890.30 ns | 136.005 ns | 186.165 ns | 11,747.24 ns |
|  Google_Flatbuffers_StringVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  5,547.91 ns |   7.616 ns |  11.163 ns |  5,547.20 ns |
|       Google_Flatbuffers_IntVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  8,233.13 ns |  63.081 ns |  90.469 ns |  8,234.37 ns |
|     Google_Flatbuffers_IntVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  2,569.25 ns |   1.535 ns |   2.298 ns |  2,569.07 ns |
|                      FlatSharp_GetMaxSize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |    288.51 ns |   0.877 ns |   1.286 ns |    289.06 ns |
|                       FlatSharp_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  4,453.52 ns |  42.299 ns |  59.298 ns |  4,408.50 ns |
|                            PBDN_Serialize | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  8,903.69 ns |  56.078 ns |  82.199 ns |  8,884.13 ns |
|   FlatSharp_Serialize_StringVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  4,222.32 ns |  41.487 ns |  60.810 ns |  4,183.19 ns |
| FlatSharp_Serialize_StringVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  2,858.77 ns |  11.863 ns |  17.756 ns |  2,857.76 ns |
|      FlatSharp_Serialize_IntVector_Sorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  2,790.48 ns | 101.763 ns | 149.163 ns |  2,920.52 ns |
|    FlatSharp_Serialize_IntVector_Unsorted | MediumRun-.NET Core 2.1 | .NET Core 2.1 |           30 |  1,461.41 ns |   1.894 ns |   2.835 ns |  1,461.62 ns |
|              Google_FlatBuffers_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  9,704.66 ns |  87.845 ns | 131.482 ns |  9,722.20 ns |
|    Google_FlatBuffers_Serialize_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 | 10,208.49 ns |  97.118 ns | 145.361 ns | 10,208.34 ns |
|    Google_Flatbuffers_StringVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 | 10,999.93 ns |  81.855 ns | 117.394 ns | 11,008.32 ns |
|  Google_Flatbuffers_StringVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  5,327.01 ns | 105.469 ns | 154.595 ns |  5,286.14 ns |
|       Google_Flatbuffers_IntVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  8,119.48 ns | 242.529 ns | 363.006 ns |  8,119.56 ns |
|     Google_Flatbuffers_IntVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  2,675.20 ns |  89.521 ns | 131.219 ns |  2,601.15 ns |
|                      FlatSharp_GetMaxSize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |    283.13 ns |   3.364 ns |   5.034 ns |    282.72 ns |
|                       FlatSharp_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  3,727.70 ns |  12.818 ns |  18.789 ns |  3,727.88 ns |
|                            PBDN_Serialize | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  8,354.92 ns | 105.992 ns | 158.644 ns |  8,343.90 ns |
|   FlatSharp_Serialize_StringVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  3,529.69 ns |  30.249 ns |  43.382 ns |  3,519.89 ns |
| FlatSharp_Serialize_StringVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  2,129.12 ns |  28.512 ns |  41.793 ns |  2,115.92 ns |
|      FlatSharp_Serialize_IntVector_Sorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  2,338.55 ns |  66.675 ns |  93.469 ns |  2,297.82 ns |
|    FlatSharp_Serialize_IntVector_Unsorted | MediumRun-.NET Core 3.1 | .NET Core 3.1 |           30 |  1,159.27 ns |  24.639 ns |  36.115 ns |  1,165.73 ns |

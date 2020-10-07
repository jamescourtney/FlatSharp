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
|                                               Method |                     Job |       Runtime | TraversalCount | VectorLength |        Mean |     Error |    StdDev |
|----------------------------------------------------- |------------------------ |-------------- |--------------- |------------- |------------:|----------:|----------:|
|                  **Google_Flatbuffers_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** |            **3** |  **1,473.7 ns** |   **7.40 ns** |  **10.37 ns** |
|           Google_Flatbuffers_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |            3 |  1,095.6 ns |   6.94 ns |  10.18 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |            3 |  2,260.7 ns |  15.46 ns |  22.65 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |            3 |  2,260.6 ns |  11.32 ns |  16.59 ns |
|                                PBDN_ParseAndTraverse |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |            3 |  3,838.4 ns | 102.37 ns | 146.81 ns |
|                         PBDN_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |            3 |  3,654.6 ns |  27.10 ns |  40.57 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |  1,394.4 ns |   6.99 ns |  10.03 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |  1,017.3 ns |   7.04 ns |  10.32 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |  2,175.9 ns |  16.21 ns |  23.76 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |  2,171.7 ns |  16.84 ns |  24.16 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |  4,139.8 ns |  26.87 ns |  39.39 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |  4,095.3 ns |  27.93 ns |  40.93 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |  1,356.0 ns |   6.45 ns |   9.25 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |    999.6 ns |   4.19 ns |   6.01 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |  2,172.6 ns |  22.04 ns |  32.99 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |  2,150.0 ns |  16.08 ns |  22.55 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |  3,507.8 ns |  32.22 ns |  46.21 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |  3,424.9 ns |  25.15 ns |  36.07 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |            3 |  1,382.9 ns |   5.58 ns |   8.35 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |            3 |    968.1 ns |   6.48 ns |   9.50 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |            3 |  2,117.4 ns |  18.14 ns |  27.16 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |            3 |  2,125.5 ns |  20.24 ns |  29.66 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |            3 |  3,290.8 ns |  25.89 ns |  37.95 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |            3 |  3,205.3 ns |  12.00 ns |  17.21 ns |
|                  **Google_Flatbuffers_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** |           **30** | **12,282.1 ns** |  **78.65 ns** | **117.73 ns** |
|           Google_Flatbuffers_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |           30 |  8,605.9 ns |  45.11 ns |  67.52 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |           30 | 19,953.9 ns | 136.69 ns | 200.36 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |           30 | 19,825.3 ns | 145.87 ns | 204.49 ns |
|                                PBDN_ParseAndTraverse |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |           30 | 26,803.4 ns | 225.42 ns | 337.40 ns |
|                         PBDN_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |           30 | 26,521.8 ns | 259.59 ns | 380.50 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 | 11,772.5 ns |  56.15 ns |  84.05 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 |  8,007.2 ns |  35.32 ns |  49.51 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 | 19,165.9 ns |  96.30 ns | 141.16 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 | 19,027.9 ns | 138.42 ns | 198.52 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 | 30,545.1 ns | 238.09 ns | 348.99 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 | 30,031.1 ns | 169.79 ns | 243.50 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 | 11,521.5 ns | 147.56 ns | 220.86 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 |  7,663.5 ns |  30.54 ns |  45.72 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 | 18,973.8 ns | 123.82 ns | 181.49 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 | 18,950.8 ns | 140.25 ns | 201.14 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 | 24,898.6 ns | 195.75 ns | 292.99 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 | 24,472.4 ns | 139.76 ns | 200.44 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |           30 | 11,465.0 ns |  51.89 ns |  77.67 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |           30 |  7,472.6 ns |  29.13 ns |  41.78 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |           30 | 18,579.6 ns |  94.44 ns | 138.42 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |           30 | 18,615.6 ns | 102.52 ns | 147.03 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |           30 | 24,040.8 ns | 154.39 ns | 231.08 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |           30 | 23,842.4 ns | 153.72 ns | 230.09 ns |
|                  **Google_Flatbuffers_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** |            **3** |  **7,382.2 ns** |  **31.84 ns** |  **45.67 ns** |
|           Google_Flatbuffers_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |            3 |  5,498.3 ns |  51.03 ns |  76.38 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |            3 |  2,311.2 ns |  12.76 ns |  18.71 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |            3 |  2,268.2 ns |  17.76 ns |  26.03 ns |
|                                PBDN_ParseAndTraverse |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |            3 |  3,962.4 ns |  35.91 ns |  52.64 ns |
|                         PBDN_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |            3 |  3,818.5 ns |  39.47 ns |  57.85 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  6,875.2 ns |  38.40 ns |  56.28 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  5,034.7 ns |  24.99 ns |  37.40 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  2,196.8 ns |  18.22 ns |  26.70 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  2,161.9 ns |  16.09 ns |  23.58 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  4,416.9 ns |  19.07 ns |  26.73 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  4,327.8 ns |  43.26 ns |  64.75 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  6,881.7 ns |  38.01 ns |  55.72 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  4,905.4 ns |  42.04 ns |  61.63 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  2,192.7 ns |  10.53 ns |  15.11 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  2,155.0 ns |  11.01 ns |  15.07 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  3,780.9 ns |  27.19 ns |  37.22 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  3,564.1 ns |  26.30 ns |  38.56 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |            3 |  6,900.6 ns |  40.04 ns |  59.94 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |            3 |  4,800.8 ns |  35.38 ns |  51.87 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |            3 |  2,187.8 ns |  22.23 ns |  31.88 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |            3 |  2,182.8 ns |  12.25 ns |  18.33 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |            3 |  3,593.9 ns |  25.94 ns |  38.03 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |            3 |  3,396.1 ns |  17.00 ns |  24.92 ns |
|                  **Google_Flatbuffers_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** |           **30** | **61,514.1 ns** | **368.21 ns** | **539.72 ns** |
|           Google_Flatbuffers_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |           30 | 42,963.1 ns | 208.85 ns | 299.53 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |           30 | 20,444.9 ns | 141.84 ns | 203.42 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |           30 | 20,074.2 ns | 183.12 ns | 274.09 ns |
|                                PBDN_ParseAndTraverse |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |           30 | 29,320.4 ns | 151.86 ns | 222.60 ns |
|                         PBDN_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |           30 | 27,591.9 ns | 268.16 ns | 384.59 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 59,038.6 ns | 353.17 ns | 528.61 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 39,866.2 ns | 146.53 ns | 210.15 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 19,595.1 ns | 102.72 ns | 143.99 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 19,282.4 ns |  84.15 ns | 123.35 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 32,480.8 ns | 117.23 ns | 164.34 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 31,119.5 ns | 192.22 ns | 287.70 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 58,101.4 ns | 345.31 ns | 484.07 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 38,337.1 ns | 262.85 ns | 393.43 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 19,341.6 ns | 126.21 ns | 176.93 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 19,293.0 ns |  96.77 ns | 144.84 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 27,507.8 ns | 144.02 ns | 206.55 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 25,842.3 ns | 196.90 ns | 288.62 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |           30 | 58,638.1 ns | 497.28 ns | 744.31 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |           30 | 37,470.5 ns | 339.70 ns | 508.44 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |           30 | 19,100.4 ns |  99.21 ns | 148.50 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |           30 | 18,635.3 ns | 119.70 ns | 179.17 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |           30 | 26,238.1 ns | 166.35 ns | 248.99 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |           30 | 24,526.5 ns | 152.57 ns | 228.35 ns |

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
|                                               Method |                     Job |       Runtime | TraversalCount | VectorLength |        Mean |     Error |      StdDev |      Median |
|----------------------------------------------------- |------------------------ |-------------- |--------------- |------------- |------------:|----------:|------------:|------------:|
|                  **Google_Flatbuffers_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** |            **3** |  **1,454.8 ns** |   **5.17 ns** |     **7.25 ns** |  **1,455.3 ns** |
|           Google_Flatbuffers_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |            3 |  1,084.8 ns |  12.49 ns |    18.31 ns |  1,086.2 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |            3 |  2,211.1 ns |  20.44 ns |    30.59 ns |  2,202.6 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |            3 |  2,200.5 ns |  18.79 ns |    26.95 ns |  2,192.7 ns |
|                                PBDN_ParseAndTraverse |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |            3 |  3,707.3 ns |  35.89 ns |    53.72 ns |  3,697.8 ns |
|                         PBDN_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |            3 |  3,646.0 ns |  30.79 ns |    45.13 ns |  3,641.4 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |  1,393.8 ns |   7.48 ns |    10.97 ns |  1,390.6 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |  1,020.3 ns |   2.77 ns |     3.89 ns |  1,019.5 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |  2,176.5 ns |  13.13 ns |    19.66 ns |  2,173.4 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |  2,152.3 ns |  12.66 ns |    17.32 ns |  2,149.7 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |  4,149.0 ns |  20.01 ns |    28.70 ns |  4,153.4 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |  4,092.2 ns |  23.06 ns |    33.80 ns |  4,088.5 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |  1,385.5 ns |  11.82 ns |    17.69 ns |  1,382.5 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |  1,004.4 ns |   4.69 ns |     6.87 ns |  1,003.2 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |  2,146.7 ns |  17.40 ns |    25.50 ns |  2,144.8 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |  2,170.5 ns |  11.73 ns |    17.20 ns |  2,168.8 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |  3,430.9 ns |  16.61 ns |    24.34 ns |  3,432.7 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |  3,437.2 ns |  24.65 ns |    36.13 ns |  3,436.2 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |            3 |  1,356.5 ns |   8.07 ns |    12.08 ns |  1,355.1 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |            3 |    964.5 ns |   6.97 ns |    10.44 ns |    965.9 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |            3 |  2,089.9 ns |  20.41 ns |    30.55 ns |  2,079.0 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |            3 |  2,087.2 ns |  13.33 ns |    19.12 ns |  2,082.7 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |            3 |  3,309.0 ns |  21.64 ns |    32.39 ns |  3,303.4 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |            3 |  3,212.2 ns |  11.68 ns |    16.74 ns |  3,209.8 ns |
|                  **Google_Flatbuffers_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** |           **30** | **11,983.4 ns** |  **39.99 ns** |    **57.36 ns** | **11,989.1 ns** |
|           Google_Flatbuffers_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |           30 |  8,497.9 ns |  65.23 ns |    97.64 ns |  8,493.0 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |           30 | 19,583.0 ns | 385.43 ns |   564.96 ns | 19,673.8 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |           30 | 19,376.8 ns |  82.28 ns |   115.35 ns | 19,372.9 ns |
|                                PBDN_ParseAndTraverse |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |           30 | 26,876.3 ns | 190.43 ns |   279.13 ns | 26,939.3 ns |
|                         PBDN_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |           30 | 27,786.2 ns | 865.76 ns | 1,295.83 ns | 27,773.9 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 | 11,749.0 ns |  61.94 ns |    90.79 ns | 11,736.5 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 |  8,039.0 ns |  65.35 ns |    91.61 ns |  8,025.5 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 | 18,848.1 ns | 163.19 ns |   234.04 ns | 18,837.3 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 | 19,188.8 ns | 194.05 ns |   290.44 ns | 19,086.7 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 | 30,536.4 ns | 231.20 ns |   346.05 ns | 30,498.8 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 | 30,101.6 ns | 167.06 ns |   244.88 ns | 30,023.6 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 | 11,486.4 ns |  56.75 ns |    83.18 ns | 11,468.2 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 |  7,581.6 ns |  64.45 ns |    94.47 ns |  7,609.9 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 | 18,741.6 ns |  79.98 ns |   117.23 ns | 18,720.3 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 | 19,057.4 ns | 114.55 ns |   171.46 ns | 19,023.6 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 | 24,898.1 ns | 154.85 ns |   226.98 ns | 24,846.9 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 | 24,497.3 ns | 129.27 ns |   181.22 ns | 24,508.6 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |           30 | 11,454.0 ns |  92.43 ns |   138.34 ns | 11,447.8 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |           30 |  7,537.8 ns |  49.11 ns |    73.51 ns |  7,523.9 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |           30 | 18,676.5 ns | 153.48 ns |   229.73 ns | 18,590.8 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |           30 | 18,467.5 ns | 153.84 ns |   230.26 ns | 18,435.9 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |           30 | 23,939.1 ns | 172.81 ns |   258.66 ns | 24,002.3 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |           30 | 23,401.7 ns | 154.20 ns |   230.80 ns | 23,373.5 ns |
|                  **Google_Flatbuffers_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** |            **3** |  **7,166.3 ns** |  **42.57 ns** |    **62.39 ns** |  **7,144.2 ns** |
|           Google_Flatbuffers_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |            3 |  5,376.1 ns |  21.56 ns |    32.27 ns |  5,375.1 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |            3 |  2,243.3 ns |  28.14 ns |    39.44 ns |  2,235.2 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |            3 |  2,212.1 ns |  13.48 ns |    19.76 ns |  2,213.1 ns |
|                                PBDN_ParseAndTraverse |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |            3 |  3,951.4 ns |  27.63 ns |    39.62 ns |  3,948.1 ns |
|                         PBDN_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |            3 |  3,806.0 ns |  31.55 ns |    47.23 ns |  3,806.7 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  6,866.4 ns |  46.31 ns |    69.31 ns |  6,845.5 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  5,022.6 ns |  33.13 ns |    47.52 ns |  5,032.5 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  2,199.2 ns |  13.14 ns |    19.27 ns |  2,197.6 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  2,175.5 ns |  20.26 ns |    29.70 ns |  2,168.7 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  4,477.9 ns |  63.59 ns |    93.21 ns |  4,489.2 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  4,250.5 ns |  33.42 ns |    48.99 ns |  4,232.0 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  6,831.1 ns |  56.39 ns |    82.65 ns |  6,837.8 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  4,915.3 ns |  45.13 ns |    66.15 ns |  4,893.0 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  2,205.3 ns |  10.01 ns |    14.68 ns |  2,203.2 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  2,175.7 ns |  13.38 ns |    20.02 ns |  2,171.4 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  3,753.0 ns |  24.26 ns |    36.32 ns |  3,746.2 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  3,580.9 ns |  25.64 ns |    35.95 ns |  3,582.8 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |            3 |  6,830.8 ns |  58.68 ns |    86.02 ns |  6,818.4 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |            3 |  4,847.5 ns |  19.06 ns |    27.33 ns |  4,849.1 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |            3 |  2,154.5 ns |  17.54 ns |    25.16 ns |  2,156.3 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |            3 |  2,130.7 ns |  10.23 ns |    14.68 ns |  2,126.5 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |            3 |  3,608.4 ns |  26.39 ns |    39.51 ns |  3,600.5 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |            3 |  3,441.5 ns |  29.60 ns |    41.50 ns |  3,432.3 ns |
|                  **Google_Flatbuffers_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** |           **30** | **60,114.4 ns** | **329.03 ns** |   **461.26 ns** | **60,249.9 ns** |
|           Google_Flatbuffers_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |           30 | 42,568.8 ns | 373.57 ns |   559.14 ns | 42,483.9 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |           30 | 19,931.1 ns | 205.62 ns |   307.76 ns | 20,019.4 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |           30 | 19,841.3 ns | 154.27 ns |   226.12 ns | 19,801.5 ns |
|                                PBDN_ParseAndTraverse |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |           30 | 30,598.5 ns | 859.99 ns | 1,233.37 ns | 29,935.5 ns |
|                         PBDN_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |           30 | 27,773.9 ns | 133.23 ns |   191.07 ns | 27,759.6 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 58,679.7 ns | 307.61 ns |   460.42 ns | 58,533.5 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 39,795.0 ns | 305.76 ns |   438.52 ns | 39,794.3 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 19,425.9 ns | 140.52 ns |   205.98 ns | 19,393.4 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 19,146.3 ns |  99.96 ns |   149.62 ns | 19,110.5 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 32,966.5 ns | 250.97 ns |   367.87 ns | 32,888.0 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 31,282.6 ns | 212.95 ns |   312.14 ns | 31,234.6 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 57,383.3 ns | 390.22 ns |   584.07 ns | 57,234.1 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 37,694.3 ns | 225.16 ns |   330.04 ns | 37,670.1 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 19,565.5 ns | 111.62 ns |   167.06 ns | 19,518.0 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 19,179.1 ns | 173.64 ns |   254.53 ns | 19,181.2 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 27,572.6 ns | 311.49 ns |   446.74 ns | 27,503.6 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 25,489.2 ns |  95.36 ns |   142.73 ns | 25,480.4 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |           30 | 57,452.7 ns | 352.09 ns |   526.99 ns | 57,273.2 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |           30 | 37,704.8 ns | 455.74 ns |   668.02 ns | 37,755.2 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |           30 | 19,254.5 ns | 171.26 ns |   251.03 ns | 19,246.8 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |           30 | 18,765.0 ns | 129.61 ns |   181.70 ns | 18,798.0 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |           30 | 26,226.4 ns | 197.13 ns |   282.72 ns | 26,221.5 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |           30 | 24,981.1 ns | 176.48 ns |   264.15 ns | 24,953.1 ns |

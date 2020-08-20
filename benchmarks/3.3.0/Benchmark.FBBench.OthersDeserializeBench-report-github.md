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
|                                               Method |                     Job |       Runtime | TraversalCount | VectorLength |        Mean |     Error |    StdDev |      Median |
|----------------------------------------------------- |------------------------ |-------------- |--------------- |------------- |------------:|----------:|----------:|------------:|
|                  **Google_Flatbuffers_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** |            **3** |    **976.3 ns** |   **7.01 ns** |  **10.06 ns** |    **977.0 ns** |
|           Google_Flatbuffers_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |            3 |    731.0 ns |   0.52 ns |   0.70 ns |    730.9 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |            3 |  1,474.2 ns |   1.72 ns |   2.36 ns |  1,474.1 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |            3 |  1,497.5 ns |   4.76 ns |   7.12 ns |  1,500.7 ns |
|                                PBDN_ParseAndTraverse |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |            3 |  2,922.6 ns |  20.25 ns |  30.30 ns |  2,925.3 ns |
|                         PBDN_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |            3 |  2,888.9 ns |  17.80 ns |  26.64 ns |  2,896.1 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |    979.6 ns |   2.05 ns |   3.07 ns |    979.4 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |    713.9 ns |   4.31 ns |   6.45 ns |    713.2 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |  1,463.7 ns |   8.87 ns |  12.72 ns |  1,465.7 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |  1,439.8 ns |   2.97 ns |   4.26 ns |  1,440.5 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |  3,176.3 ns |   9.56 ns |  13.71 ns |  3,177.4 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |            3 |  3,173.1 ns |  12.59 ns |  18.84 ns |  3,177.8 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |    917.9 ns |   4.94 ns |   7.40 ns |    917.8 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |    678.2 ns |   3.34 ns |   4.90 ns |    677.0 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |  1,472.9 ns |  13.38 ns |  19.61 ns |  1,468.6 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |  1,433.9 ns |   8.19 ns |  12.00 ns |  1,429.0 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |  2,861.7 ns |   9.20 ns |  12.60 ns |  2,862.3 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |            3 |  2,779.8 ns |  21.39 ns |  29.28 ns |  2,778.7 ns |
|                  **Google_Flatbuffers_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** |           **30** |  **8,276.8 ns** |  **99.15 ns** | **148.41 ns** |  **8,261.6 ns** |
|           Google_Flatbuffers_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |           30 |  5,587.6 ns |  24.27 ns |  36.32 ns |  5,585.6 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |           30 | 12,643.5 ns |  46.24 ns |  64.82 ns | 12,676.5 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |           30 | 12,750.5 ns | 105.07 ns | 157.26 ns | 12,763.9 ns |
|                                PBDN_ParseAndTraverse |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |           30 | 20,967.0 ns |  95.48 ns | 139.96 ns | 20,992.0 ns |
|                         PBDN_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |           30 | 20,522.5 ns | 246.81 ns | 369.41 ns | 20,492.9 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 |  8,305.8 ns |  32.87 ns |  46.08 ns |  8,288.8 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 |  5,708.2 ns |   5.90 ns |   8.64 ns |  5,708.6 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 | 12,818.1 ns |  22.99 ns |  34.42 ns | 12,826.8 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 | 12,738.7 ns |  24.98 ns |  36.61 ns | 12,732.5 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 | 22,699.6 ns | 113.37 ns | 162.59 ns | 22,695.5 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |           30 | 22,435.6 ns |  87.37 ns | 128.07 ns | 22,440.5 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 |  7,749.7 ns | 193.93 ns | 290.26 ns |  7,783.0 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 |  4,983.5 ns |  85.91 ns | 128.58 ns |  4,971.6 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 | 12,909.0 ns |  60.98 ns |  89.38 ns | 12,880.1 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 | 12,634.8 ns |  75.77 ns | 111.07 ns | 12,607.1 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 | 19,534.6 ns | 177.92 ns | 266.30 ns | 19,469.9 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |           30 | 19,624.2 ns |  78.31 ns | 114.79 ns | 19,610.5 ns |
|                  **Google_Flatbuffers_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** |            **3** |  **4,694.9 ns** |  **42.37 ns** |  **59.40 ns** |  **4,734.7 ns** |
|           Google_Flatbuffers_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |            3 |  3,464.4 ns |   5.63 ns |   8.25 ns |  3,465.2 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |            3 |  1,523.8 ns |   4.90 ns |   7.33 ns |  1,523.5 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |            3 |  1,477.7 ns |   2.92 ns |   4.37 ns |  1,478.0 ns |
|                                PBDN_ParseAndTraverse |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |            3 |  3,144.5 ns |  19.43 ns |  29.08 ns |  3,135.3 ns |
|                         PBDN_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |            3 |  3,019.1 ns |  10.09 ns |  14.47 ns |  3,022.7 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  4,771.8 ns |  25.58 ns |  36.69 ns |  4,785.5 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  3,429.1 ns |   4.80 ns |   6.73 ns |  3,429.1 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  1,490.2 ns |   3.00 ns |   4.49 ns |  1,489.8 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  1,471.0 ns |   3.96 ns |   5.93 ns |  1,470.9 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  3,410.6 ns |   9.71 ns |  14.53 ns |  3,411.4 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |            3 |  3,309.1 ns |  32.39 ns |  47.48 ns |  3,302.9 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  4,654.1 ns |  66.69 ns |  91.28 ns |  4,594.5 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  3,216.1 ns |  26.04 ns |  37.35 ns |  3,226.6 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  1,477.9 ns |   8.93 ns |  13.37 ns |  1,472.5 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  1,451.7 ns |   3.52 ns |   5.05 ns |  1,451.1 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  3,104.3 ns |  67.92 ns | 101.66 ns |  3,103.8 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |            3 |  2,905.7 ns |  10.35 ns |  15.49 ns |  2,904.9 ns |
|                  **Google_Flatbuffers_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** |           **30** | **39,716.7 ns** | **656.15 ns** | **919.82 ns** | **38,989.9 ns** |
|           Google_Flatbuffers_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |           30 | 27,700.6 ns | 261.75 ns | 349.43 ns | 27,954.6 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |           30 | 13,458.3 ns |  14.49 ns |  20.79 ns | 13,457.8 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |           30 | 13,259.7 ns |  19.96 ns |  29.87 ns | 13,259.1 ns |
|                                PBDN_ParseAndTraverse |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |           30 | 22,965.6 ns | 119.65 ns | 179.08 ns | 22,992.2 ns |
|                         PBDN_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |           30 | 21,019.5 ns | 107.57 ns | 161.01 ns | 20,980.0 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 41,594.3 ns |  35.20 ns |  50.48 ns | 41,592.0 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 28,509.8 ns | 102.61 ns | 147.15 ns | 28,447.8 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 13,039.0 ns |  39.87 ns |  59.67 ns | 13,014.3 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 12,943.7 ns |  34.74 ns |  49.82 ns | 12,959.4 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 24,586.5 ns |  58.61 ns |  80.23 ns | 24,580.3 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |           30 | 23,451.3 ns |  90.31 ns | 135.17 ns | 23,452.4 ns |
|                  Google_Flatbuffers_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 40,679.7 ns | 286.64 ns | 411.10 ns | 40,455.7 ns |
|           Google_Flatbuffers_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 24,653.3 ns | 466.57 ns | 683.89 ns | 24,407.0 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 13,141.3 ns |  38.57 ns |  56.54 ns | 13,153.0 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 12,847.4 ns | 188.74 ns | 282.49 ns | 12,833.3 ns |
|                                PBDN_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 21,621.5 ns |  53.98 ns |  80.79 ns | 21,624.7 ns |
|                         PBDN_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |           30 | 20,124.3 ns |  48.71 ns |  69.86 ns | 20,120.9 ns |

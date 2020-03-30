``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.18363
AMD Ryzen 9 3900X 12-Core Processor (Max: 3.80GHz), 1 CPU, 24 logical and 12 physical cores
.NET Core SDK=3.1.200
  [Host]   : .NET Core 2.1.16 (CoreCLR 4.6.28516.03, CoreFX 4.6.28516.10), 64bit RyuJIT
  ShortRun : .NET Core 2.1.16 (CoreCLR 4.6.28516.03, CoreFX 4.6.28516.10), 64bit RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                                               Method | TraversalCount | VectorLength |        Mean |       Error |      StdDev |
|----------------------------------------------------- |--------------- |------------- |------------:|------------:|------------:|
|                  **Google_Flatbuffers_ParseAndTraverse** |              **1** |            **3** |  **1,033.1 ns** |    **38.40 ns** |   **2.1696 ns** |
|           Google_Flatbuffers_ParseAndTraversePartial |              1 |            3 |    700.8 ns |    23.92 ns |   1.3514 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |              1 |            3 |  1,507.9 ns |    26.47 ns |   1.4956 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |              1 |            3 |  1,504.5 ns |    52.17 ns |   2.9476 ns |
|                                PBDN_ParseAndTraverse |              1 |            3 |  3,238.9 ns |   122.44 ns |   6.9178 ns |
|                         PBDN_ParseAndTraversePartial |              1 |            3 |  3,247.2 ns |   395.02 ns |  22.3195 ns |
|                  **Google_Flatbuffers_ParseAndTraverse** |              **1** |           **30** |  **8,786.0 ns** |    **87.14 ns** |   **4.9237 ns** |
|           Google_Flatbuffers_ParseAndTraversePartial |              1 |           30 |  5,553.8 ns |   213.35 ns |  12.0549 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |              1 |           30 | 13,483.4 ns |   453.44 ns |  25.6200 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |              1 |           30 | 13,376.5 ns |   175.88 ns |   9.9376 ns |
|                                PBDN_ParseAndTraverse |              1 |           30 | 23,548.9 ns | 2,091.87 ns | 118.1948 ns |
|                         PBDN_ParseAndTraversePartial |              1 |           30 | 24,080.6 ns | 1,714.82 ns |  96.8906 ns |
|                  **Google_Flatbuffers_ParseAndTraverse** |              **5** |            **3** |  **5,054.0 ns** |    **53.41 ns** |   **3.0177 ns** |
|           Google_Flatbuffers_ParseAndTraversePartial |              5 |            3 |  3,508.4 ns |    11.99 ns |   0.6774 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |              5 |            3 |  1,537.3 ns |    50.19 ns |   2.8360 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |              5 |            3 |  1,526.0 ns |    81.95 ns |   4.6301 ns |
|                                PBDN_ParseAndTraverse |              5 |            3 |  3,576.3 ns | 1,839.37 ns | 103.9280 ns |
|                         PBDN_ParseAndTraversePartial |              5 |            3 |  3,382.2 ns |   293.87 ns |  16.6039 ns |
|                  **Google_Flatbuffers_ParseAndTraverse** |              **5** |           **30** | **43,056.7 ns** |   **497.05 ns** |  **28.0840 ns** |
|           Google_Flatbuffers_ParseAndTraversePartial |              5 |           30 | 28,585.4 ns | 4,213.12 ns | 238.0493 ns |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |              5 |           30 | 13,794.5 ns |   627.36 ns |  35.4470 ns |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |              5 |           30 | 13,347.6 ns |   680.48 ns |  38.4484 ns |
|                                PBDN_ParseAndTraverse |              5 |           30 | 26,063.3 ns | 3,254.30 ns | 183.8738 ns |
|                         PBDN_ParseAndTraversePartial |              5 |           30 | 24,455.6 ns |   784.10 ns |  44.3034 ns |

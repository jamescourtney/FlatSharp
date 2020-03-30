``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.18363
AMD Ryzen 9 3900X 12-Core Processor (Max: 3.80GHz), 1 CPU, 24 logical and 12 physical cores
.NET Core SDK=3.1.200
  [Host]   : .NET Core 2.1.16 (CoreCLR 4.6.28516.03, CoreFX 4.6.28516.10), 64bit RyuJIT
  ShortRun : .NET Core 2.1.16 (CoreCLR 4.6.28516.03, CoreFX 4.6.28516.10), 64bit RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                                     Method | TraversalCount | VectorLength |        Mean |       Error |     StdDev |
|------------------------------------------- |--------------- |------------- |------------:|------------:|-----------:|
|        **Google_Flatbuffers_ParseAndTraverse** |              **1** |            **3** |  **1,014.3 ns** |    **18.46 ns** |   **1.043 ns** |
| Google_Flatbuffers_ParseAndTraversePartial |              1 |            3 |    698.7 ns |   110.71 ns |   6.255 ns |
|                      PBDN_ParseAndTraverse |              1 |            3 |  3,302.1 ns |   109.82 ns |   6.205 ns |
|               PBDN_ParseAndTraversePartial |              1 |            3 |  3,257.4 ns |   123.37 ns |   6.970 ns |
|        **Google_Flatbuffers_ParseAndTraverse** |              **1** |           **30** |  **8,277.3 ns** |    **82.52 ns** |   **4.663 ns** |
| Google_Flatbuffers_ParseAndTraversePartial |              1 |           30 |  5,381.1 ns |    68.28 ns |   3.858 ns |
|                      PBDN_ParseAndTraverse |              1 |           30 | 24,073.8 ns | 2,280.50 ns | 128.852 ns |
|               PBDN_ParseAndTraversePartial |              1 |           30 | 22,870.8 ns |   935.38 ns |  52.851 ns |
|        **Google_Flatbuffers_ParseAndTraverse** |              **5** |            **3** |  **4,746.7 ns** |   **119.53 ns** |   **6.754 ns** |
| Google_Flatbuffers_ParseAndTraversePartial |              5 |            3 |  3,267.7 ns |    61.69 ns |   3.485 ns |
|                      PBDN_ParseAndTraverse |              5 |            3 |  3,535.4 ns |   179.76 ns |  10.157 ns |
|               PBDN_ParseAndTraversePartial |              5 |            3 |  3,379.7 ns |   269.58 ns |  15.232 ns |
|        **Google_Flatbuffers_ParseAndTraverse** |              **5** |           **30** | **42,207.3 ns** | **1,014.03 ns** |  **57.294 ns** |
| Google_Flatbuffers_ParseAndTraversePartial |              5 |           30 | 25,942.9 ns | 1,282.82 ns |  72.482 ns |
|                      PBDN_ParseAndTraverse |              5 |           30 | 25,402.2 ns | 2,386.51 ns | 134.842 ns |
|               PBDN_ParseAndTraversePartial |              5 |           30 | 24,091.7 ns |   759.31 ns |  42.903 ns |

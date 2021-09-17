``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1237 (20H2/October2020Update)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=5.0.201
  [Host]   : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT
  ShortRun : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET 5.0  
IterationCount=5  LaunchCount=7  WarmupCount=3  

```
|                                               Method | TraversalCount | VectorLength |      Mean |     Error |    StdDev |    Median |       P25 |       P95 |  Gen 0 |  Gen 1 | Allocated |
|----------------------------------------------------- |--------------- |------------- |----------:|----------:|----------:|----------:|----------:|----------:|-------:|-------:|----------:|
|                  **Google_Flatbuffers_ParseAndTraverse** |              **1** |           **30** |  **4.928 μs** | **0.1676 μs** | **0.2754 μs** |  **4.824 μs** |  **4.806 μs** |  **5.591 μs** | **0.1755** |      **-** |      **3 KB** |
|           Google_Flatbuffers_ParseAndTraversePartial |              1 |           30 |  3.428 μs | 0.0103 μs | 0.0166 μs |  3.423 μs |  3.421 μs |  3.466 μs | 0.1755 |      - |      3 KB |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |              1 |           30 |  7.734 μs | 0.1116 μs | 0.1738 μs |  7.698 μs |  7.610 μs |  8.060 μs | 0.6104 | 0.0153 |     10 KB |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |              1 |           30 |  7.707 μs | 0.0523 μs | 0.0813 μs |  7.681 μs |  7.649 μs |  7.830 μs | 0.6104 | 0.0153 |     10 KB |
|                                PBDN_ParseAndTraverse |              1 |           30 |  9.472 μs | 0.0559 μs | 0.0902 μs |  9.498 μs |  9.449 μs |  9.566 μs | 0.4120 |      - |      7 KB |
|                         PBDN_ParseAndTraversePartial |              1 |           30 |  9.095 μs | 0.1368 μs | 0.2210 μs |  8.983 μs |  8.946 μs |  9.531 μs | 0.4120 |      - |      7 KB |
|                     PBDN_ParseAndTraverse_NonVirtual |              1 |           30 |  8.464 μs | 0.1046 μs | 0.1659 μs |  8.533 μs |  8.241 μs |  8.625 μs | 0.4120 |      - |      7 KB |
|              PBDN_ParseAndTraversePartial_NonVirtual |              1 |           30 |  8.405 μs | 0.0967 μs | 0.1506 μs |  8.381 μs |  8.260 μs |  8.664 μs | 0.4120 |      - |      7 KB |
|                             MsgPack_ParseAndTraverse |              1 |           30 |  5.491 μs | 0.2864 μs | 0.4706 μs |  5.269 μs |  5.162 μs |  6.498 μs | 0.4120 | 0.0076 |      7 KB |
|                      MsgPack_ParseAndTraversePartial |              1 |           30 |  5.292 μs | 0.1539 μs | 0.2396 μs |  5.218 μs |  5.196 μs |  5.883 μs | 0.4120 | 0.0076 |      7 KB |
|                  **Google_Flatbuffers_ParseAndTraverse** |              **5** |           **30** | **23.958 μs** | **0.0863 μs** | **0.1344 μs** | **23.903 μs** | **23.860 μs** | **24.192 μs** | **0.8850** |      **-** |     **14 KB** |
|           Google_Flatbuffers_ParseAndTraversePartial |              5 |           30 | 17.120 μs | 0.0638 μs | 0.1031 μs | 17.123 μs | 17.064 μs | 17.261 μs | 0.8850 |      - |     14 KB |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |              5 |           30 |  7.946 μs | 0.0949 μs | 0.1559 μs |  7.898 μs |  7.801 μs |  8.198 μs | 0.6104 | 0.0153 |     10 KB |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |              5 |           30 |  7.832 μs | 0.0943 μs | 0.1549 μs |  7.797 μs |  7.676 μs |  8.093 μs | 0.6104 | 0.0153 |     10 KB |
|                                PBDN_ParseAndTraverse |              5 |           30 | 11.220 μs | 0.0926 μs | 0.1522 μs | 11.266 μs | 11.205 μs | 11.374 μs | 0.4120 |      - |      7 KB |
|                         PBDN_ParseAndTraversePartial |              5 |           30 |  9.916 μs | 0.0801 μs | 0.1315 μs |  9.879 μs |  9.801 μs | 10.142 μs | 0.4120 |      - |      7 KB |
|                     PBDN_ParseAndTraverse_NonVirtual |              5 |           30 |  8.714 μs | 0.0851 μs | 0.1399 μs |  8.701 μs |  8.591 μs |  8.951 μs | 0.4120 |      - |      7 KB |
|              PBDN_ParseAndTraversePartial_NonVirtual |              5 |           30 |  8.666 μs | 0.1748 μs | 0.2823 μs |  8.638 μs |  8.537 μs |  9.388 μs | 0.4120 |      - |      7 KB |
|                             MsgPack_ParseAndTraverse |              5 |           30 |  5.995 μs | 0.3480 μs | 0.5620 μs |  5.658 μs |  5.527 μs |  6.772 μs | 0.4120 | 0.0076 |      7 KB |
|                      MsgPack_ParseAndTraversePartial |              5 |           30 |  5.591 μs | 0.2432 μs | 0.3927 μs |  5.427 μs |  5.294 μs |  6.326 μs | 0.4120 | 0.0076 |      7 KB |

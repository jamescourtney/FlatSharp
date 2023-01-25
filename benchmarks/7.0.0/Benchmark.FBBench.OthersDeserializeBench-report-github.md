``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.819)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET 7.0  
IterationCount=5  LaunchCount=7  WarmupCount=3  

```
|                                               Method | TraversalCount | VectorLength |      Mean |     Error |    StdDev |    Median |       P25 |       P95 |   Gen0 |   Gen1 | Allocated |
|----------------------------------------------------- |--------------- |------------- |----------:|----------:|----------:|----------:|----------:|----------:|-------:|-------:|----------:|
|                  **Google_Flatbuffers_ParseAndTraverse** |              **1** |           **30** |  **4.587 μs** | **0.0155 μs** | **0.0254 μs** |  **4.585 μs** |  **4.565 μs** |  **4.628 μs** | **0.1755** |      **-** |    **2.9 KB** |
|           Google_Flatbuffers_ParseAndTraversePartial |              1 |           30 |  3.264 μs | 0.0207 μs | 0.0309 μs |  3.254 μs |  3.238 μs |  3.317 μs | 0.1755 |      - |    2.9 KB |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |              1 |           30 |  7.394 μs | 0.0592 μs | 0.0922 μs |  7.376 μs |  7.325 μs |  7.568 μs | 0.6104 | 0.0153 |  10.09 KB |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |              1 |           30 |  7.339 μs | 0.0356 μs | 0.0575 μs |  7.348 μs |  7.284 μs |  7.419 μs | 0.6104 | 0.0153 |  10.09 KB |
|                                PBDN_ParseAndTraverse |              1 |           30 |  9.310 μs | 0.0727 μs | 0.1174 μs |  9.309 μs |  9.222 μs |  9.546 μs | 0.4120 |      - |   6.77 KB |
|                         PBDN_ParseAndTraversePartial |              1 |           30 |  9.142 μs | 0.0706 μs | 0.1120 μs |  9.158 μs |  9.014 μs |  9.328 μs | 0.4120 |      - |   6.77 KB |
|                     PBDN_ParseAndTraverse_NonVirtual |              1 |           30 |  8.466 μs | 0.0963 μs | 0.1500 μs |  8.424 μs |  8.385 μs |  8.810 μs | 0.4120 |      - |   6.77 KB |
|              PBDN_ParseAndTraversePartial_NonVirtual |              1 |           30 |  8.423 μs | 0.0728 μs | 0.1197 μs |  8.412 μs |  8.347 μs |  8.634 μs | 0.4120 |      - |   6.77 KB |
|                             MsgPack_ParseAndTraverse |              1 |           30 |  4.877 μs | 0.2437 μs | 0.3866 μs |  4.735 μs |  4.629 μs |  5.749 μs | 0.4120 | 0.0076 |   6.74 KB |
|                      MsgPack_ParseAndTraversePartial |              1 |           30 |  4.904 μs | 0.2218 μs | 0.3581 μs |  4.720 μs |  4.681 μs |  5.690 μs | 0.4120 | 0.0076 |   6.74 KB |
|                  **Google_Flatbuffers_ParseAndTraverse** |              **5** |           **30** | **22.857 μs** | **0.0883 μs** | **0.1375 μs** | **22.826 μs** | **22.738 μs** | **23.075 μs** | **0.8850** |      **-** |  **14.49 KB** |
|           Google_Flatbuffers_ParseAndTraversePartial |              5 |           30 | 16.196 μs | 0.0756 μs | 0.1177 μs | 16.203 μs | 16.157 μs | 16.436 μs | 0.8850 |      - |  14.49 KB |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |              5 |           30 |  7.710 μs | 0.0852 μs | 0.1352 μs |  7.657 μs |  7.618 μs |  8.000 μs | 0.6104 | 0.0153 |  10.09 KB |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |              5 |           30 |  7.507 μs | 0.0557 μs | 0.0900 μs |  7.509 μs |  7.475 μs |  7.636 μs | 0.6104 | 0.0153 |  10.09 KB |
|                                PBDN_ParseAndTraverse |              5 |           30 | 10.952 μs | 0.0850 μs | 0.1348 μs | 10.973 μs | 10.882 μs | 11.189 μs | 0.4120 |      - |   6.77 KB |
|                         PBDN_ParseAndTraversePartial |              5 |           30 | 10.221 μs | 0.2226 μs | 0.3595 μs | 10.233 μs |  9.922 μs | 10.924 μs | 0.4120 |      - |   6.77 KB |
|                     PBDN_ParseAndTraverse_NonVirtual |              5 |           30 |  8.863 μs | 0.0877 μs | 0.1392 μs |  8.902 μs |  8.674 μs |  9.034 μs | 0.4120 |      - |   6.77 KB |
|              PBDN_ParseAndTraversePartial_NonVirtual |              5 |           30 |  8.599 μs | 0.0939 μs | 0.1517 μs |  8.540 μs |  8.511 μs |  8.906 μs | 0.4120 |      - |   6.77 KB |
|                             MsgPack_ParseAndTraverse |              5 |           30 |  5.247 μs | 0.1876 μs | 0.2921 μs |  5.140 μs |  5.099 μs |  5.909 μs | 0.4120 | 0.0076 |   6.74 KB |
|                      MsgPack_ParseAndTraversePartial |              5 |           30 |  5.175 μs | 0.1595 μs | 0.2576 μs |  5.011 μs |  4.981 μs |  5.518 μs | 0.4120 | 0.0076 |   6.74 KB |

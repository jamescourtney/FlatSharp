``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.17763.1757 (1809/October2018Update/Redstone5)
AMD EPYC 7452, 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.103
  [Host]   : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT
  ShortRun : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET Core 5.0  
IterationCount=7  LaunchCount=7  WarmupCount=5  

```
|                                               Method | TraversalCount | VectorLength |      Mean |     Error |    StdDev |    Median |       P25 |       P50 |       P67 |       P80 |       P90 |       P95 |
|----------------------------------------------------- |--------------- |------------- |----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|
|                  **Google_Flatbuffers_ParseAndTraverse** |              **1** |           **30** | **10.633 μs** | **0.0894 μs** | **0.1764 μs** | **10.630 μs** | **10.516 μs** | **10.630 μs** | **10.719 μs** | **10.796 μs** | **10.826 μs** | **10.888 μs** |
|           Google_Flatbuffers_ParseAndTraversePartial |              1 |           30 |  6.751 μs | 0.1249 μs | 0.2494 μs |  6.799 μs |  6.667 μs |  6.799 μs |  6.845 μs |  6.891 μs |  7.073 μs |  7.107 μs |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |              1 |           30 | 16.978 μs | 0.6699 μs | 1.3067 μs | 16.454 μs | 16.361 μs | 16.454 μs | 16.571 μs | 17.185 μs | 19.751 μs | 20.003 μs |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |              1 |           30 | 16.578 μs | 0.1534 μs | 0.3027 μs | 16.513 μs | 16.434 μs | 16.513 μs | 16.561 μs | 16.640 μs | 17.110 μs | 17.156 μs |
|                                PBDN_ParseAndTraverse |              1 |           30 | 27.564 μs | 0.2031 μs | 0.3913 μs | 27.430 μs | 27.381 μs | 27.430 μs | 27.540 μs | 27.768 μs | 28.160 μs | 28.382 μs |
|                         PBDN_ParseAndTraversePartial |              1 |           30 | 26.939 μs | 0.2457 μs | 0.4850 μs | 26.867 μs | 26.618 μs | 26.867 μs | 27.080 μs | 27.334 μs | 27.608 μs | 27.770 μs |
|                     PBDN_ParseAndTraverse_NonVirtual |              1 |           30 | 25.702 μs | 0.3225 μs | 0.6441 μs | 25.780 μs | 25.177 μs | 25.780 μs | 25.991 μs | 26.311 μs | 26.472 μs | 26.625 μs |
|              PBDN_ParseAndTraversePartial_NonVirtual |              1 |           30 | 24.851 μs | 0.2558 μs | 0.5109 μs | 24.870 μs | 24.455 μs | 24.870 μs | 25.010 μs | 25.313 μs | 25.606 μs | 25.724 μs |
|                             MsgPack_ParseAndTraverse |              1 |           30 | 11.255 μs | 0.1151 μs | 0.2272 μs | 11.222 μs | 11.095 μs | 11.222 μs | 11.357 μs | 11.471 μs | 11.527 μs | 11.641 μs |
|                      MsgPack_ParseAndTraversePartial |              1 |           30 | 11.273 μs | 0.2175 μs | 0.4241 μs | 11.175 μs | 10.958 μs | 11.175 μs | 11.388 μs | 11.440 μs | 11.755 μs | 11.930 μs |
|                  **Google_Flatbuffers_ParseAndTraverse** |              **5** |           **30** | **52.455 μs** | **0.3505 μs** | **0.6837 μs** | **52.473 μs** | **51.914 μs** | **52.473 μs** | **52.593 μs** | **53.104 μs** | **53.223 μs** | **53.370 μs** |
|           Google_Flatbuffers_ParseAndTraversePartial |              5 |           30 | 33.206 μs | 0.5018 μs | 0.9906 μs | 33.558 μs | 32.626 μs | 33.558 μs | 33.724 μs | 34.011 μs | 34.088 μs | 34.111 μs |
|        Google_Flatbuffers_ParseAndTraverse_ObjectApi |              5 |           30 | 17.372 μs | 0.2941 μs | 0.5666 μs | 17.403 μs | 16.863 μs | 17.403 μs | 17.653 μs | 17.834 μs | 18.225 μs | 18.403 μs |
| Google_Flatbuffers_ParseAndTraversePartial_ObjectApi |              5 |           30 | 16.981 μs | 0.2384 μs | 0.4593 μs | 16.863 μs | 16.703 μs | 16.863 μs | 16.959 μs | 17.043 μs | 17.580 μs | 17.696 μs |
|                                PBDN_ParseAndTraverse |              5 |           30 | 30.018 μs | 0.1587 μs | 0.3057 μs | 30.052 μs | 29.825 μs | 30.052 μs | 30.106 μs | 30.251 μs | 30.391 μs | 30.480 μs |
|                         PBDN_ParseAndTraversePartial |              5 |           30 | 28.026 μs | 0.2812 μs | 0.5485 μs | 28.071 μs | 27.804 μs | 28.071 μs | 28.204 μs | 28.349 μs | 28.675 μs | 28.922 μs |
|                     PBDN_ParseAndTraverse_NonVirtual |              5 |           30 | 26.022 μs | 0.3051 μs | 0.6022 μs | 26.105 μs | 25.452 μs | 26.105 μs | 26.358 μs | 26.536 μs | 26.756 μs | 26.862 μs |
|              PBDN_ParseAndTraversePartial_NonVirtual |              5 |           30 | 25.010 μs | 0.2176 μs | 0.4346 μs | 25.066 μs | 24.675 μs | 25.066 μs | 25.247 μs | 25.431 μs | 25.534 μs | 25.622 μs |
|                             MsgPack_ParseAndTraverse |              5 |           30 | 11.911 μs | 0.1564 μs | 0.3050 μs | 12.021 μs | 11.662 μs | 12.021 μs | 12.076 μs | 12.161 μs | 12.259 μs | 12.285 μs |
|                      MsgPack_ParseAndTraversePartial |              5 |           30 | 11.284 μs | 0.1721 μs | 0.3357 μs | 11.434 μs | 10.926 μs | 11.434 μs | 11.557 μs | 11.614 μs | 11.650 μs | 11.663 μs |

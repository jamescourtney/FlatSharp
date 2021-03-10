``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.17763.1757 (1809/October2018Update/Redstone5)
AMD EPYC 7452, 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.103
  [Host]   : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT
  ShortRun : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET Core 5.0  
IterationCount=7  LaunchCount=7  WarmupCount=5  

```
|                                       Method | TraversalCount |  DeserializeOption | VectorLength |      Mean |     Error |    StdDev |       P25 |       P50 |       P67 |       P80 |       P90 |       P95 |
|--------------------------------------------- |--------------- |------------------- |------------- |----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|
|                   **FlatSharp_ParseAndTraverse** |              **1** |               **Lazy** |           **30** |  **5.860 μs** | **0.0279 μs** | **0.0545 μs** |  **5.818 μs** |  **5.874 μs** |  **5.891 μs** |  **5.908 μs** |  **5.924 μs** |  **5.932 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 |               Lazy |           30 |  4.098 μs | 0.0915 μs | 0.1784 μs |  3.920 μs |  4.149 μs |  4.185 μs |  4.277 μs |  4.318 μs |  4.321 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |               Lazy |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |               Lazy |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
|                   **FlatSharp_ParseAndTraverse** |              **1** |      **PropertyCache** |           **30** |  **6.569 μs** | **0.0710 μs** | **0.1402 μs** |  **6.428 μs** |  **6.596 μs** |  **6.651 μs** |  **6.704 μs** |  **6.735 μs** |  **6.740 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 |      PropertyCache |           30 |  4.305 μs | 0.0639 μs | 0.1276 μs |  4.199 μs |  4.294 μs |  4.342 μs |  4.381 μs |  4.518 μs |  4.543 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |      PropertyCache |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |      PropertyCache |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
|                   **FlatSharp_ParseAndTraverse** |              **1** |        **VectorCache** |           **30** |  **7.249 μs** | **0.0422 μs** | **0.0824 μs** |  **7.191 μs** |  **7.230 μs** |  **7.263 μs** |  **7.300 μs** |  **7.347 μs** |  **7.419 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 |        VectorCache |           30 |  4.916 μs | 0.0384 μs | 0.0748 μs |  4.858 μs |  4.888 μs |  4.945 μs |  4.972 μs |  5.017 μs |  5.047 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |        VectorCache |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |        VectorCache |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
|                   **FlatSharp_ParseAndTraverse** |              **1** | **VectorCacheMutable** |           **30** |  **6.892 μs** | **0.0473 μs** | **0.0911 μs** |  **6.814 μs** |  **6.924 μs** |  **6.955 μs** |  **6.967 μs** |  **6.996 μs** |  **7.022 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 | VectorCacheMutable |           30 |  4.960 μs | 0.0366 μs | 0.0723 μs |  4.919 μs |  4.975 μs |  5.001 μs |  5.013 μs |  5.028 μs |  5.074 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 | VectorCacheMutable |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 | VectorCacheMutable |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
|                   **FlatSharp_ParseAndTraverse** |              **1** |             **Greedy** |           **30** |  **6.553 μs** | **0.0979 μs** | **0.1933 μs** |  **6.405 μs** |  **6.556 μs** |  **6.598 μs** |  **6.675 μs** |  **6.893 μs** |  **6.924 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 |             Greedy |           30 |  6.325 μs | 0.1089 μs | 0.2125 μs |  6.257 μs |  6.395 μs |  6.435 μs |  6.462 μs |  6.494 μs |  6.528 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |             Greedy |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |             Greedy |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
|                   **FlatSharp_ParseAndTraverse** |              **1** |      **GreedyMutable** |           **30** |  **6.562 μs** | **0.1177 μs** | **0.2240 μs** |  **6.508 μs** |  **6.569 μs** |  **6.694 μs** |  **6.719 μs** |  **6.768 μs** |  **6.871 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 |      GreedyMutable |           30 |  6.274 μs | 0.0823 μs | 0.1565 μs |  6.147 μs |  6.314 μs |  6.373 μs |  6.404 μs |  6.433 μs |  6.446 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |      GreedyMutable |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |      GreedyMutable |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
|                   **FlatSharp_ParseAndTraverse** |              **5** |               **Lazy** |           **30** | **29.322 μs** | **0.2208 μs** | **0.4254 μs** | **29.153 μs** | **29.431 μs** | **29.506 μs** | **29.633 μs** | **29.747 μs** | **29.855 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 |               Lazy |           30 | 19.936 μs | 0.4961 μs | 0.9319 μs | 19.019 μs | 19.832 μs | 20.731 μs | 20.923 μs | 21.033 μs | 21.095 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |               Lazy |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |               Lazy |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
|                   **FlatSharp_ParseAndTraverse** |              **5** |      **PropertyCache** |           **30** | **31.884 μs** | **0.3219 μs** | **0.6278 μs** | **31.501 μs** | **31.740 μs** | **31.839 μs** | **32.131 μs** | **32.574 μs** | **32.780 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 |      PropertyCache |           30 | 21.610 μs | 0.5661 μs | 1.1174 μs | 20.612 μs | 21.825 μs | 22.304 μs | 22.595 μs | 23.021 μs | 23.115 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |      PropertyCache |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |      PropertyCache |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
|                   **FlatSharp_ParseAndTraverse** |              **5** |        **VectorCache** |           **30** | **11.385 μs** | **0.1125 μs** | **0.2194 μs** | **11.262 μs** | **11.426 μs** | **11.484 μs** | **11.544 μs** | **11.664 μs** | **11.705 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 |        VectorCache |           30 |  6.874 μs | 0.0909 μs | 0.1752 μs |  6.720 μs |  6.942 μs |  6.977 μs |  7.021 μs |  7.036 μs |  7.059 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |        VectorCache |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |        VectorCache |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
|                   **FlatSharp_ParseAndTraverse** |              **5** | **VectorCacheMutable** |           **30** | **10.434 μs** | **0.0968 μs** | **0.1865 μs** | **10.309 μs** | **10.436 μs** | **10.544 μs** | **10.586 μs** | **10.683 μs** | **10.714 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 | VectorCacheMutable |           30 |  6.536 μs | 0.0730 μs | 0.1407 μs |  6.482 μs |  6.548 μs |  6.595 μs |  6.640 μs |  6.709 μs |  6.754 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 | VectorCacheMutable |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 | VectorCacheMutable |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
|                   **FlatSharp_ParseAndTraverse** |              **5** |             **Greedy** |           **30** |  **9.637 μs** | **0.1139 μs** | **0.2194 μs** |  **9.419 μs** |  **9.665 μs** |  **9.720 μs** |  **9.826 μs** |  **9.919 μs** |  **9.970 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 |             Greedy |           30 |  7.862 μs | 0.1279 μs | 0.2465 μs |  7.859 μs |  7.937 μs |  7.974 μs |  8.004 μs |  8.038 μs |  8.068 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |             Greedy |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |             Greedy |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
|                   **FlatSharp_ParseAndTraverse** |              **5** |      **GreedyMutable** |           **30** |  **9.033 μs** | **0.1548 μs** | **0.2983 μs** |  **8.782 μs** |  **9.131 μs** |  **9.203 μs** |  **9.274 μs** |  **9.340 μs** |  **9.382 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 |      GreedyMutable |           30 |  7.452 μs | 0.0936 μs | 0.1870 μs |  7.332 μs |  7.448 μs |  7.500 μs |  7.535 μs |  7.579 μs |  7.792 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |      GreedyMutable |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |      GreedyMutable |           30 |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |        NA |

Benchmarks with issues:
  FBDeserializeBench.FlatSharp_ParseAndTraverse_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=1, DeserializeOption=Lazy, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraversePartial_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=1, DeserializeOption=Lazy, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraverse_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=1, DeserializeOption=PropertyCache, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraversePartial_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=1, DeserializeOption=PropertyCache, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraverse_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=1, DeserializeOption=VectorCache, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraversePartial_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=1, DeserializeOption=VectorCache, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraverse_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=1, DeserializeOption=VectorCacheMutable, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraversePartial_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=1, DeserializeOption=VectorCacheMutable, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraverse_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=1, DeserializeOption=Greedy, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraversePartial_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=1, DeserializeOption=Greedy, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraverse_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=1, DeserializeOption=GreedyMutable, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraversePartial_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=1, DeserializeOption=GreedyMutable, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraverse_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=5, DeserializeOption=Lazy, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraversePartial_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=5, DeserializeOption=Lazy, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraverse_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=5, DeserializeOption=PropertyCache, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraversePartial_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=5, DeserializeOption=PropertyCache, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraverse_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=5, DeserializeOption=VectorCache, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraversePartial_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=5, DeserializeOption=VectorCache, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraverse_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=5, DeserializeOption=VectorCacheMutable, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraversePartial_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=5, DeserializeOption=VectorCacheMutable, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraverse_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=5, DeserializeOption=Greedy, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraversePartial_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=5, DeserializeOption=Greedy, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraverse_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=5, DeserializeOption=GreedyMutable, VectorLength=30]
  FBDeserializeBench.FlatSharp_ParseAndTraversePartial_NonVirtual: ShortRun(AnalyzeLaunchVariance=True, Runtime=.NET Core 5.0, IterationCount=7, LaunchCount=7, WarmupCount=5) [TraversalCount=5, DeserializeOption=GreedyMutable, VectorLength=30]

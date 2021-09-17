``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1237 (20H2/October2020Update)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=5.0.201
  [Host]   : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT
  ShortRun : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET 5.0  
IterationCount=5  LaunchCount=7  WarmupCount=3  

```
|                                       Method | TraversalCount |  DeserializeOption | VectorLength |      Mean |     Error |    StdDev |    Median |       P25 |       P95 |  Gen 0 |  Gen 1 | Allocated |
|--------------------------------------------- |--------------- |------------------- |------------- |----------:|----------:|----------:|----------:|----------:|----------:|-------:|-------:|----------:|
|                   **FlatSharp_ParseAndTraverse** |              **1** |               **Lazy** |           **30** |  **2.279 μs** | **0.0194 μs** | **0.0307 μs** |  **2.284 μs** |  **2.262 μs** |  **2.342 μs** | **0.4539** |      **-** |      **7 KB** |
|            FlatSharp_ParseAndTraversePartial |              1 |               Lazy |           30 |  1.656 μs | 0.0203 μs | 0.0315 μs |  1.645 μs |  1.636 μs |  1.715 μs | 0.4539 |      - |      7 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |               Lazy |           30 |  2.056 μs | 0.0082 μs | 0.0134 μs |  2.052 μs |  2.046 μs |  2.079 μs | 0.4539 |      - |      7 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |               Lazy |           30 |  2.244 μs | 0.1385 μs | 0.2197 μs |  2.125 μs |  2.054 μs |  2.673 μs | 0.4539 |      - |      7 KB |
|                   **FlatSharp_ParseAndTraverse** |              **1** |      **PropertyCache** |           **30** |  **2.669 μs** | **0.2018 μs** | **0.3259 μs** |  **2.543 μs** |  **2.527 μs** |  **3.443 μs** | **0.5836** |      **-** |     **10 KB** |
|            FlatSharp_ParseAndTraversePartial |              1 |      PropertyCache |           30 |  1.839 μs | 0.0091 μs | 0.0138 μs |  1.841 μs |  1.828 μs |  1.855 μs | 0.5856 |      - |     10 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |      PropertyCache |           30 |  2.179 μs | 0.0809 μs | 0.1329 μs |  2.085 μs |  2.069 μs |  2.371 μs | 0.4539 |      - |      7 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |      PropertyCache |           30 |  2.083 μs | 0.0258 μs | 0.0416 μs |  2.071 μs |  2.050 μs |  2.158 μs | 0.4539 |      - |      7 KB |
|                   **FlatSharp_ParseAndTraverse** |              **1** |        **VectorCache** |           **30** |  **2.912 μs** | **0.0375 μs** | **0.0584 μs** |  **2.923 μs** |  **2.852 μs** |  **2.987 μs** | **0.6027** | **0.0191** |     **10 KB** |
|            FlatSharp_ParseAndTraversePartial |              1 |        VectorCache |           30 |  2.307 μs | 0.0255 μs | 0.0411 μs |  2.303 μs |  2.290 μs |  2.393 μs | 0.6027 | 0.0191 |     10 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |        VectorCache |           30 |  2.529 μs | 0.0245 μs | 0.0374 μs |  2.528 μs |  2.499 μs |  2.601 μs | 0.4730 | 0.0114 |      8 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |        VectorCache |           30 |  2.517 μs | 0.0328 μs | 0.0529 μs |  2.501 μs |  2.485 μs |  2.637 μs | 0.4730 | 0.0114 |      8 KB |
|                   **FlatSharp_ParseAndTraverse** |              **1** | **VectorCacheMutable** |           **30** |  **3.061 μs** | **0.0559 μs** | **0.0887 μs** |  **3.055 μs** |  **2.986 μs** |  **3.211 μs** | **0.6027** | **0.0191** |     **10 KB** |
|            FlatSharp_ParseAndTraversePartial |              1 | VectorCacheMutable |           30 |  2.248 μs | 0.0255 μs | 0.0411 μs |  2.253 μs |  2.238 μs |  2.293 μs | 0.6027 | 0.0191 |     10 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 | VectorCacheMutable |           30 |  2.484 μs | 0.0406 μs | 0.0667 μs |  2.512 μs |  2.416 μs |  2.560 μs | 0.4692 | 0.0114 |      8 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 | VectorCacheMutable |           30 |  2.456 μs | 0.0311 μs | 0.0493 μs |  2.446 μs |  2.422 μs |  2.540 μs | 0.4692 | 0.0114 |      8 KB |
|                   **FlatSharp_ParseAndTraverse** |              **1** |             **Greedy** |           **30** |  **2.790 μs** | **0.0229 μs** | **0.0370 μs** |  **2.782 μs** |  **2.765 μs** |  **2.849 μs** | **0.5150** | **0.0153** |      **8 KB** |
|            FlatSharp_ParseAndTraversePartial |              1 |             Greedy |           30 |  2.513 μs | 0.0154 μs | 0.0249 μs |  2.516 μs |  2.500 μs |  2.543 μs | 0.5150 | 0.0153 |      8 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |             Greedy |           30 |  2.378 μs | 0.0311 μs | 0.0493 μs |  2.379 μs |  2.335 μs |  2.440 μs | 0.4158 | 0.0076 |      7 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |             Greedy |           30 |  2.342 μs | 0.0199 μs | 0.0322 μs |  2.330 μs |  2.319 μs |  2.406 μs | 0.4158 | 0.0076 |      7 KB |
|                   **FlatSharp_ParseAndTraverse** |              **1** |      **GreedyMutable** |           **30** |  **2.746 μs** | **0.0194 μs** | **0.0314 μs** |  **2.747 μs** |  **2.724 μs** |  **2.792 μs** | **0.5150** | **0.0153** |      **8 KB** |
|            FlatSharp_ParseAndTraversePartial |              1 |      GreedyMutable |           30 |  2.519 μs | 0.0369 μs | 0.0574 μs |  2.502 μs |  2.490 μs |  2.646 μs | 0.5150 | 0.0153 |      8 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |      GreedyMutable |           30 |  2.357 μs | 0.0392 μs | 0.0621 μs |  2.323 μs |  2.314 μs |  2.470 μs | 0.4120 | 0.0076 |      7 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |      GreedyMutable |           30 |  2.305 μs | 0.0195 μs | 0.0314 μs |  2.311 μs |  2.289 μs |  2.353 μs | 0.4120 | 0.0076 |      7 KB |
|                   **FlatSharp_ParseAndTraverse** |              **5** |               **Lazy** |           **30** | **12.296 μs** | **0.1890 μs** | **0.3105 μs** | **12.210 μs** | **12.150 μs** | **12.906 μs** | **2.2583** |      **-** |     **37 KB** |
|            FlatSharp_ParseAndTraversePartial |              5 |               Lazy |           30 |  8.784 μs | 0.0574 μs | 0.0893 μs |  8.753 μs |  8.733 μs |  8.993 μs | 2.2583 |      - |     37 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |               Lazy |           30 | 10.669 μs | 0.1187 μs | 0.1950 μs | 10.635 μs | 10.526 μs | 11.034 μs | 2.2278 |      - |     36 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |               Lazy |           30 | 10.991 μs | 0.3842 μs | 0.5867 μs | 10.700 μs | 10.533 μs | 12.187 μs | 2.2278 |      - |     36 KB |
|                   **FlatSharp_ParseAndTraverse** |              **5** |      **PropertyCache** |           **30** | **13.083 μs** | **0.0767 μs** | **0.1260 μs** | **13.040 μs** | **13.003 μs** | **13.282 μs** | **2.8687** |      **-** |     **47 KB** |
|            FlatSharp_ParseAndTraversePartial |              5 |      PropertyCache |           30 |  9.424 μs | 0.0760 μs | 0.1227 μs |  9.430 μs |  9.353 μs |  9.583 μs | 2.8687 |      - |     47 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |      PropertyCache |           30 | 10.905 μs | 0.1865 μs | 0.2958 μs | 10.839 μs | 10.782 μs | 11.659 μs | 2.2278 |      - |     36 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |      PropertyCache |           30 | 10.773 μs | 0.0642 μs | 0.1036 μs | 10.787 μs | 10.721 μs | 10.910 μs | 2.2278 |      - |     36 KB |
|                   **FlatSharp_ParseAndTraverse** |              **5** |        **VectorCache** |           **30** |  **5.268 μs** | **0.0379 μs** | **0.0601 μs** |  **5.255 μs** |  **5.225 μs** |  **5.386 μs** | **0.6027** | **0.0153** |     **10 KB** |
|            FlatSharp_ParseAndTraversePartial |              5 |        VectorCache |           30 |  3.515 μs | 0.0286 μs | 0.0453 μs |  3.513 μs |  3.475 μs |  3.575 μs | 0.6027 | 0.0191 |     10 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |        VectorCache |           30 |  2.957 μs | 0.0307 μs | 0.0487 μs |  2.969 μs |  2.930 μs |  3.026 μs | 0.4730 | 0.0114 |      8 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |        VectorCache |           30 |  2.928 μs | 0.0828 μs | 0.1336 μs |  2.865 μs |  2.814 μs |  3.156 μs | 0.4730 | 0.0114 |      8 KB |
|                   **FlatSharp_ParseAndTraverse** |              **5** | **VectorCacheMutable** |           **30** |  **5.237 μs** | **0.0615 μs** | **0.0976 μs** |  **5.257 μs** |  **5.200 μs** |  **5.377 μs** | **0.6027** | **0.0153** |     **10 KB** |
|            FlatSharp_ParseAndTraversePartial |              5 | VectorCacheMutable |           30 |  3.359 μs | 0.0249 μs | 0.0395 μs |  3.350 μs |  3.331 μs |  3.427 μs | 0.6027 | 0.0191 |     10 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 | VectorCacheMutable |           30 |  2.750 μs | 0.0113 μs | 0.0179 μs |  2.754 μs |  2.738 μs |  2.783 μs | 0.4692 | 0.0114 |      8 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 | VectorCacheMutable |           30 |  2.723 μs | 0.0330 μs | 0.0514 μs |  2.736 μs |  2.679 μs |  2.797 μs | 0.4692 | 0.0114 |      8 KB |
|                   **FlatSharp_ParseAndTraverse** |              **5** |             **Greedy** |           **30** |  **4.861 μs** | **0.0650 μs** | **0.1050 μs** |  **4.866 μs** |  **4.808 μs** |  **5.016 μs** | **0.5112** | **0.0153** |      **8 KB** |
|            FlatSharp_ParseAndTraversePartial |              5 |             Greedy |           30 |  3.457 μs | 0.0492 μs | 0.0808 μs |  3.446 μs |  3.420 μs |  3.570 μs | 0.5150 | 0.0153 |      8 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |             Greedy |           30 |  2.876 μs | 0.0394 μs | 0.0636 μs |  2.846 μs |  2.836 μs |  2.970 μs | 0.4158 | 0.0076 |      7 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |             Greedy |           30 |  2.652 μs | 0.0322 μs | 0.0510 μs |  2.672 μs |  2.621 μs |  2.717 μs | 0.4158 | 0.0076 |      7 KB |
|                   **FlatSharp_ParseAndTraverse** |              **5** |      **GreedyMutable** |           **30** |  **4.579 μs** | **0.0556 μs** | **0.0914 μs** |  **4.599 μs** |  **4.554 μs** |  **4.685 μs** | **0.5112** | **0.0153** |      **8 KB** |
|            FlatSharp_ParseAndTraversePartial |              5 |      GreedyMutable |           30 |  3.392 μs | 0.0256 μs | 0.0421 μs |  3.397 μs |  3.377 μs |  3.450 μs | 0.5150 | 0.0153 |      8 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |      GreedyMutable |           30 |  2.796 μs | 0.1362 μs | 0.2199 μs |  2.703 μs |  2.655 μs |  3.259 μs | 0.4120 | 0.0076 |      7 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |      GreedyMutable |           30 |  2.537 μs | 0.0199 μs | 0.0321 μs |  2.551 μs |  2.512 μs |  2.578 μs | 0.4120 | 0.0076 |      7 KB |

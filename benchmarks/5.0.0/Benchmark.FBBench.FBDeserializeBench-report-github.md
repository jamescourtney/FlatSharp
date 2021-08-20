``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.17763.1757 (1809/October2018Update/Redstone5)
AMD EPYC 7452, 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.103
  [Host]   : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT
  ShortRun : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET Core 5.0  
IterationCount=7  LaunchCount=7  WarmupCount=5  

```
|                                       Method | TraversalCount |  DeserializeOption | VectorLength |      Mean |     Error |    StdDev |    Median |       P25 |       P50 |       P67 |       P80 |       P90 |       P95 |
|--------------------------------------------- |--------------- |------------------- |------------- |----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|
|                   **FlatSharp_ParseAndTraverse** |              **1** |               **Lazy** |           **30** |  **4.739 μs** | **0.1008 μs** | **0.1942 μs** |  **4.711 μs** |  **4.600 μs** |  **4.711 μs** |  **4.766 μs** |  **4.779 μs** |  **4.899 μs** |  **5.169 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 |               Lazy |           30 |  3.388 μs | 0.0556 μs | 0.1098 μs |  3.415 μs |  3.285 μs |  3.415 μs |  3.450 μs |  3.477 μs |  3.514 μs |  3.556 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |               Lazy |           30 |  4.091 μs | 0.0938 μs | 0.1830 μs |  4.163 μs |  3.935 μs |  4.163 μs |  4.205 μs |  4.222 μs |  4.268 μs |  4.280 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |               Lazy |           30 |  4.305 μs | 0.2886 μs | 0.5421 μs |  4.079 μs |  3.952 μs |  4.079 μs |  4.460 μs |  4.616 μs |  4.782 μs |  5.147 μs |
|                   **FlatSharp_ParseAndTraverse** |              **1** |      **PropertyCache** |           **30** |  **5.194 μs** | **0.0733 μs** | **0.1378 μs** |  **5.146 μs** |  **5.098 μs** |  **5.146 μs** |  **5.212 μs** |  **5.304 μs** |  **5.341 μs** |  **5.490 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 |      PropertyCache |           30 |  4.141 μs | 0.1744 μs | 0.3402 μs |  4.074 μs |  3.863 μs |  4.074 μs |  4.164 μs |  4.472 μs |  4.669 μs |  4.805 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |      PropertyCache |           30 |  4.482 μs | 0.1064 μs | 0.2075 μs |  4.485 μs |  4.295 μs |  4.485 μs |  4.598 μs |  4.675 μs |  4.751 μs |  4.794 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |      PropertyCache |           30 |  4.337 μs | 0.0861 μs | 0.1638 μs |  4.349 μs |  4.236 μs |  4.349 μs |  4.401 μs |  4.507 μs |  4.543 μs |  4.567 μs |
|                   **FlatSharp_ParseAndTraverse** |              **1** |        **VectorCache** |           **30** |  **6.156 μs** | **0.0650 μs** | **0.1268 μs** |  **6.152 μs** |  **6.068 μs** |  **6.152 μs** |  **6.201 μs** |  **6.224 μs** |  **6.272 μs** |  **6.409 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 |        VectorCache |           30 |  4.796 μs | 0.1375 μs | 0.2682 μs |  4.788 μs |  4.681 μs |  4.788 μs |  4.976 μs |  5.008 μs |  5.130 μs |  5.199 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |        VectorCache |           30 |  5.186 μs | 0.1140 μs | 0.2276 μs |  5.176 μs |  5.021 μs |  5.176 μs |  5.223 μs |  5.442 μs |  5.462 μs |  5.599 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |        VectorCache |           30 |  5.023 μs | 0.0828 μs | 0.1653 μs |  4.987 μs |  4.892 μs |  4.987 μs |  5.083 μs |  5.174 μs |  5.219 μs |  5.317 μs |
|                   **FlatSharp_ParseAndTraverse** |              **1** | **VectorCacheMutable** |           **30** |  **5.792 μs** | **0.1192 μs** | **0.2325 μs** |  **5.861 μs** |  **5.548 μs** |  **5.861 μs** |  **5.939 μs** |  **6.009 μs** |  **6.072 μs** |  **6.115 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 | VectorCacheMutable |           30 |  4.254 μs | 0.0252 μs | 0.0486 μs |  4.253 μs |  4.214 μs |  4.253 μs |  4.280 μs |  4.299 μs |  4.315 μs |  4.334 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 | VectorCacheMutable |           30 |  4.566 μs | 0.0539 μs | 0.1026 μs |  4.597 μs |  4.486 μs |  4.597 μs |  4.626 μs |  4.648 μs |  4.687 μs |  4.697 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 | VectorCacheMutable |           30 |  4.494 μs | 0.0681 μs | 0.1313 μs |  4.500 μs |  4.401 μs |  4.500 μs |  4.542 μs |  4.562 μs |  4.694 μs |  4.724 μs |
|                   **FlatSharp_ParseAndTraverse** |              **1** |             **Greedy** |           **30** |  **5.052 μs** | **0.1187 μs** | **0.2314 μs** |  **5.016 μs** |  **4.904 μs** |  **5.016 μs** |  **5.097 μs** |  **5.223 μs** |  **5.294 μs** |  **5.369 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 |             Greedy |           30 |  4.747 μs | 0.1225 μs | 0.2360 μs |  4.745 μs |  4.556 μs |  4.745 μs |  4.814 μs |  4.883 μs |  5.130 μs |  5.206 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |             Greedy |           30 |  4.661 μs | 0.2336 μs | 0.4557 μs |  4.577 μs |  4.409 μs |  4.577 μs |  4.640 μs |  4.736 μs |  4.867 μs |  5.630 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |             Greedy |           30 |  4.417 μs | 0.0563 μs | 0.1085 μs |  4.398 μs |  4.346 μs |  4.398 μs |  4.447 μs |  4.509 μs |  4.555 μs |  4.594 μs |
|                   **FlatSharp_ParseAndTraverse** |              **1** |      **GreedyMutable** |           **30** |  **4.836 μs** | **0.0444 μs** | **0.0834 μs** |  **4.819 μs** |  **4.782 μs** |  **4.819 μs** |  **4.853 μs** |  **4.897 μs** |  **4.944 μs** |  **4.990 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 |      GreedyMutable |           30 |  4.458 μs | 0.1238 μs | 0.2444 μs |  4.388 μs |  4.326 μs |  4.388 μs |  4.457 μs |  4.540 μs |  4.640 μs |  4.912 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |      GreedyMutable |           30 |  4.394 μs | 0.0441 μs | 0.0849 μs |  4.383 μs |  4.339 μs |  4.383 μs |  4.431 μs |  4.476 μs |  4.515 μs |  4.534 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |      GreedyMutable |           30 |  4.429 μs | 0.0247 μs | 0.0488 μs |  4.424 μs |  4.394 μs |  4.424 μs |  4.449 μs |  4.458 μs |  4.488 μs |  4.509 μs |
|                   **FlatSharp_ParseAndTraverse** |              **5** |               **Lazy** |           **30** | **22.766 μs** | **0.2994 μs** | **0.5697 μs** | **22.940 μs** | **22.267 μs** | **22.940 μs** | **23.145 μs** | **23.228 μs** | **23.311 μs** | **23.394 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 |               Lazy |           30 | 16.858 μs | 0.4354 μs | 0.8285 μs | 16.971 μs | 15.843 μs | 16.971 μs | 17.330 μs | 17.427 μs | 17.627 μs | 17.869 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |               Lazy |           30 | 19.588 μs | 0.3584 μs | 0.6905 μs | 19.918 μs | 19.111 μs | 19.918 μs | 20.044 μs | 20.150 μs | 20.277 μs | 20.346 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |               Lazy |           30 | 19.919 μs | 0.4058 μs | 0.7818 μs | 19.954 μs | 19.161 μs | 19.954 μs | 20.534 μs | 20.668 μs | 20.735 μs | 20.830 μs |
|                   **FlatSharp_ParseAndTraverse** |              **5** |      **PropertyCache** |           **30** | **25.419 μs** | **0.3172 μs** | **0.6111 μs** | **25.430 μs** | **24.820 μs** | **25.430 μs** | **25.744 μs** | **25.894 μs** | **26.145 μs** | **26.432 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 |      PropertyCache |           30 | 18.959 μs | 0.1542 μs | 0.3043 μs | 19.017 μs | 18.721 μs | 19.017 μs | 19.130 μs | 19.225 μs | 19.307 μs | 19.385 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |      PropertyCache |           30 | 19.624 μs | 0.2588 μs | 0.5048 μs | 19.605 μs | 19.312 μs | 19.605 μs | 19.771 μs | 19.870 μs | 20.511 μs | 20.558 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |      PropertyCache |           30 | 19.770 μs | 0.3302 μs | 0.6362 μs | 19.524 μs | 19.308 μs | 19.524 μs | 20.021 μs | 20.411 μs | 20.745 μs | 20.882 μs |
|                   **FlatSharp_ParseAndTraverse** |              **5** |        **VectorCache** |           **30** |  **9.670 μs** | **0.1185 μs** | **0.2226 μs** |  **9.701 μs** |  **9.479 μs** |  **9.701 μs** |  **9.801 μs** |  **9.896 μs** |  **9.919 μs** |  **9.934 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 |        VectorCache |           30 |  6.296 μs | 0.1131 μs | 0.2206 μs |  6.234 μs |  6.156 μs |  6.234 μs |  6.285 μs |  6.354 μs |  6.722 μs |  6.807 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |        VectorCache |           30 |  5.535 μs | 0.1009 μs | 0.1870 μs |  5.467 μs |  5.394 μs |  5.467 μs |  5.585 μs |  5.760 μs |  5.799 μs |  5.806 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |        VectorCache |           30 |  5.413 μs | 0.0982 μs | 0.1893 μs |  5.434 μs |  5.341 μs |  5.434 μs |  5.456 μs |  5.480 μs |  5.503 μs |  5.556 μs |
|                   **FlatSharp_ParseAndTraverse** |              **5** | **VectorCacheMutable** |           **30** |  **9.050 μs** | **0.0627 μs** | **0.1209 μs** |  **9.072 μs** |  **8.980 μs** |  **9.072 μs** |  **9.119 μs** |  **9.152 μs** |  **9.186 μs** |  **9.209 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 | VectorCacheMutable |           30 |  5.902 μs | 0.0729 μs | 0.1387 μs |  5.889 μs |  5.815 μs |  5.889 μs |  5.918 μs |  5.939 μs |  5.975 μs |  6.016 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 | VectorCacheMutable |           30 |  5.039 μs | 0.0748 μs | 0.1459 μs |  5.024 μs |  4.959 μs |  5.024 μs |  5.096 μs |  5.156 μs |  5.253 μs |  5.269 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 | VectorCacheMutable |           30 |  4.966 μs | 0.0501 μs | 0.0964 μs |  4.964 μs |  4.908 μs |  4.964 μs |  5.005 μs |  5.040 μs |  5.110 μs |  5.126 μs |
|                   **FlatSharp_ParseAndTraverse** |              **5** |             **Greedy** |           **30** |  **7.928 μs** | **0.0664 μs** | **0.1280 μs** |  **7.940 μs** |  **7.856 μs** |  **7.940 μs** |  **7.983 μs** |  **8.036 μs** |  **8.073 μs** |  **8.113 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 |             Greedy |           30 |  6.116 μs | 0.1089 μs | 0.2098 μs |  6.021 μs |  5.958 μs |  6.021 μs |  6.156 μs |  6.362 μs |  6.426 μs |  6.466 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |             Greedy |           30 |  5.272 μs | 0.0727 μs | 0.1418 μs |  5.247 μs |  5.195 μs |  5.247 μs |  5.294 μs |  5.351 μs |  5.415 μs |  5.490 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |             Greedy |           30 |  5.128 μs | 0.0966 μs | 0.1883 μs |  5.077 μs |  4.976 μs |  5.077 μs |  5.229 μs |  5.311 μs |  5.385 μs |  5.432 μs |
|                   **FlatSharp_ParseAndTraverse** |              **5** |      **GreedyMutable** |           **30** |  **7.482 μs** | **0.1085 μs** | **0.2090 μs** |  **7.428 μs** |  **7.341 μs** |  **7.428 μs** |  **7.495 μs** |  **7.613 μs** |  **7.792 μs** |  **7.938 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 |      GreedyMutable |           30 |  5.618 μs | 0.0610 μs | 0.1160 μs |  5.599 μs |  5.562 μs |  5.599 μs |  5.643 μs |  5.663 μs |  5.715 μs |  5.810 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |      GreedyMutable |           30 |  4.935 μs | 0.0639 μs | 0.1261 μs |  4.918 μs |  4.848 μs |  4.918 μs |  4.984 μs |  5.048 μs |  5.127 μs |  5.146 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |      GreedyMutable |           30 |  4.770 μs | 0.0647 μs | 0.1291 μs |  4.753 μs |  4.709 μs |  4.753 μs |  4.772 μs |  4.810 μs |  4.852 μs |  4.865 μs |

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
|                   **FlatSharp_ParseAndTraverse** |              **1** |               **Lazy** |           **30** |  **2.082 μs** | **0.0308 μs** | **0.0488 μs** |  **2.079 μs** |  **2.020 μs** |  **2.157 μs** | **0.4692** |      **-** |      **8 KB** |
|            FlatSharp_ParseAndTraversePartial |              1 |               Lazy |           30 |  1.599 μs | 0.0169 μs | 0.0268 μs |  1.583 μs |  1.579 μs |  1.649 μs | 0.4692 |      - |      8 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |               Lazy |           30 |  1.763 μs | 0.0251 μs | 0.0383 μs |  1.744 μs |  1.736 μs |  1.839 μs | 0.4692 |      - |      8 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |               Lazy |           30 |  1.747 μs | 0.0131 μs | 0.0200 μs |  1.745 μs |  1.733 μs |  1.792 μs | 0.4692 |      - |      8 KB |
|                   **FlatSharp_ParseAndTraverse** |              **1** |      **PropertyCache** |           **30** |  **2.347 μs** | **0.0217 μs** | **0.0345 μs** |  **2.339 μs** |  **2.321 μs** |  **2.425 μs** | **0.5989** |      **-** |     **10 KB** |
|            FlatSharp_ParseAndTraversePartial |              1 |      PropertyCache |           30 |  1.770 μs | 0.0175 μs | 0.0267 μs |  1.771 μs |  1.755 μs |  1.827 μs | 0.5989 |      - |     10 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |      PropertyCache |           30 |  1.873 μs | 0.0898 μs | 0.1371 μs |  1.808 μs |  1.788 μs |  2.153 μs | 0.4692 |      - |      8 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |      PropertyCache |           30 |  1.768 μs | 0.0245 μs | 0.0381 μs |  1.770 μs |  1.730 μs |  1.821 μs | 0.4692 |      - |      8 KB |
|                   **FlatSharp_ParseAndTraverse** |              **1** |        **VectorCache** |           **30** |  **2.617 μs** | **0.0570 μs** | **0.0904 μs** |  **2.624 μs** |  **2.527 μs** |  **2.786 μs** | **0.6180** | **0.0191** |     **10 KB** |
|            FlatSharp_ParseAndTraversePartial |              1 |        VectorCache |           30 |  2.025 μs | 0.0113 μs | 0.0179 μs |  2.028 μs |  2.009 μs |  2.056 μs | 0.6180 | 0.0191 |     10 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |        VectorCache |           30 |  2.097 μs | 0.0321 μs | 0.0509 μs |  2.075 μs |  2.057 μs |  2.186 μs | 0.4883 | 0.0114 |      8 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |        VectorCache |           30 |  2.048 μs | 0.0232 μs | 0.0374 μs |  2.051 μs |  2.020 μs |  2.118 μs | 0.4883 | 0.0114 |      8 KB |
|                   **FlatSharp_ParseAndTraverse** |              **1** | **VectorCacheMutable** |           **30** |  **2.743 μs** | **0.1712 μs** | **0.2665 μs** |  **2.639 μs** |  **2.558 μs** |  **3.268 μs** | **0.6180** | **0.0191** |     **10 KB** |
|            FlatSharp_ParseAndTraversePartial |              1 | VectorCacheMutable |           30 |  2.176 μs | 0.1459 μs | 0.2357 μs |  2.082 μs |  2.050 μs |  2.681 μs | 0.6180 | 0.0191 |     10 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 | VectorCacheMutable |           30 |  2.342 μs | 0.1549 μs | 0.2412 μs |  2.254 μs |  2.180 μs |  2.809 μs | 0.4845 | 0.0114 |      8 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 | VectorCacheMutable |           30 |  2.048 μs | 0.0764 μs | 0.1212 μs |  2.003 μs |  1.972 μs |  2.332 μs | 0.4845 | 0.0114 |      8 KB |
|                   **FlatSharp_ParseAndTraverse** |              **1** |             **Greedy** |           **30** |  **2.317 μs** | **0.0192 μs** | **0.0311 μs** |  **2.315 μs** |  **2.296 μs** |  **2.357 μs** | **0.5150** | **0.0153** |      **8 KB** |
|            FlatSharp_ParseAndTraversePartial |              1 |             Greedy |           30 |  2.058 μs | 0.0163 μs | 0.0259 μs |  2.056 μs |  2.039 μs |  2.097 μs | 0.5150 | 0.0153 |      8 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |             Greedy |           30 |  1.882 μs | 0.0268 μs | 0.0409 μs |  1.878 μs |  1.850 μs |  1.941 μs | 0.4158 | 0.0095 |      7 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |             Greedy |           30 |  1.843 μs | 0.0154 μs | 0.0244 μs |  1.845 μs |  1.818 μs |  1.878 μs | 0.4158 | 0.0095 |      7 KB |
|                   **FlatSharp_ParseAndTraverse** |              **1** |      **GreedyMutable** |           **30** |  **2.375 μs** | **0.0621 μs** | **0.0967 μs** |  **2.387 μs** |  **2.257 μs** |  **2.488 μs** | **0.5150** | **0.0153** |      **8 KB** |
|            FlatSharp_ParseAndTraversePartial |              1 |      GreedyMutable |           30 |  2.067 μs | 0.0454 μs | 0.0720 μs |  2.019 μs |  2.003 μs |  2.160 μs | 0.5150 | 0.0153 |      8 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |      GreedyMutable |           30 |  1.917 μs | 0.0981 μs | 0.1468 μs |  1.819 μs |  1.812 μs |  2.221 μs | 0.4120 | 0.0076 |      7 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |      GreedyMutable |           30 |  1.867 μs | 0.0725 μs | 0.1129 μs |  1.809 μs |  1.781 μs |  2.065 μs | 0.4139 | 0.0095 |      7 KB |
|                   **FlatSharp_ParseAndTraverse** |              **5** |               **Lazy** |           **30** | **10.606 μs** | **0.3032 μs** | **0.4808 μs** | **10.398 μs** | **10.275 μs** | **11.470 μs** | **2.3346** |      **-** |     **38 KB** |
|            FlatSharp_ParseAndTraversePartial |              5 |               Lazy |           30 |  7.869 μs | 0.0848 μs | 0.1346 μs |  7.897 μs |  7.751 μs |  8.024 μs | 2.3346 |      - |     38 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |               Lazy |           30 |  8.566 μs | 0.2680 μs | 0.4173 μs |  8.412 μs |  8.352 μs |  9.635 μs | 2.3041 |      - |     38 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |               Lazy |           30 |  8.562 μs | 0.3824 μs | 0.6175 μs |  8.291 μs |  8.229 μs |  9.993 μs | 2.3041 |      - |     38 KB |
|                   **FlatSharp_ParseAndTraverse** |              **5** |      **PropertyCache** |           **30** | **11.901 μs** | **0.6931 μs** | **1.0790 μs** | **11.354 μs** | **11.071 μs** | **14.175 μs** | **2.9449** |      **-** |     **48 KB** |
|            FlatSharp_ParseAndTraversePartial |              5 |      PropertyCache |           30 |  8.314 μs | 0.0302 μs | 0.0478 μs |  8.321 μs |  8.274 μs |  8.379 μs | 2.9449 |      - |     48 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |      PropertyCache |           30 |  8.552 μs | 0.1622 μs | 0.2525 μs |  8.586 μs |  8.340 μs |  9.101 μs | 2.3041 |      - |     38 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |      PropertyCache |           30 |  8.829 μs | 0.3986 μs | 0.6323 μs |  8.588 μs |  8.493 μs | 10.195 μs | 2.3041 |      - |     38 KB |
|                   **FlatSharp_ParseAndTraverse** |              **5** |        **VectorCache** |           **30** |  **4.835 μs** | **0.0500 μs** | **0.0778 μs** |  **4.844 μs** |  **4.798 μs** |  **4.943 μs** | **0.6180** | **0.0153** |     **10 KB** |
|            FlatSharp_ParseAndTraversePartial |              5 |        VectorCache |           30 |  3.172 μs | 0.0477 μs | 0.0742 μs |  3.164 μs |  3.111 μs |  3.325 μs | 0.6180 | 0.0191 |     10 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |        VectorCache |           30 |  2.443 μs | 0.0206 μs | 0.0321 μs |  2.436 μs |  2.409 μs |  2.493 μs | 0.4883 | 0.0114 |      8 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |        VectorCache |           30 |  2.438 μs | 0.0788 μs | 0.1250 μs |  2.416 μs |  2.314 μs |  2.673 μs | 0.4883 | 0.0114 |      8 KB |
|                   **FlatSharp_ParseAndTraverse** |              **5** | **VectorCacheMutable** |           **30** |  **4.802 μs** | **0.1925 μs** | **0.3109 μs** |  **4.645 μs** |  **4.595 μs** |  **5.358 μs** | **0.6180** | **0.0153** |     **10 KB** |
|            FlatSharp_ParseAndTraversePartial |              5 | VectorCacheMutable |           30 |  3.002 μs | 0.0292 μs | 0.0446 μs |  3.012 μs |  2.957 μs |  3.070 μs | 0.6180 | 0.0191 |     10 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 | VectorCacheMutable |           30 |  2.271 μs | 0.0387 μs | 0.0626 μs |  2.230 μs |  2.223 μs |  2.379 μs | 0.4845 | 0.0114 |      8 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 | VectorCacheMutable |           30 |  2.206 μs | 0.0319 μs | 0.0514 μs |  2.221 μs |  2.197 μs |  2.248 μs | 0.4845 | 0.0114 |      8 KB |
|                   **FlatSharp_ParseAndTraverse** |              **5** |             **Greedy** |           **30** |  **4.285 μs** | **0.0489 μs** | **0.0789 μs** |  **4.283 μs** |  **4.249 μs** |  **4.416 μs** | **0.5112** | **0.0153** |      **8 KB** |
|            FlatSharp_ParseAndTraversePartial |              5 |             Greedy |           30 |  2.980 μs | 0.0367 μs | 0.0571 μs |  2.979 μs |  2.958 μs |  3.052 μs | 0.5150 | 0.0153 |      8 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |             Greedy |           30 |  2.272 μs | 0.0087 μs | 0.0138 μs |  2.271 μs |  2.260 μs |  2.293 μs | 0.4158 | 0.0076 |      7 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |             Greedy |           30 |  2.202 μs | 0.0199 μs | 0.0310 μs |  2.201 μs |  2.186 μs |  2.271 μs | 0.4158 | 0.0076 |      7 KB |
|                   **FlatSharp_ParseAndTraverse** |              **5** |      **GreedyMutable** |           **30** |  **4.062 μs** | **0.1021 μs** | **0.1649 μs** |  **4.029 μs** |  **3.964 μs** |  **4.311 μs** | **0.5112** | **0.0153** |      **8 KB** |
|            FlatSharp_ParseAndTraversePartial |              5 |      GreedyMutable |           30 |  2.929 μs | 0.0586 μs | 0.0963 μs |  2.917 μs |  2.860 μs |  3.124 μs | 0.5150 | 0.0153 |      8 KB |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |      GreedyMutable |           30 |  2.217 μs | 0.0544 μs | 0.0878 μs |  2.221 μs |  2.150 μs |  2.381 μs | 0.4120 | 0.0076 |      7 KB |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |      GreedyMutable |           30 |  2.036 μs | 0.0513 μs | 0.0813 μs |  1.988 μs |  1.982 μs |  2.176 μs | 0.4120 | 0.0076 |      7 KB |

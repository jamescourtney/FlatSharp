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
|                   **FlatSharp_ParseAndTraverse** |              **1** |               **Lazy** |           **30** |  **5.038 μs** | **0.0320 μs** | **0.0616 μs** |  **5.040 μs** |  **4.990 μs** |  **5.040 μs** |  **5.076 μs** |  **5.095 μs** |  **5.113 μs** |  **5.119 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 |               Lazy |           30 |  3.656 μs | 0.0625 μs | 0.1220 μs |  3.632 μs |  3.566 μs |  3.632 μs |  3.705 μs |  3.750 μs |  3.792 μs |  3.828 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |               Lazy |           30 |  4.836 μs | 0.2284 μs | 0.4456 μs |  4.705 μs |  4.613 μs |  4.705 μs |  4.851 μs |  4.958 μs |  5.237 μs |  5.972 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |               Lazy |           30 |  4.759 μs | 0.1574 μs | 0.2917 μs |  4.799 μs |  4.720 μs |  4.799 μs |  4.886 μs |  5.027 μs |  5.050 μs |  5.070 μs |
|                   **FlatSharp_ParseAndTraverse** |              **1** |      **PropertyCache** |           **30** |  **5.660 μs** | **0.1176 μs** | **0.2237 μs** |  **5.680 μs** |  **5.441 μs** |  **5.680 μs** |  **5.778 μs** |  **5.824 μs** |  **5.870 μs** |  **5.978 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 |      PropertyCache |           30 |  3.962 μs | 0.0534 μs | 0.1016 μs |  4.005 μs |  3.929 μs |  4.005 μs |  4.018 μs |  4.036 μs |  4.046 μs |  4.058 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |      PropertyCache |           30 |  4.851 μs | 0.0899 μs | 0.1753 μs |  4.812 μs |  4.685 μs |  4.812 μs |  4.943 μs |  5.035 μs |  5.099 μs |  5.125 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |      PropertyCache |           30 |  4.785 μs | 0.0683 μs | 0.1300 μs |  4.824 μs |  4.680 μs |  4.824 μs |  4.887 μs |  4.903 μs |  4.926 μs |  4.937 μs |
|                   **FlatSharp_ParseAndTraverse** |              **1** |        **VectorCache** |           **30** |  **6.169 μs** | **0.0883 μs** | **0.1702 μs** |  **6.150 μs** |  **6.084 μs** |  **6.150 μs** |  **6.187 μs** |  **6.262 μs** |  **6.363 μs** |  **6.475 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 |        VectorCache |           30 |  4.498 μs | 0.1169 μs | 0.2251 μs |  4.471 μs |  4.319 μs |  4.471 μs |  4.534 μs |  4.734 μs |  4.829 μs |  4.855 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |        VectorCache |           30 |  5.280 μs | 0.0400 μs | 0.0750 μs |  5.293 μs |  5.257 μs |  5.293 μs |  5.319 μs |  5.337 μs |  5.356 μs |  5.359 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |        VectorCache |           30 |  5.314 μs | 0.0633 μs | 0.1203 μs |  5.315 μs |  5.212 μs |  5.315 μs |  5.364 μs |  5.400 μs |  5.442 μs |  5.470 μs |
|                   **FlatSharp_ParseAndTraverse** |              **1** | **VectorCacheMutable** |           **30** |  **6.089 μs** | **0.0442 μs** | **0.0862 μs** |  **6.078 μs** |  **6.014 μs** |  **6.078 μs** |  **6.125 μs** |  **6.146 μs** |  **6.213 μs** |  **6.237 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 | VectorCacheMutable |           30 |  4.337 μs | 0.0465 μs | 0.0863 μs |  4.332 μs |  4.240 μs |  4.332 μs |  4.392 μs |  4.428 μs |  4.447 μs |  4.459 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 | VectorCacheMutable |           30 |  5.108 μs | 0.0598 μs | 0.1138 μs |  5.082 μs |  5.010 μs |  5.082 μs |  5.204 μs |  5.229 μs |  5.250 μs |  5.266 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 | VectorCacheMutable |           30 |  5.187 μs | 0.0804 μs | 0.1568 μs |  5.181 μs |  5.082 μs |  5.181 μs |  5.204 μs |  5.304 μs |  5.405 μs |  5.516 μs |
|                   **FlatSharp_ParseAndTraverse** |              **1** |             **Greedy** |           **30** |  **5.584 μs** | **0.0755 μs** | **0.1455 μs** |  **5.630 μs** |  **5.473 μs** |  **5.630 μs** |  **5.662 μs** |  **5.695 μs** |  **5.737 μs** |  **5.758 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 |             Greedy |           30 |  5.107 μs | 0.0837 μs | 0.1612 μs |  5.163 μs |  5.000 μs |  5.163 μs |  5.218 μs |  5.238 μs |  5.268 μs |  5.301 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |             Greedy |           30 |  4.897 μs | 0.1124 μs | 0.2138 μs |  4.851 μs |  4.773 μs |  4.851 μs |  4.906 μs |  4.965 μs |  5.319 μs |  5.356 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |             Greedy |           30 |  4.806 μs | 0.0421 μs | 0.0801 μs |  4.810 μs |  4.759 μs |  4.810 μs |  4.837 μs |  4.888 μs |  4.902 μs |  4.908 μs |
|                   **FlatSharp_ParseAndTraverse** |              **1** |      **GreedyMutable** |           **30** |  **5.558 μs** | **0.0834 μs** | **0.1607 μs** |  **5.506 μs** |  **5.469 μs** |  **5.506 μs** |  **5.629 μs** |  **5.701 μs** |  **5.790 μs** |  **5.834 μs** |
|            FlatSharp_ParseAndTraversePartial |              1 |      GreedyMutable |           30 |  5.076 μs | 0.0944 μs | 0.1820 μs |  5.003 μs |  4.922 μs |  5.003 μs |  5.052 μs |  5.298 μs |  5.369 μs |  5.373 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              1 |      GreedyMutable |           30 |  4.922 μs | 0.0878 μs | 0.1733 μs |  4.857 μs |  4.775 μs |  4.857 μs |  4.998 μs |  5.120 μs |  5.177 μs |  5.202 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |      GreedyMutable |           30 |  4.808 μs | 0.1120 μs | 0.2237 μs |  4.685 μs |  4.613 μs |  4.685 μs |  4.917 μs |  5.096 μs |  5.125 μs |  5.152 μs |
|                   **FlatSharp_ParseAndTraverse** |              **5** |               **Lazy** |           **30** | **24.615 μs** | **0.6886 μs** | **1.3102 μs** | **24.883 μs** | **23.843 μs** | **24.883 μs** | **25.174 μs** | **25.324 μs** | **26.824 μs** | **26.968 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 |               Lazy |           30 | 17.765 μs | 0.2922 μs | 0.5698 μs | 17.663 μs | 17.298 μs | 17.663 μs | 17.974 μs | 18.194 μs | 18.708 μs | 18.834 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |               Lazy |           30 | 23.468 μs | 0.8020 μs | 1.5451 μs | 22.633 μs | 22.249 μs | 22.633 μs | 23.810 μs | 25.558 μs | 25.724 μs | 25.790 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |               Lazy |           30 | 24.681 μs | 1.1499 μs | 2.2428 μs | 24.773 μs | 22.837 μs | 24.773 μs | 24.949 μs | 25.126 μs | 29.065 μs | 29.245 μs |
|                   **FlatSharp_ParseAndTraverse** |              **5** |      **PropertyCache** |           **30** | **28.668 μs** | **0.3807 μs** | **0.7515 μs** | **28.488 μs** | **28.110 μs** | **28.488 μs** | **28.675 μs** | **29.155 μs** | **29.724 μs** | **29.941 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 |      PropertyCache |           30 | 18.899 μs | 0.3031 μs | 0.5839 μs | 19.066 μs | 18.465 μs | 19.066 μs | 19.162 μs | 19.209 μs | 19.567 μs | 19.692 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |      PropertyCache |           30 | 24.145 μs | 0.3087 μs | 0.5948 μs | 24.352 μs | 23.808 μs | 24.352 μs | 24.477 μs | 24.607 μs | 24.787 μs | 24.833 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |      PropertyCache |           30 | 23.756 μs | 0.7250 μs | 1.3794 μs | 23.765 μs | 22.197 μs | 23.765 μs | 24.170 μs | 24.418 μs | 26.171 μs | 26.273 μs |
|                   **FlatSharp_ParseAndTraverse** |              **5** |        **VectorCache** |           **30** | **10.100 μs** | **0.0812 μs** | **0.1546 μs** | **10.158 μs** | **10.011 μs** | **10.158 μs** | **10.200 μs** | **10.226 μs** | **10.249 μs** | **10.284 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 |        VectorCache |           30 |  6.373 μs | 0.1175 μs | 0.2264 μs |  6.345 μs |  6.249 μs |  6.345 μs |  6.391 μs |  6.651 μs |  6.712 μs |  6.722 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |        VectorCache |           30 |  6.122 μs | 0.0481 μs | 0.0938 μs |  6.110 μs |  6.045 μs |  6.110 μs |  6.178 μs |  6.199 μs |  6.244 μs |  6.257 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |        VectorCache |           30 |  5.992 μs | 0.0379 μs | 0.0722 μs |  6.013 μs |  5.927 μs |  6.013 μs |  6.028 μs |  6.041 μs |  6.065 μs |  6.079 μs |
|                   **FlatSharp_ParseAndTraverse** |              **5** | **VectorCacheMutable** |           **30** |  **9.479 μs** | **0.0796 μs** | **0.1553 μs** |  **9.465 μs** |  **9.351 μs** |  **9.465 μs** |  **9.561 μs** |  **9.601 μs** |  **9.715 μs** |  **9.747 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 | VectorCacheMutable |           30 |  6.095 μs | 0.0984 μs | 0.1965 μs |  6.064 μs |  5.978 μs |  6.064 μs |  6.091 μs |  6.157 μs |  6.229 μs |  6.559 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 | VectorCacheMutable |           30 |  5.654 μs | 0.0869 μs | 0.1673 μs |  5.680 μs |  5.530 μs |  5.680 μs |  5.740 μs |  5.769 μs |  5.804 μs |  5.886 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 | VectorCacheMutable |           30 |  5.553 μs | 0.0688 μs | 0.1325 μs |  5.546 μs |  5.476 μs |  5.546 μs |  5.582 μs |  5.609 μs |  5.757 μs |  5.811 μs |
|                   **FlatSharp_ParseAndTraverse** |              **5** |             **Greedy** |           **30** |  **8.672 μs** | **0.0885 μs** | **0.1746 μs** |  **8.701 μs** |  **8.534 μs** |  **8.701 μs** |  **8.790 μs** |  **8.831 μs** |  **8.872 μs** |  **8.925 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 |             Greedy |           30 |  6.482 μs | 0.0896 μs | 0.1727 μs |  6.417 μs |  6.348 μs |  6.417 μs |  6.542 μs |  6.577 μs |  6.794 μs |  6.832 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |             Greedy |           30 |  5.686 μs | 0.1415 μs | 0.2726 μs |  5.598 μs |  5.520 μs |  5.598 μs |  5.673 μs |  5.800 μs |  6.239 μs |  6.272 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |             Greedy |           30 |  5.429 μs | 0.0742 μs | 0.1412 μs |  5.440 μs |  5.322 μs |  5.440 μs |  5.475 μs |  5.566 μs |  5.628 μs |  5.652 μs |
|                   **FlatSharp_ParseAndTraverse** |              **5** |      **GreedyMutable** |           **30** |  **8.631 μs** | **0.3053 μs** | **0.6026 μs** |  **8.328 μs** |  **8.203 μs** |  **8.328 μs** |  **8.683 μs** |  **9.091 μs** |  **9.783 μs** |  **9.874 μs** |
|            FlatSharp_ParseAndTraversePartial |              5 |      GreedyMutable |           30 |  6.272 μs | 0.0485 μs | 0.0922 μs |  6.255 μs |  6.198 μs |  6.255 μs |  6.297 μs |  6.342 μs |  6.427 μs |  6.458 μs |
|        FlatSharp_ParseAndTraverse_NonVirtual |              5 |      GreedyMutable |           30 |  5.504 μs | 0.1259 μs | 0.2396 μs |  5.401 μs |  5.296 μs |  5.401 μs |  5.659 μs |  5.702 μs |  5.859 μs |  5.892 μs |
| FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |      GreedyMutable |           30 |  5.309 μs | 0.0900 μs | 0.1756 μs |  5.384 μs |  5.165 μs |  5.384 μs |  5.436 μs |  5.470 μs |  5.498 μs |  5.509 μs |

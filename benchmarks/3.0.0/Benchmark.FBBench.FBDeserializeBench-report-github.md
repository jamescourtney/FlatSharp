``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.18363
AMD Ryzen 9 3900X 12-Core Processor (Max: 3.80GHz), 1 CPU, 24 logical and 12 physical cores
.NET Core SDK=3.1.200
  [Host]   : .NET Core 2.1.16 (CoreCLR 4.6.28516.03, CoreFX 4.6.28516.10), 64bit RyuJIT
  ShortRun : .NET Core 2.1.16 (CoreCLR 4.6.28516.03, CoreFX 4.6.28516.10), 64bit RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                            Method | TraversalCount |  DeserializeOption | VectorLength |        Mean |      Error |     StdDev |
|---------------------------------- |--------------- |------------------- |------------- |------------:|-----------:|-----------:|
|        **FlatSharp_ParseAndTraverse** |              **1** |             **Greedy** |            **3** |    **715.4 ns** |  **19.242 ns** |  **1.0872 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |             Greedy |            3 |    673.3 ns |  42.436 ns |  2.3977 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |             **Greedy** |           **30** |  **5,030.6 ns** | **101.130 ns** |  **5.7141 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |             Greedy |           30 |  4,811.6 ns | 835.313 ns | 47.1967 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |      **GreedyMutable** |            **3** |    **707.1 ns** |  **20.761 ns** |  **1.1730 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |      GreedyMutable |            3 |    681.3 ns |   2.521 ns |  0.1425 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |      **GreedyMutable** |           **30** |  **5,110.2 ns** | **689.978 ns** | **38.9850 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |      GreedyMutable |           30 |  4,844.5 ns | 785.514 ns | 44.3830 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |               **Lazy** |            **3** |    **611.4 ns** |  **32.589 ns** |  **1.8414 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |               Lazy |            3 |    487.8 ns |  29.644 ns |  1.6750 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |               **Lazy** |           **30** |  **4,673.0 ns** |   **8.804 ns** |  **0.4974 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |               Lazy |           30 |  3,239.1 ns |  80.053 ns |  4.5231 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |      **PropertyCache** |            **3** |    **670.0 ns** |  **12.248 ns** |  **0.6921 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |      PropertyCache |            3 |    485.4 ns |  23.264 ns |  1.3145 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |      **PropertyCache** |           **30** |  **5,128.4 ns** |  **26.023 ns** |  **1.4704 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |      PropertyCache |           30 |  3,339.4 ns | 155.866 ns |  8.8067 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |        **VectorCache** |            **3** |    **726.0 ns** |  **18.851 ns** |  **1.0651 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |        VectorCache |            3 |    579.9 ns |  37.176 ns |  2.1005 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |        **VectorCache** |           **30** |  **5,373.5 ns** | **180.308 ns** | **10.1877 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |        VectorCache |           30 |  3,906.2 ns |  80.646 ns |  4.5566 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** | **VectorCacheMutable** |            **3** |    **706.6 ns** |  **19.140 ns** |  **1.0814 ns** |
| FlatSharp_ParseAndTraversePartial |              1 | VectorCacheMutable |            3 |    549.9 ns |  15.953 ns |  0.9014 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** | **VectorCacheMutable** |           **30** |  **5,062.9 ns** |  **32.580 ns** |  **1.8408 ns** |
| FlatSharp_ParseAndTraversePartial |              1 | VectorCacheMutable |           30 |  3,796.4 ns | 346.669 ns | 19.5874 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |             **Greedy** |            **3** |    **956.6 ns** |  **33.168 ns** |  **1.8740 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |             Greedy |            3 |    817.4 ns |  12.479 ns |  0.7051 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |             **Greedy** |           **30** |  **7,246.9 ns** | **167.947 ns** |  **9.4893 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |             Greedy |           30 |  5,867.7 ns |  86.827 ns |  4.9059 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |      **GreedyMutable** |            **3** |    **924.4 ns** |  **36.242 ns** |  **2.0478 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |      GreedyMutable |            3 |    804.0 ns |  24.036 ns |  1.3581 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |      **GreedyMutable** |           **30** |  **7,012.3 ns** | **232.244 ns** | **13.1222 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |      GreedyMutable |           30 |  5,785.7 ns | 235.633 ns | 13.3137 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |               **Lazy** |            **3** |  **2,796.6 ns** |  **54.587 ns** |  **3.0843 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |               Lazy |            3 |  2,008.3 ns | 499.096 ns | 28.1999 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |               **Lazy** |           **30** | **21,949.6 ns** | **504.655 ns** | **28.5139 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |               Lazy |           30 | 15,929.7 ns | 185.144 ns | 10.4610 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |      **PropertyCache** |            **3** |  **2,690.6 ns** |  **51.381 ns** |  **2.9031 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |      PropertyCache |            3 |  1,799.9 ns |  53.279 ns |  3.0104 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |      **PropertyCache** |           **30** | **25,126.4 ns** | **671.732 ns** | **37.9541 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |      PropertyCache |           30 | 15,315.6 ns | 818.018 ns | 46.2196 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |        **VectorCache** |            **3** |  **1,073.7 ns** | **150.680 ns** |  **8.5137 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |        VectorCache |            3 |    755.0 ns |  60.433 ns |  3.4146 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |        **VectorCache** |           **30** |  **8,390.6 ns** | **253.783 ns** | **14.3392 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |        VectorCache |           30 |  5,445.0 ns | 394.450 ns | 22.2871 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** | **VectorCacheMutable** |            **3** |  **1,025.4 ns** |  **28.519 ns** |  **1.6114 ns** |
| FlatSharp_ParseAndTraversePartial |              5 | VectorCacheMutable |            3 |    713.4 ns |  25.940 ns |  1.4657 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** | **VectorCacheMutable** |           **30** |  **7,781.5 ns** | **229.227 ns** | **12.9517 ns** |
| FlatSharp_ParseAndTraversePartial |              5 | VectorCacheMutable |           30 |  4,744.1 ns | 578.565 ns | 32.6900 ns |

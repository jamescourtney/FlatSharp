``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1237 (20H2/October2020Update)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=5.0.201
  [Host]   : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT
  ShortRun : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET 5.0  
IterationCount=5  LaunchCount=7  WarmupCount=3  

```
|                                                    Method | TraversalCount | DeserializeOption | VectorLength |     Mean |     Error |    StdDev |   Median |      P25 |      P95 |  Gen 0 |  Gen 1 | Allocated |
|---------------------------------------------------------- |--------------- |------------------ |------------- |---------:|----------:|----------:|---------:|---------:|---------:|-------:|-------:|----------:|
|                                **FlatSharp_ParseAndTraverse** |              **1** |              **Lazy** |           **30** | **1.951 μs** | **0.0379 μs** | **0.0612 μs** | **1.936 μs** | **1.901 μs** | **2.056 μs** | **0.4692** |      **-** |      **8 KB** |
|                         FlatSharp_ParseAndTraversePartial |              1 |              Lazy |           30 | 1.514 μs | 0.0142 μs | 0.0220 μs | 1.517 μs | 1.494 μs | 1.547 μs | 0.4711 |      - |      8 KB |
|                     FlatSharp_ParseAndTraverse_NonVirtual |              1 |              Lazy |           30 | 1.746 μs | 0.0318 μs | 0.0504 μs | 1.743 μs | 1.713 μs | 1.834 μs | 0.4711 |      - |      8 KB |
|              FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |              Lazy |           30 | 1.718 μs | 0.0286 μs | 0.0461 μs | 1.709 μs | 1.678 μs | 1.815 μs | 0.4711 |      - |      8 KB |
|                   FlatSharp_ParseAndTraverse_ValueStructs |              1 |              Lazy |           30 | 1.746 μs | 0.0092 μs | 0.0148 μs | 1.744 μs | 1.737 μs | 1.768 μs | 0.3700 |      - |      6 KB |
|            FlatSharp_ParseAndTraversePartial_ValueStructs |              1 |              Lazy |           30 | 1.622 μs | 0.0348 μs | 0.0552 μs | 1.592 μs | 1.581 μs | 1.732 μs | 0.3700 |      - |      6 KB |
|        FlatSharp_ParseAndTraverse_ValueStructs_NonVirtual |              1 |              Lazy |           30 | 1.655 μs | 0.0144 μs | 0.0224 μs | 1.647 μs | 1.641 μs | 1.703 μs | 0.3700 |      - |      6 KB |
| FlatSharp_ParseAndTraversePartial_ValueStructs_NonVirtual |              1 |              Lazy |           30 | 1.592 μs | 0.0077 μs | 0.0117 μs | 1.590 μs | 1.580 μs | 1.607 μs | 0.3700 |      - |      6 KB |
|                                **FlatSharp_ParseAndTraverse** |              **1** |       **Progressive** |           **30** | **2.421 μs** | **0.0441 μs** | **0.0673 μs** | **2.382 μs** | **2.356 μs** | **2.536 μs** | **0.6218** | **0.0229** |     **10 KB** |
|                         FlatSharp_ParseAndTraversePartial |              1 |       Progressive |           30 | 2.014 μs | 0.0443 μs | 0.0715 μs | 1.978 μs | 1.961 μs | 2.134 μs | 0.6218 | 0.0229 |     10 KB |
|                     FlatSharp_ParseAndTraverse_NonVirtual |              1 |       Progressive |           30 | 1.943 μs | 0.0483 μs | 0.0751 μs | 1.938 μs | 1.876 μs | 2.086 μs | 0.4921 | 0.0114 |      8 KB |
|              FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |       Progressive |           30 | 1.906 μs | 0.0228 μs | 0.0375 μs | 1.906 μs | 1.875 μs | 1.954 μs | 0.4921 | 0.0134 |      8 KB |
|                   FlatSharp_ParseAndTraverse_ValueStructs |              1 |       Progressive |           30 | 2.210 μs | 0.0578 μs | 0.0916 μs | 2.210 μs | 2.118 μs | 2.376 μs | 0.4921 | 0.0114 |      8 KB |
|            FlatSharp_ParseAndTraversePartial_ValueStructs |              1 |       Progressive |           30 | 1.973 μs | 0.0162 μs | 0.0243 μs | 1.974 μs | 1.946 μs | 2.005 μs | 0.4921 | 0.0114 |      8 KB |
|        FlatSharp_ParseAndTraverse_ValueStructs_NonVirtual |              1 |       Progressive |           30 | 1.833 μs | 0.0192 μs | 0.0305 μs | 1.825 μs | 1.814 μs | 1.907 μs | 0.3910 | 0.0076 |      6 KB |
| FlatSharp_ParseAndTraversePartial_ValueStructs_NonVirtual |              1 |       Progressive |           30 | 1.784 μs | 0.0046 μs | 0.0074 μs | 1.787 μs | 1.778 μs | 1.794 μs | 0.3910 | 0.0076 |      6 KB |
|                                **FlatSharp_ParseAndTraverse** |              **1** |            **Greedy** |           **30** | **2.438 μs** | **0.0593 μs** | **0.0958 μs** | **2.456 μs** | **2.313 μs** | **2.544 μs** | **0.5188** | **0.0153** |      **8 KB** |
|                         FlatSharp_ParseAndTraversePartial |              1 |            Greedy |           30 | 2.034 μs | 0.0358 μs | 0.0579 μs | 2.034 μs | 1.990 μs | 2.146 μs | 0.5188 | 0.0153 |      8 KB |
|                     FlatSharp_ParseAndTraverse_NonVirtual |              1 |            Greedy |           30 | 1.874 μs | 0.0433 μs | 0.0687 μs | 1.844 μs | 1.811 μs | 1.982 μs | 0.4177 | 0.0095 |      7 KB |
|              FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |            Greedy |           30 | 1.885 μs | 0.0499 μs | 0.0807 μs | 1.867 μs | 1.798 μs | 2.023 μs | 0.4158 | 0.0076 |      7 KB |
|                   FlatSharp_ParseAndTraverse_ValueStructs |              1 |            Greedy |           30 | 2.094 μs | 0.0143 μs | 0.0235 μs | 2.102 μs | 2.080 μs | 2.126 μs | 0.4616 | 0.0114 |      8 KB |
|            FlatSharp_ParseAndTraversePartial_ValueStructs |              1 |            Greedy |           30 | 1.971 μs | 0.0305 μs | 0.0484 μs | 1.949 μs | 1.935 μs | 2.053 μs | 0.4616 | 0.0114 |      8 KB |
|        FlatSharp_ParseAndTraverse_ValueStructs_NonVirtual |              1 |            Greedy |           30 | 1.968 μs | 0.0106 μs | 0.0174 μs | 1.968 μs | 1.958 μs | 1.997 μs | 0.3433 | 0.0038 |      6 KB |
| FlatSharp_ParseAndTraversePartial_ValueStructs_NonVirtual |              1 |            Greedy |           30 | 1.892 μs | 0.0367 μs | 0.0592 μs | 1.874 μs | 1.849 μs | 2.023 μs | 0.3452 | 0.0057 |      6 KB |
|                                **FlatSharp_ParseAndTraverse** |              **1** |     **GreedyMutable** |           **30** | **2.211 μs** | **0.0463 μs** | **0.0747 μs** | **2.244 μs** | **2.125 μs** | **2.299 μs** | **0.5150** | **0.0153** |      **8 KB** |
|                         FlatSharp_ParseAndTraversePartial |              1 |     GreedyMutable |           30 | 2.013 μs | 0.0643 μs | 0.1039 μs | 1.965 μs | 1.946 μs | 2.173 μs | 0.5150 | 0.0153 |      8 KB |
|                     FlatSharp_ParseAndTraverse_NonVirtual |              1 |     GreedyMutable |           30 | 1.891 μs | 0.0238 μs | 0.0384 μs | 1.907 μs | 1.867 μs | 1.932 μs | 0.4158 | 0.0076 |      7 KB |
|              FlatSharp_ParseAndTraversePartial_NonVirtual |              1 |     GreedyMutable |           30 | 1.801 μs | 0.0240 μs | 0.0381 μs | 1.793 μs | 1.782 μs | 1.860 μs | 0.4158 | 0.0095 |      7 KB |
|                   FlatSharp_ParseAndTraverse_ValueStructs |              1 |     GreedyMutable |           30 | 2.015 μs | 0.0225 μs | 0.0370 μs | 2.015 μs | 1.989 μs | 2.081 μs | 0.4578 | 0.0114 |      8 KB |
|            FlatSharp_ParseAndTraversePartial_ValueStructs |              1 |     GreedyMutable |           30 | 1.926 μs | 0.0222 μs | 0.0352 μs | 1.923 μs | 1.902 μs | 1.993 μs | 0.4578 | 0.0114 |      8 KB |
|        FlatSharp_ParseAndTraverse_ValueStructs_NonVirtual |              1 |     GreedyMutable |           30 | 1.941 μs | 0.0188 μs | 0.0287 μs | 1.944 μs | 1.910 μs | 1.980 μs | 0.3433 | 0.0038 |      6 KB |
| FlatSharp_ParseAndTraversePartial_ValueStructs_NonVirtual |              1 |     GreedyMutable |           30 | 1.851 μs | 0.0200 μs | 0.0329 μs | 1.837 μs | 1.831 μs | 1.911 μs | 0.3452 | 0.0057 |      6 KB |
|                                **FlatSharp_ParseAndTraverse** |              **5** |              **Lazy** |           **30** | **9.367 μs** | **0.1468 μs** | **0.2413 μs** | **9.407 μs** | **9.148 μs** | **9.777 μs** | **2.3346** |      **-** |     **38 KB** |
|                         FlatSharp_ParseAndTraversePartial |              5 |              Lazy |           30 | 7.414 μs | 0.1365 μs | 0.2042 μs | 7.350 μs | 7.261 μs | 7.835 μs | 2.3346 |      - |     38 KB |
|                     FlatSharp_ParseAndTraverse_NonVirtual |              5 |              Lazy |           30 | 8.142 μs | 0.0709 μs | 0.1125 μs | 8.125 μs | 8.069 μs | 8.348 μs | 2.3041 |      - |     38 KB |
|              FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |              Lazy |           30 | 8.218 μs | 0.1152 μs | 0.1893 μs | 8.197 μs | 8.043 μs | 8.485 μs | 2.3041 |      - |     38 KB |
|                   FlatSharp_ParseAndTraverse_ValueStructs |              5 |              Lazy |           30 | 8.467 μs | 0.0559 μs | 0.0918 μs | 8.433 μs | 8.400 μs | 8.621 μs | 1.8311 |      - |     30 KB |
|            FlatSharp_ParseAndTraversePartial_ValueStructs |              5 |              Lazy |           30 | 7.732 μs | 0.0472 μs | 0.0735 μs | 7.703 μs | 7.691 μs | 7.894 μs | 1.8311 |      - |     30 KB |
|        FlatSharp_ParseAndTraverse_ValueStructs_NonVirtual |              5 |              Lazy |           30 | 7.767 μs | 0.0466 μs | 0.0725 μs | 7.748 μs | 7.723 μs | 7.921 μs | 1.8005 |      - |     30 KB |
| FlatSharp_ParseAndTraversePartial_ValueStructs_NonVirtual |              5 |              Lazy |           30 | 7.440 μs | 0.0382 μs | 0.0605 μs | 7.430 μs | 7.398 μs | 7.549 μs | 1.8005 |      - |     30 KB |
|                                **FlatSharp_ParseAndTraverse** |              **5** |       **Progressive** |           **30** | **4.613 μs** | **0.0439 μs** | **0.0721 μs** | **4.630 μs** | **4.550 μs** | **4.725 μs** | **0.6180** | **0.0229** |     **10 KB** |
|                         FlatSharp_ParseAndTraversePartial |              5 |       Progressive |           30 | 3.133 μs | 0.0300 μs | 0.0476 μs | 3.127 μs | 3.090 μs | 3.201 μs | 0.6218 | 0.0229 |     10 KB |
|                     FlatSharp_ParseAndTraverse_NonVirtual |              5 |       Progressive |           30 | 2.414 μs | 0.0332 μs | 0.0537 μs | 2.404 μs | 2.365 μs | 2.505 μs | 0.4921 | 0.0114 |      8 KB |
|              FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |       Progressive |           30 | 2.263 μs | 0.0316 μs | 0.0502 μs | 2.249 μs | 2.233 μs | 2.361 μs | 0.4921 | 0.0114 |      8 KB |
|                   FlatSharp_ParseAndTraverse_ValueStructs |              5 |       Progressive |           30 | 3.969 μs | 0.0778 μs | 0.1212 μs | 3.981 μs | 3.858 μs | 4.195 μs | 0.4883 | 0.0076 |      8 KB |
|            FlatSharp_ParseAndTraversePartial_ValueStructs |              5 |       Progressive |           30 | 3.353 μs | 0.0243 μs | 0.0399 μs | 3.336 μs | 3.323 μs | 3.414 μs | 0.4921 | 0.0114 |      8 KB |
|        FlatSharp_ParseAndTraverse_ValueStructs_NonVirtual |              5 |       Progressive |           30 | 3.084 μs | 0.0099 μs | 0.0163 μs | 3.078 μs | 3.074 μs | 3.111 μs | 0.3891 | 0.0076 |      6 KB |
| FlatSharp_ParseAndTraversePartial_ValueStructs_NonVirtual |              5 |       Progressive |           30 | 2.960 μs | 0.0275 μs | 0.0437 μs | 2.958 μs | 2.917 μs | 3.038 μs | 0.3891 | 0.0076 |      6 KB |
|                                **FlatSharp_ParseAndTraverse** |              **5** |            **Greedy** |           **30** | **4.287 μs** | **0.0957 μs** | **0.1517 μs** | **4.265 μs** | **4.180 μs** | **4.591 μs** | **0.5188** | **0.0153** |      **8 KB** |
|                         FlatSharp_ParseAndTraversePartial |              5 |            Greedy |           30 | 3.143 μs | 0.1524 μs | 0.2461 μs | 3.039 μs | 2.971 μs | 3.647 μs | 0.5188 | 0.0153 |      8 KB |
|                     FlatSharp_ParseAndTraverse_NonVirtual |              5 |            Greedy |           30 | 2.348 μs | 0.0412 μs | 0.0629 μs | 2.348 μs | 2.302 μs | 2.440 μs | 0.4158 | 0.0076 |      7 KB |
|              FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |            Greedy |           30 | 2.238 μs | 0.0862 μs | 0.1343 μs | 2.179 μs | 2.141 μs | 2.467 μs | 0.4158 | 0.0076 |      7 KB |
|                   FlatSharp_ParseAndTraverse_ValueStructs |              5 |            Greedy |           30 | 3.911 μs | 0.1140 μs | 0.1740 μs | 3.887 μs | 3.781 μs | 4.309 μs | 0.4616 | 0.0114 |      8 KB |
|            FlatSharp_ParseAndTraversePartial_ValueStructs |              5 |            Greedy |           30 | 3.311 μs | 0.0426 μs | 0.0651 μs | 3.318 μs | 3.268 μs | 3.409 μs | 0.4616 | 0.0114 |      8 KB |
|        FlatSharp_ParseAndTraverse_ValueStructs_NonVirtual |              5 |            Greedy |           30 | 3.594 μs | 0.0366 μs | 0.0602 μs | 3.606 μs | 3.562 μs | 3.660 μs | 0.3433 | 0.0038 |      6 KB |
| FlatSharp_ParseAndTraversePartial_ValueStructs_NonVirtual |              5 |            Greedy |           30 | 3.027 μs | 0.0211 μs | 0.0341 μs | 3.016 μs | 3.006 μs | 3.075 μs | 0.3433 | 0.0038 |      6 KB |
|                                **FlatSharp_ParseAndTraverse** |              **5** |     **GreedyMutable** |           **30** | **3.919 μs** | **0.0567 μs** | **0.0916 μs** | **3.932 μs** | **3.856 μs** | **4.034 μs** | **0.5150** | **0.0153** |      **8 KB** |
|                         FlatSharp_ParseAndTraversePartial |              5 |     GreedyMutable |           30 | 2.937 μs | 0.0388 μs | 0.0638 μs | 2.939 μs | 2.894 μs | 3.029 μs | 0.5150 | 0.0153 |      8 KB |
|                     FlatSharp_ParseAndTraverse_NonVirtual |              5 |     GreedyMutable |           30 | 2.191 μs | 0.0470 μs | 0.0745 μs | 2.191 μs | 2.107 μs | 2.293 μs | 0.4158 | 0.0076 |      7 KB |
|              FlatSharp_ParseAndTraversePartial_NonVirtual |              5 |     GreedyMutable |           30 | 2.066 μs | 0.0638 μs | 0.1012 μs | 2.028 μs | 1.977 μs | 2.277 μs | 0.4158 | 0.0076 |      7 KB |
|                   FlatSharp_ParseAndTraverse_ValueStructs |              5 |     GreedyMutable |           30 | 3.692 μs | 0.0309 μs | 0.0491 μs | 3.693 μs | 3.681 μs | 3.758 μs | 0.4578 | 0.0114 |      8 KB |
|            FlatSharp_ParseAndTraversePartial_ValueStructs |              5 |     GreedyMutable |           30 | 3.166 μs | 0.0575 μs | 0.0928 μs | 3.161 μs | 3.069 μs | 3.315 μs | 0.4578 | 0.0114 |      8 KB |
|        FlatSharp_ParseAndTraverse_ValueStructs_NonVirtual |              5 |     GreedyMutable |           30 | 3.456 μs | 0.0250 μs | 0.0389 μs | 3.467 μs | 3.419 μs | 3.504 μs | 0.3433 | 0.0038 |      6 KB |
| FlatSharp_ParseAndTraversePartial_ValueStructs_NonVirtual |              5 |     GreedyMutable |           30 | 2.900 μs | 0.0306 μs | 0.0503 μs | 2.878 μs | 2.865 μs | 3.001 μs | 0.3433 | 0.0038 |      6 KB |

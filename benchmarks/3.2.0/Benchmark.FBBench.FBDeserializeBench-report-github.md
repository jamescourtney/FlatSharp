``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.14393.3930 (1607/AnniversaryUpdate/Redstone1), VM=Hyper-V
Intel Xeon CPU E5-2667 v3 3.20GHz, 1 CPU, 8 logical and 8 physical cores
  [Host]                  : .NET Framework 4.8 (4.8.4240.0), X64 RyuJIT
  MediumRun-.NET 4.7      : .NET Framework 4.8 (4.8.4240.0), X64 RyuJIT
  MediumRun-.NET Core 2.1 : .NET Core 2.1.22 (CoreCLR 4.6.29220.03, CoreFX 4.6.29220.01), X64 RyuJIT
  MediumRun-.NET Core 3.1 : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT
  MediumRun-.NET Core 5.0 : .NET Core 5.0.0 (CoreCLR 5.0.20.45114, CoreFX 5.0.20.45114), X64 RyuJIT

IterationCount=15  LaunchCount=2  WarmupCount=10  

```
|                            Method |                     Job |       Runtime | TraversalCount |  DeserializeOption | VectorLength |        Mean |     Error |      StdDev |
|---------------------------------- |------------------------ |-------------- |--------------- |------------------- |------------- |------------:|----------:|------------:|
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** |               **Lazy** |            **3** |  **1,942.9 ns** |  **31.78 ns** |    **47.57 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |               Lazy |            3 |  1,449.7 ns |  12.00 ns |    17.96 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |               Lazy |            3 |    848.6 ns |  11.94 ns |    17.50 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |               Lazy |            3 |    687.2 ns |  12.90 ns |    19.31 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |               Lazy |            3 |    900.0 ns |   7.57 ns |    11.10 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |               Lazy |            3 |    720.9 ns |   5.74 ns |     8.59 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |               Lazy |            3 |    895.7 ns |   6.98 ns |    10.00 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |               Lazy |            3 |    724.9 ns |   8.14 ns |    12.18 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** |               **Lazy** |           **30** | **15,344.4 ns** | **152.22 ns** |   **223.13 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |               Lazy |           30 | 10,149.8 ns | 168.72 ns |   252.53 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |               Lazy |           30 |  6,397.5 ns | 115.76 ns |   173.27 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |               Lazy |           30 |  4,777.9 ns |  95.97 ns |   140.67 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |               Lazy |           30 |  6,720.9 ns |  39.35 ns |    57.68 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |               Lazy |           30 |  4,977.2 ns |  24.43 ns |    35.81 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |               Lazy |           30 |  6,756.8 ns |  41.77 ns |    61.22 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |               Lazy |           30 |  5,001.9 ns |  19.65 ns |    28.80 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** |      **PropertyCache** |            **3** |  **2,116.6 ns** |  **17.48 ns** |    **25.07 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |      PropertyCache |            3 |  1,553.0 ns |  22.23 ns |    33.27 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |      PropertyCache |            3 |    955.3 ns |  16.40 ns |    24.55 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |      PropertyCache |            3 |    762.1 ns |  15.77 ns |    22.61 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |      PropertyCache |            3 |    984.6 ns |   6.58 ns |     9.85 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |      PropertyCache |            3 |    794.8 ns |   3.45 ns |     4.72 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |      PropertyCache |            3 |    980.3 ns |  10.58 ns |    15.51 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |      PropertyCache |            3 |    791.6 ns |   3.94 ns |     5.77 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** |      **PropertyCache** |           **30** | **16,344.1 ns** | **127.70 ns** |   **183.15 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |      PropertyCache |           30 | 10,939.8 ns |  67.05 ns |    96.17 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |      PropertyCache |           30 |  7,236.8 ns |  70.95 ns |   101.76 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |      PropertyCache |           30 |  5,314.1 ns |  97.06 ns |   139.20 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |      PropertyCache |           30 |  7,580.1 ns |  93.93 ns |   140.60 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |      PropertyCache |           30 |  5,583.9 ns |  64.69 ns |    96.82 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |      PropertyCache |           30 |  7,570.1 ns |  58.61 ns |    87.72 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |      PropertyCache |           30 |  5,549.8 ns |  42.41 ns |    59.45 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** |        **VectorCache** |            **3** |  **2,139.0 ns** |  **26.60 ns** |    **39.82 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |        VectorCache |            3 |  1,604.7 ns |  10.30 ns |    15.10 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |        VectorCache |            3 |  1,036.7 ns |  16.83 ns |    25.19 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |        VectorCache |            3 |    845.1 ns |   9.31 ns |    13.93 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |        VectorCache |            3 |  1,129.2 ns |  28.28 ns |    40.56 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |        VectorCache |            3 |    884.4 ns |   8.59 ns |    12.32 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |        VectorCache |            3 |  1,087.5 ns |   5.32 ns |     7.79 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |        VectorCache |            3 |    889.3 ns |   9.80 ns |    14.36 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** |        **VectorCache** |           **30** | **17,052.0 ns** | **183.03 ns** |   **273.95 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |        VectorCache |           30 | 11,446.2 ns | 159.09 ns |   238.11 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |        VectorCache |           30 |  7,861.7 ns | 112.15 ns |   167.87 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |        VectorCache |           30 |  5,626.3 ns | 155.99 ns |   223.72 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |        VectorCache |           30 |  8,237.6 ns |  44.90 ns |    65.82 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |        VectorCache |           30 |  6,233.8 ns |  47.80 ns |    71.55 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |        VectorCache |           30 |  8,282.8 ns |  70.57 ns |   103.45 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |        VectorCache |           30 |  6,028.3 ns |  42.78 ns |    61.36 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** | **VectorCacheMutable** |            **3** |  **2,139.8 ns** |  **13.95 ns** |    **20.44 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 | VectorCacheMutable |            3 |  1,596.0 ns |  23.30 ns |    34.87 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 | VectorCacheMutable |            3 |  1,016.7 ns |  19.51 ns |    29.21 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 | VectorCacheMutable |            3 |    794.7 ns |  23.75 ns |    35.54 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 | VectorCacheMutable |            3 |  1,068.3 ns |   7.97 ns |    11.68 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 | VectorCacheMutable |            3 |    862.9 ns |  11.97 ns |    17.54 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 | VectorCacheMutable |            3 |  1,106.0 ns |  10.28 ns |    15.38 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 | VectorCacheMutable |            3 |    882.2 ns |   5.24 ns |     7.68 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** | **VectorCacheMutable** |           **30** | **16,677.2 ns** | **212.40 ns** |   **304.62 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 | VectorCacheMutable |           30 | 11,178.9 ns | 187.62 ns |   275.01 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 | VectorCacheMutable |           30 |  7,608.9 ns | 100.24 ns |   150.04 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 | VectorCacheMutable |           30 |  5,522.9 ns | 202.70 ns |   303.39 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 | VectorCacheMutable |           30 |  8,061.9 ns |  52.39 ns |    76.80 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 | VectorCacheMutable |           30 |  6,051.2 ns |  64.61 ns |    92.66 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 | VectorCacheMutable |           30 |  8,073.3 ns |  52.13 ns |    76.42 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 | VectorCacheMutable |           30 |  6,316.7 ns | 152.47 ns |   228.22 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** |             **Greedy** |            **3** |  **2,098.7 ns** |  **12.15 ns** |    **17.80 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |             Greedy |            3 |  2,071.6 ns |  30.77 ns |    46.06 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |             Greedy |            3 |  1,002.7 ns |  12.10 ns |    17.73 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |             Greedy |            3 |    958.5 ns |  18.01 ns |    26.96 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |             Greedy |            3 |  1,039.1 ns |   5.19 ns |     7.28 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |             Greedy |            3 |    993.7 ns |   7.52 ns |    11.25 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |             Greedy |            3 |  1,020.4 ns |  10.40 ns |    15.25 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |             Greedy |            3 |    986.4 ns |   6.10 ns |     7.93 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** |             **Greedy** |           **30** | **16,307.7 ns** | **130.27 ns** |   **186.83 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |             Greedy |           30 | 15,994.4 ns | 130.48 ns |   195.29 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |             Greedy |           30 |  7,333.6 ns | 127.96 ns |   183.52 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |             Greedy |           30 |  7,038.3 ns |  56.49 ns |    79.20 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |             Greedy |           30 |  7,612.6 ns |  63.48 ns |    88.99 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |             Greedy |           30 |  7,293.9 ns |  69.99 ns |   102.60 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |             Greedy |           30 |  7,626.3 ns |  82.77 ns |   121.32 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |             Greedy |           30 |  7,222.4 ns |  38.96 ns |    55.88 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** |      **GreedyMutable** |            **3** |  **2,086.0 ns** |  **11.02 ns** |    **15.80 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |      GreedyMutable |            3 |  2,025.6 ns |  17.53 ns |    25.70 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |      GreedyMutable |            3 |    943.9 ns |  14.18 ns |    20.78 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |      GreedyMutable |            3 |    951.0 ns |  10.65 ns |    14.93 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |      GreedyMutable |            3 |  1,006.5 ns |   6.36 ns |     9.12 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |      GreedyMutable |            3 |    973.7 ns |   6.03 ns |     8.84 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |      GreedyMutable |            3 |    998.4 ns |   6.91 ns |    10.35 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |      GreedyMutable |            3 |    976.6 ns |   8.25 ns |    12.34 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **1** |      **GreedyMutable** |           **30** | **16,409.2 ns** | **158.84 ns** |   **227.80 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              1 |      GreedyMutable |           30 | 15,899.1 ns | 153.83 ns |   230.24 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |      GreedyMutable |           30 |  7,105.9 ns | 190.99 ns |   279.95 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              1 |      GreedyMutable |           30 |  6,918.9 ns |  65.92 ns |    96.62 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |      GreedyMutable |           30 |  7,544.6 ns |  47.97 ns |    67.24 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              1 |      GreedyMutable |           30 |  7,270.8 ns | 104.28 ns |   152.86 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |      GreedyMutable |           30 |  7,462.3 ns |  32.12 ns |    46.07 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              1 |      GreedyMutable |           30 |  7,201.7 ns |  42.94 ns |    60.19 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** |               **Lazy** |            **3** |  **9,633.3 ns** |  **44.91 ns** |    **62.96 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |               Lazy |            3 |  6,962.1 ns |  26.06 ns |    39.00 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |               Lazy |            3 |  3,920.0 ns |  75.62 ns |   110.85 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |               Lazy |            3 |  3,063.4 ns |  72.38 ns |   108.33 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |               Lazy |            3 |  4,177.3 ns |  22.08 ns |    30.96 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |               Lazy |            3 |  3,193.5 ns |  56.06 ns |    83.91 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |               Lazy |            3 |  4,208.1 ns |  35.97 ns |    53.83 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |               Lazy |            3 |  3,266.6 ns |  23.48 ns |    35.15 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** |               **Lazy** |           **30** | **76,945.9 ns** | **478.36 ns** |   **701.17 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |               Lazy |           30 | 50,636.7 ns | 776.12 ns | 1,161.67 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |               Lazy |           30 | 32,107.0 ns | 239.01 ns |   327.16 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |               Lazy |           30 | 22,449.0 ns | 372.13 ns |   556.99 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |               Lazy |           30 | 33,693.0 ns | 230.62 ns |   315.68 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |               Lazy |           30 | 24,468.6 ns | 332.57 ns |   497.78 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |               Lazy |           30 | 33,632.6 ns | 159.10 ns |   223.03 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |               Lazy |           30 | 24,804.8 ns | 266.88 ns |   399.45 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** |      **PropertyCache** |            **3** |  **8,951.5 ns** |  **68.90 ns** |    **98.82 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |      PropertyCache |            3 |  6,057.6 ns |  91.10 ns |   136.36 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |      PropertyCache |            3 |  3,915.1 ns |  62.39 ns |    93.38 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |      PropertyCache |            3 |  2,799.8 ns |  58.23 ns |    87.15 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |      PropertyCache |            3 |  4,077.1 ns |  33.83 ns |    50.64 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |      PropertyCache |            3 |  3,045.4 ns |  36.71 ns |    52.65 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |      PropertyCache |            3 |  4,062.2 ns |  36.40 ns |    53.35 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |      PropertyCache |            3 |  3,056.5 ns |  28.05 ns |    40.22 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** |      **PropertyCache** |           **30** | **79,628.8 ns** | **547.66 ns** |   **819.72 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |      PropertyCache |           30 | 51,314.4 ns | 946.49 ns | 1,416.67 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |      PropertyCache |           30 | 35,902.9 ns | 674.72 ns |   989.00 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |      PropertyCache |           30 | 24,184.9 ns | 546.41 ns |   817.84 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |      PropertyCache |           30 | 37,631.5 ns | 289.51 ns |   415.21 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |      PropertyCache |           30 | 26,355.6 ns | 229.38 ns |   328.97 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |      PropertyCache |           30 | 37,072.9 ns | 315.58 ns |   472.35 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |      PropertyCache |           30 | 26,432.0 ns | 268.50 ns |   401.88 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** |        **VectorCache** |            **3** |  **2,588.2 ns** |   **9.39 ns** |    **12.53 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |        VectorCache |            3 |  1,878.6 ns |  17.47 ns |    26.14 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |        VectorCache |            3 |  1,492.7 ns |   4.65 ns |     6.81 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |        VectorCache |            3 |  1,127.0 ns |  14.51 ns |    21.71 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |        VectorCache |            3 |  1,529.0 ns |  10.81 ns |    16.18 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |        VectorCache |            3 |  1,142.8 ns |   5.47 ns |     7.85 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |        VectorCache |            3 |  1,498.6 ns |  10.67 ns |    15.30 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |        VectorCache |            3 |  1,139.3 ns |   8.57 ns |    12.29 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** |        **VectorCache** |           **30** | **20,703.9 ns** | **112.29 ns** |   **164.59 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |        VectorCache |           30 | 13,442.7 ns | 282.38 ns |   422.65 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |        VectorCache |           30 | 11,875.9 ns | 214.40 ns |   320.90 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |        VectorCache |           30 |  7,827.0 ns | 148.47 ns |   217.62 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |        VectorCache |           30 | 11,909.8 ns | 152.54 ns |   228.32 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |        VectorCache |           30 |  8,017.9 ns |  62.10 ns |    91.02 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |        VectorCache |           30 | 11,898.1 ns |  83.55 ns |   117.13 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |        VectorCache |           30 |  7,971.8 ns |  44.45 ns |    63.75 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** | **VectorCacheMutable** |            **3** |  **2,525.3 ns** |  **23.66 ns** |    **35.41 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 | VectorCacheMutable |            3 |  1,774.6 ns |  19.53 ns |    28.01 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 | VectorCacheMutable |            3 |  1,394.1 ns |   9.72 ns |    14.55 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 | VectorCacheMutable |            3 |  1,010.8 ns |  12.07 ns |    18.06 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 | VectorCacheMutable |            3 |  1,439.6 ns |   8.35 ns |    11.97 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 | VectorCacheMutable |            3 |  1,073.8 ns |   7.21 ns |    10.79 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 | VectorCacheMutable |            3 |  1,466.5 ns |   5.36 ns |     7.34 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 | VectorCacheMutable |            3 |  1,102.2 ns |   9.99 ns |    14.65 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** | **VectorCacheMutable** |           **30** | **20,013.5 ns** | **175.91 ns** |   **252.28 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 | VectorCacheMutable |           30 | 12,707.1 ns |  99.91 ns |   143.29 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 | VectorCacheMutable |           30 | 11,034.1 ns |  69.45 ns |   103.95 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 | VectorCacheMutable |           30 |  7,160.2 ns | 156.63 ns |   234.44 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 | VectorCacheMutable |           30 | 11,479.1 ns | 135.48 ns |   194.30 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 | VectorCacheMutable |           30 |  7,594.0 ns |  88.50 ns |   132.46 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 | VectorCacheMutable |           30 | 11,481.4 ns | 106.81 ns |   156.56 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 | VectorCacheMutable |           30 |  7,661.0 ns |  99.07 ns |   148.28 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** |             **Greedy** |            **3** |  **2,423.1 ns** |  **10.35 ns** |    **14.85 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |             Greedy |            3 |  2,275.4 ns |  14.00 ns |    20.08 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |             Greedy |            3 |  1,295.1 ns |  16.18 ns |    24.22 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |             Greedy |            3 |  1,141.6 ns |   9.47 ns |    13.88 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |             Greedy |            3 |  1,360.7 ns |   8.80 ns |    12.90 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |             Greedy |            3 |  1,187.1 ns |   6.24 ns |     9.15 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |             Greedy |            3 |  1,348.2 ns |   5.31 ns |     7.94 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |             Greedy |            3 |  1,185.8 ns |   8.21 ns |    12.04 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** |             **Greedy** |           **30** | **19,170.7 ns** | **106.91 ns** |   **153.33 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |             Greedy |           30 | 17,604.3 ns | 121.35 ns |   181.64 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |             Greedy |           30 |  9,906.7 ns |  36.39 ns |    52.18 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |             Greedy |           30 |  8,382.1 ns | 108.29 ns |   158.73 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |             Greedy |           30 | 10,514.8 ns |  65.21 ns |    95.58 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |             Greedy |           30 |  8,807.1 ns |  69.37 ns |    97.24 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |             Greedy |           30 | 10,299.4 ns |  97.15 ns |   145.40 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |             Greedy |           30 |  8,687.4 ns |  59.81 ns |    85.77 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** |      **GreedyMutable** |            **3** |  **2,375.3 ns** |  **10.82 ns** |    **15.85 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |      GreedyMutable |            3 |  2,205.3 ns |  15.80 ns |    23.64 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |      GreedyMutable |            3 |  1,213.9 ns |   8.79 ns |    12.60 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |      GreedyMutable |            3 |  1,082.5 ns |  12.65 ns |    18.93 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |      GreedyMutable |            3 |  1,275.3 ns |   9.50 ns |    13.92 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |      GreedyMutable |            3 |  1,143.8 ns |   5.73 ns |     8.58 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |      GreedyMutable |            3 |  1,301.2 ns |  18.93 ns |    28.33 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |      GreedyMutable |            3 |  1,123.2 ns |   4.35 ns |     6.09 ns |
|        **FlatSharp_ParseAndTraverse** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |              **5** |      **GreedyMutable** |           **30** | **18,706.3 ns** | **135.04 ns** |   **197.94 ns** |
| FlatSharp_ParseAndTraversePartial |      MediumRun-.NET 4.7 |      .NET 4.7 |              5 |      GreedyMutable |           30 | 17,144.6 ns | 128.65 ns |   192.56 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |      GreedyMutable |           30 |  9,460.5 ns |  87.00 ns |   130.22 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 2.1 | .NET Core 2.1 |              5 |      GreedyMutable |           30 |  7,926.6 ns |  69.96 ns |   104.71 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |      GreedyMutable |           30 |  9,932.6 ns |  58.97 ns |    86.44 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 3.1 | .NET Core 3.1 |              5 |      GreedyMutable |           30 |  8,351.2 ns |  55.37 ns |    82.87 ns |
|        FlatSharp_ParseAndTraverse | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |      GreedyMutable |           30 |  9,905.7 ns |  74.86 ns |   112.05 ns |
| FlatSharp_ParseAndTraversePartial | MediumRun-.NET Core 5.0 | .NET Core 5.0 |              5 |      GreedyMutable |           30 |  8,291.4 ns |  47.05 ns |    68.97 ns |

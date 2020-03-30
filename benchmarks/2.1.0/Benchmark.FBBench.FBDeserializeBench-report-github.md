``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.18363
AMD Ryzen 9 3900X 12-Core Processor (Max: 3.80GHz), 1 CPU, 24 logical and 12 physical cores
.NET Core SDK=3.1.200
  [Host]   : .NET Core 2.1.16 (CoreCLR 4.6.28516.03, CoreFX 4.6.28516.10), 64bit RyuJIT
  ShortRun : .NET Core 2.1.16 (CoreCLR 4.6.28516.03, CoreFX 4.6.28516.10), 64bit RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                            Method | TraversalCount |  DeserializeOption | VectorLength |        Mean |        Error |     StdDev |
|---------------------------------- |--------------- |------------------- |------------- |------------:|-------------:|-----------:|
|        **FlatSharp_ParseAndTraverse** |              **1** |             **Greedy** |            **3** |    **760.9 ns** |    **32.386 ns** |  **1.8299 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |             Greedy |            3 |    700.3 ns |    44.398 ns |  2.5086 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |             **Greedy** |           **30** |  **5,238.0 ns** |   **233.865 ns** | **13.2138 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |             Greedy |           30 |  5,083.7 ns |   349.848 ns | 19.7671 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |      **GreedyMutable** |            **3** |    **709.9 ns** |    **16.872 ns** |  **0.9533 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |      GreedyMutable |            3 |    685.6 ns |    49.776 ns |  2.8125 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |      **GreedyMutable** |           **30** |  **4,950.2 ns** |   **409.436 ns** | **23.1339 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |      GreedyMutable |           30 |  4,908.3 ns | 1,555.206 ns | 87.8720 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |               **Lazy** |            **3** |    **650.9 ns** |    **25.902 ns** |  **1.4635 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |               Lazy |            3 |    509.8 ns |     3.948 ns |  0.2231 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |               **Lazy** |           **30** |  **5,013.5 ns** |   **132.336 ns** |  **7.4772 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |               Lazy |           30 |  3,582.7 ns |    34.916 ns |  1.9728 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |      **PropertyCache** |            **3** |    **691.1 ns** |    **14.409 ns** |  **0.8141 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |      PropertyCache |            3 |    526.5 ns |    13.909 ns |  0.7859 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |      **PropertyCache** |           **30** |  **5,362.0 ns** |    **61.570 ns** |  **3.4788 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |      PropertyCache |           30 |  3,710.4 ns |   102.218 ns |  5.7755 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |        **VectorCache** |            **3** |    **751.7 ns** |     **8.854 ns** |  **0.5002 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |        VectorCache |            3 |    596.5 ns |     6.371 ns |  0.3600 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** |        **VectorCache** |           **30** |  **5,570.1 ns** |   **130.303 ns** |  **7.3623 ns** |
| FlatSharp_ParseAndTraversePartial |              1 |        VectorCache |           30 |  4,157.6 ns |    40.305 ns |  2.2773 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** | **VectorCacheMutable** |            **3** |    **739.3 ns** |    **16.095 ns** |  **0.9094 ns** |
| FlatSharp_ParseAndTraversePartial |              1 | VectorCacheMutable |            3 |    582.9 ns |    16.944 ns |  0.9573 ns |
|        **FlatSharp_ParseAndTraverse** |              **1** | **VectorCacheMutable** |           **30** |  **5,557.5 ns** |   **462.247 ns** | **26.1178 ns** |
| FlatSharp_ParseAndTraversePartial |              1 | VectorCacheMutable |           30 |  3,921.4 ns |   236.591 ns | 13.3678 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |             **Greedy** |            **3** |    **962.4 ns** |    **25.491 ns** |  **1.4403 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |             Greedy |            3 |    856.1 ns |    49.425 ns |  2.7926 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |             **Greedy** |           **30** |  **7,577.8 ns** |   **176.417 ns** |  **9.9679 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |             Greedy |           30 |  6,211.7 ns |    89.155 ns |  5.0374 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |      **GreedyMutable** |            **3** |    **928.1 ns** |    **30.454 ns** |  **1.7207 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |      GreedyMutable |            3 |    803.1 ns |    12.615 ns |  0.7128 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |      **GreedyMutable** |           **30** |  **6,886.8 ns** |    **85.276 ns** |  **4.8182 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |      GreedyMutable |           30 |  5,769.5 ns |   816.899 ns | 46.1563 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |               **Lazy** |            **3** |  **3,006.8 ns** |    **57.782 ns** |  **3.2648 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |               Lazy |            3 |  2,312.6 ns |    61.479 ns |  3.4737 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |               **Lazy** |           **30** | **24,528.2 ns** |   **571.328 ns** | **32.2811 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |               Lazy |           30 | 17,534.4 ns |   128.936 ns |  7.2851 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |      **PropertyCache** |            **3** |  **2,771.8 ns** |    **89.119 ns** |  **5.0354 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |      PropertyCache |            3 |  1,949.8 ns |    80.360 ns |  4.5405 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |      **PropertyCache** |           **30** | **25,256.0 ns** |   **308.697 ns** | **17.4419 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |      PropertyCache |           30 | 17,697.7 ns |   345.900 ns | 19.5440 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |        **VectorCache** |            **3** |  **1,105.8 ns** |    **34.505 ns** |  **1.9496 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |        VectorCache |            3 |    795.8 ns |    33.950 ns |  1.9183 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** |        **VectorCache** |           **30** |  **8,486.3 ns** |   **909.611 ns** | **51.3947 ns** |
| FlatSharp_ParseAndTraversePartial |              5 |        VectorCache |           30 |  5,476.0 ns |    82.795 ns |  4.6781 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** | **VectorCacheMutable** |            **3** |  **1,057.9 ns** |    **18.340 ns** |  **1.0363 ns** |
| FlatSharp_ParseAndTraversePartial |              5 | VectorCacheMutable |            3 |    756.3 ns |    51.704 ns |  2.9214 ns |
|        **FlatSharp_ParseAndTraverse** |              **5** | **VectorCacheMutable** |           **30** |  **8,259.9 ns** |   **511.685 ns** | **28.9112 ns** |
| FlatSharp_ParseAndTraversePartial |              5 | VectorCacheMutable |           30 |  5,183.4 ns |   361.414 ns | 20.4206 ns |

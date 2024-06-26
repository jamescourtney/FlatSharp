``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.819)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET 7.0  
IterationCount=5  LaunchCount=7  WarmupCount=3  

```
|                                         Method | TraversalCount | DeserializeOption | VectorLength |     Mean |     Error |    StdDev |   Median |      P25 |      P95 |   Gen0 |   Gen1 | Allocated |
|----------------------------------------------- |--------------- |------------------ |------------- |---------:|----------:|----------:|---------:|---------:|---------:|-------:|-------:|----------:|
|                     **FlatSharp_ParseAndTraverse** |              **1** |              **Lazy** |           **30** | **1.709 μs** | **0.0132 μs** | **0.0205 μs** | **1.703 μs** | **1.696 μs** | **1.748 μs** | **0.4864** |      **-** |   **7.95 KB** |
|              FlatSharp_ParseAndTraversePartial |              1 |              Lazy |           30 | 1.401 μs | 0.0190 μs | 0.0307 μs | 1.391 μs | 1.383 μs | 1.468 μs | 0.4864 |      - |   7.95 KB |
|        FlatSharp_ParseAndTraverse_ValueStructs |              1 |              Lazy |           30 | 1.681 μs | 0.0041 μs | 0.0067 μs | 1.679 μs | 1.677 μs | 1.695 μs | 0.3719 |      - |   6.08 KB |
| FlatSharp_ParseAndTraversePartial_ValueStructs |              1 |              Lazy |           30 | 1.502 μs | 0.0085 μs | 0.0127 μs | 1.502 μs | 1.494 μs | 1.517 μs | 0.3719 |      - |   6.08 KB |
|                     **FlatSharp_ParseAndTraverse** |              **1** |       **Progressive** |           **30** | **2.412 μs** | **0.0658 μs** | **0.1025 μs** | **2.379 μs** | **2.360 μs** | **2.675 μs** | **0.6294** | **0.0191** |   **10.3 KB** |
|              FlatSharp_ParseAndTraversePartial |              1 |       Progressive |           30 | 1.873 μs | 0.0248 μs | 0.0400 μs | 1.868 μs | 1.847 μs | 1.936 μs | 0.6294 | 0.0210 |   10.3 KB |
|        FlatSharp_ParseAndTraverse_ValueStructs |              1 |       Progressive |           30 | 2.186 μs | 0.0141 μs | 0.0224 μs | 2.178 μs | 2.174 μs | 2.245 μs | 0.5150 | 0.0153 |   8.43 KB |
| FlatSharp_ParseAndTraversePartial_ValueStructs |              1 |       Progressive |           30 | 2.045 μs | 0.1063 μs | 0.1716 μs | 1.987 μs | 1.955 μs | 2.450 μs | 0.5150 | 0.0153 |   8.43 KB |
|                     **FlatSharp_ParseAndTraverse** |              **1** |            **Greedy** |           **30** | **1.896 μs** | **0.0118 μs** | **0.0191 μs** | **1.898 μs** | **1.892 μs** | **1.924 μs** | **0.5188** | **0.0153** |    **8.5 KB** |
|              FlatSharp_ParseAndTraversePartial |              1 |            Greedy |           30 | 1.745 μs | 0.0368 μs | 0.0561 μs | 1.722 μs | 1.700 μs | 1.849 μs | 0.5188 | 0.0153 |    8.5 KB |
|        FlatSharp_ParseAndTraverse_ValueStructs |              1 |            Greedy |           30 | 1.979 μs | 0.0298 μs | 0.0454 μs | 1.961 μs | 1.951 μs | 2.088 μs | 0.4616 | 0.0114 |   7.56 KB |
| FlatSharp_ParseAndTraversePartial_ValueStructs |              1 |            Greedy |           30 | 1.849 μs | 0.0326 μs | 0.0526 μs | 1.825 μs | 1.820 μs | 1.976 μs | 0.4616 | 0.0114 |   7.56 KB |
|                     **FlatSharp_ParseAndTraverse** |              **1** |     **GreedyMutable** |           **30** | **1.896 μs** | **0.0130 μs** | **0.0203 μs** | **1.900 μs** | **1.888 μs** | **1.919 μs** | **0.5188** | **0.0153** |    **8.5 KB** |
|              FlatSharp_ParseAndTraversePartial |              1 |     GreedyMutable |           30 | 1.721 μs | 0.0110 μs | 0.0161 μs | 1.724 μs | 1.717 μs | 1.743 μs | 0.5188 | 0.0153 |    8.5 KB |
|        FlatSharp_ParseAndTraverse_ValueStructs |              1 |     GreedyMutable |           30 | 1.984 μs | 0.0177 μs | 0.0286 μs | 1.980 μs | 1.956 μs | 2.029 μs | 0.4616 | 0.0114 |   7.56 KB |
| FlatSharp_ParseAndTraversePartial_ValueStructs |              1 |     GreedyMutable |           30 | 1.866 μs | 0.0247 μs | 0.0399 μs | 1.859 μs | 1.834 μs | 1.927 μs | 0.4616 | 0.0114 |   7.56 KB |
|                     **FlatSharp_ParseAndTraverse** |              **5** |              **Lazy** |           **30** | **8.286 μs** | **0.0630 μs** | **0.1000 μs** | **8.272 μs** | **8.206 μs** | **8.448 μs** | **2.4109** |      **-** |  **39.42 KB** |
|              FlatSharp_ParseAndTraversePartial |              5 |              Lazy |           30 | 6.649 μs | 0.0491 μs | 0.0778 μs | 6.641 μs | 6.594 μs | 6.815 μs | 2.4109 |      - |  39.42 KB |
|        FlatSharp_ParseAndTraverse_ValueStructs |              5 |              Lazy |           30 | 8.269 μs | 0.0348 μs | 0.0563 μs | 8.277 μs | 8.248 μs | 8.341 μs | 1.8311 |      - |  30.05 KB |
| FlatSharp_ParseAndTraversePartial_ValueStructs |              5 |              Lazy |           30 | 7.314 μs | 0.0714 μs | 0.1111 μs | 7.278 μs | 7.247 μs | 7.567 μs | 1.8387 |      - |  30.05 KB |
|                     **FlatSharp_ParseAndTraverse** |              **5** |       **Progressive** |           **30** | **4.518 μs** | **0.0476 μs** | **0.0755 μs** | **4.499 μs** | **4.475 μs** | **4.636 μs** | **0.6256** | **0.0153** |   **10.3 KB** |
|              FlatSharp_ParseAndTraversePartial |              5 |       Progressive |           30 | 3.082 μs | 0.0086 μs | 0.0134 μs | 3.082 μs | 3.077 μs | 3.107 μs | 0.6294 | 0.0191 |   10.3 KB |
|        FlatSharp_ParseAndTraverse_ValueStructs |              5 |       Progressive |           30 | 4.104 μs | 0.0212 μs | 0.0336 μs | 4.101 μs | 4.081 μs | 4.156 μs | 0.5112 | 0.0153 |   8.43 KB |
| FlatSharp_ParseAndTraversePartial_ValueStructs |              5 |       Progressive |           30 | 3.292 μs | 0.0107 μs | 0.0173 μs | 3.292 μs | 3.283 μs | 3.312 μs | 0.5150 | 0.0153 |   8.43 KB |
|                     **FlatSharp_ParseAndTraverse** |              **5** |            **Greedy** |           **30** | **3.437 μs** | **0.0494 μs** | **0.0798 μs** | **3.439 μs** | **3.370 μs** | **3.561 μs** | **0.5188** | **0.0153** |    **8.5 KB** |
|              FlatSharp_ParseAndTraversePartial |              5 |            Greedy |           30 | 2.627 μs | 0.0272 μs | 0.0424 μs | 2.633 μs | 2.593 μs | 2.696 μs | 0.5188 | 0.0153 |    8.5 KB |
|        FlatSharp_ParseAndTraverse_ValueStructs |              5 |            Greedy |           30 | 3.638 μs | 0.0316 μs | 0.0492 μs | 3.616 μs | 3.610 μs | 3.728 μs | 0.4616 | 0.0114 |   7.56 KB |
| FlatSharp_ParseAndTraversePartial_ValueStructs |              5 |            Greedy |           30 | 2.986 μs | 0.0139 μs | 0.0225 μs | 2.980 μs | 2.970 μs | 3.037 μs | 0.4616 | 0.0114 |   7.56 KB |
|                     **FlatSharp_ParseAndTraverse** |              **5** |     **GreedyMutable** |           **30** | **3.474 μs** | **0.0462 μs** | **0.0733 μs** | **3.452 μs** | **3.410 μs** | **3.577 μs** | **0.5188** | **0.0153** |    **8.5 KB** |
|              FlatSharp_ParseAndTraversePartial |              5 |     GreedyMutable |           30 | 2.640 μs | 0.0372 μs | 0.0611 μs | 2.612 μs | 2.584 μs | 2.724 μs | 0.5188 | 0.0153 |    8.5 KB |
|        FlatSharp_ParseAndTraverse_ValueStructs |              5 |     GreedyMutable |           30 | 3.639 μs | 0.0176 μs | 0.0279 μs | 3.628 μs | 3.620 μs | 3.692 μs | 0.4616 | 0.0114 |   7.56 KB |
| FlatSharp_ParseAndTraversePartial_ValueStructs |              5 |     GreedyMutable |           30 | 3.013 μs | 0.0347 μs | 0.0551 μs | 2.987 μs | 2.981 μs | 3.128 μs | 0.4616 | 0.0114 |   7.56 KB |

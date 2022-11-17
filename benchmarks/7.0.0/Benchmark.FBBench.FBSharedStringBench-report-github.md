``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.819)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET 7.0  
IterationCount=5  LaunchCount=7  WarmupCount=3  

```
|                                         Method | CacheSize | VectorLength |     Mean |    Error |   StdDev |   Median |      P25 |      P95 | Allocated |
|----------------------------------------------- |---------- |------------- |---------:|---------:|---------:|---------:|---------:|---------:|----------:|
| **Serialize_RandomStringVector_WithRegularString** |       **127** |         **1000** | **14.65 μs** | **0.056 μs** | **0.084 μs** | **14.66 μs** | **14.60 μs** | **14.79 μs** |         **-** |
|       Serialize_RandomStringVector_WithSharing |       127 |         1000 | 49.45 μs | 0.120 μs | 0.190 μs | 49.37 μs | 49.33 μs | 49.86 μs |         - |
|    Serialize_NonRandomStringVector_WithSharing |       127 |         1000 | 48.45 μs | 0.134 μs | 0.212 μs | 48.46 μs | 48.22 μs | 48.75 μs |         - |
| **Serialize_RandomStringVector_WithRegularString** |      **1024** |         **1000** | **14.48 μs** | **0.087 μs** | **0.133 μs** | **14.41 μs** | **14.38 μs** | **14.72 μs** |         **-** |
|       Serialize_RandomStringVector_WithSharing |      1024 |         1000 | 47.17 μs | 0.335 μs | 0.521 μs | 47.05 μs | 46.74 μs | 48.10 μs |         - |
|    Serialize_NonRandomStringVector_WithSharing |      1024 |         1000 | 45.30 μs | 0.282 μs | 0.447 μs | 45.46 μs | 45.01 μs | 45.84 μs |         - |

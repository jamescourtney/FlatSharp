``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.819)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET 7.0  
IterationCount=5  LaunchCount=7  WarmupCount=3  

```
|                                         Method | CacheSize | VectorLength |     Mean |    Error |   StdDev |      P25 |      P95 | Allocated |
|----------------------------------------------- |---------- |------------- |---------:|---------:|---------:|---------:|---------:|----------:|
| **Serialize_RandomStringVector_WithRegularString** |       **127** |         **1000** | **14.54 μs** | **0.093 μs** | **0.147 μs** | **14.46 μs** | **14.84 μs** |         **-** |
|       Serialize_RandomStringVector_WithSharing |       127 |         1000 | 49.60 μs | 0.218 μs | 0.346 μs | 49.39 μs | 50.52 μs |         - |
|    Serialize_NonRandomStringVector_WithSharing |       127 |         1000 | 48.84 μs | 0.248 μs | 0.401 μs | 48.57 μs | 49.54 μs |         - |
| **Serialize_RandomStringVector_WithRegularString** |      **1024** |         **1000** | **14.48 μs** | **0.069 μs** | **0.105 μs** | **14.47 μs** | **14.59 μs** |         **-** |
|       Serialize_RandomStringVector_WithSharing |      1024 |         1000 | 47.59 μs | 0.380 μs | 0.568 μs | 47.07 μs | 48.64 μs |         - |
|    Serialize_NonRandomStringVector_WithSharing |      1024 |         1000 | 45.42 μs | 0.301 μs | 0.487 μs | 44.95 μs | 46.08 μs |         - |

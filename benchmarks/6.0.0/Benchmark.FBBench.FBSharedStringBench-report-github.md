``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1237 (20H2/October2020Update)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=5.0.201
  [Host]   : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT
  ShortRun : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET 5.0  
IterationCount=5  LaunchCount=7  WarmupCount=3  

```
|                                         Method | CacheSize | VectorLength |     Mean |    Error |   StdDev |      P25 |      P95 | Allocated |
|----------------------------------------------- |---------- |------------- |---------:|---------:|---------:|---------:|---------:|----------:|
| **Serialize_RandomStringVector_WithRegularString** |       **127** |         **1000** | **16.13 μs** | **0.056 μs** | **0.086 μs** | **16.04 μs** | **16.24 μs** |         **-** |
|       Serialize_RandomStringVector_WithSharing |       127 |         1000 | 48.29 μs | 0.115 μs | 0.182 μs | 48.18 μs | 48.57 μs |         - |
|    Serialize_NonRandomStringVector_WithSharing |       127 |         1000 | 47.22 μs | 0.135 μs | 0.214 μs | 47.11 μs | 47.58 μs |         - |
| **Serialize_RandomStringVector_WithRegularString** |      **1024** |         **1000** | **15.95 μs** | **0.071 μs** | **0.112 μs** | **15.89 μs** | **16.10 μs** |         **-** |
|       Serialize_RandomStringVector_WithSharing |      1024 |         1000 | 47.48 μs | 0.195 μs | 0.310 μs | 47.30 μs | 47.95 μs |         - |
|    Serialize_NonRandomStringVector_WithSharing |      1024 |         1000 | 45.57 μs | 0.265 μs | 0.412 μs | 45.24 μs | 46.40 μs |         - |

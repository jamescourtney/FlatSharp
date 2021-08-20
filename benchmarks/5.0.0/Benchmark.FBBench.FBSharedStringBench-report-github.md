``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.17763.1757 (1809/October2018Update/Redstone5)
AMD EPYC 7452, 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.103
  [Host]   : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT
  ShortRun : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT

Job=ShortRun  AnalyzeLaunchVariance=True  Runtime=.NET Core 5.0  
IterationCount=7  LaunchCount=7  WarmupCount=5  

```
|                                                            Method | CacheSize | VectorLength |     Mean |    Error |   StdDev |   Median |      P25 |      P50 |      P67 |      P80 |      P90 |      P95 |
|------------------------------------------------------------------ |---------- |------------- |---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|
|                    **Serialize_RandomStringVector_WithRegularString** |       **100** |         **1000** | **29.65 μs** | **0.063 μs** | **0.121 μs** | **29.64 μs** | **29.59 μs** | **29.64 μs** | **29.65 μs** | **29.67 μs** | **29.72 μs** | **29.90 μs** |
|                          Serialize_RandomStringVector_WithSharing |       100 |         1000 | 58.64 μs | 1.620 μs | 3.197 μs | 56.46 μs | 56.21 μs | 56.46 μs | 59.15 μs | 61.75 μs | 64.56 μs | 64.69 μs |
|                       Serialize_NonRandomStringVector_WithSharing |       100 |         1000 | 57.67 μs | 0.974 μs | 1.922 μs | 56.26 μs | 56.11 μs | 56.26 μs | 59.29 μs | 60.10 μs | 60.20 μs | 60.30 μs |
|                      Parse_RepeatedStringVector_WithRegularString |       100 |         1000 | 58.66 μs | 1.151 μs | 2.298 μs | 58.23 μs | 57.11 μs | 58.23 μs | 59.10 μs | 59.79 μs | 60.64 μs | 61.92 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |       100 |         1000 | 81.88 μs | 1.197 μs | 2.277 μs | 82.17 μs | 79.97 μs | 82.17 μs | 83.46 μs | 84.09 μs | 85.12 μs | 85.47 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |       100 |         1000 | 78.87 μs | 1.062 μs | 2.071 μs | 78.11 μs | 77.08 μs | 78.11 μs | 80.61 μs | 81.35 μs | 81.52 μs | 81.83 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |       100 |         1000 | 85.60 μs | 1.111 μs | 2.193 μs | 85.73 μs | 83.89 μs | 85.73 μs | 86.39 μs | 87.19 μs | 88.42 μs | 89.57 μs |
|                    **Serialize_RandomStringVector_WithRegularString** |       **200** |         **1000** | **31.54 μs** | **1.902 μs** | **3.665 μs** | **29.80 μs** | **29.48 μs** | **29.80 μs** | **30.18 μs** | **32.15 μs** | **39.80 μs** | **39.91 μs** |
|                          Serialize_RandomStringVector_WithSharing |       200 |         1000 | 61.07 μs | 0.931 μs | 1.816 μs | 61.55 μs | 58.84 μs | 61.55 μs | 62.04 μs | 63.05 μs | 63.17 μs | 63.36 μs |
|                       Serialize_NonRandomStringVector_WithSharing |       200 |         1000 | 59.74 μs | 1.451 μs | 2.830 μs | 58.90 μs | 58.35 μs | 58.90 μs | 59.10 μs | 61.44 μs | 61.68 μs | 62.37 μs |
|                      Parse_RepeatedStringVector_WithRegularString |       200 |         1000 | 58.15 μs | 1.092 μs | 2.130 μs | 57.98 μs | 56.89 μs | 57.98 μs | 58.50 μs | 60.34 μs | 60.86 μs | 61.19 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |       200 |         1000 | 79.11 μs | 1.212 μs | 2.306 μs | 79.09 μs | 78.22 μs | 79.09 μs | 79.58 μs | 80.69 μs | 82.22 μs | 82.77 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |       200 |         1000 | 74.78 μs | 1.086 μs | 2.145 μs | 74.01 μs | 73.11 μs | 74.01 μs | 74.78 μs | 76.88 μs | 78.71 μs | 78.93 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |       200 |         1000 | 80.50 μs | 1.207 μs | 2.383 μs | 80.23 μs | 78.99 μs | 80.23 μs | 81.08 μs | 81.49 μs | 82.15 μs | 85.64 μs |
|                    **Serialize_RandomStringVector_WithRegularString** |       **400** |         **1000** | **29.66 μs** | **0.199 μs** | **0.392 μs** | **29.51 μs** | **29.39 μs** | **29.51 μs** | **29.69 μs** | **29.84 μs** | **30.40 μs** | **30.57 μs** |
|                          Serialize_RandomStringVector_WithSharing |       400 |         1000 | 61.67 μs | 0.737 μs | 1.420 μs | 61.19 μs | 60.65 μs | 61.19 μs | 61.55 μs | 63.11 μs | 64.43 μs | 64.48 μs |
|                       Serialize_NonRandomStringVector_WithSharing |       400 |         1000 | 59.22 μs | 0.560 μs | 1.078 μs | 58.74 μs | 58.41 μs | 58.74 μs | 59.72 μs | 59.86 μs | 61.30 μs | 61.40 μs |
|                      Parse_RepeatedStringVector_WithRegularString |       400 |         1000 | 58.31 μs | 0.736 μs | 1.453 μs | 58.15 μs | 57.06 μs | 58.15 μs | 58.89 μs | 59.41 μs | 60.52 μs | 60.95 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |       400 |         1000 | 82.37 μs | 1.256 μs | 2.478 μs | 80.88 μs | 80.27 μs | 80.88 μs | 84.39 μs | 85.10 μs | 85.53 μs | 85.81 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |       400 |         1000 | 71.31 μs | 0.680 μs | 1.310 μs | 71.13 μs | 70.26 μs | 71.13 μs | 71.86 μs | 72.78 μs | 73.11 μs | 73.28 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |       400 |         1000 | 78.22 μs | 1.324 μs | 2.550 μs | 77.66 μs | 76.32 μs | 77.66 μs | 78.47 μs | 79.46 μs | 83.34 μs | 83.76 μs |
|                    **Serialize_RandomStringVector_WithRegularString** |       **800** |         **1000** | **30.11 μs** | **0.550 μs** | **1.033 μs** | **29.72 μs** | **29.59 μs** | **29.72 μs** | **29.79 μs** | **29.91 μs** | **32.31 μs** | **32.48 μs** |
|                          Serialize_RandomStringVector_WithSharing |       800 |         1000 | 62.62 μs | 0.552 μs | 1.077 μs | 62.82 μs | 61.91 μs | 62.82 μs | 63.01 μs | 63.21 μs | 64.31 μs | 64.55 μs |
|                       Serialize_NonRandomStringVector_WithSharing |       800 |         1000 | 57.15 μs | 0.507 μs | 1.013 μs | 57.11 μs | 56.50 μs | 57.11 μs | 57.45 μs | 57.70 μs | 58.92 μs | 59.08 μs |
|                      Parse_RepeatedStringVector_WithRegularString |       800 |         1000 | 57.59 μs | 0.704 μs | 1.406 μs | 57.67 μs | 57.02 μs | 57.67 μs | 57.96 μs | 58.47 μs | 59.29 μs | 59.77 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |       800 |         1000 | 80.88 μs | 1.141 μs | 2.280 μs | 80.21 μs | 79.28 μs | 80.21 μs | 80.89 μs | 82.08 μs | 85.05 μs | 85.47 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |       800 |         1000 | 66.79 μs | 1.335 μs | 2.636 μs | 66.25 μs | 65.26 μs | 66.25 μs | 67.72 μs | 68.28 μs | 68.81 μs | 71.88 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |       800 |         1000 | 71.40 μs | 0.991 μs | 1.933 μs | 71.18 μs | 69.76 μs | 71.18 μs | 72.79 μs | 73.24 μs | 73.48 μs | 73.79 μs |

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
|                    **Serialize_RandomStringVector_WithRegularString** |       **100** |         **1000** | **30.13 μs** | **0.476 μs** | **0.939 μs** | **29.75 μs** | **29.66 μs** | **29.75 μs** | **29.87 μs** | **29.98 μs** | **32.23 μs** | **32.42 μs** |
|                          Serialize_RandomStringVector_WithSharing |       100 |         1000 | 56.87 μs | 0.386 μs | 0.734 μs | 56.69 μs | 56.42 μs | 56.69 μs | 56.94 μs | 57.29 μs | 57.83 μs | 58.53 μs |
|                       Serialize_NonRandomStringVector_WithSharing |       100 |         1000 | 56.67 μs | 0.311 μs | 0.614 μs | 56.47 μs | 56.27 μs | 56.47 μs | 56.64 μs | 56.98 μs | 57.76 μs | 58.03 μs |
|                      Parse_RepeatedStringVector_WithRegularString |       100 |         1000 | 60.04 μs | 1.188 μs | 2.288 μs | 59.81 μs | 58.08 μs | 59.81 μs | 60.42 μs | 61.51 μs | 63.41 μs | 64.48 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |       100 |         1000 | 81.63 μs | 1.311 μs | 2.462 μs | 82.17 μs | 79.16 μs | 82.17 μs | 83.07 μs | 83.43 μs | 84.90 μs | 85.17 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |       100 |         1000 | 78.50 μs | 1.496 μs | 2.918 μs | 77.73 μs | 76.30 μs | 77.73 μs | 78.55 μs | 80.84 μs | 82.62 μs | 84.57 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |       100 |         1000 | 84.38 μs | 0.951 μs | 1.854 μs | 84.14 μs | 82.92 μs | 84.14 μs | 85.03 μs | 86.20 μs | 87.06 μs | 87.37 μs |
|                    **Serialize_RandomStringVector_WithRegularString** |       **200** |         **1000** | **29.73 μs** | **0.097 μs** | **0.188 μs** | **29.68 μs** | **29.60 μs** | **29.68 μs** | **29.73 μs** | **29.82 μs** | **29.98 μs** | **30.10 μs** |
|                          Serialize_RandomStringVector_WithSharing |       200 |         1000 | 59.50 μs | 0.270 μs | 0.533 μs | 59.58 μs | 59.03 μs | 59.58 μs | 59.74 μs | 60.03 μs | 60.17 μs | 60.25 μs |
|                       Serialize_NonRandomStringVector_WithSharing |       200 |         1000 | 58.37 μs | 0.286 μs | 0.557 μs | 58.23 μs | 58.00 μs | 58.23 μs | 58.41 μs | 58.89 μs | 59.34 μs | 59.40 μs |
|                      Parse_RepeatedStringVector_WithRegularString |       200 |         1000 | 58.51 μs | 0.615 μs | 1.200 μs | 58.21 μs | 57.76 μs | 58.21 μs | 58.73 μs | 59.02 μs | 60.18 μs | 61.35 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |       200 |         1000 | 81.33 μs | 0.997 μs | 1.897 μs | 81.03 μs | 79.44 μs | 81.03 μs | 82.45 μs | 83.16 μs | 83.52 μs | 84.29 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |       200 |         1000 | 77.22 μs | 0.971 μs | 1.893 μs | 77.15 μs | 75.73 μs | 77.15 μs | 78.07 μs | 78.72 μs | 79.63 μs | 79.91 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |       200 |         1000 | 83.77 μs | 1.343 μs | 2.652 μs | 82.70 μs | 81.68 μs | 82.70 μs | 85.02 μs | 85.70 μs | 86.70 μs | 87.21 μs |
|                    **Serialize_RandomStringVector_WithRegularString** |       **400** |         **1000** | **29.89 μs** | **0.188 μs** | **0.363 μs** | **29.90 μs** | **29.51 μs** | **29.90 μs** | **30.00 μs** | **30.15 μs** | **30.43 μs** | **30.53 μs** |
|                          Serialize_RandomStringVector_WithSharing |       400 |         1000 | 61.53 μs | 0.480 μs | 0.937 μs | 61.35 μs | 60.63 μs | 61.35 μs | 62.12 μs | 62.52 μs | 62.72 μs | 62.92 μs |
|                       Serialize_NonRandomStringVector_WithSharing |       400 |         1000 | 59.57 μs | 0.378 μs | 0.745 μs | 59.69 μs | 59.04 μs | 59.69 μs | 59.95 μs | 60.26 μs | 60.52 μs | 60.61 μs |
|                      Parse_RepeatedStringVector_WithRegularString |       400 |         1000 | 58.43 μs | 0.511 μs | 1.008 μs | 58.41 μs | 57.50 μs | 58.41 μs | 58.82 μs | 59.22 μs | 59.68 μs | 59.91 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |       400 |         1000 | 81.31 μs | 0.808 μs | 1.594 μs | 81.30 μs | 80.21 μs | 81.30 μs | 81.89 μs | 82.27 μs | 83.30 μs | 83.63 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |       400 |         1000 | 72.40 μs | 0.673 μs | 1.344 μs | 72.12 μs | 71.53 μs | 72.12 μs | 72.98 μs | 73.38 μs | 74.50 μs | 74.68 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |       400 |         1000 | 78.60 μs | 0.781 μs | 1.542 μs | 78.72 μs | 77.73 μs | 78.72 μs | 79.37 μs | 80.14 μs | 80.41 μs | 80.68 μs |
|                    **Serialize_RandomStringVector_WithRegularString** |       **800** |         **1000** | **29.82 μs** | **0.322 μs** | **0.606 μs** | **29.60 μs** | **29.51 μs** | **29.60 μs** | **29.75 μs** | **29.87 μs** | **30.54 μs** | **31.00 μs** |
|                          Serialize_RandomStringVector_WithSharing |       800 |         1000 | 62.42 μs | 0.256 μs | 0.505 μs | 62.49 μs | 62.04 μs | 62.49 μs | 62.63 μs | 62.77 μs | 63.01 μs | 63.28 μs |
|                       Serialize_NonRandomStringVector_WithSharing |       800 |         1000 | 58.25 μs | 0.724 μs | 1.429 μs | 57.97 μs | 57.27 μs | 57.97 μs | 59.35 μs | 59.78 μs | 59.93 μs | 60.01 μs |
|                      Parse_RepeatedStringVector_WithRegularString |       800 |         1000 | 59.65 μs | 1.295 μs | 2.587 μs | 58.95 μs | 58.29 μs | 58.95 μs | 59.34 μs | 60.02 μs | 63.93 μs | 65.78 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |       800 |         1000 | 82.11 μs | 1.093 μs | 2.106 μs | 82.03 μs | 80.41 μs | 82.03 μs | 82.62 μs | 83.77 μs | 84.75 μs | 86.09 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |       800 |         1000 | 66.66 μs | 0.904 μs | 1.741 μs | 67.02 μs | 65.48 μs | 67.02 μs | 67.80 μs | 68.52 μs | 68.71 μs | 68.76 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |       800 |         1000 | 72.47 μs | 0.813 μs | 1.547 μs | 72.19 μs | 71.20 μs | 72.19 μs | 72.96 μs | 73.28 μs | 75.10 μs | 75.39 μs |

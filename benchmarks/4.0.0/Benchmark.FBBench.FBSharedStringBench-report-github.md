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
|                                                            Method |                     Job |       Runtime | CacheSize | VectorLength |      Mean |    Error |   StdDev |    Median |
|------------------------------------------------------------------ |------------------------ |-------------- |---------- |------------- |----------:|---------:|---------:|----------:|
|                    **Serialize_RandomStringVector_WithRegularString** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |       **100** |         **1000** | **131.63 μs** | **0.921 μs** | **1.321 μs** | **131.60 μs** |
|                          Serialize_RandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 204.81 μs | 1.682 μs | 2.466 μs | 204.41 μs |
|                       Serialize_NonRandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 196.67 μs | 1.297 μs | 1.941 μs | 196.69 μs |
|                      Parse_RepeatedStringVector_WithRegularString |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 134.92 μs | 1.189 μs | 1.705 μs | 134.43 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 167.14 μs | 1.264 μs | 1.892 μs | 166.91 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 151.14 μs | 1.116 μs | 1.601 μs | 151.59 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 165.38 μs | 1.105 μs | 1.619 μs | 165.20 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  59.89 μs | 0.246 μs | 0.368 μs |  59.82 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  88.15 μs | 0.657 μs | 0.963 μs |  87.90 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  85.78 μs | 0.430 μs | 0.631 μs |  85.79 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  61.44 μs | 0.788 μs | 1.155 μs |  61.57 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  86.78 μs | 0.749 μs | 1.122 μs |  86.32 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  84.57 μs | 0.555 μs | 0.813 μs |  84.64 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  95.63 μs | 0.563 μs | 0.843 μs |  95.56 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  36.68 μs | 0.213 μs | 0.319 μs |  36.68 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  68.84 μs | 0.477 μs | 0.714 μs |  68.85 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  68.45 μs | 0.901 μs | 1.348 μs |  68.27 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  57.74 μs | 0.742 μs | 1.110 μs |  57.57 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  83.89 μs | 1.268 μs | 1.819 μs |  83.22 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  80.04 μs | 1.014 μs | 1.454 μs |  79.55 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  92.40 μs | 1.121 μs | 1.678 μs |  91.77 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       100 |         1000 |  34.99 μs | 0.224 μs | 0.335 μs |  35.00 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       100 |         1000 |  68.32 μs | 1.942 μs | 2.785 μs |  69.90 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       100 |         1000 |  65.39 μs | 0.934 μs | 1.339 μs |  64.73 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       100 |         1000 |  59.61 μs | 1.361 μs | 1.996 μs |  58.87 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       100 |         1000 |  82.37 μs | 1.765 μs | 2.475 μs |  82.29 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       100 |         1000 |  81.27 μs | 0.938 μs | 1.345 μs |  80.86 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       100 |         1000 |  91.90 μs | 1.424 μs | 2.042 μs |  92.12 μs |
|                    **Serialize_RandomStringVector_WithRegularString** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |       **200** |         **1000** | **131.20 μs** | **0.967 μs** | **1.387 μs** | **131.23 μs** |
|                          Serialize_RandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 196.61 μs | 0.932 μs | 1.395 μs | 196.31 μs |
|                       Serialize_NonRandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 187.21 μs | 0.979 μs | 1.436 μs | 187.04 μs |
|                      Parse_RepeatedStringVector_WithRegularString |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 134.63 μs | 1.306 μs | 1.955 μs | 134.31 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 170.21 μs | 1.532 μs | 2.197 μs | 169.50 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 147.66 μs | 0.819 μs | 1.226 μs | 147.48 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 161.53 μs | 0.844 μs | 1.237 μs | 161.36 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  60.10 μs | 0.262 μs | 0.384 μs |  60.04 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  87.14 μs | 0.560 μs | 0.820 μs |  87.09 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  85.05 μs | 0.538 μs | 0.789 μs |  84.91 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  62.97 μs | 0.327 μs | 0.459 μs |  62.81 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  88.03 μs | 0.445 μs | 0.666 μs |  87.94 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  79.80 μs | 0.549 μs | 0.805 μs |  79.77 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  93.28 μs | 0.523 μs | 0.783 μs |  93.16 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  36.30 μs | 0.243 μs | 0.357 μs |  36.18 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  69.83 μs | 0.489 μs | 0.685 μs |  69.60 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  68.32 μs | 0.924 μs | 1.384 μs |  68.46 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  58.58 μs | 1.450 μs | 2.170 μs |  58.39 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  82.46 μs | 1.020 μs | 1.463 μs |  82.89 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  76.90 μs | 0.675 μs | 0.947 μs |  76.76 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  87.63 μs | 0.696 μs | 1.042 μs |  87.33 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       200 |         1000 |  34.32 μs | 0.169 μs | 0.248 μs |  34.27 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       200 |         1000 |  68.62 μs | 0.654 μs | 0.917 μs |  68.59 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       200 |         1000 |  66.40 μs | 0.683 μs | 0.980 μs |  66.65 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       200 |         1000 |  59.17 μs | 1.216 μs | 1.821 μs |  59.43 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       200 |         1000 |  88.79 μs | 1.708 μs | 2.557 μs |  88.73 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       200 |         1000 |  78.63 μs | 1.359 μs | 1.992 μs |  78.55 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       200 |         1000 |  90.19 μs | 1.028 μs | 1.507 μs |  90.08 μs |
|                    **Serialize_RandomStringVector_WithRegularString** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |       **400** |         **1000** | **129.64 μs** | **1.302 μs** | **1.867 μs** | **129.69 μs** |
|                          Serialize_RandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 191.47 μs | 2.059 μs | 3.018 μs | 191.35 μs |
|                       Serialize_NonRandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 178.92 μs | 2.531 μs | 3.710 μs | 180.40 μs |
|                      Parse_RepeatedStringVector_WithRegularString |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 134.20 μs | 0.701 μs | 1.028 μs | 134.21 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 166.65 μs | 0.897 μs | 1.286 μs | 166.50 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 133.95 μs | 1.075 μs | 1.608 μs | 133.83 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 146.42 μs | 0.876 μs | 1.256 μs | 146.10 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  59.91 μs | 0.226 μs | 0.331 μs |  59.90 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  88.02 μs | 0.474 μs | 0.665 μs |  87.85 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  84.15 μs | 0.369 μs | 0.552 μs |  84.02 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  60.24 μs | 0.410 μs | 0.602 μs |  60.16 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  88.98 μs | 0.692 μs | 1.035 μs |  88.96 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  74.79 μs | 0.895 μs | 1.312 μs |  75.17 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  88.86 μs | 0.711 μs | 1.064 μs |  88.53 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  36.58 μs | 0.276 μs | 0.404 μs |  36.60 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  70.55 μs | 1.016 μs | 1.489 μs |  70.19 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  67.68 μs | 0.734 μs | 1.099 μs |  67.52 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  55.43 μs | 0.934 μs | 1.339 μs |  55.22 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  83.60 μs | 0.740 μs | 1.085 μs |  83.38 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  70.24 μs | 0.938 μs | 1.404 μs |  70.65 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  84.90 μs | 0.861 μs | 1.288 μs |  84.71 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       400 |         1000 |  35.96 μs | 0.180 μs | 0.252 μs |  35.86 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       400 |         1000 |  68.93 μs | 0.931 μs | 1.305 μs |  68.37 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       400 |         1000 |  67.24 μs | 0.382 μs | 0.560 μs |  67.19 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       400 |         1000 |  58.33 μs | 0.877 μs | 1.313 μs |  58.71 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       400 |         1000 |  84.05 μs | 1.698 μs | 2.435 μs |  84.54 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       400 |         1000 |  73.76 μs | 0.663 μs | 0.929 μs |  73.57 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       400 |         1000 |  90.31 μs | 2.761 μs | 4.132 μs |  89.70 μs |
|                    **Serialize_RandomStringVector_WithRegularString** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |       **800** |         **1000** | **131.60 μs** | **1.021 μs** | **1.496 μs** | **131.51 μs** |
|                          Serialize_RandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 191.88 μs | 2.561 μs | 3.754 μs | 191.16 μs |
|                       Serialize_NonRandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 172.60 μs | 4.082 μs | 5.983 μs | 175.60 μs |
|                      Parse_RepeatedStringVector_WithRegularString |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 133.49 μs | 1.309 μs | 1.959 μs | 133.18 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 166.02 μs | 0.871 μs | 1.276 μs | 165.79 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 123.88 μs | 0.562 μs | 0.841 μs | 123.71 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 141.64 μs | 1.555 μs | 2.180 μs | 142.26 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  60.04 μs | 0.267 μs | 0.400 μs |  60.01 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  90.81 μs | 0.657 μs | 0.983 μs |  90.72 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  81.81 μs | 1.078 μs | 1.580 μs |  81.99 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  61.79 μs | 1.701 μs | 2.547 μs |  60.68 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  88.03 μs | 0.686 μs | 1.026 μs |  88.23 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  70.80 μs | 0.566 μs | 0.847 μs |  70.93 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  81.62 μs | 0.492 μs | 0.690 μs |  81.68 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  36.49 μs | 0.378 μs | 0.542 μs |  36.39 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  73.67 μs | 0.721 μs | 1.079 μs |  73.69 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  67.49 μs | 0.536 μs | 0.751 μs |  67.61 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  53.56 μs | 0.343 μs | 0.492 μs |  53.50 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  81.27 μs | 1.580 μs | 2.316 μs |  79.88 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  68.92 μs | 0.825 μs | 1.101 μs |  68.64 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  81.38 μs | 1.300 μs | 1.905 μs |  81.46 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       800 |         1000 |  34.93 μs | 0.266 μs | 0.398 μs |  34.89 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       800 |         1000 |  71.64 μs | 0.596 μs | 0.892 μs |  71.65 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       800 |         1000 |  67.40 μs | 0.421 μs | 0.617 μs |  67.49 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       800 |         1000 |  55.92 μs | 0.634 μs | 0.949 μs |  56.10 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       800 |         1000 |  85.54 μs | 0.575 μs | 0.861 μs |  85.67 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       800 |         1000 |  68.75 μs | 0.898 μs | 1.288 μs |  69.27 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       800 |         1000 |  81.41 μs | 0.494 μs | 0.709 μs |  81.48 μs |

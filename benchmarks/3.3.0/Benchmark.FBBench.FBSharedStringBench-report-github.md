``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.450 (2004/?/20H1)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET Core SDK=3.1.401
  [Host]                  : .NET Core 2.1.21 (CoreCLR 4.6.29130.01, CoreFX 4.6.29130.02), X64 RyuJIT
  MediumRun-.NET 4.7      : .NET Framework 4.8 (4.8.4200.0), X64 RyuJIT
  MediumRun-.NET Core 2.1 : .NET Core 2.1.21 (CoreCLR 4.6.29130.01, CoreFX 4.6.29130.02), X64 RyuJIT
  MediumRun-.NET Core 3.1 : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

IterationCount=15  LaunchCount=2  WarmupCount=10  

```
|                                                            Method |                     Job |       Runtime | CacheSize | VectorLength |      Mean |    Error |   StdDev |    Median |
|------------------------------------------------------------------ |------------------------ |-------------- |---------- |------------- |----------:|---------:|---------:|----------:|
|                    **Serialize_RandomStringVector_WithRegularString** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |       **100** |         **1000** | **111.27 μs** | **0.854 μs** | **1.225 μs** | **112.05 μs** |
|                          Serialize_RandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 149.28 μs | 0.522 μs | 0.748 μs | 149.16 μs |
|                       Serialize_NonRandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 143.73 μs | 0.598 μs | 0.876 μs | 143.69 μs |
|                      Parse_RepeatedStringVector_WithRegularString |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 105.08 μs | 2.614 μs | 3.832 μs | 107.76 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 132.52 μs | 0.614 μs | 0.861 μs | 132.98 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 122.13 μs | 2.758 μs | 3.866 μs | 118.90 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 129.38 μs | 1.044 μs | 1.497 μs | 129.17 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  48.71 μs | 0.107 μs | 0.160 μs |  48.67 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  53.90 μs | 2.952 μs | 4.419 μs |  54.34 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  52.93 μs | 3.072 μs | 4.599 μs |  53.15 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  48.17 μs | 0.485 μs | 0.648 μs |  48.38 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  64.74 μs | 0.451 μs | 0.633 μs |  64.63 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  62.13 μs | 0.295 μs | 0.424 μs |  61.91 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  66.42 μs | 0.612 μs | 0.917 μs |  66.38 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  29.30 μs | 1.775 μs | 2.430 μs |  27.16 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  39.14 μs | 0.172 μs | 0.247 μs |  39.17 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  40.35 μs | 0.519 μs | 0.745 μs |  40.45 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  43.25 μs | 0.492 μs | 0.721 μs |  43.11 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  58.53 μs | 0.281 μs | 0.421 μs |  58.59 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  56.08 μs | 1.421 μs | 2.127 μs |  56.20 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  60.70 μs | 1.549 μs | 2.222 μs |  61.73 μs |
|                    **Serialize_RandomStringVector_WithRegularString** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |       **200** |         **1000** | **109.91 μs** | **0.350 μs** | **0.502 μs** | **109.97 μs** |
|                          Serialize_RandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 140.01 μs | 0.715 μs | 1.048 μs | 140.21 μs |
|                       Serialize_NonRandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 135.29 μs | 0.819 μs | 1.226 μs | 135.50 μs |
|                      Parse_RepeatedStringVector_WithRegularString |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 104.66 μs | 2.257 μs | 3.379 μs | 104.68 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 139.53 μs | 1.689 μs | 2.367 μs | 141.14 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 125.09 μs | 1.535 μs | 2.250 μs | 126.48 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 128.14 μs | 0.208 μs | 0.304 μs | 128.17 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  43.35 μs | 4.046 μs | 5.672 μs |  48.29 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  54.64 μs | 2.832 μs | 4.239 μs |  54.68 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  53.64 μs | 2.820 μs | 4.045 μs |  56.97 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  47.79 μs | 0.566 μs | 0.794 μs |  47.19 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  65.53 μs | 0.354 μs | 0.529 μs |  65.59 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  57.13 μs | 0.253 μs | 0.379 μs |  57.01 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  60.26 μs | 3.030 μs | 4.535 μs |  60.80 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  30.81 μs | 2.354 μs | 3.451 μs |  33.99 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  40.54 μs | 0.259 μs | 0.354 μs |  40.54 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  40.93 μs | 0.181 μs | 0.272 μs |  41.01 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  44.21 μs | 0.424 μs | 0.608 μs |  44.28 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  56.91 μs | 0.854 μs | 1.278 μs |  57.37 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  53.64 μs | 0.456 μs | 0.683 μs |  53.61 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  57.65 μs | 1.735 μs | 2.432 μs |  59.66 μs |
|                    **Serialize_RandomStringVector_WithRegularString** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |       **400** |         **1000** | **109.69 μs** | **0.094 μs** | **0.122 μs** | **109.69 μs** |
|                          Serialize_RandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 139.43 μs | 0.719 μs | 1.053 μs | 139.78 μs |
|                       Serialize_NonRandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 128.41 μs | 1.783 μs | 2.614 μs | 129.37 μs |
|                      Parse_RepeatedStringVector_WithRegularString |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 |  99.54 μs | 0.645 μs | 0.966 μs |  99.62 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 137.94 μs | 0.898 μs | 1.288 μs | 137.99 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 111.95 μs | 0.295 μs | 0.442 μs | 111.91 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 112.12 μs | 2.510 μs | 3.680 μs | 109.61 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  48.80 μs | 0.175 μs | 0.256 μs |  48.72 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  58.82 μs | 0.168 μs | 0.241 μs |  58.88 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  53.64 μs | 2.494 μs | 3.656 μs |  56.84 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  50.94 μs | 3.214 μs | 4.609 μs |  54.97 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  60.30 μs | 0.634 μs | 0.948 μs |  60.55 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  51.06 μs | 1.592 μs | 2.384 μs |  51.08 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  61.09 μs | 1.395 μs | 2.045 μs |  61.42 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  27.55 μs | 0.379 μs | 0.531 μs |  27.77 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  43.80 μs | 0.352 μs | 0.505 μs |  43.58 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  42.54 μs | 0.297 μs | 0.435 μs |  42.28 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  44.61 μs | 0.651 μs | 0.954 μs |  44.39 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  57.74 μs | 0.775 μs | 1.161 μs |  57.52 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  52.64 μs | 1.562 μs | 2.289 μs |  53.39 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  56.10 μs | 1.276 μs | 1.830 μs |  56.23 μs |
|                    **Serialize_RandomStringVector_WithRegularString** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |       **800** |         **1000** | **111.55 μs** | **1.036 μs** | **1.550 μs** | **111.78 μs** |
|                          Serialize_RandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 136.50 μs | 1.394 μs | 2.043 μs | 136.44 μs |
|                       Serialize_NonRandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 124.84 μs | 1.532 μs | 2.197 μs | 126.20 μs |
|                      Parse_RepeatedStringVector_WithRegularString |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 102.03 μs | 0.324 μs | 0.484 μs | 102.11 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 137.07 μs | 4.195 μs | 5.881 μs | 139.83 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 |  95.91 μs | 4.545 μs | 6.803 μs |  96.78 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 102.44 μs | 3.893 μs | 5.707 μs |  98.24 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  45.56 μs | 4.732 μs | 6.786 μs |  51.59 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  57.76 μs | 2.838 μs | 4.071 μs |  54.96 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  52.94 μs | 2.949 μs | 4.322 μs |  49.28 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  45.02 μs | 1.599 μs | 2.242 μs |  43.38 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  69.61 μs | 0.235 μs | 0.330 μs |  69.63 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  48.42 μs | 1.181 μs | 1.731 μs |  49.59 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  51.90 μs | 0.986 μs | 1.476 μs |  52.19 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  30.02 μs | 2.052 μs | 2.943 μs |  31.92 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  44.84 μs | 0.184 μs | 0.270 μs |  44.86 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  42.40 μs | 0.683 μs | 0.957 μs |  42.80 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  47.85 μs | 0.565 μs | 0.846 μs |  47.79 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  62.27 μs | 2.091 μs | 2.999 μs |  60.29 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  47.88 μs | 0.623 μs | 0.933 μs |  48.29 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  50.89 μs | 0.684 μs | 1.024 μs |  50.72 μs |

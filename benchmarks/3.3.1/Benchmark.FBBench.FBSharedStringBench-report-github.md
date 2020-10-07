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
|                    **Serialize_RandomStringVector_WithRegularString** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |       **100** |         **1000** | **162.12 μs** | **3.168 μs** | **4.742 μs** | **161.77 μs** |
|                          Serialize_RandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 212.23 μs | 1.101 μs | 1.543 μs | 212.31 μs |
|                       Serialize_NonRandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 205.15 μs | 1.158 μs | 1.661 μs | 205.16 μs |
|                      Parse_RepeatedStringVector_WithRegularString |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 147.96 μs | 1.430 μs | 2.140 μs | 147.64 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 173.39 μs | 1.404 μs | 2.058 μs | 172.66 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 159.39 μs | 1.668 μs | 2.445 μs | 159.75 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       100 |         1000 | 173.99 μs | 1.082 μs | 1.587 μs | 173.37 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  59.19 μs | 0.280 μs | 0.411 μs |  59.12 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  75.64 μs | 0.373 μs | 0.559 μs |  75.64 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  73.58 μs | 0.338 μs | 0.484 μs |  73.47 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  64.76 μs | 1.308 μs | 1.957 μs |  64.77 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  83.02 μs | 0.388 μs | 0.556 μs |  82.81 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  81.54 μs | 1.266 μs | 1.896 μs |  81.78 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       100 |         1000 |  92.06 μs | 1.048 μs | 1.504 μs |  92.47 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  41.72 μs | 0.815 μs | 1.168 μs |  41.78 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  62.79 μs | 0.590 μs | 0.864 μs |  62.69 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  62.96 μs | 0.425 μs | 0.623 μs |  62.89 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  57.20 μs | 0.576 μs | 0.827 μs |  56.97 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  82.36 μs | 1.002 μs | 1.468 μs |  82.32 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  76.06 μs | 0.712 μs | 1.022 μs |  76.24 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       100 |         1000 |  91.84 μs | 1.053 μs | 1.576 μs |  91.97 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       100 |         1000 |  38.96 μs | 0.431 μs | 0.645 μs |  39.26 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       100 |         1000 |  58.39 μs | 0.492 μs | 0.706 μs |  58.43 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       100 |         1000 |  57.27 μs | 0.197 μs | 0.277 μs |  57.23 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       100 |         1000 |  58.51 μs | 0.426 μs | 0.637 μs |  58.28 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       100 |         1000 |  79.03 μs | 0.561 μs | 0.787 μs |  79.18 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       100 |         1000 |  74.37 μs | 0.993 μs | 1.424 μs |  74.40 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       100 |         1000 |  90.47 μs | 0.944 μs | 1.413 μs |  90.32 μs |
|                    **Serialize_RandomStringVector_WithRegularString** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |       **200** |         **1000** | **160.87 μs** | **3.010 μs** | **4.219 μs** | **159.18 μs** |
|                          Serialize_RandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 203.46 μs | 1.626 μs | 2.279 μs | 202.50 μs |
|                       Serialize_NonRandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 194.30 μs | 1.521 μs | 2.276 μs | 193.83 μs |
|                      Parse_RepeatedStringVector_WithRegularString |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 146.43 μs | 1.103 μs | 1.616 μs | 146.53 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 174.45 μs | 1.861 μs | 2.669 μs | 173.24 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 157.43 μs | 1.782 μs | 2.667 μs | 156.93 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       200 |         1000 | 170.00 μs | 1.136 μs | 1.665 μs | 169.72 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  59.37 μs | 0.348 μs | 0.476 μs |  59.37 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  76.00 μs | 0.409 μs | 0.612 μs |  76.01 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  74.76 μs | 0.763 μs | 1.094 μs |  74.76 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  62.08 μs | 0.388 μs | 0.569 μs |  61.99 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  87.00 μs | 0.633 μs | 0.947 μs |  86.91 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  77.72 μs | 0.777 μs | 1.163 μs |  77.35 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       200 |         1000 |  90.37 μs | 0.683 μs | 1.002 μs |  90.45 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  40.35 μs | 0.209 μs | 0.312 μs |  40.40 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  64.40 μs | 0.521 μs | 0.780 μs |  64.34 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  63.85 μs | 1.652 μs | 2.422 μs |  65.35 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  59.25 μs | 0.462 μs | 0.677 μs |  59.11 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  82.03 μs | 1.103 μs | 1.650 μs |  82.25 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  71.89 μs | 1.506 μs | 2.061 μs |  70.89 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       200 |         1000 |  87.50 μs | 0.866 μs | 1.242 μs |  87.33 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       200 |         1000 |  39.80 μs | 0.788 μs | 1.180 μs |  39.69 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       200 |         1000 |  60.79 μs | 0.424 μs | 0.622 μs |  60.73 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       200 |         1000 |  59.28 μs | 0.295 μs | 0.432 μs |  59.29 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       200 |         1000 |  57.16 μs | 0.856 μs | 1.281 μs |  57.00 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       200 |         1000 |  79.78 μs | 1.242 μs | 1.820 μs |  79.40 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       200 |         1000 |  71.71 μs | 1.119 μs | 1.569 μs |  72.51 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       200 |         1000 |  90.67 μs | 0.938 μs | 1.404 μs |  90.66 μs |
|                    **Serialize_RandomStringVector_WithRegularString** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |       **400** |         **1000** | **157.25 μs** | **0.691 μs** | **1.035 μs** | **157.08 μs** |
|                          Serialize_RandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 198.33 μs | 1.232 μs | 1.845 μs | 198.49 μs |
|                       Serialize_NonRandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 179.81 μs | 1.077 μs | 1.510 μs | 179.54 μs |
|                      Parse_RepeatedStringVector_WithRegularString |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 144.60 μs | 0.487 μs | 0.682 μs | 144.48 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 174.93 μs | 1.489 μs | 2.183 μs | 174.38 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 141.21 μs | 1.487 μs | 2.132 μs | 141.82 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       400 |         1000 | 155.40 μs | 1.106 μs | 1.656 μs | 155.58 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  59.23 μs | 0.346 μs | 0.507 μs |  59.11 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  78.23 μs | 0.564 μs | 0.827 μs |  78.09 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  73.51 μs | 0.299 μs | 0.428 μs |  73.40 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  62.50 μs | 0.649 μs | 0.972 μs |  62.33 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  83.73 μs | 2.364 μs | 3.465 μs |  86.26 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  72.37 μs | 0.890 μs | 1.332 μs |  72.24 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       400 |         1000 |  86.25 μs | 0.708 μs | 1.060 μs |  86.54 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  40.35 μs | 0.640 μs | 0.938 μs |  40.93 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  67.07 μs | 0.435 μs | 0.651 μs |  66.95 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  63.89 μs | 0.693 μs | 1.015 μs |  63.93 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  58.61 μs | 0.836 μs | 1.199 μs |  58.75 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  80.36 μs | 0.774 μs | 1.159 μs |  80.18 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  70.20 μs | 1.346 μs | 1.886 μs |  70.28 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       400 |         1000 |  81.85 μs | 0.872 μs | 1.251 μs |  82.25 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       400 |         1000 |  38.85 μs | 0.202 μs | 0.296 μs |  38.78 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       400 |         1000 |  63.59 μs | 0.499 μs | 0.731 μs |  63.76 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       400 |         1000 |  61.40 μs | 0.438 μs | 0.642 μs |  61.49 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       400 |         1000 |  57.17 μs | 0.896 μs | 1.313 μs |  57.66 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       400 |         1000 |  78.60 μs | 1.297 μs | 1.901 μs |  78.11 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       400 |         1000 |  66.32 μs | 0.936 μs | 1.373 μs |  65.87 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       400 |         1000 |  84.32 μs | 1.211 μs | 1.813 μs |  84.03 μs |
|                    **Serialize_RandomStringVector_WithRegularString** |      **MediumRun-.NET 4.7** |      **.NET 4.7** |       **800** |         **1000** | **157.71 μs** | **1.109 μs** | **1.591 μs** | **157.55 μs** |
|                          Serialize_RandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 199.57 μs | 1.504 μs | 2.158 μs | 200.22 μs |
|                       Serialize_NonRandomStringVector_WithSharing |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 177.67 μs | 1.009 μs | 1.447 μs | 177.99 μs |
|                      Parse_RepeatedStringVector_WithRegularString |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 146.04 μs | 0.976 μs | 1.430 μs | 145.55 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 173.60 μs | 0.920 μs | 1.320 μs | 173.21 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 121.84 μs | 2.920 μs | 4.371 μs | 121.88 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe |      MediumRun-.NET 4.7 |      .NET 4.7 |       800 |         1000 | 145.14 μs | 1.178 μs | 1.726 μs | 144.80 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  59.63 μs | 0.372 μs | 0.545 μs |  59.56 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  81.42 μs | 0.642 μs | 0.921 μs |  80.99 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  73.38 μs | 0.364 μs | 0.534 μs |  73.14 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  62.06 μs | 0.454 μs | 0.651 μs |  61.79 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  86.68 μs | 1.112 μs | 1.665 μs |  86.79 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  67.66 μs | 0.516 μs | 0.756 μs |  67.81 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 2.1 | .NET Core 2.1 |       800 |         1000 |  80.75 μs | 1.008 μs | 1.508 μs |  80.96 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  40.46 μs | 0.266 μs | 0.398 μs |  40.28 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  68.36 μs | 0.470 μs | 0.703 μs |  68.50 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  64.79 μs | 0.469 μs | 0.673 μs |  64.69 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  60.76 μs | 1.753 μs | 2.570 μs |  59.70 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  81.97 μs | 0.768 μs | 1.125 μs |  82.13 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  64.98 μs | 0.542 μs | 0.777 μs |  64.89 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 3.1 | .NET Core 3.1 |       800 |         1000 |  79.53 μs | 0.804 μs | 1.179 μs |  79.23 μs |
|                    Serialize_RandomStringVector_WithRegularString | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       800 |         1000 |  38.49 μs | 0.150 μs | 0.225 μs |  38.46 μs |
|                          Serialize_RandomStringVector_WithSharing | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       800 |         1000 |  66.92 μs | 0.522 μs | 0.781 μs |  66.85 μs |
|                       Serialize_NonRandomStringVector_WithSharing | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       800 |         1000 |  63.32 μs | 0.627 μs | 0.920 μs |  63.72 μs |
|                      Parse_RepeatedStringVector_WithRegularString | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       800 |         1000 |  64.78 μs | 0.968 μs | 1.389 μs |  64.80 μs |
|                      Parse_RepeatedStringVector_WithSharedStrings | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       800 |         1000 |  81.01 μs | 1.378 μs | 2.021 μs |  80.65 μs |
| Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       800 |         1000 |  63.81 μs | 1.168 μs | 1.749 μs |  64.32 μs |
|    Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe | MediumRun-.NET Core 5.0 | .NET Core 5.0 |       800 |         1000 |  76.71 μs | 0.935 μs | 1.399 μs |  76.54 μs |

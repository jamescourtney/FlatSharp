``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.18363
AMD Ryzen 9 3900X 12-Core Processor (Max: 3.80GHz), 1 CPU, 24 logical and 12 physical cores
.NET Core SDK=3.1.200
  [Host]   : .NET Core 2.1.16 (CoreCLR 4.6.28516.03, CoreFX 4.6.28516.10), 64bit RyuJIT
  ShortRun : .NET Core 2.1.16 (CoreCLR 4.6.28516.03, CoreFX 4.6.28516.10), 64bit RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                       Method | VectorLength |        Mean |      Error |     StdDev |
|----------------------------- |------------- |------------:|-----------:|-----------:|
| **Google_FlatBuffers_Serialize** |            **3** | **1,160.86 ns** |  **32.828 ns** |  **1.8548 ns** |
|         FlatSharp_GetMaxSize |            3 |    64.94 ns |  17.989 ns |  1.0164 ns |
|          FlatSharp_Serialize |            3 |   615.20 ns |  35.624 ns |  2.0128 ns |
|               PBDN_Serialize |            3 | 1,331.25 ns |  27.770 ns |  1.5690 ns |
| **Google_FlatBuffers_Serialize** |           **30** | **9,276.51 ns** | **430.944 ns** | **24.3491 ns** |
|         FlatSharp_GetMaxSize |           30 |   254.81 ns |   4.097 ns |  0.2315 ns |
|          FlatSharp_Serialize |           30 | 4,600.13 ns | 167.040 ns |  9.4381 ns |
|               PBDN_Serialize |           30 | 9,034.99 ns | 566.687 ns | 32.0188 ns |

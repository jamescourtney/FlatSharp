dotnet build -c release

pushd Benchmarks\Benchmark\bin\release\net5.0
Benchmark.exe
popd

pushd Benchmarks\Benchmark.5.7.1\bin\release\net5.0
Benchmark.5.7.1.exe
popd

pushd Benchmarks\Benchmark.5.0.0\bin\release\net5.0
Benchmark.5.0.0.exe
popd

pushd Benchmarks\Benchmark.4.0.0\bin\release\net5.0
Benchmark.4.0.0.exe
popd

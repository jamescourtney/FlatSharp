dotnet build -c release

pushd Benchmarks\Benchmark\bin\release\net7.0
Benchmark.exe
popd

pushd Benchmarks\Benchmark.6.0.0\bin\release\net7.0
Benchmark.6.0.0.exe
popd



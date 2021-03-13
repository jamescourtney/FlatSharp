dotnet build -c release

pushd Benchmark\bin\release\net5.0
Benchmark.exe
popd

pushd Benchmark.5.0.0\bin\release\net5.0
Benchmark.4.0.0.exe
popd

pushd Benchmark.5.0.0\bin\release\net5.0
Benchmark.4.0.0.exe
popd

pushd Benchmark.3.3.0\bin\release\net5.0
Benchmark.3.3.0.exe
popd

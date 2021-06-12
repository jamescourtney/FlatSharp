dotnet build -c release

pushd Benchmark\bin\Release\net5.0
.\Benchmark.exe
popd

pushd Benchmark.5.1.0\bin\Release\net5.0
.\Benchmark.5.1.0.exe
popd

pushd Benchmark.5.0.0\bin\Release\net5.0
.\Benchmark.5.0.0.exe
popd

pushd Benchmark.4.0.0\bin\Release\net5.0
.\Benchmark.4.0.0.exe
popd

pushd Benchmark.3.3.0\bin\Release\net5.0
.\Benchmark.3.3.0.exe
popd

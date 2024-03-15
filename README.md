## FlatSharp
![Main](https://github.com/jamescourtney/FlatSharp/actions/workflows/dotnet.yml/badge.svg?branch=main)
[![codecov](https://codecov.io/gh/jamescourtney/FlatSharp/branch/main/graph/badge.svg?token=6EUECHZGT4)](https://codecov.io/gh/jamescourtney/FlatSharp)

FlatSharp is Google's FlatBuffers serialization format implemented in C#, for C#. FlatBuffers is a zero-copy binary serialization format intended for high-performance scenarios. 
FlatSharp leverages the latest and greatest from .NET in the form of `Memory<T>` and `Span<T>`.
As such, FlatSharp's safe-code implementations are often faster than other implementations using unsafe code. FlatSharp aims to provide 4 core priorities:

- Usability
- Safety & Speed
- Compatibility with other C#-focused projects like Unity, Blazor, and Xamarin.
- Compatibility with AOT scenarios including dotnet and Mono.

All FlatSharp packages are published on nuget.org:
- **FlatSharp.Runtime**: The runtime library. You always need this.
- **FlatSharp.Compiler**: Build time compiler for generating C# from an FBS schema.

FlatSharp is a mature library and has been shipped to production at Microsoft, Unity3D, and others. Full status can be found at [ProjectStatus.md](ProjectStatus.md). FlatSharp is [extensively tested](https://github.com/jamescourtney/FlatSharp/wiki/Testing), using Mutation Testing, Code Coverage, Oracle Testing, and other techniques to ensure the library doesn't regress.

### Issues, Contributions, and Feedback
Don't be a stranger! All issues and feedback are welcome here. If you'd like to share how you use FlatSharp, please consider filling out the form [here](https://forms.office.com/r/sHkumrr6sK)!

### Sponsorship
FlatSharp is free and always will be. However, the project does take a significant amount of time to maintain. If you or your organization find the project useful, please consider a [Github sponsorship](https://github.com/sponsors/jamescourtney). Any amount is appreciated!

### Getting Started
If you're completely new to FlatBuffers, take a minute to look over [the FlatBuffer overview](https://google.github.io/flatbuffers/index.html#flatbuffers_overview). Additionally, it's worth the time to understand the different elements of [FlatBuffer schemas](https://google.github.io/flatbuffers/flatbuffers_guide_writing_schema.html).

### Quick Start

#### 1. Reference FlatSharp
Reference both `FlatSharp.Runtime` and `FlatSharp.Compiler` from NuGet. Use the same version for both.

#### 2. Define a Schema

```idl
// all FlatSharp FBS attributes start with the 'fs_' prefix.
attribute "fs_serializer";

namespace MyNamespace;

enum Color : ubyte { Red = 1, Green, Blue }

table Person (fs_serializer) {
    id : int;
    name : string;
    parent : Person (deprecated);
    children : [ Person ];
    favorite_color : Color = Blue;
    position : Location;
}

struct Location {
    latitude : float;
    longitude : float;
}
```

#### 3. Update Your csproj
```xml
<ItemGroup>
  <FlatSharpSchema Include="YourSchema.fbs" />
</ItemGroup>
```

#### 4. Serialize Your Data
```c#
Person person = new Person(...);
int maxBytesNeeded = Person.Serializer.GetMaxSize(person);
byte[] buffer = new byte[maxBytesNeeded];
int bytesWritten = Person.Serializer.Serialize(buffer, person);
```

#### 5. Parse Your Data

```c#
Person p = Person.Serializer.Parse(data);
```

### Samples & Documentation
FlatSharp supports some interesting features not covered here. Detailed documentation is in the [wiki](https://github.com/jamescourtney/FlatSharp/wiki). The [samples solution](samples/) is a good tutorial and has full examples of:
- [Basic Usage](samples/Example00-Basics/)
- [Deserialization Modes (Lazy, Greedy, and everything in between)](samples/Example01-DeserializationModes/)
- [Vectors (Lists)](samples/Example02-Vectors/)
- [IO Options](samples/Example03-IOOptions/)
- [gRPC](samples/Example04-gRPC/)
- [Copy Constructors](samples/Example05-CopyConstructors/)
- [FBS Includes](samples/Example06-Includes/)
- [Sorted Vectors](samples/Example07-SortedVectors/)
- [Indexed Vectors](samples/Example08-IndexedVectors/)
- [Unions](samples/Example09-Unions/)
- [String deduplication](samples/Example10-SharedStrings/)
- [Fixed-Length Struct Vectors](samples/Example11-StructVectors/)
- [Write-Through (update buffers in place)](samples/Example12-WriteThrough/)
- [Value Type Structs](samples/Example13-ValueStructs/)
- [Unsafe Options](samples/Example14-UnsafeOptions/)

### Internals
FlatSharp works by generating subclasses of your data contracts based on the schema that you define. 
That is, when you attempt to deserialize a `MonsterTable` object, you actually get back a subclass of `MonsterTable`, 
which has properties defined in such a way as to index into the buffer, according to the deserialization mode specified (greedy, lazy, etc).

### Security
Serializers are a common vector for security issues. FlatSharp takes the following approach to security:
- No unsafe code; all operations bounds-checked
- No IL generation
- Use standard .NET libraries for reading and writing from memory
- Depth tracking enabled for recursive schemas to prevent stack overflows.

FlatSharp *does* use some techniques such as `MemoryMarshal.Read` on certain hot paths, but these usages are narrowly scoped and well tested.

### Performance & Benchmarks
FlatSharp is really, really fast. The FlatSharp benchmarks were run on .NET 8.0 with PGO enabled using a C# approximation of [Google's FlatBuffer benchmark](https://github.com/google/flatbuffers/tree/benchmarks/benchmarks/cpp/FB), which can be found [here](src/Benchmark).

The benchmarks test 4 different serialization frameworks, all using default settings:
- FlatSharp -- 7.5.0
- Protobuf.NET -- 3.2.30
- Google's C# Flatbuffers -- 22.10.26
- Message Pack C# -- 2.5.140

The full results for each version of FlatSharp can be viewed in the [benchmarks folder](benchmarks). Additionally, the benchmark data contains performance data for many different configurations of FlatSharp and other features, such as sorted vectors and shared strings.

#### Word of Warning
Serialization benchmarks are not reflective of "real-world" performance, because processes rarely do serialization-only workflows. In reality, your serializer is going to be competing for L1 cache and other resources along with everything else in your program (and everything else on the machine). So while these benchmarks show that FlatSharp is faster by a wide margin, these benefits may not translate to any practical effect in your environment, depending completely upon your own workflows and data structures. Your choice of serialization format and library should be informed by your needs: Do you need lazy access? Do you care about compact message size? Is serialization on your hot path? Don't make your choice based on the results of a benchmark that shows best-case results for all serializers by virtue of that being the only thing running on the machine at that point in time.

#### Serialization
This data shows the mean time it takes to serialize a typical message containing a 30-item vector containing a variety of data types:

| Library            | Time (JIT) | Time (NativeAOT) | Data Size |
|--------------------|------------|------------------|-----------|
| FlatSharp          |     732 ns |           809 ns |      3085 |
| Message Pack C#    |      1,998 |              N/A |      2497 |
| Google FlatBuffers |      2,544 |            4,324 |      3312 |
| Protobuf           |      2,688 |            3,092 |      2646 |
| Protobuf.NET       |      5,038 |              N/A |      2646 |

#### Deserialization
How much time does it take to parse and then fully enumerate the message from the serialization benchmark?

| Library                         | Time (JIT) | Time (NativeAOT) |
|---------------------------------|------------|------------------|
| FlatSharp (Lazy)                |   1,263 ns |         1,347 ns |
| FlatSharp (Greedy)              |      1,130 |            2,641 |
| Message Pack C#                 |      2,777 |              N/A |
| Google FlatBuffers              |      1,741 |            3,070 |
| Google FlatBuffers (Object API) |      2,660 |            5,009 |
| Protobuf                        |      3,289 |            3,575 |
| Protobuf.NET                    |      5,092 |              N/A |

Finally, FlatSharp scales quite well in scenarios without PGO such as AOT compilation and older runtimes.

### License
FlatSharp is licensed under Apache 2.0. Have fun!

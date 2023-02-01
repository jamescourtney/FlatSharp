## FlatSharp
![Main](https://github.com/jamescourtney/FlatSharp/actions/workflows/dotnet.yml/badge.svg?branch=main)
[![codecov](https://codecov.io/gh/jamescourtney/FlatSharp/branch/main/graph/badge.svg?token=6EUECHZGT4)](https://codecov.io/gh/jamescourtney/FlatSharp)

FlatSharp is Google's FlatBuffers serialization format implemented in C#, for C#. FlatBuffers is a zero-copy binary serialization format intended for high-performance scenarios. 
FlatSharp leverages the latest and greatest from .NET in the form of `Memory<T>` and `Span<T>`.
As such, FlatSharp's safe-code implementations are often faster than other implementations using unsafe code. FlatSharp aims to provide 3 core priorities:

- Usability
- Safety & Speed
- Compatibility with other C#-focused projects like Unity, Blazor, and Xamarin.

All FlatSharp packages are published on nuget.org:
- **FlatSharp.Runtime**: The runtime library. You always need this.
- **FlatSharp.Compiler**: Build time compiler for generating C# from an FBS schema.

FlatSharp is a mature library and has been shipped to production at Microsoft, Unity3D, and others. Full status can be found at [ProjectStatus.md](ProjectStatus.md). FlatSharp is [extensively tested](https://github.com/jamescourtney/FlatSharp/wiki/Testing), using Mutation Testing, Code Coverage, Oracle Testing, and other techniques to ensure the library doesn't regress.

### Getting Started
If you're completely new to FlatBuffers, take a minute to look over [the FlatBuffer overview](https://google.github.io/flatbuffers/index.html#flatbuffers_overview). Additionally, it's worth the time to understand the different elements of [FlatBuffer schemas](https://google.github.io/flatbuffers/flatbuffers_guide_writing_schema.html).

#### 1. Define a schema
FlatSharp, like other FlatBuffers implementations, uses [FBS files](samples/Example00-Basics/Basics.fbs) to define schemas. Because FlatSharp runs with your build, all code is generated at build time, making FlatSharp compatible with .NET AOT, Blazor, and Unity.

``` fbs
// all FlatSharp FBS attributes start with the 'fs_' prefix.
attribute "fs_serializer";

namespace MyNamespace;

enum Color : ubyte { Red = 1, Green, Blue }

table Person (fs_serializer) {
    Id:int;
    Name:string;
    Parent:Person (deprecated);
    Children:[Person];
    FavoriteColor:Color = Blue;
    Position:Location;
}

struct Location {
    Latitude:float;
    Longitude:float;
}

rpc_service PersonService {
    GetParent(Person):Person;
}
```

#### 2. Serialize your data
Serialization is easy!
```c#
Person person = new Person(...);
int maxBytesNeeded = Person.Serializer.GetMaxSize(person);
byte[] buffer = new byte[maxBytesNeeded];
int bytesWritten = Person.Serializer.Serialize(buffer, person);
```

#### 3. Parse your data
Deserializing is easier!
```c#
Person p = Person.Serializer.Parse(data);
```

#### Samples & Documentation
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
- All operations are overflow-checked
- No unsafe code; all operations bounds-checked
- No IL generation
- Use standard .NET libraries for reading and writing from memory
- Depth tracking enabled for recursive schemas to prevent stack overflows.

FlatSharp *does* use some techniques such as `MemoryMarshal.Read` on certain hot paths, but these usages are narrowly scoped and well tested.

### Performance & Benchmarks
FlatSharp is really, really fast. The FlatSharp benchmarks were run on .NET 7.0 with PGO disabled, using a C# approximation of [Google's FlatBuffer benchmark](https://github.com/google/flatbuffers/tree/benchmarks/benchmarks/cpp/FB), which can be found [here](src/Benchmark).

The benchmarks test 4 different serialization frameworks, all using default settings:
- FlatSharp -- 7.1.0
- Protobuf.NET -- 3.1.22
- Google's C# Flatbuffers -- 22.10.26
- Message Pack C# -- 2.4.35

The full results for each version of FlatSharp can be viewed in the [benchmarks folder](benchmarks). Additionally, the benchmark data contains performance data for many different configurations of FlatSharp and other features, such as sorted vectors and shared strings.

#### Word of Warning
Serialization benchmarks are not reflective of "real-world" performance, because processes rarely do serialization-only workflows. In reality, your serializer is going to be competing for L1 cache and other resources along with everything else in your program (and everything else on the machine). So while these benchmarks show that FlatSharp is faster by a wide margin, these benefits may not translate to any practical effect in your environment, depending completely upon your own workflows and data structures. Your choice of serialization format and library should be informed by your needs: Do you need lazy access? Do you care about compact message size? Is serialization on your hot path? Don't make your choice based on the results of a benchmark that shows best-case results for all serializers by virtue of that being the only thing running on the machine at that point in time.

#### Serialization
This data shows the mean time it takes to serialize a typical message containing a 30-item vector containing a variety of data types:

| Library                         | Time     | Relative Performance | Data Size |
|---------------------------------|----------|----------------------|-----------|
| FlatSharp                       | 981 ns   | 100%                 | 3085      |
| Message Pack C#                 | 2,021    | 205%                 | 2497      |
| Google Flatbuffers              | 4,299    | 433%                 | 3312      |
| Google Flatbuffers (Object API) | 4,498    | 453%                 | 3312      |
| Protobuf.NET                    | 5,695    | 574%                 | 2646      |

#### Deserialization
How much time does it take to parse and then fully enumerate the message from the serialization benchmark?
| Library                         | Time     | Relative Performance |
|---------------------------------|----------|----------------------|
| FlatSharp                       | 1,404 ns | 100%                 |
| Message Pack C#                 | 3,363    | 240%                 |
| Google Flatbuffers              | 3,820    | 272%                 |
| Google Flatbuffers (Object API) | 5,585    | 419%                 |
| Protobuf.NET                    | 6,169    | 439%                 |

Finally, FlatSharp scales quite well when used with [PGO](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1/#dynamic-pgo). Both serialization and parse performance improve by ~20% with this feature enabled.

### License
FlatSharp is licensed under Apache 2.0.

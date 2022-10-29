## FlatSharp
![Main](https://github.com/jamescourtney/FlatSharp/actions/workflows/dotnet.yml/badge.svg?branch=main)
[![codecov](https://codecov.io/gh/jamescourtney/FlatSharp/branch/main/graph/badge.svg?token=6EUECHZGT4)](https://codecov.io/gh/jamescourtney/FlatSharp)

FlatSharp is Google's FlatBuffers serialization format implemented in C#, for C#. FlatBuffers is a zero-copy binary serialization format intended for high-performance scenarios. 
FlatSharp leverages the latest and greatest from .NET in the form of `Memory<T>` and `Span<T>`.
As such, FlatSharp's safe-code implementations are often faster than other implementations using unsafe code. FlatSharp aims to provide 4 core priorities:

- Safety
- Speed
- FlatBuffers schema correctness
- Compatibility with other C#-focused projects like Unity, Blazor, and Xamarin.

All FlatSharp packages are published on nuget.org:
- **FlatSharp.Runtime**: The runtime library. You always need this.
- **FlatSharp.Compiler**: Build time compiler for generating C# from an FBS schema.
- **FlatSharp**: Use attributes to define FlatBuffer schemas at Runtime. Most users should not consume this package as the Compiler offers much more flexibility.

FlatSharp is a mature library and has been shipped to production at Microsoft, Unity3D, and others. Full status can be found at [ProjectStatus.md](ProjectStatus.md).

### Getting Started
If you're completely new to FlatBuffers, take a minute to look over [the FlatBuffer overview](https://google.github.io/flatbuffers/index.html#flatbuffers_overview). Additionally, it's worth the time to understand the different elements of [FlatBuffer schemas](https://google.github.io/flatbuffers/flatbuffers_guide_writing_schema.html).

#### 1. Define a schema
There are two ways to define a FlatBuffer schema with FlatSharp. The recommended way is to use a `.fbs` use an [FBS schema file](samples/Example02-SchemaFiles/SchemaFilesExample.fbs) and run the FlatSharp compiler at build-time with your project.
This enables the best experience with FlatSharp, including AOT compilation.

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

FlatSharp is also usable with [C# attribute annotations at Runtime](samples/Example00-AttributeBasedSchemas/MonsterAttributeExample.cs), though this is not the recommended pattern.

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
FlatSharp supports some interesting features not covered here. Detailed documentation is in the [wiki](https://github.com/jamescourtney/FlatSharp/wiki). The [samples solution](samples/) has full examples of:
- [Build-time serializer code generation](samples/Example03-SchemaFiles2/)
- [Deserialization options (Lazy, Greedy, and everything in between)](samples/Example01-SerializerOptions/SerializerOptionsExample.cs)
- [IO Options](samples/Example04-IOOptions/)
- [gRPC](samples/Example05-gRPC/)
- [Copy Constructors](samples/Example06-CopyConstructors/)
- [FBS Includes](samples/Example07-Includes/)
- [Sorted Vectors](samples/Example08-SortedVectors/)
- [Unions](samples/Example09-Unions/)
- [String deduplication](samples/Example10-SharedStrings/)
- [Indexed Vectors (Dictionary-like functionality)](samples/Example11-IndexedVectors/)
- [Type Facades](samples/Example12-TypeFacades/)
- [Fixed-Length Vectors](samples/Example13-StructVectors/)
- [Write-Through -- update buffers in place](samples/Example14-WriteThrough/)

### Internals
FlatSharp works by generating subclasses of your data contracts based on the schema that you define. 
That is, when you attempt to deserialize a `MonsterTable` object, you actually get back a subclass of `MonsterTable`, 
which has properties defined in such a way as to index into the buffer, according to the deserialization mode specified (greedy, lazy, etc).

### Security
Serializers are a common vector for security issues. FlatSharp takes the following approach to security:
- All operations are overflow-checked
- No unsafe code
- No IL generation
- Use standard .NET libraries for reading and writing from memory

FlatSharp *does* use some techniques such as `MemoryMarshal.Read` on certain hot paths, but these usages are narrowly scoped.

### Performance & Benchmarks
FlatSharp is really, really fast. The FlatSharp benchmarks were run on .NET 7.0, using a C# approximation of [Google's FlatBuffer benchmark](https://github.com/google/flatbuffers/tree/benchmarks/benchmarks/cpp/FB), which can be found [here](src/Benchmark).

The benchmarks test 4 different serialization frameworks, all using default settings:
- FlatSharp -- 7.0.0
- Protobuf.NET -- 3.0.101
- Google's C# Flatbuffers -- 2.0.0
- Message Pack C# -- 2.3.75

The full results for each version of FlatSharp can be viewed in the [benchmarks folder](benchmarks). Additionally, the benchmark data contains performance data for many different configurations of FlatSharp and other features, such as sorted vectors and shared strings.

#### Word of Warning
Serialization benchmarks are not reflective of "real-world" performance, because processes rarely do serialization-only workflows. In reality, your serializer is going to be competing for L1 cache and other resources along with everything else in your program (and everything else on the machine). So while these benchmarks show that FlatSharp is faster by a wide margin, these benefits may not translate to any practical effect in your environment, depending completely upon your own workflows and data structures. Your choice of serialization format and library should be informed by your needs (Do you need lazy access? Do you care about compact message size?) and not by the results of a benchmark that shows best-case results for all serializers by virtue of that being the only thing running on the machine at that point in time.

#### Serialization
This data shows the mean time it takes to serialize a typical message containing a 30-item vector.
| Library                         | Time     | Relative Performance | Data Size |
|---------------------------------|----------|----------------------|-----------|
| FlatSharp (Optimized)           | 1,127 ns | 63%                  | 3085      |
| FlatSharp (Default)             | 1,799    | 100%                 | 3085      |
| Message Pack C#                 | 2,613    | 145%                 | 2497      |
| Google Flatbuffers              | 6,157    | 342%                 | 3312      |
| Google Flatbuffers (Object API) | 6,490    | 361%                 | 3312      |
| Protobuf.NET                    | 8,518    | 473%                 | 2646      |

#### Deserialization
How much time does it take to parse and then fully enumerate the message from the serialization benchmark?
| Library                         | Time     | Relative Performance |
|---------------------------------|----------|----------------------|
| FlatSharp (Optimized)           | 1,746 ns | 79%                  |
| FlatSharp (Default)             | 2,211    | 100%                 |
| Message Pack C#                 | 5,491    | 248%                 |
| Google Flatbuffers              | 4,928    | 223%                 |
| Google Flatbuffers (Object API) | 7,734    | 350%                 |
| Protobuf.NET                    | 8,464    | 383%                 |

### So What Packages Do I Need?
There are two main ways to use FlatSharp: Precompilation with .fbs files and runtime compilation using attributes on C# classes. Both of these produce and load the same code, so the performance will be identical. 
There are some good reasons to use precompilation over runtime compilation:
- No runtime overhead -- Roslyn can take a little bit to spin up the first time
- Fewer package dependencies
- Better interop with other FlatBuffers languages via .fbs files
- gRPC Support
- Schema validation errors caught at build-time instead of runtime.
- Better supported with other .NET toolchains (Unity / Blazor / etc)

Scenario | FlatSharp.Runtime | FlatSharp | FlatSharp.Compiler
------------ | ------------- | -------- | -----------------
Ahead of Time Compilation | ✅ | ❌ | ✅
Runtime Compilation | ✅ | ✅ | ❌

### License
FlatSharp is licensed under Apache 2.0.

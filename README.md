## FlatSharp
![Main](https://github.com/jamescourtney/FlatSharp/actions/workflows/dotnet.yml/badge.svg?branch=main)
[![codecov](https://codecov.io/gh/jamescourtney/FlatSharp/branch/main/graph/badge.svg?token=6EUECHZGT4)](https://codecov.io/gh/jamescourtney/FlatSharp)

FlatSharp is Google's FlatBuffers serialization format implemented in C#, for C#. FlatBuffers is a zero-copy binary serialization format intended for high-performance scenarios. FlatSharp leverages the latest and greatest from .NET in the form of `Memory<T>` and `Span<T>`. As such, FlatSharp's safe-code implementations are often faster than other implementations using unsafe code. FlatSharp aims to provide 4 core priorities:

- Full safety (no unsafe code or IL generation -- more on that below).
- Speed
- FlatBuffers schema correctness
- Compatibility with other C#-focused projects like Unity, Blazor, and Xamarin. If it supports .NET standard 2.0, it supports FlatSharp.

All FlatSharp packages are published on nuget.org:
- **FlatSharp.Runtime**: The runtime library. You always need this.
- **FlatSharp**: Support for runtime schemas with C# attributes. Includes `FlatBufferSerializer`.
- **FlatSharp.Compiler**: Build time compiler for generating C# from an FBS schema.

FlatSharp is a mature library and has been shipped to production at Microsoft and other companies. Full status can be found at [ProjectStatus.md](ProjectStatus.md).

### Getting Started
If you're completely new to FlatBuffers, take a minute to look over [the FlatBuffer overview](https://google.github.io/flatbuffers/index.html#flatbuffers_overview). Additionally, it's worth the time to understand the different elements of [FlatBuffer schemas](https://google.github.io/flatbuffers/flatbuffers_guide_writing_schema.html).

#### 1. Define a schema
There are two ways to define a FlatBuffer schema with FlatSharp. The first is to use [C# attributes to annotate classes](samples/Example00-AttributeBasedSchemas/MonsterAttributeExample.cs), like you would with other serializers:
```c#
// FlatSharp supports enums, but makes you promise not to change the underlying type.
[FlatBufferEnum(typeof(byte))]
public enum Color : byte { Red = 1, Green, Blue }

// Tables are flexible objects meant to allow schema changes. Numeric properties can have default values,
// and all properties can be deprecated. Each index may only be used once, so once the "Parent" property is
// deprecated, index 2 cannot be used again by a different property.
[FlatBufferTable]
public class Person : object
{   
    [FlatBufferItem(0)] public virtual int Id { get; set; }
    [FlatBufferItem(1)] public virtual string Name { get; set; }
    [FlatBufferItem(2, Deprecated = true)] public virtual Person Parent { get; set; }
    [FlatBufferItem(3)] public virtual IList<Person> Children { get; set; }
    [FlatBufferItem(4, DefaultValue = Color.Blue)] public virtual Color FavoriteColor { get; set; } = Color.Blue;
    [FlatBufferItem(5)] public virtual Location Position { get; set; }
}

// Structs are really fast, but may only contain scalars and other structs. Structs
// cannot be versioned, so use only when you're sure the schema won't change.
[FlatBufferStruct]
public class Location : object
{
    [FlatBufferItem(0)] public virtual float Latitude { get; set; }
    [FlatBufferItem(1)] public virtual float Longitude { get; set; }
}
```

The second way to define a schema is to use an [FBS schema file](samples/Example02-SchemaFiles/SchemaFilesExample.fbs) and run the FlatSharp compiler at build-time with your project. This enables fancy options like precompiling your serializers, interop with FlatBuffers in other languages, and GRPC definitions.
``` fbs
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
int maxBytesNeeded = FlatBufferSerializer.Default.GetMaxSize(person);
byte[] buffer = new byte[maxBytesNeeded];
int bytesWritten = FlatBufferSerializer.Default.Serialize(person, buffer);
```

#### 3. Parse your data
Deserializing is easier!
```c#
// By default, FlatSharp deserializes greedily, so everything in the Person is read from the data buffer
// and copied into the Person object, and the data buffer is no longer used after the Parse method returns.
// However, FlatSharp supports a variety of Lazy modes that read data from the buffer on demand and are
// often faster. These are covered under advanced topics below.
Person p = FlatBufferSerializer.Default.Parse<Person>(data);
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
FlatSharp works by generating subclasses of your data contracts based on the schema that you define. That is, when you attempt to deserialize a `MonsterTable` object, you actually get back a subclass of `MonsterTable`, which has properties defined in such a way as to index into the buffer, according to the deserialization mode specified (greedy, lazy, etc).

### Security
Serializers are a common vector for security issues. FlatSharp takes the following approach to security:
- All core operations are overflow-checked
- No unsafe code
- No IL generation
- Use standard .NET libraries for reading and writing from memory

At its core, FlatSharp is a tool to convert a FlatBuffer schema into a pile of safe C# code that depends only upon standard .NET libraries. There is no "secret sauce". Buffer overflows are intended to be impossible by design, due to the features of .NET and the CLR. A malicious input may lead to corrupt data or an Exception being thrown, but the process will not be compromised. As always, a best practice is to encrypt data at rest, in transit, and decorate it with some checksums.

### Performance & Benchmarks
FlatSharp is really, really fast. The FlatSharp benchmarks were run on .NET 5.0, using a C# approximation of [Google's FlatBuffer benchmark](https://github.com/google/flatbuffers/tree/benchmarks/benchmarks/cpp/FB), which can be found [here](src/Benchmark).

The benchmarks test 4 different serialization frameworks, all using default settings:
- FlatSharp -- 6.0.0
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
There are two main ways to use FlatSharp: Precompilation with .fbs files and runtime compilation using attributes on C# classes. Both of these produce and load the same code, so the performance will be identical. There are some good reasons to use precompilation over runtime compilation:
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

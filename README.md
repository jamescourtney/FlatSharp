## FlatSharp

FlatSharp is Google's FlatBuffers serialization format implemented in C#, for C#. FlatBuffers is a zero-copy binary serialization format intended for high-performance scenarios. FlatSharp leverages the latest and greatest from .NET in the form of ```Memory<T>``` and ```Span<T>```. As such, FlatSharp's safe-code implementations are often faster than other implementations using unsafe code. FlatSharp aims to provide 3 core priorities:
- Full safety (no unsafe code or IL generation -- more on that below).
- Speed
- FlatBuffers schema correctness

### Current Status
FlatSharp is in occasional development, as time permits for the author. Contributions and proposals are always welcomed. Currently, FlatSharp supports the following FlatBuffers features:
- Structs
- Tables
- Scalars / Strings / Enums
- ```IList<T>```, ```IReadOnlyList<T>```, and ```T[]``` Vectors of Strings, Tables, Structs, and Scalars
- ```Memory<T>```/```ReadOnlyMemory<T>``` Vectors of scalars when on little-endian systems
- Discriminated/tagged unions of structs, tables, and strings
- Build time compilation of FBS files

What's not supported (and why):
- Vectors of Unions. This is a reasonably complex feature that can be approximated with a vector of tables, where each table element contains a union.

### Packages
All FlatSharp packages are published on nuget.org:
- Runtime Library: FlatSharp.Runtime
- Core Library: FlatSharp
- Unsafe Extensions: FlatSharp.Unsafe
- FBS to C# Compiler: FlatSharp.Compiler

### Getting Started
FlatSharp uses C# as its schema, and does not require any additional files or build-time code generation. Like many other serializers, the process is to annotate your data contracts with attributes, and you're on your way.

### Samples
FlatSharp maintains a samples solution to show how to use FlatSharp in a variety of configurations. The solution is standalone and depends only on the FlatSharp nuget packages. You can find it under ```/samples/Samples.sln```.

#### Defining a Contract

```C#
[FlatBufferTable]
public class MonsterTable
{
    [FlatBufferItem(0)]
    public virtual Position Position { get; set; }
    
    [FlatBufferItem(1, DefaultValue = (short)150)]
    public virtual short Mana { get; set; } = (short)150;
    
    [FlatBufferItem(2)]
    public virtual string Name { get; set; }
    
    [FlatBufferItem(3, Deprecated = true)]
    public virtual bool Friendly { get; set; }
    
    [FlatBufferItem(4)]
    public virtual ReadOnlyMemory<byte> Inventory { get; set; }
    
    [FlatBufferItem(5)]
    public virtual FlatBufferUnion<string, Position> DiscriminatedUnion { get; set; }
    
    // Note that that the next index starts at 7. Unions are 'double-wide' types, so the previous
    // element occupies indices 5 and 6!
    
    [FlatBufferItem(7, DefaultValue = Color.Blue)]
    public virtual Color Color { get; set; }
}

[FlatBufferEnum(typeof(byte))]
public enum Color : byte { Blue, Red, Green }

[FlatBufferStruct]
public class Position
{
   [FlatBufferItem(0)]
   public virtual float X { get; set; }
   
   [FlatBufferItem(1)]
   public virtual float Y { get; set; }
   
   [FlatBufferItem(2)]
   public virtual float Z { get; set; }
}
```
For FlatSharp to be able to work with your schema, it must obey the following set of contraints:
- All types must be public and externally visible.
- All types must be unsealed.
- All properties decorated by ```[FlatSharpItem]``` must be virtual and public. Setters may be omitted, but Getters are required.
- All FlatSharpItem indexes must be unique within the given data type.
- Struct/Table vectors must be defined as ```IList<T>```, ```IReadOnlyList<T>```, or ```T[]```.
- Scalar vectors must be defined as either ```IList<T>```, ```IReadOnlyList<T>```, ```Memory<T>```, ```ReadOnlyMemory<T>```, or ```T[]```.
- All types must be serializable in FlatBuffers (that is -- you can't throw in an arbitrary C# type).

When versioning your schema, the [FlatBuffer rules apply](https://google.github.io/flatbuffers/flatbuffers_guide_writing_schema.html).

#### FlatSharp Compiler (FBS Schema support)
Installing the FlatSharp.Compiler package in one of your projects will allow you to use existing FBS schemas to generate FlatSharp contracts like above:

Simply add the FBS schema to your csproj file
```xml
  <ItemGroup>
    <FlatSharpSchema Include="Schema.fbs" />
  </ItemGroup>
```
The FlatSharp compiler supports most elements of the FBS schema:
```fbs
namespace MyGame;

enum Color:byte { Red = 0, Green, Blue = 2 }
struct Vec3 { x:float; y:float; z:float; }
table Weapon { name:string; damage:short; }
union Equipment { Weapon }

table Monster 
  (PrecompiledSerializer:Greedy) // Pre-generate a serializer at build time instead of runtime.
{
  pos:Vec3;
  mana:short = 150;
  name:string;
  friendly:bool = false (deprecated);
  inventory:[ubyte];
  color:Color = Blue;
  weapons:[Weapon];
  equipped:Equipment;
  path1:[Vec3] (vectortype:IReadOnlyList);
  path2:[Vec3]; // defaults to IList
  path3:[Vec3] (vectortype:Array);
  path4:[ubyte] (vectortype:Memory);
  path5:[ubyte] (vectortype:ReadOnlyMemory);
}
```
The main departures from the flatc compiler are that the flatsharp compiler does not support:
- Imports (ignored)
- Root types (ignored)
- Most of the flatc attributes, such as ID (will cause errors).

#### Serializing and Deserializing
```C#
public void ReadMonsterMemory(Memory<byte> monsterBuffer) =>
   FlatBufferSerializer.Default.Parse<MonsterTable>(monsterBuffer);

public Monster ReadMonsterArray(byte[] monsterBuffer) =>
   FlatBufferSerializer.Default.Parse<MonsterTable>(monsterBuffer);

public void WriteMonster(MonsterTable monster) =>
   FlatBufferSerializer.Default.Serialize(monster, new byte[10 * 1024]);

// Get the maximum number of bytes it will take to serialize this monster instance:
public int GetBufferSize(MonsterTable monster) =>
   FlatBufferSerializer.Default.GetMaxSize(monster);
```

#### Serializer Options and Default Behaviors
FlatSharp does not expose any special options for the serialization flow; the binary format is the binary format, and isn't customizable. However, there are some knobs to tune on the deserialization flows. 

The default behavior of the FlatSharp parser is to greedily parse all information from the buffer. This is a change in behavior from previous versions of FlatSharp, and is made to give the best out-of-the-box experience and to not violate the principle of least surprise.

These behaviors can be changed by specifying your own ```FlatBufferSerializerOptions``` class, which can be passed into the ```FlatBufferSerializer``` constructor.

The options are:
- ```CacheListVectorData``` (Default=false): Allocate extra arrays when reading an ```IList<T>``` vector and use a progressive cache like FlatSharp does with conventional properties. This increases the memory footprint of your object, but for situations where you will iterate over a vector multiple times, this becomes a useful optimization.
- ```GenerateMutableObjects``` (Default=false): All objects returned from FlatSharp will be mutable, where allowable. Mutations to objects are never stored back into the original buffer, but are instead stored in Memory using "Copy On Write" semantics. Note that this implies the ```CacheListVectorData``` option since vectors must support Add/Clear/RemoveAt semantics. When this option is enabled, FlatSharp provides the invariant that the original buffer is not modified. Any changes will require a re-serialization.
- ```GreedyDeserialize``` (Default=true): At parse time, the entire object graph is traversed and the contents are copied into the class structure. This option generally has the worst performance, but provides the guarantee that the original buffer will not be referenced by FlatSharp and can be recycled / used for other purposes.

More information and examples can be found in the Samples solution.

### Internals
FlatSharp works by generating dynamic subclasses of your data contracts based on the schema that you define, which is why they must be public and virtual. That is, when you attempt to deserialize a ```MonsterTable``` object, you actually get back a dynamic subclass of ```MonsterTable```, which has properties defined in such a way as to index into the buffer. When a FlatSharp object reads a value for it, it goes ahead and makes a copy of that value so that it does not need to consult the original buffer again.


### Safety without GreedyDeserialize
When GreedyDeserialize is disabled, FlatSharp becomes a lazy parser. That is -- data from the underlying buffer is not actually parsed until you request it. This keeps things very lean throughout your application and prevents your application from paying a deserialize tax on items that you will not use. However, this is a double-edged sword, because the deserialized objects must maintain a reference to the original buffer, and any changes to the underlying buffer will modify, and possibly corrupt, the state of any objects that reference that buffer.

```C#
public void ReadMonster(byte[] monsterBuffer)
{
  MonsterTable monster = FlatBufferSerializer.Default.Parse<MonsterTable>(monsterBuffer);
  monsterBuffer[7] = 123;
  
  // This data is no longer valid, and using the Monster object results in undefined behavior.
  Console.WriteLine($"{monster.Position.X}, {monster.Position.Y}, {monster.Position.Z}");
}
```
Therefore, when ```GreedyDeserialize``` is disabled, you must keep buffer lifecycle management in mind. The simplest way to accomplish is to just let the GC take care of it for you.

### Security
Serializers are a common vector for security issues. FlatSharp takes the following approach to security:
- All core operations are overflow-checked
- No unsafe code or IL generation via IL.Emit (with the exception of the Unsafe package)
- Use standard .NET libraries for reading and writing from memory (with the exception of the Unsafe package)

FlatSharp manages to acheive performance by generating safe C# code at runtime and pushing that through the Roslyn C# compiler to emit and load a runtime assembly. This provides an additional level of safety over generating IL directly with minimimal performance impact, since we compile without allowing unsafe code and can embed other invariants in the code (such as readonly). Further, it is easier to debug generated C# than generated IL. Roslyn does have a 3 to 4 second first-run penalty, but subsequent invocations are very quick, and "compilation" is only done once per root-level FlatBuffer object.

In its default configuration, the FlatSharp library uses no unsafe code at all, and only uses overflow-checked operators. FlatSharp does come with an unsafe companion library that can meaningfully improve performance in some scenarios (```UnsafeMemoryInputBuffer``` provides roughly double the performance of ```MemoryInputBuffer``` with the caveat that it must be disposed for performance to be acceptable. This is less of an issue with .NET Core 3.1).

### Performance & Benchmarks
FlatSharp is really fast. This is primarily thanks to new changes in C# with Memory and Span, as well as FlatBuffers itself exposing a very simple type system that makes optimization simple. FlatSharp has a default serializer instance (```FlatBuffersSerializer.Default```), however it is possible to tune the serializer by creating your own with a custom ```FlatBufferSerializerOptions``` instance. Right now, the only option is ```CacheListVectorData```, which instructs FlatSharp to generate List Vector deserializers that progressively caches data as it is read. This costs an additional array allocation, and is slower in the case when each element is accessed only once. However, when each element is accessed multiple times, this is a useful optimization.

The FlatSharp benchmarks were run on .NET Core 2.1, using a C# approximation of [Google's FlatBuffer benchmark](https://github.com/google/flatbuffers/tree/benchmarks/benchmarks/cpp/FB). The FlatSharp benchmarks use this schema, but with the following parameters:
- Vector length = 3 or 30
- Traversal count = 1 or 5

The benchmarks test 4 different serialization frameworks:
- FlatSharp (of course :))
- Protobuf.NET
- Google's C# Flatbuffers implementation
- ZeroFormatter

#### Serialization
![image](doc/s_3.png) | ![image](doc/s_30.png)
----------------------|-----------------------

#### Deserialization
![image](doc/d_1_3.png) | ![image](doc/d_5_3.png)
------------------------|-------------------------
![image](doc/d_1_30.png)|![image](doc/d_5_30.png)

### Roadmap
- [ ] Security hardening and fuzzing
- [x] Code gen based on FBS schema files
- [x] Build time generation of Serializers
- [ ] GRPC support

### License
FlatSharp is a C# implementation of Google's FlatBuffer binary format, which is licensed under the Apache 2.0 License. Accordingly, FlatSharp is also licensed under Apache 2.0. FlatSharp incorporates code from the Google FlatSharp library for testing and benchmarking purposes.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.FBBench.ReflectionBased;

#if AOT

public class ProtoContractAttribute : Attribute { }
public class ProtoMemberAttribute : Attribute { public ProtoMemberAttribute(int i) { } }
public class KeyAttribute : Attribute { public KeyAttribute(int i) { } }
public class MessagePackObjectAttribute : Attribute { }

#else
using MessagePack;
using ProtoBuf;
#endif

[ProtoContract]
[MessagePackObject]
public sealed class FooBarListContainer
{
    [ProtoMember(1)]
    [Key(0)]
    public IList<FooBar> List { get; set; }

    [ProtoMember(2)]
    [Key(1)]
    public bool Initialized { get; set; }

    [ProtoMember(3)]
    [Key(2)]
    public short Fruit { get; set; }

    [ProtoMember(4)]
    [Key(3)]
    public string Location { get; set; }
}

[ProtoContract]
[MessagePackObject]
public sealed class FooBar
{
    [ProtoMember(1)]
    [Key(0)]
    public Bar Sibling { get; set; }

    [ProtoMember(2)]
    [Key(1)]
    public string Name { get; set; }

    [ProtoMember(3)]
    [Key(2)]
    public double Rating { get; set; }

    [ProtoMember(4)]
    [Key(3)]
    public byte Postfix { get; set; }
}

[ProtoContract]
[MessagePackObject]
public sealed class Bar
{
    [ProtoMember(1)]
    [Key(0)]
    public Foo Parent { get; set; }

    [ProtoMember(2)]
    [Key(1)]
    public int Time { get; set; }

    [ProtoMember(3)]
    [Key(2)]
    public float Ratio { get; set; }

    [ProtoMember(4)]
    [Key(3)]
    public ushort Size { get; set; }
}

[ProtoContract]
[MessagePackObject]
public sealed class Foo
{
    [ProtoMember(1)]
    [Key(0)]
    public ulong Id { get; set; }

    [ProtoMember(2)]
    [Key(1)]
    public short Count { get; set; }

    [ProtoMember(3)]
    [Key(2)]
    public sbyte Prefix { get; set; }

    [ProtoMember(4)]
    [Key(3)]
    public uint Length { get; set; }
}
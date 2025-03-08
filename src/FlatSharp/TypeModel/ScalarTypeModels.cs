/*
 * Copyright 2020 James Courtney
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */



namespace FlatSharp.TypeModel;

    
/// <summary>
/// Type Model for <see cref="bool"/>.
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public partial class BoolTypeModel : ScalarTypeModel
{
    public BoolTypeModel(TypeModelContainer container) : base(container, typeof(bool), sizeof(bool)) 
    {
    }

    public override bool TryGetSpanComparerType([NotNullWhen(true)] out Type? comparerType)
    {
        comparerType = typeof(BoolSpanComparer);
        return true;
    }

    protected override string InputBufferReadMethodName => "ReadBool";

    protected override string WriteMethodName => "WriteBool";

    protected override string UnsafeWriteMethodName => "UnsafeWriteBool";

    protected override string TypeNameAlias => "bool";
    
    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return "bool";
    }
}

    
/// <summary>
/// Type Model for <see cref="byte"/>.
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public partial class ByteTypeModel : ScalarTypeModel
{
    public ByteTypeModel(TypeModelContainer container) : base(container, typeof(byte), sizeof(byte)) 
    {
    }

    public override bool TryGetSpanComparerType([NotNullWhen(true)] out Type? comparerType)
    {
        comparerType = typeof(ByteSpanComparer);
        return true;
    }

    protected override string InputBufferReadMethodName => "ReadByte";

    protected override string WriteMethodName => "WriteByte";

    protected override string UnsafeWriteMethodName => "UnsafeWriteByte";

    protected override string TypeNameAlias => "byte";
    
    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return "byte";
    }
}

    
/// <summary>
/// Type Model for <see cref="sbyte"/>.
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public partial class SByteTypeModel : ScalarTypeModel
{
    public SByteTypeModel(TypeModelContainer container) : base(container, typeof(sbyte), sizeof(sbyte)) 
    {
    }

    public override bool TryGetSpanComparerType([NotNullWhen(true)] out Type? comparerType)
    {
        comparerType = typeof(SByteSpanComparer);
        return true;
    }

    protected override string InputBufferReadMethodName => "ReadSByte";

    protected override string WriteMethodName => "WriteSByte";

    protected override string UnsafeWriteMethodName => "UnsafeWriteSByte";

    protected override string TypeNameAlias => "sbyte";
    
    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return "sbyte";
    }
}

    
/// <summary>
/// Type Model for <see cref="ushort"/>.
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public partial class UShortTypeModel : ScalarTypeModel
{
    public UShortTypeModel(TypeModelContainer container) : base(container, typeof(ushort), sizeof(ushort)) 
    {
    }

    public override bool TryGetSpanComparerType([NotNullWhen(true)] out Type? comparerType)
    {
        comparerType = typeof(UShortSpanComparer);
        return true;
    }

    protected override string InputBufferReadMethodName => "ReadUShort";

    protected override string WriteMethodName => "WriteUShort";

    protected override string UnsafeWriteMethodName => "UnsafeWriteUShort";

    protected override string TypeNameAlias => "ushort";
    
    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return "ushort";
    }
}

    
/// <summary>
/// Type Model for <see cref="short"/>.
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public partial class ShortTypeModel : ScalarTypeModel
{
    public ShortTypeModel(TypeModelContainer container) : base(container, typeof(short), sizeof(short)) 
    {
    }

    public override bool TryGetSpanComparerType([NotNullWhen(true)] out Type? comparerType)
    {
        comparerType = typeof(ShortSpanComparer);
        return true;
    }

    protected override string InputBufferReadMethodName => "ReadShort";

    protected override string WriteMethodName => "WriteShort";

    protected override string UnsafeWriteMethodName => "UnsafeWriteShort";

    protected override string TypeNameAlias => "short";
    
    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return "short";
    }
}

    
/// <summary>
/// Type Model for <see cref="int"/>.
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public partial class IntTypeModel : ScalarTypeModel
{
    public IntTypeModel(TypeModelContainer container) : base(container, typeof(int), sizeof(int)) 
    {
    }

    public override bool TryGetSpanComparerType([NotNullWhen(true)] out Type? comparerType)
    {
        comparerType = typeof(IntSpanComparer);
        return true;
    }

    protected override string InputBufferReadMethodName => "ReadInt";

    protected override string WriteMethodName => "WriteInt";

    protected override string UnsafeWriteMethodName => "UnsafeWriteInt";

    protected override string TypeNameAlias => "int";
    
    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return "int";
    }
}

    
/// <summary>
/// Type Model for <see cref="uint"/>.
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public partial class UIntTypeModel : ScalarTypeModel
{
    public UIntTypeModel(TypeModelContainer container) : base(container, typeof(uint), sizeof(uint)) 
    {
    }

    public override bool TryGetSpanComparerType([NotNullWhen(true)] out Type? comparerType)
    {
        comparerType = typeof(UIntSpanComparer);
        return true;
    }

    protected override string InputBufferReadMethodName => "ReadUInt";

    protected override string WriteMethodName => "WriteUInt";

    protected override string UnsafeWriteMethodName => "UnsafeWriteUInt";

    protected override string TypeNameAlias => "uint";
    
    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return "uint";
    }
}

    
/// <summary>
/// Type Model for <see cref="long"/>.
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public partial class LongTypeModel : ScalarTypeModel
{
    public LongTypeModel(TypeModelContainer container) : base(container, typeof(long), sizeof(long)) 
    {
    }

    public override bool TryGetSpanComparerType([NotNullWhen(true)] out Type? comparerType)
    {
        comparerType = typeof(LongSpanComparer);
        return true;
    }

    protected override string InputBufferReadMethodName => "ReadLong";

    protected override string WriteMethodName => "WriteLong";

    protected override string UnsafeWriteMethodName => "UnsafeWriteLong";

    protected override string TypeNameAlias => "long";
    
    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return "long";
    }
}

    
/// <summary>
/// Type Model for <see cref="ulong"/>.
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public partial class ULongTypeModel : ScalarTypeModel
{
    public ULongTypeModel(TypeModelContainer container) : base(container, typeof(ulong), sizeof(ulong)) 
    {
    }

    public override bool TryGetSpanComparerType([NotNullWhen(true)] out Type? comparerType)
    {
        comparerType = typeof(ULongSpanComparer);
        return true;
    }

    protected override string InputBufferReadMethodName => "ReadULong";

    protected override string WriteMethodName => "WriteULong";

    protected override string UnsafeWriteMethodName => "UnsafeWriteULong";

    protected override string TypeNameAlias => "ulong";
    
    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return "ulong";
    }
}

    
/// <summary>
/// Type Model for <see cref="float"/>.
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public partial class FloatTypeModel : ScalarTypeModel
{
    public FloatTypeModel(TypeModelContainer container) : base(container, typeof(float), sizeof(float)) 
    {
    }

    public override bool TryGetSpanComparerType([NotNullWhen(true)] out Type? comparerType)
    {
        comparerType = typeof(FloatSpanComparer);
        return true;
    }

    protected override string InputBufferReadMethodName => "ReadFloat";

    protected override string WriteMethodName => "WriteFloat";

    protected override string UnsafeWriteMethodName => "UnsafeWriteFloat";

    protected override string TypeNameAlias => "float";
    
    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return "float";
    }
}

    
/// <summary>
/// Type Model for <see cref="double"/>.
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public partial class DoubleTypeModel : ScalarTypeModel
{
    public DoubleTypeModel(TypeModelContainer container) : base(container, typeof(double), sizeof(double)) 
    {
    }

    public override bool TryGetSpanComparerType([NotNullWhen(true)] out Type? comparerType)
    {
        comparerType = typeof(DoubleSpanComparer);
        return true;
    }

    protected override string InputBufferReadMethodName => "ReadDouble";

    protected override string WriteMethodName => "WriteDouble";

    protected override string UnsafeWriteMethodName => "UnsafeWriteDouble";

    protected override string TypeNameAlias => "double";
    
    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return "double";
    }
}


public class ScalarTypeModelProvider : ITypeModelProvider
{
    public bool TryCreateTypeModel(
        TypeModelContainer container, 
        Type type, 
        [NotNullWhen(true)] out ITypeModel? typeModel)
    {
        typeModel = null;
    
        if (type == typeof(bool))
        {
            typeModel = new BoolTypeModel(container); 
            return true;
        }

    
        if (type == typeof(byte))
        {
            typeModel = new ByteTypeModel(container); 
            return true;
        }

    
        if (type == typeof(sbyte))
        {
            typeModel = new SByteTypeModel(container); 
            return true;
        }

    
        if (type == typeof(ushort))
        {
            typeModel = new UShortTypeModel(container); 
            return true;
        }

    
        if (type == typeof(short))
        {
            typeModel = new ShortTypeModel(container); 
            return true;
        }

    
        if (type == typeof(int))
        {
            typeModel = new IntTypeModel(container); 
            return true;
        }

    
        if (type == typeof(uint))
        {
            typeModel = new UIntTypeModel(container); 
            return true;
        }

    
        if (type == typeof(long))
        {
            typeModel = new LongTypeModel(container); 
            return true;
        }

    
        if (type == typeof(ulong))
        {
            typeModel = new ULongTypeModel(container); 
            return true;
        }

    
        if (type == typeof(float))
        {
            typeModel = new FloatTypeModel(container); 
            return true;
        }

    
        if (type == typeof(double))
        {
            typeModel = new DoubleTypeModel(container); 
            return true;
        }

    
        return false;
    }
}

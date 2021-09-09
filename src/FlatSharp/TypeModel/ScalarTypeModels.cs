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



namespace FlatSharp.TypeModel
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;

		
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

        protected override string InputBufferReadMethodName => nameof(InputBufferExtensions.ReadBool);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriterExtensions.WriteBool);
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

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadByte);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteByte);
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

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadSByte);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteSByte);
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

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadUShort);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteUShort);
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

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadShort);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteShort);
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

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadInt);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteInt);
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

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadUInt);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteUInt);
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

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadLong);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteLong);
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

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadULong);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteULong);
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

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadFloat);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteFloat);
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

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadDouble);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteDouble);
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
}
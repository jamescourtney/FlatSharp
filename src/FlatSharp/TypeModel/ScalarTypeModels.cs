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

		
    /// <summary>
    /// Type Model for <see cref="bool"/>.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class BoolTypeModel : ScalarTypeModel
    {
        public BoolTypeModel() : base(typeof(bool), sizeof(bool)) 
        {
        }
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(bool)({value})";
			return true;
		}

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = typeof(BoolSpanComparer);;
            return true;
        }

        protected override string InputBufferReadMethodName => nameof(InputBufferExtensions.ReadBool);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriterExtensions.WriteBool);
    }

	
    /// <summary>
    /// Type Model for <see cref="bool?"/>.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class NullableBoolTypeModel : ScalarTypeModel
    {
        public NullableBoolTypeModel() : base(typeof(bool?), sizeof(bool)) 
        {
        }

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = null;
            return false;
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
        public ByteTypeModel() : base(typeof(byte), sizeof(byte)) 
        {
        }
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(byte)({value})";
			return true;
		}

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = typeof(ByteSpanComparer);;
            return true;
        }

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadByte);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteByte);
    }

	
    /// <summary>
    /// Type Model for <see cref="byte?"/>.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class NullableByteTypeModel : ScalarTypeModel
    {
        public NullableByteTypeModel() : base(typeof(byte?), sizeof(byte)) 
        {
        }

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = null;
            return false;
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
        public SByteTypeModel() : base(typeof(sbyte), sizeof(sbyte)) 
        {
        }
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(sbyte)({value})";
			return true;
		}

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = typeof(SByteSpanComparer);;
            return true;
        }

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadSByte);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteSByte);
    }

	
    /// <summary>
    /// Type Model for <see cref="sbyte?"/>.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class NullableSByteTypeModel : ScalarTypeModel
    {
        public NullableSByteTypeModel() : base(typeof(sbyte?), sizeof(sbyte)) 
        {
        }

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = null;
            return false;
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
        public UShortTypeModel() : base(typeof(ushort), sizeof(ushort)) 
        {
        }
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(ushort)({value})";
			return true;
		}

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = typeof(UShortSpanComparer);;
            return true;
        }

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadUShort);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteUShort);
    }

	
    /// <summary>
    /// Type Model for <see cref="ushort?"/>.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class NullableUShortTypeModel : ScalarTypeModel
    {
        public NullableUShortTypeModel() : base(typeof(ushort?), sizeof(ushort)) 
        {
        }

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = null;
            return false;
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
        public ShortTypeModel() : base(typeof(short), sizeof(short)) 
        {
        }
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(short)({value})";
			return true;
		}

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = typeof(ShortSpanComparer);;
            return true;
        }

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadShort);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteShort);
    }

	
    /// <summary>
    /// Type Model for <see cref="short?"/>.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class NullableShortTypeModel : ScalarTypeModel
    {
        public NullableShortTypeModel() : base(typeof(short?), sizeof(short)) 
        {
        }

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = null;
            return false;
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
        public IntTypeModel() : base(typeof(int), sizeof(int)) 
        {
        }
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(int)({value})";
			return true;
		}

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = typeof(IntSpanComparer);;
            return true;
        }

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadInt);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteInt);
    }

	
    /// <summary>
    /// Type Model for <see cref="int?"/>.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class NullableIntTypeModel : ScalarTypeModel
    {
        public NullableIntTypeModel() : base(typeof(int?), sizeof(int)) 
        {
        }

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = null;
            return false;
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
        public UIntTypeModel() : base(typeof(uint), sizeof(uint)) 
        {
        }
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(uint)({value}u)";
			return true;
		}

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = typeof(UIntSpanComparer);;
            return true;
        }

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadUInt);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteUInt);
    }

	
    /// <summary>
    /// Type Model for <see cref="uint?"/>.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class NullableUIntTypeModel : ScalarTypeModel
    {
        public NullableUIntTypeModel() : base(typeof(uint?), sizeof(uint)) 
        {
        }

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = null;
            return false;
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
        public LongTypeModel() : base(typeof(long), sizeof(long)) 
        {
        }
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(long)({value}L)";
			return true;
		}

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = typeof(LongSpanComparer);;
            return true;
        }

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadLong);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteLong);
    }

	
    /// <summary>
    /// Type Model for <see cref="long?"/>.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class NullableLongTypeModel : ScalarTypeModel
    {
        public NullableLongTypeModel() : base(typeof(long?), sizeof(long)) 
        {
        }

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = null;
            return false;
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
        public ULongTypeModel() : base(typeof(ulong), sizeof(ulong)) 
        {
        }
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(ulong)({value}ul)";
			return true;
		}

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = typeof(ULongSpanComparer);;
            return true;
        }

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadULong);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteULong);
    }

	
    /// <summary>
    /// Type Model for <see cref="ulong?"/>.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class NullableULongTypeModel : ScalarTypeModel
    {
        public NullableULongTypeModel() : base(typeof(ulong?), sizeof(ulong)) 
        {
        }

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = null;
            return false;
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
        public FloatTypeModel() : base(typeof(float), sizeof(float)) 
        {
        }
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(float)({value}f)";
			return true;
		}

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = typeof(FloatSpanComparer);;
            return true;
        }

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadFloat);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteFloat);
    }

	
    /// <summary>
    /// Type Model for <see cref="float?"/>.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class NullableFloatTypeModel : ScalarTypeModel
    {
        public NullableFloatTypeModel() : base(typeof(float?), sizeof(float)) 
        {
        }

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = null;
            return false;
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
        public DoubleTypeModel() : base(typeof(double), sizeof(double)) 
        {
        }
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(double)({value}d)";
			return true;
		}

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = typeof(DoubleSpanComparer);;
            return true;
        }

        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadDouble);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteDouble);
    }

	
    /// <summary>
    /// Type Model for <see cref="double?"/>.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class NullableDoubleTypeModel : ScalarTypeModel
    {
        public NullableDoubleTypeModel() : base(typeof(double?), sizeof(double)) 
        {
        }

        public override bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = null;
            return false;
        }
		
        protected override string InputBufferReadMethodName => nameof(IInputBuffer.ReadDouble);

        protected override string SpanWriterWriteMethodName => nameof(ISpanWriter.WriteDouble);
    }

	
	public class ScalarTypeModelProvider : ITypeModelProvider
	{
        public bool TryCreateTypeModel(TypeModelContainer container, Type type, out ITypeModel typeModel)
		{
			typeModel = null;
		
			if (type == typeof(bool))
			{
				typeModel = new BoolTypeModel(); 
				return true;
			}

			if (type == typeof(bool?))
			{
				typeModel = new NullableBoolTypeModel(); 
				return true;
			}

		
			if (type == typeof(byte))
			{
				typeModel = new ByteTypeModel(); 
				return true;
			}

			if (type == typeof(byte?))
			{
				typeModel = new NullableByteTypeModel(); 
				return true;
			}

		
			if (type == typeof(sbyte))
			{
				typeModel = new SByteTypeModel(); 
				return true;
			}

			if (type == typeof(sbyte?))
			{
				typeModel = new NullableSByteTypeModel(); 
				return true;
			}

		
			if (type == typeof(ushort))
			{
				typeModel = new UShortTypeModel(); 
				return true;
			}

			if (type == typeof(ushort?))
			{
				typeModel = new NullableUShortTypeModel(); 
				return true;
			}

		
			if (type == typeof(short))
			{
				typeModel = new ShortTypeModel(); 
				return true;
			}

			if (type == typeof(short?))
			{
				typeModel = new NullableShortTypeModel(); 
				return true;
			}

		
			if (type == typeof(int))
			{
				typeModel = new IntTypeModel(); 
				return true;
			}

			if (type == typeof(int?))
			{
				typeModel = new NullableIntTypeModel(); 
				return true;
			}

		
			if (type == typeof(uint))
			{
				typeModel = new UIntTypeModel(); 
				return true;
			}

			if (type == typeof(uint?))
			{
				typeModel = new NullableUIntTypeModel(); 
				return true;
			}

		
			if (type == typeof(long))
			{
				typeModel = new LongTypeModel(); 
				return true;
			}

			if (type == typeof(long?))
			{
				typeModel = new NullableLongTypeModel(); 
				return true;
			}

		
			if (type == typeof(ulong))
			{
				typeModel = new ULongTypeModel(); 
				return true;
			}

			if (type == typeof(ulong?))
			{
				typeModel = new NullableULongTypeModel(); 
				return true;
			}

		
			if (type == typeof(float))
			{
				typeModel = new FloatTypeModel(); 
				return true;
			}

			if (type == typeof(float?))
			{
				typeModel = new NullableFloatTypeModel(); 
				return true;
			}

		
			if (type == typeof(double))
			{
				typeModel = new DoubleTypeModel(); 
				return true;
			}

			if (type == typeof(double?))
			{
				typeModel = new NullableDoubleTypeModel(); 
				return true;
			}

		
			return false;
		}
		
        public bool TryResolveFbsAlias(TypeModelContainer container, string alias, out ITypeModel typeModel)
		{
			typeModel = null;
			switch (alias)
			{
						case "bool":
							typeModel = new BoolTypeModel();
					break;

						case "ubyte":
						case "uint8":
							typeModel = new ByteTypeModel();
					break;

						case "byte":
						case "int8":
							typeModel = new SByteTypeModel();
					break;

						case "ushort":
						case "uint16":
							typeModel = new UShortTypeModel();
					break;

						case "short":
						case "int16":
							typeModel = new ShortTypeModel();
					break;

						case "int":
						case "int32":
							typeModel = new IntTypeModel();
					break;

						case "uint":
						case "uint32":
							typeModel = new UIntTypeModel();
					break;

						case "long":
						case "int64":
							typeModel = new LongTypeModel();
					break;

						case "ulong":
						case "uint64":
							typeModel = new ULongTypeModel();
					break;

						case "float":
						case "float32":
							typeModel = new FloatTypeModel();
					break;

						case "double":
						case "float64":
							typeModel = new DoubleTypeModel();
					break;

					}

			return typeModel != null;
		}
	}
}
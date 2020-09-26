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
    public partial class BoolTypeModel : ScalarTypeModel
    {
        public BoolTypeModel() : base(typeof(bool), sizeof(bool)) 
        {
        }
		
        public override Type SpanComparerType => typeof(BoolSpanComparer);
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(bool)({value})";
			return true;
		}

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadBool);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteBool);
    }

	
    /// <summary>
    /// Type Model for <see cref="bool?"/>.
    /// </summary>
    public partial class NullableBoolTypeModel : ScalarTypeModel
    {
        public NullableBoolTypeModel() : base(typeof(bool?), sizeof(bool)) 
        {
        }
		
		public override Type SpanComparerType => typeof(NullableBoolSpanComparer);

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadBool);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteBool);
    }

		
    /// <summary>
    /// Type Model for <see cref="byte"/>.
    /// </summary>
    public partial class ByteTypeModel : ScalarTypeModel
    {
        public ByteTypeModel() : base(typeof(byte), sizeof(byte)) 
        {
        }
		
        public override Type SpanComparerType => typeof(ByteSpanComparer);
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(byte)({value})";
			return true;
		}

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadByte);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteByte);
    }

	
    /// <summary>
    /// Type Model for <see cref="byte?"/>.
    /// </summary>
    public partial class NullableByteTypeModel : ScalarTypeModel
    {
        public NullableByteTypeModel() : base(typeof(byte?), sizeof(byte)) 
        {
        }
		
		public override Type SpanComparerType => typeof(NullableByteSpanComparer);

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadByte);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteByte);
    }

		
    /// <summary>
    /// Type Model for <see cref="sbyte"/>.
    /// </summary>
    public partial class SByteTypeModel : ScalarTypeModel
    {
        public SByteTypeModel() : base(typeof(sbyte), sizeof(sbyte)) 
        {
        }
		
        public override Type SpanComparerType => typeof(SByteSpanComparer);
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(sbyte)({value})";
			return true;
		}

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadSByte);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteSByte);
    }

	
    /// <summary>
    /// Type Model for <see cref="sbyte?"/>.
    /// </summary>
    public partial class NullableSByteTypeModel : ScalarTypeModel
    {
        public NullableSByteTypeModel() : base(typeof(sbyte?), sizeof(sbyte)) 
        {
        }
		
		public override Type SpanComparerType => typeof(NullableSByteSpanComparer);

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadSByte);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteSByte);
    }

		
    /// <summary>
    /// Type Model for <see cref="ushort"/>.
    /// </summary>
    public partial class UShortTypeModel : ScalarTypeModel
    {
        public UShortTypeModel() : base(typeof(ushort), sizeof(ushort)) 
        {
        }
		
        public override Type SpanComparerType => typeof(UShortSpanComparer);
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(ushort)({value})";
			return true;
		}

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadUShort);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteUShort);
    }

	
    /// <summary>
    /// Type Model for <see cref="ushort?"/>.
    /// </summary>
    public partial class NullableUShortTypeModel : ScalarTypeModel
    {
        public NullableUShortTypeModel() : base(typeof(ushort?), sizeof(ushort)) 
        {
        }
		
		public override Type SpanComparerType => typeof(NullableUShortSpanComparer);

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadUShort);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteUShort);
    }

		
    /// <summary>
    /// Type Model for <see cref="short"/>.
    /// </summary>
    public partial class ShortTypeModel : ScalarTypeModel
    {
        public ShortTypeModel() : base(typeof(short), sizeof(short)) 
        {
        }
		
        public override Type SpanComparerType => typeof(ShortSpanComparer);
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(short)({value})";
			return true;
		}

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadShort);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteShort);
    }

	
    /// <summary>
    /// Type Model for <see cref="short?"/>.
    /// </summary>
    public partial class NullableShortTypeModel : ScalarTypeModel
    {
        public NullableShortTypeModel() : base(typeof(short?), sizeof(short)) 
        {
        }
		
		public override Type SpanComparerType => typeof(NullableShortSpanComparer);

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadShort);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteShort);
    }

		
    /// <summary>
    /// Type Model for <see cref="int"/>.
    /// </summary>
    public partial class IntTypeModel : ScalarTypeModel
    {
        public IntTypeModel() : base(typeof(int), sizeof(int)) 
        {
        }
		
        public override Type SpanComparerType => typeof(IntSpanComparer);
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(int)({value})";
			return true;
		}

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadInt);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteInt);
    }

	
    /// <summary>
    /// Type Model for <see cref="int?"/>.
    /// </summary>
    public partial class NullableIntTypeModel : ScalarTypeModel
    {
        public NullableIntTypeModel() : base(typeof(int?), sizeof(int)) 
        {
        }
		
		public override Type SpanComparerType => typeof(NullableIntSpanComparer);

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadInt);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteInt);
    }

		
    /// <summary>
    /// Type Model for <see cref="uint"/>.
    /// </summary>
    public partial class UIntTypeModel : ScalarTypeModel
    {
        public UIntTypeModel() : base(typeof(uint), sizeof(uint)) 
        {
        }
		
        public override Type SpanComparerType => typeof(UIntSpanComparer);
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(uint)({value}u)";
			return true;
		}

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadUInt);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteUInt);
    }

	
    /// <summary>
    /// Type Model for <see cref="uint?"/>.
    /// </summary>
    public partial class NullableUIntTypeModel : ScalarTypeModel
    {
        public NullableUIntTypeModel() : base(typeof(uint?), sizeof(uint)) 
        {
        }
		
		public override Type SpanComparerType => typeof(NullableUIntSpanComparer);

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadUInt);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteUInt);
    }

		
    /// <summary>
    /// Type Model for <see cref="long"/>.
    /// </summary>
    public partial class LongTypeModel : ScalarTypeModel
    {
        public LongTypeModel() : base(typeof(long), sizeof(long)) 
        {
        }
		
        public override Type SpanComparerType => typeof(LongSpanComparer);
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(long)({value}L)";
			return true;
		}

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadLong);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteLong);
    }

	
    /// <summary>
    /// Type Model for <see cref="long?"/>.
    /// </summary>
    public partial class NullableLongTypeModel : ScalarTypeModel
    {
        public NullableLongTypeModel() : base(typeof(long?), sizeof(long)) 
        {
        }
		
		public override Type SpanComparerType => typeof(NullableLongSpanComparer);

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadLong);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteLong);
    }

		
    /// <summary>
    /// Type Model for <see cref="ulong"/>.
    /// </summary>
    public partial class ULongTypeModel : ScalarTypeModel
    {
        public ULongTypeModel() : base(typeof(ulong), sizeof(ulong)) 
        {
        }
		
        public override Type SpanComparerType => typeof(ULongSpanComparer);
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(ulong)({value}ul)";
			return true;
		}

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadULong);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteULong);
    }

	
    /// <summary>
    /// Type Model for <see cref="ulong?"/>.
    /// </summary>
    public partial class NullableULongTypeModel : ScalarTypeModel
    {
        public NullableULongTypeModel() : base(typeof(ulong?), sizeof(ulong)) 
        {
        }
		
		public override Type SpanComparerType => typeof(NullableULongSpanComparer);

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadULong);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteULong);
    }

		
    /// <summary>
    /// Type Model for <see cref="float"/>.
    /// </summary>
    public partial class FloatTypeModel : ScalarTypeModel
    {
        public FloatTypeModel() : base(typeof(float), sizeof(float)) 
        {
        }
		
        public override Type SpanComparerType => typeof(FloatSpanComparer);
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(float)({value}f)";
			return true;
		}

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadFloat);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteFloat);
    }

	
    /// <summary>
    /// Type Model for <see cref="float?"/>.
    /// </summary>
    public partial class NullableFloatTypeModel : ScalarTypeModel
    {
        public NullableFloatTypeModel() : base(typeof(float?), sizeof(float)) 
        {
        }
		
		public override Type SpanComparerType => typeof(NullableFloatSpanComparer);

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadFloat);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteFloat);
    }

		
    /// <summary>
    /// Type Model for <see cref="double"/>.
    /// </summary>
    public partial class DoubleTypeModel : ScalarTypeModel
    {
        public DoubleTypeModel() : base(typeof(double), sizeof(double)) 
        {
        }
		
        public override Type SpanComparerType => typeof(DoubleSpanComparer);
		
        public override bool TryFormatStringAsLiteral(string value, out string literal)
		{
			literal = $"(double)({value}d)";
			return true;
		}

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadDouble);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteDouble);
    }

	
    /// <summary>
    /// Type Model for <see cref="double?"/>.
    /// </summary>
    public partial class NullableDoubleTypeModel : ScalarTypeModel
    {
        public NullableDoubleTypeModel() : base(typeof(double?), sizeof(double)) 
        {
        }
		
		public override Type SpanComparerType => typeof(NullableDoubleSpanComparer);

        protected override string InputBufferReadMethodName => nameof(InputBuffer.ReadDouble);

        protected override string SpanWriterWriteMethodName => nameof(SpanWriter.WriteDouble);
    }

	
	public class ScalarTypeModelProvider : ITypeModelProvider
	{
        public bool TryCreateTypeModel(Type type, out ITypeModel typeModel)
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
		
        public bool TryResolveFbsAlias(string alias, out ITypeModel typeModel)
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
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



namespace FlatSharp
{
	using System;
	using System.ComponentModel;

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct BoolSpanComparer : ISpanComparer
    {
		private readonly bool defaultValue;

		public BoolSpanComparer(bool defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			bool leftValue = leftExists ? ScalarSpanReader.ReadBool(left) : this.defaultValue;
			bool rightValue = rightExists ? ScalarSpanReader.ReadBool(right) : this.defaultValue;

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage] // Not currently used.
	public struct NullableBoolSpanComparer : ISpanComparer
    {
		public NullableBoolSpanComparer(bool? notUsed)
		{
		}
		
        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			if (!leftExists || !rightExists)
			{
				return leftExists.CompareTo(rightExists);
			}

			return ScalarSpanReader.ReadBool(left).CompareTo(ScalarSpanReader.ReadBool(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct ByteSpanComparer : ISpanComparer
    {
		private readonly byte defaultValue;

		public ByteSpanComparer(byte defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			byte leftValue = leftExists ? ScalarSpanReader.ReadByte(left) : this.defaultValue;
			byte rightValue = rightExists ? ScalarSpanReader.ReadByte(right) : this.defaultValue;

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage] // Not currently used.
	public struct NullableByteSpanComparer : ISpanComparer
    {
		public NullableByteSpanComparer(byte? notUsed)
		{
		}
		
        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			if (!leftExists || !rightExists)
			{
				return leftExists.CompareTo(rightExists);
			}

			return ScalarSpanReader.ReadByte(left).CompareTo(ScalarSpanReader.ReadByte(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct SByteSpanComparer : ISpanComparer
    {
		private readonly sbyte defaultValue;

		public SByteSpanComparer(sbyte defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			sbyte leftValue = leftExists ? ScalarSpanReader.ReadSByte(left) : this.defaultValue;
			sbyte rightValue = rightExists ? ScalarSpanReader.ReadSByte(right) : this.defaultValue;

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage] // Not currently used.
	public struct NullableSByteSpanComparer : ISpanComparer
    {
		public NullableSByteSpanComparer(sbyte? notUsed)
		{
		}
		
        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			if (!leftExists || !rightExists)
			{
				return leftExists.CompareTo(rightExists);
			}

			return ScalarSpanReader.ReadSByte(left).CompareTo(ScalarSpanReader.ReadSByte(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct UShortSpanComparer : ISpanComparer
    {
		private readonly ushort defaultValue;

		public UShortSpanComparer(ushort defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			ushort leftValue = leftExists ? ScalarSpanReader.ReadUShort(left) : this.defaultValue;
			ushort rightValue = rightExists ? ScalarSpanReader.ReadUShort(right) : this.defaultValue;

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage] // Not currently used.
	public struct NullableUShortSpanComparer : ISpanComparer
    {
		public NullableUShortSpanComparer(ushort? notUsed)
		{
		}
		
        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			if (!leftExists || !rightExists)
			{
				return leftExists.CompareTo(rightExists);
			}

			return ScalarSpanReader.ReadUShort(left).CompareTo(ScalarSpanReader.ReadUShort(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct ShortSpanComparer : ISpanComparer
    {
		private readonly short defaultValue;

		public ShortSpanComparer(short defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			short leftValue = leftExists ? ScalarSpanReader.ReadShort(left) : this.defaultValue;
			short rightValue = rightExists ? ScalarSpanReader.ReadShort(right) : this.defaultValue;

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage] // Not currently used.
	public struct NullableShortSpanComparer : ISpanComparer
    {
		public NullableShortSpanComparer(short? notUsed)
		{
		}
		
        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			if (!leftExists || !rightExists)
			{
				return leftExists.CompareTo(rightExists);
			}

			return ScalarSpanReader.ReadShort(left).CompareTo(ScalarSpanReader.ReadShort(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct IntSpanComparer : ISpanComparer
    {
		private readonly int defaultValue;

		public IntSpanComparer(int defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			int leftValue = leftExists ? ScalarSpanReader.ReadInt(left) : this.defaultValue;
			int rightValue = rightExists ? ScalarSpanReader.ReadInt(right) : this.defaultValue;

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage] // Not currently used.
	public struct NullableIntSpanComparer : ISpanComparer
    {
		public NullableIntSpanComparer(int? notUsed)
		{
		}
		
        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			if (!leftExists || !rightExists)
			{
				return leftExists.CompareTo(rightExists);
			}

			return ScalarSpanReader.ReadInt(left).CompareTo(ScalarSpanReader.ReadInt(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct UIntSpanComparer : ISpanComparer
    {
		private readonly uint defaultValue;

		public UIntSpanComparer(uint defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			uint leftValue = leftExists ? ScalarSpanReader.ReadUInt(left) : this.defaultValue;
			uint rightValue = rightExists ? ScalarSpanReader.ReadUInt(right) : this.defaultValue;

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage] // Not currently used.
	public struct NullableUIntSpanComparer : ISpanComparer
    {
		public NullableUIntSpanComparer(uint? notUsed)
		{
		}
		
        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			if (!leftExists || !rightExists)
			{
				return leftExists.CompareTo(rightExists);
			}

			return ScalarSpanReader.ReadUInt(left).CompareTo(ScalarSpanReader.ReadUInt(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct LongSpanComparer : ISpanComparer
    {
		private readonly long defaultValue;

		public LongSpanComparer(long defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			long leftValue = leftExists ? ScalarSpanReader.ReadLong(left) : this.defaultValue;
			long rightValue = rightExists ? ScalarSpanReader.ReadLong(right) : this.defaultValue;

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage] // Not currently used.
	public struct NullableLongSpanComparer : ISpanComparer
    {
		public NullableLongSpanComparer(long? notUsed)
		{
		}
		
        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			if (!leftExists || !rightExists)
			{
				return leftExists.CompareTo(rightExists);
			}

			return ScalarSpanReader.ReadLong(left).CompareTo(ScalarSpanReader.ReadLong(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct ULongSpanComparer : ISpanComparer
    {
		private readonly ulong defaultValue;

		public ULongSpanComparer(ulong defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			ulong leftValue = leftExists ? ScalarSpanReader.ReadULong(left) : this.defaultValue;
			ulong rightValue = rightExists ? ScalarSpanReader.ReadULong(right) : this.defaultValue;

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage] // Not currently used.
	public struct NullableULongSpanComparer : ISpanComparer
    {
		public NullableULongSpanComparer(ulong? notUsed)
		{
		}
		
        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			if (!leftExists || !rightExists)
			{
				return leftExists.CompareTo(rightExists);
			}

			return ScalarSpanReader.ReadULong(left).CompareTo(ScalarSpanReader.ReadULong(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct FloatSpanComparer : ISpanComparer
    {
		private readonly float defaultValue;

		public FloatSpanComparer(float defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			float leftValue = leftExists ? ScalarSpanReader.ReadFloat(left) : this.defaultValue;
			float rightValue = rightExists ? ScalarSpanReader.ReadFloat(right) : this.defaultValue;

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage] // Not currently used.
	public struct NullableFloatSpanComparer : ISpanComparer
    {
		public NullableFloatSpanComparer(float? notUsed)
		{
		}
		
        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			if (!leftExists || !rightExists)
			{
				return leftExists.CompareTo(rightExists);
			}

			return ScalarSpanReader.ReadFloat(left).CompareTo(ScalarSpanReader.ReadFloat(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct DoubleSpanComparer : ISpanComparer
    {
		private readonly double defaultValue;

		public DoubleSpanComparer(double defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			double leftValue = leftExists ? ScalarSpanReader.ReadDouble(left) : this.defaultValue;
			double rightValue = rightExists ? ScalarSpanReader.ReadDouble(right) : this.defaultValue;

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage] // Not currently used.
	public struct NullableDoubleSpanComparer : ISpanComparer
    {
		public NullableDoubleSpanComparer(double? notUsed)
		{
		}
		
        public int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right)
        {
			if (!leftExists || !rightExists)
			{
				return leftExists.CompareTo(rightExists);
			}

			return ScalarSpanReader.ReadDouble(left).CompareTo(ScalarSpanReader.ReadDouble(right));
        }
    }

	}
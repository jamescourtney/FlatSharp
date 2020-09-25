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
    public class BoolSpanComparer : ISpanComparer
    {
		private readonly bool defaultValue;

		public BoolSpanComparer(bool defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			bool leftValue = this.defaultValue;
			bool rightValue = this.defaultValue;

			if (left.Length > 0)
			{
				leftValue = ScalarSpanReader.ReadBool(left);
			}

			if (right.Length > 0)
			{
				rightValue = ScalarSpanReader.ReadBool(right);
			}

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	public class NullableBoolSpanComparer : ISpanComparer
    {
		public NullableBoolSpanComparer(bool? notUsed)
		{
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			bool leftNull = left.Length == 0;
			bool rightNull = right.Length == 0;

			if (leftNull || rightNull)
			{
				return rightNull.CompareTo(leftNull);
			}

			return ScalarSpanReader.ReadBool(left).CompareTo(ScalarSpanReader.ReadBool(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ByteSpanComparer : ISpanComparer
    {
		private readonly byte defaultValue;

		public ByteSpanComparer(byte defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			byte leftValue = this.defaultValue;
			byte rightValue = this.defaultValue;

			if (left.Length > 0)
			{
				leftValue = ScalarSpanReader.ReadByte(left);
			}

			if (right.Length > 0)
			{
				rightValue = ScalarSpanReader.ReadByte(right);
			}

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	public class NullableByteSpanComparer : ISpanComparer
    {
		public NullableByteSpanComparer(byte? notUsed)
		{
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			bool leftNull = left.Length == 0;
			bool rightNull = right.Length == 0;

			if (leftNull || rightNull)
			{
				return rightNull.CompareTo(leftNull);
			}

			return ScalarSpanReader.ReadByte(left).CompareTo(ScalarSpanReader.ReadByte(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class SByteSpanComparer : ISpanComparer
    {
		private readonly sbyte defaultValue;

		public SByteSpanComparer(sbyte defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			sbyte leftValue = this.defaultValue;
			sbyte rightValue = this.defaultValue;

			if (left.Length > 0)
			{
				leftValue = ScalarSpanReader.ReadSByte(left);
			}

			if (right.Length > 0)
			{
				rightValue = ScalarSpanReader.ReadSByte(right);
			}

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	public class NullableSByteSpanComparer : ISpanComparer
    {
		public NullableSByteSpanComparer(sbyte? notUsed)
		{
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			bool leftNull = left.Length == 0;
			bool rightNull = right.Length == 0;

			if (leftNull || rightNull)
			{
				return rightNull.CompareTo(leftNull);
			}

			return ScalarSpanReader.ReadSByte(left).CompareTo(ScalarSpanReader.ReadSByte(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class UShortSpanComparer : ISpanComparer
    {
		private readonly ushort defaultValue;

		public UShortSpanComparer(ushort defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			ushort leftValue = this.defaultValue;
			ushort rightValue = this.defaultValue;

			if (left.Length > 0)
			{
				leftValue = ScalarSpanReader.ReadUShort(left);
			}

			if (right.Length > 0)
			{
				rightValue = ScalarSpanReader.ReadUShort(right);
			}

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	public class NullableUShortSpanComparer : ISpanComparer
    {
		public NullableUShortSpanComparer(ushort? notUsed)
		{
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			bool leftNull = left.Length == 0;
			bool rightNull = right.Length == 0;

			if (leftNull || rightNull)
			{
				return rightNull.CompareTo(leftNull);
			}

			return ScalarSpanReader.ReadUShort(left).CompareTo(ScalarSpanReader.ReadUShort(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ShortSpanComparer : ISpanComparer
    {
		private readonly short defaultValue;

		public ShortSpanComparer(short defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			short leftValue = this.defaultValue;
			short rightValue = this.defaultValue;

			if (left.Length > 0)
			{
				leftValue = ScalarSpanReader.ReadShort(left);
			}

			if (right.Length > 0)
			{
				rightValue = ScalarSpanReader.ReadShort(right);
			}

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	public class NullableShortSpanComparer : ISpanComparer
    {
		public NullableShortSpanComparer(short? notUsed)
		{
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			bool leftNull = left.Length == 0;
			bool rightNull = right.Length == 0;

			if (leftNull || rightNull)
			{
				return rightNull.CompareTo(leftNull);
			}

			return ScalarSpanReader.ReadShort(left).CompareTo(ScalarSpanReader.ReadShort(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class IntSpanComparer : ISpanComparer
    {
		private readonly int defaultValue;

		public IntSpanComparer(int defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			int leftValue = this.defaultValue;
			int rightValue = this.defaultValue;

			if (left.Length > 0)
			{
				leftValue = ScalarSpanReader.ReadInt(left);
			}

			if (right.Length > 0)
			{
				rightValue = ScalarSpanReader.ReadInt(right);
			}

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	public class NullableIntSpanComparer : ISpanComparer
    {
		public NullableIntSpanComparer(int? notUsed)
		{
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			bool leftNull = left.Length == 0;
			bool rightNull = right.Length == 0;

			if (leftNull || rightNull)
			{
				return rightNull.CompareTo(leftNull);
			}

			return ScalarSpanReader.ReadInt(left).CompareTo(ScalarSpanReader.ReadInt(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class UIntSpanComparer : ISpanComparer
    {
		private readonly uint defaultValue;

		public UIntSpanComparer(uint defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			uint leftValue = this.defaultValue;
			uint rightValue = this.defaultValue;

			if (left.Length > 0)
			{
				leftValue = ScalarSpanReader.ReadUInt(left);
			}

			if (right.Length > 0)
			{
				rightValue = ScalarSpanReader.ReadUInt(right);
			}

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	public class NullableUIntSpanComparer : ISpanComparer
    {
		public NullableUIntSpanComparer(uint? notUsed)
		{
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			bool leftNull = left.Length == 0;
			bool rightNull = right.Length == 0;

			if (leftNull || rightNull)
			{
				return rightNull.CompareTo(leftNull);
			}

			return ScalarSpanReader.ReadUInt(left).CompareTo(ScalarSpanReader.ReadUInt(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class LongSpanComparer : ISpanComparer
    {
		private readonly long defaultValue;

		public LongSpanComparer(long defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			long leftValue = this.defaultValue;
			long rightValue = this.defaultValue;

			if (left.Length > 0)
			{
				leftValue = ScalarSpanReader.ReadLong(left);
			}

			if (right.Length > 0)
			{
				rightValue = ScalarSpanReader.ReadLong(right);
			}

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	public class NullableLongSpanComparer : ISpanComparer
    {
		public NullableLongSpanComparer(long? notUsed)
		{
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			bool leftNull = left.Length == 0;
			bool rightNull = right.Length == 0;

			if (leftNull || rightNull)
			{
				return rightNull.CompareTo(leftNull);
			}

			return ScalarSpanReader.ReadLong(left).CompareTo(ScalarSpanReader.ReadLong(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ULongSpanComparer : ISpanComparer
    {
		private readonly ulong defaultValue;

		public ULongSpanComparer(ulong defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			ulong leftValue = this.defaultValue;
			ulong rightValue = this.defaultValue;

			if (left.Length > 0)
			{
				leftValue = ScalarSpanReader.ReadULong(left);
			}

			if (right.Length > 0)
			{
				rightValue = ScalarSpanReader.ReadULong(right);
			}

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	public class NullableULongSpanComparer : ISpanComparer
    {
		public NullableULongSpanComparer(ulong? notUsed)
		{
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			bool leftNull = left.Length == 0;
			bool rightNull = right.Length == 0;

			if (leftNull || rightNull)
			{
				return rightNull.CompareTo(leftNull);
			}

			return ScalarSpanReader.ReadULong(left).CompareTo(ScalarSpanReader.ReadULong(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class FloatSpanComparer : ISpanComparer
    {
		private readonly float defaultValue;

		public FloatSpanComparer(float defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			float leftValue = this.defaultValue;
			float rightValue = this.defaultValue;

			if (left.Length > 0)
			{
				leftValue = ScalarSpanReader.ReadFloat(left);
			}

			if (right.Length > 0)
			{
				rightValue = ScalarSpanReader.ReadFloat(right);
			}

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	public class NullableFloatSpanComparer : ISpanComparer
    {
		public NullableFloatSpanComparer(float? notUsed)
		{
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			bool leftNull = left.Length == 0;
			bool rightNull = right.Length == 0;

			if (leftNull || rightNull)
			{
				return rightNull.CompareTo(leftNull);
			}

			return ScalarSpanReader.ReadFloat(left).CompareTo(ScalarSpanReader.ReadFloat(right));
        }
    }

		
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DoubleSpanComparer : ISpanComparer
    {
		private readonly double defaultValue;

		public DoubleSpanComparer(double defaultValue)
		{
			this.defaultValue = defaultValue;
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			double leftValue = this.defaultValue;
			double rightValue = this.defaultValue;

			if (left.Length > 0)
			{
				leftValue = ScalarSpanReader.ReadDouble(left);
			}

			if (right.Length > 0)
			{
				rightValue = ScalarSpanReader.ReadDouble(right);
			}

			return leftValue.CompareTo(rightValue);
        }
    }
	
    [EditorBrowsable(EditorBrowsableState.Never)]
	public class NullableDoubleSpanComparer : ISpanComparer
    {
		public NullableDoubleSpanComparer(double? notUsed)
		{
		}

        public int Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
			bool leftNull = left.Length == 0;
			bool rightNull = right.Length == 0;

			if (leftNull || rightNull)
			{
				return rightNull.CompareTo(leftNull);
			}

			return ScalarSpanReader.ReadDouble(left).CompareTo(ScalarSpanReader.ReadDouble(right));
        }
    }

	}
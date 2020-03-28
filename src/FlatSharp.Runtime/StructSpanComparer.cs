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

    /// <summary>
    /// Compares two spans according to the default comparer for type T.
    /// </summary>
    public class StructSpanComparer<T> : ISpanComparer
        where T : struct, IComparable<T>
    {
        public delegate T StructSpanComparerReader(ReadOnlySpan<byte> span, int offset);

        private readonly StructSpanComparerReader reader;
        private readonly T defaultValue;

        public StructSpanComparer(StructSpanComparerReader reader, T defaultValue)
        {
            this.reader = reader;
            this.defaultValue = defaultValue;
        }

        public int Compare(
            ReadOnlySpan<byte> left, 
            int leftOffset,
            int leftLength,
            ReadOnlySpan<byte> right,
            int rightOffset,
            int rightLength)
        {
            T leftValue, rightValue;

            if (leftOffset < 0)
            {
                leftValue = this.defaultValue;
            }
            else
            {
                leftValue = this.reader(left, leftOffset);
            }

            if (rightOffset < 0)
            {
                rightValue = this.defaultValue;
            }
            else
            {
                rightValue = this.reader(right, rightOffset);
            }

            return leftValue.CompareTo(rightValue);
        }
    }
}

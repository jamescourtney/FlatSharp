/*
 * Copyright 2018 James Courtney
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

namespace FlatSharpTests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;
    using Xunit;

    /// <summary>
    /// Verifies parsing and handling of list vector of unions.
    /// </summary>
    public class ListVectorOfUnionTests
    {
        private readonly FlatBufferSerializer lazySerializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Lazy);
        private readonly FlatBufferSerializer greedySerializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Greedy);
        private readonly FlatBufferSerializer progressiveSerializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Progressive);

        [Theory]
        [InlineData(FlatBufferDeserializationOption.Lazy, false, 0, typeof(FlatBufferVectorOfUnion<FlatBufferUnion<string>, ArrayInputBuffer>))]
        [InlineData(FlatBufferDeserializationOption.Lazy, false, 10, typeof(FlatBufferVectorOfUnion<FlatBufferUnion<string>, ArrayInputBuffer>))]
        [InlineData(FlatBufferDeserializationOption.Progressive, false, 0, typeof(FlatBufferProgressiveVector<FlatBufferUnion<string>>))]
        [InlineData(FlatBufferDeserializationOption.Progressive, false, 10, typeof(FlatBufferProgressiveVector<FlatBufferUnion<string>>))]
        [InlineData(FlatBufferDeserializationOption.Greedy, true, 0, null)]
        public void Table_PreallocationLimit_0(
            FlatBufferDeserializationOption option,
            bool expectException,
            int size,
            Type expectedType)
        {
            this.RunTest<Table_Limit0>(option, expectException, size, expectedType);
        }

        [Theory]
        [InlineData(FlatBufferDeserializationOption.Greedy, true, 0, null)]
        [InlineData(FlatBufferDeserializationOption.Lazy, true, 0, null)]
        [InlineData(FlatBufferDeserializationOption.Progressive, false, 9, typeof(ReadOnlyCollection<FlatBufferUnion<string>>))]
        [InlineData(FlatBufferDeserializationOption.Progressive, false, 10, typeof(FlatBufferProgressiveVector<FlatBufferUnion<string>>))]
        [InlineData(FlatBufferDeserializationOption.Progressive, false, 11, typeof(FlatBufferProgressiveVector<FlatBufferUnion<string>>))]
        public void Table_PreallocationLimit_10(
            FlatBufferDeserializationOption option,
            bool expectException,
            int size,
            Type expectedType)
        {
            this.RunTest<Table_Limit10>(option, expectException, size, expectedType);
        }

        [Theory]
        [InlineData(FlatBufferDeserializationOption.Lazy, true, 0, null)]
        [InlineData(FlatBufferDeserializationOption.Progressive, false, 1025, typeof(ReadOnlyCollection<FlatBufferUnion<string>>))]
        [InlineData(FlatBufferDeserializationOption.Greedy, false, 1025, typeof(ReadOnlyCollection<FlatBufferUnion<string>>))]
        public void Table_PreallocationLimit_Max(
            FlatBufferDeserializationOption option,
            bool expectException,
            int size,
            Type expectedType)
        {
            this.RunTest<Table_LimitMax>(option, expectException, size, expectedType);
        }

        [Theory]
        [InlineData(FlatBufferDeserializationOption.Lazy, false, 1023, typeof(FlatBufferVectorOfUnion<FlatBufferUnion<string>, ArrayInputBuffer>))]
        [InlineData(FlatBufferDeserializationOption.Lazy, false, 1024, typeof(FlatBufferVectorOfUnion<FlatBufferUnion<string>, ArrayInputBuffer>))]
        [InlineData(FlatBufferDeserializationOption.Lazy, false, 1025, typeof(FlatBufferVectorOfUnion<FlatBufferUnion<string>, ArrayInputBuffer>))]
        [InlineData(FlatBufferDeserializationOption.Progressive, false, 1023, typeof(ReadOnlyCollection<FlatBufferUnion<string>>))]
        [InlineData(FlatBufferDeserializationOption.Progressive, false, 1024, typeof(FlatBufferProgressiveVector<FlatBufferUnion<string>>))]
        [InlineData(FlatBufferDeserializationOption.Progressive, false, 1025, typeof(FlatBufferProgressiveVector<FlatBufferUnion<string>>))]
        [InlineData(FlatBufferDeserializationOption.Greedy, false, 1023, typeof(ReadOnlyCollection<FlatBufferUnion<string>>))]
        [InlineData(FlatBufferDeserializationOption.Greedy, false, 1024, typeof(ReadOnlyCollection<FlatBufferUnion<string>>))]
        [InlineData(FlatBufferDeserializationOption.Greedy, false, 1025, typeof(ReadOnlyCollection<FlatBufferUnion<string>>))]
        public void Table_PreallocationLimit_Null(
            FlatBufferDeserializationOption option,
            bool expectException,
            int size,
            Type expectedType)
        {
            this.RunTest<Table_LimitNull>(option, expectException, size, expectedType);
        }

        private void RunTest<T>(
            FlatBufferDeserializationOption option,
            bool expectException,
            int size,
            Type expectedType) where T : class, ITable, new()
        {
            var serializer = this.GetSerializer(option);
            List<FlatBufferUnion<string>> items = new List<FlatBufferUnion<string>>();
            for (int i = 0; i < size; ++i)
            {
                items.Add(new(Guid.NewGuid().ToString()));
            }

            byte[] buffer = new byte[1024 * 1024];

            if (expectException)
            {
                Assert.Throws<InvalidFlatBufferDefinitionException>(() => serializer.Serialize(new T { Items = items }, buffer));
                return;
            }
            else
            {
                serializer.Serialize(new T { Items = items }, buffer);
            }

            T result = serializer.Parse<T>(buffer);

            Assert.IsAssignableFrom(expectedType, result.Items);
            var parsedItems = result.Items;
            for (int i = 0; i < size; ++i)
            {
                string a = parsedItems[i].Item1;
                string b = parsedItems[i].Item1;

                // Make sure that for non-lazy modes, we give back the same instance each time.
                Assert.Equal(
                    option != FlatBufferDeserializationOption.Lazy,
                    object.ReferenceEquals(a, b));
            }
        }

        private FlatBufferSerializer GetSerializer(FlatBufferDeserializationOption option)
        {
            return option switch
            {
                FlatBufferDeserializationOption.Lazy => this.lazySerializer,
                FlatBufferDeserializationOption.Progressive => this.progressiveSerializer,
                _ => this.greedySerializer,
            };
        }

        public interface ITable
        {
            IList<FlatBufferUnion<string>>? Items { get; set; }
        }

        [FlatBufferTable]
        public class Table_Limit0 : ITable
        {
            [FlatBufferItem(0, VectorPreallocationLimit = 0)]
            public virtual IList<FlatBufferUnion<string>>? Items { get; set; }
        }

        [FlatBufferTable]
        public class Table_LimitMax : ITable
        {
            [FlatBufferItem(0, VectorPreallocationLimit = long.MaxValue)]
            public virtual IList<FlatBufferUnion<string>>? Items { get; set; }
        }

        [FlatBufferTable]
        public class Table_Limit10 : ITable
        {
            [FlatBufferItem(0, VectorPreallocationLimit = 10)]
            public virtual IList<FlatBufferUnion<string>>? Items { get; set; }
        }

        [FlatBufferTable]
        public class Table_LimitNull : ITable
        {
            [FlatBufferItem(0)]
            public virtual IList<FlatBufferUnion<string>>? Items { get; set; }
        }
    }
}

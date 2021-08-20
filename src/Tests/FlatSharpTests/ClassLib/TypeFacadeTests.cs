/*
 * Copyright 2021 James Courtney
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
    using System.Linq;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Runtime;
    using FlatSharp.TypeModel;
    using Xunit;

    
    public class TypeFacadeTests
    {
        [Fact]
        public void Facade_DateTimeOffset_Ticks()
        {
            DateTimeOffset ts = DateTimeOffset.UtcNow;
            this.FacadeTest<long, DateTimeOffset, DateTimeTicksConverter>(ts.UtcTicks, ts);
        }

        [Fact]
        public void Facade_Chained()
        {
            TypeModelContainer container = TypeModelContainer.CreateDefault();
            container.RegisterTypeFacade<long, TimeSpan, TimeSpanTicksConverter>();
            container.RegisterTypeFacade<TimeSpan, DateTimeOffset, DateTimeTimeSpanConverter>();

            DateTimeOffset ts = DateTimeOffset.UtcNow;
            TimeSpan span = ts - DateTimeOffset.MinValue;
            long expectedValue = span.Ticks;

            this.FacadeTest<long, TimeSpan, TimeSpanTicksConverter>(expectedValue, span, container);
            this.FacadeTest<TimeSpan, DateTimeOffset, DateTimeTimeSpanConverter>(span, ts, container);
        }

        /// <summary>
        /// Test using a string to store a DateTimeOffset.
        /// </summary>
        [Fact]
        public void Facade_DateTimeOffset_String()
        {
            DateTimeOffset ts = DateTimeOffset.UtcNow;
            string str = ts.ToString("O");

            this.FacadeTest<string, DateTimeOffset, DateTimeStringConverter>(str, ts);
        }

        /// <summary>
        /// Test using nullable long to store nullable datetimeoffset.
        /// </summary>
        [Fact]
        public void Facade_NullableDateTimeOffset_NullableLong()
        {
            DateTimeOffset ts = DateTimeOffset.UtcNow;
            long ticks = ts.UtcTicks;

            this.FacadeTest<long?, DateTimeOffset?, NullableDateTimeTicksConverter>(ticks, ts);
            this.FacadeTest<long?, DateTimeOffset?, NullableDateTimeTicksConverter>(null, null);
        }

        /// <summary>
        /// Test using string to store nullable datetimeoffset.
        /// </summary>
        [Fact]
        public void Facade_NullableDateTimeOffset_NullableString()
        {
            DateTimeOffset ts = DateTimeOffset.UtcNow;
            string value = ts.ToString("O");

            this.FacadeTest<string, DateTimeOffset?, NullableDateTimeStringConverter>(value, ts);
            this.FacadeTest<string, DateTimeOffset?, NullableDateTimeStringConverter>(null, null);
        }

        /// <summary>
        /// Test using an improperly defined facade (returns null when converting to the underlying type).
        /// </summary>
        [Fact]
        public void Facade_NullReturnedFromConverter()
        {
            TypeModelContainer container = TypeModelContainer.CreateDefault();
            container.RegisterTypeFacade<string, DateTimeOffset, InvalidDateTimeStringConverter>();

            FlatBufferSerializer serializer = new FlatBufferSerializer(
                new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Greedy),
                container);

            byte[] destination = new byte[200];
            var compiled = serializer.Compile<ExtensionTable<DateTimeOffset>>();

            try
            {
                serializer.Serialize(
                    new ExtensionTable<DateTimeOffset> { Item = DateTimeOffset.UtcNow },
                    destination);

                Assert.False(true, "expected exception");
            }
            catch (InvalidOperationException ex)
            {
                Assert.Contains("ITypeFacadeConverter", ex.Message);
            }
        }

        /// <summary>
        /// Test using an improperly defined facade (returns null when converting to the underlying type).
        /// </summary>
        [Fact]
        public void Facade_NullReturnedFromConverter2()
        {
            TypeModelContainer container = TypeModelContainer.CreateDefault();
            container.RegisterTypeFacade<long?, DateTimeOffset?, InvalidNullableDateTimeNullableLongConverter>();

            FlatBufferSerializer serializer = new FlatBufferSerializer(
                new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Greedy),
                container);

            byte[] destination = new byte[200];
            var compiled = serializer.Compile<ExtensionTable<DateTimeOffset?>>();

            try
            {
                serializer.Serialize(
                    new ExtensionTable<DateTimeOffset?> { Item = DateTimeOffset.UtcNow },
                    destination);

                Assert.False(true, "Expected exception");
            }
            catch (InvalidOperationException ex)
            {
                Assert.Contains("ITypeFacadeConverter", ex.Message);
            }
        }

        [Fact]
        public void Facade_ReversedString()
        {
            ReversedString reversed = new ReversedString("foobar");
            string regular = "raboof";

            this.FacadeTest<string, ReversedString, StringReversedStringConverter>(regular, reversed);
        }

        [Fact]
        public void Facade_SortedVector()
        {
            TypeModelContainer container = TypeModelContainer.CreateDefault();
            container.RegisterTypeFacade<long, DateTimeOffset, DateTimeTicksConverter>();

            var ex = Assert.Throws<InvalidFlatBufferDefinitionException>(
                () => container.CreateTypeModel(typeof(ExtensionTable<IIndexedVector<DateTimeOffset, KeyTable>>)));

            Assert.Equal(
                "Table FlatSharpTests.TypeFacadeTests.KeyTable declares a key property on a type that that does not support being a key in a sorted vector.",
                ex.Message);
        }

        private void FacadeTest<TUnderlyingType, TType, TConverter>(
            TUnderlyingType underlyingValue,
            TType value,
            TypeModelContainer container = null) where TConverter : struct, ITypeFacadeConverter<TUnderlyingType, TType>
        {
            if (container == null)
            {
                container = TypeModelContainer.CreateDefault();
                container.RegisterTypeFacade<TUnderlyingType, TType, TConverter>();
            }

            FlatBufferSerializer serializer = new FlatBufferSerializer(
                new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Greedy),
                container);

            byte[] destination = new byte[1024];
            byte[] destination2 = new byte[1024];

            var compiled = serializer.Compile<ExtensionTable<TType>>();

            var underlyingItem = new ExtensionTable<TUnderlyingType> { Item = underlyingValue };
            var facadeItem = new ExtensionTable<TType> { Item = value };

            serializer.Serialize(facadeItem, destination);
            serializer.Serialize(underlyingItem, destination2);

            Assert.True(destination.AsSpan().SequenceEqual(destination2));

            var parsed = serializer.Parse<ExtensionTable<TType>>(destination);

            Assert.Equal(parsed.Item, value);
            Assert.Equal(serializer.GetMaxSize(facadeItem), serializer.GetMaxSize(underlyingItem));
        }

        [FlatBufferTable]
        public class ExtensionTable<T>
        {
            [FlatBufferItem(0)]
            public virtual T? Item { get; set; }
        }

        [FlatBufferTable]
        public class KeyTable
        {
            [FlatBufferItem(0, Key = true)]
            public virtual DateTimeOffset Key { get; set; }
        }

        public struct NullableDateTimeTicksConverter : ITypeFacadeConverter<long?, DateTimeOffset?>
        {
            public long? ConvertToUnderlyingType(DateTimeOffset? item)
            {
                return item?.UtcTicks;
            }

            public DateTimeOffset? ConvertFromUnderlyingType(long? item)
            {
                if (item != null)
                {
                    return new DateTimeOffset(item.Value, TimeSpan.Zero);
                }

                return null;
            }
        }

        public struct NullableDateTimeStringConverter : ITypeFacadeConverter<string, DateTimeOffset?>
        {
            public string ConvertToUnderlyingType(DateTimeOffset? item)
            {
                return item.Value.ToString("O");
            }

            public DateTimeOffset? ConvertFromUnderlyingType(string item)
            {
                return DateTimeOffset.Parse(item);
            }
        }

        public struct DateTimeStringConverter : ITypeFacadeConverter<string, DateTimeOffset>
        {
            public DateTimeOffset ConvertFromUnderlyingType(string item) => DateTimeOffset.Parse(item);

            public string ConvertToUnderlyingType(DateTimeOffset item) => item.ToString("O");
        }

        public struct InvalidDateTimeStringConverter : ITypeFacadeConverter<string, DateTimeOffset>
        {
            public DateTimeOffset ConvertFromUnderlyingType(string item) => DateTimeOffset.Parse(item);

            public string ConvertToUnderlyingType(DateTimeOffset item) => null;
        }

        public struct InvalidNullableDateTimeNullableLongConverter : ITypeFacadeConverter<long?, DateTimeOffset?>
        {
            public DateTimeOffset? ConvertFromUnderlyingType(long? item) => new DateTimeOffset(item.Value, TimeSpan.Zero);

            public long? ConvertToUnderlyingType(DateTimeOffset? item) => null;
        }

        public struct DateTimeTicksConverter : ITypeFacadeConverter<long, DateTimeOffset>
        {
            public DateTimeOffset ConvertFromUnderlyingType(long item) => new DateTimeOffset(item, TimeSpan.Zero);

            public long ConvertToUnderlyingType(DateTimeOffset item) => item.UtcTicks;
        }

        public struct TimeSpanTicksConverter : ITypeFacadeConverter<long, TimeSpan>
        {
            public long ConvertToUnderlyingType(TimeSpan item) => item.Ticks;

            public TimeSpan ConvertFromUnderlyingType(long item) => TimeSpan.FromTicks(item);
        }


        public struct DateTimeTimeSpanConverter : ITypeFacadeConverter<TimeSpan, DateTimeOffset>
        {
            public TimeSpan ConvertToUnderlyingType(DateTimeOffset item) => item - DateTimeOffset.MinValue;

            public DateTimeOffset ConvertFromUnderlyingType(TimeSpan item) => DateTimeOffset.MinValue + item;
        }

        public class ReversedString
        {
            public ReversedString(string str)
            {
                this.Value = str;
            }

            public string Value { get; }

            public override int GetHashCode()
            {
                return this.Value.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return this.Value == (obj as ReversedString)?.Value;
            }
        }

        public struct StringReversedStringConverter : ITypeFacadeConverter<string, ReversedString>
        {
            public string ConvertToUnderlyingType(ReversedString item) => Reverse(item?.Value);

            public ReversedString ConvertFromUnderlyingType(string item) => new ReversedString(Reverse(item));

            private static string Reverse(string str)
            {
                if (str == null)
                {
                    return null;
                }

                return string.Join("", str.Reverse());
            }
        }
    }
}

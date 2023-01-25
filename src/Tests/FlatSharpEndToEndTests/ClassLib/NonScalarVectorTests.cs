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

using System.Linq;

namespace FlatSharpEndToEndTests.ClassLib.NonScalarVectorTests;

/// <summary>
/// Tests various types of vectors (List/ReadOnlyList/Memory/ReadOnlyMemory/Array) for non-primitive types.
/// </summary>
public class NonScalarVectorTests
{
    private static readonly Random r = new Random();

    public static IEnumerable<object[]> EnumValues()
    {
        foreach (var item in Enum.GetValues(typeof(FlatBufferDeserializationOption)))
        {
            yield return new object[] { item };
        }
    }

    [Theory]
    [MemberData(nameof(EnumValues))]
    public void StringVector(FlatBufferDeserializationOption option)
    {
        this.TestType<Root, RootReadOnly, string>(
            option,
            () =>
            {
                byte[] b = new byte[20];
                r.NextBytes(b);
                return Convert.ToBase64String(b);
            });
    }

    [Theory]
    [MemberData(nameof(EnumValues))]
    public void TableVector(FlatBufferDeserializationOption option)
    {
        this.TestType<Root, RootReadOnly, InnerTable>(
            option,
            () =>
            {
                byte[] b = new byte[20];
                r.NextBytes(b);
                return new InnerTable { Value = Convert.ToBase64String(b) };
            });
    }

    [Theory]
    [MemberData(nameof(EnumValues))]
    public void StructVector(FlatBufferDeserializationOption option)
    {
        this.TestType<Root, RootReadOnly, InnerStruct>(
            option,
            () =>
            {
                return new InnerStruct { Value = r.Next() };
            });
    }

    private void TestType<TTable, TReadOnlyTable, T>(FlatBufferDeserializationOption option, Func<T> generator) 
        where T : IEquatable<T>
        where TTable : class, ITable<T>, IFlatBufferSerializable<TTable>, new()
        where TReadOnlyTable : class, IReadOnlyTable<T>, IFlatBufferSerializable<TReadOnlyTable>, new()
    {
        {
            var memoryTable = new TTable()
            {
                Vector = Enumerable.Range(0, 10).Select(i => generator()).ToArray()
            };

            var memoryTableResult = memoryTable.SerializeAndParse(option);
            var resultVector = memoryTableResult.Vector;

            for (int i = 0; i < memoryTableResult.Vector.Count; ++i)
            {
                Assert.Equal<T>(memoryTable.Vector[i], resultVector[i]);

                // reference equality should correspond to the serializer.
                Assert.Equal(option != FlatBufferDeserializationOption.Lazy, object.ReferenceEquals(resultVector[i], resultVector[i]));
            }
        }

        {
            var memoryTable = new TReadOnlyTable()
            {
                Vector = Enumerable.Range(0, 10).Select(i => generator()).ToArray()
            };

            var memoryTableResult = memoryTable.SerializeAndParse(option);
            var resultVector = memoryTableResult.Vector;

            for (int i = 0; i < memoryTableResult.Vector.Count; ++i)
            {
                Assert.Equal(memoryTable.Vector[i], resultVector[i]);

                // reference equality should correspond to the serializer.
                Assert.Equal(option != FlatBufferDeserializationOption.Lazy, object.ReferenceEquals(resultVector[i], resultVector[i]));
            }
        }
    }
}
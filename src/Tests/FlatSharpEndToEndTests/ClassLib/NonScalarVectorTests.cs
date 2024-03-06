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
[TestClass]
public class NonScalarVectorTests
{
    private static readonly Random r = new Random();

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
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

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
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

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
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
                Assert.AreEqual<T>(memoryTable.Vector[i], resultVector[i]);

                // reference equality should correspond to the serializer.
                Assert.AreEqual(option != FlatBufferDeserializationOption.Lazy, object.ReferenceEquals(resultVector[i], resultVector[i]));
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
                Assert.AreEqual(memoryTable.Vector[i], resultVector[i]);

                // reference equality should correspond to the serializer.
                Assert.AreEqual(option != FlatBufferDeserializationOption.Lazy, object.ReferenceEquals(resultVector[i], resultVector[i]));
            }
        }
    }
}

public interface ITable<T> where T : IEquatable<T>
{ 
    IList<T> Vector { get; set; }
}


public interface IReadOnlyTable<T> where T : IEquatable<T>
{
    IReadOnlyList<T> Vector { get; set; }
}


public partial class Root : ITable<InnerTable>, ITable<InnerStruct>, ITable<string>
{
    IList<string> ITable<string>.Vector
    {
        get => this.StringVector;
        set => this.StringVector = value;
    }

    IList<InnerTable> ITable<InnerTable>.Vector
    {
        get => this.TableVector;
        set => this.TableVector = value;
    }

    IList<InnerStruct> ITable<InnerStruct>.Vector
    {
        get => this.StructVector;
        set => this.StructVector = value;
    }
}

public partial class RootReadOnly : IReadOnlyTable<InnerTable>, IReadOnlyTable<InnerStruct>, IReadOnlyTable<string>
{
    IReadOnlyList<string> IReadOnlyTable<string>.Vector
    {
        get => this.StringVector;
        set => this.StringVector = value;
    }

    IReadOnlyList<InnerTable> IReadOnlyTable<InnerTable>.Vector
    {
        get => this.TableVector;
        set => this.TableVector = value;
    }

    IReadOnlyList<InnerStruct> IReadOnlyTable<InnerStruct>.Vector
    {
        get => this.StructVector;
        set => this.StructVector = value;
    }
}

public partial class InnerTable : IEquatable<InnerTable>
{
    public bool Equals(InnerTable other)
    {
        return other?.Value == this.Value;
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as InnerTable);
    }

    public override int GetHashCode()
    {
        return this.Value.GetHashCode();
    }
}

public partial class InnerStruct : IEquatable<InnerStruct>
{
    public bool Equals(InnerStruct other)
    {
        return other?.Value == this.Value;
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as InnerStruct);
    }

    public override int GetHashCode()
    {
        return this.Value.GetHashCode();
    }
}
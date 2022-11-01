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

using System.Collections.ObjectModel;

namespace FlatSharpTests;

/// <summary>
/// Verifies parsing and handling of list vector of unions.
/// </summary>
public class ListVectorOfUnionTests
{
    [Theory]
    [InlineData(FlatBufferDeserializationOption.Lazy, 1023, "FlatBufferVectorBase")]
    [InlineData(FlatBufferDeserializationOption.Progressive, 1023, "FlatBufferProgressiveVector")]
    [InlineData(FlatBufferDeserializationOption.Greedy, 1023, "ImmutableList")]
    [InlineData(FlatBufferDeserializationOption.GreedyMutable, 1023, "List")]
    public void Table_PreallocationLimit_Null(
        FlatBufferDeserializationOption option,
        int size,
        string expectedType)
    {
        this.RunTest<Table>(option, size, expectedType);
    }

    private void RunTest<T>(
        FlatBufferDeserializationOption option,
        int size,
        string expectedType) where T : class, ITable, new()
    {
        var serializer = this.GetSerializer<T>(option);
        List<FlatBufferUnion<string>> items = new List<FlatBufferUnion<string>>();
        for (int i = 0; i < size; ++i)
        {
            items.Add(new(Guid.NewGuid().ToString()));
        }

        byte[] buffer = new byte[1024 * 1024];
        serializer.Write(buffer, new T { Items = items });

        T result = serializer.Parse<T>(buffer);

        Assert.Contains(expectedType, result.Items.GetType().FullName);
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

    private ISerializer<T> GetSerializer<T>(FlatBufferDeserializationOption option) where T : class
    {
        return FlatBufferSerializer.Default.Compile<T>().WithSettings(s => s.UseDeserializationMode(option));
    }

    public interface ITable
    {
        IList<FlatBufferUnion<string>>? Items { get; set; }
    }

    [FlatBufferTable]
    public class Table : ITable
    {
        [FlatBufferItem(0)]
        public virtual IList<FlatBufferUnion<string>>? Items { get; set; }
    }
}

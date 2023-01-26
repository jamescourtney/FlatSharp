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

namespace FlatSharpEndToEndTests.PartialMethods;

public class PartialMethods
{
    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void OnInitialized(FlatBufferDeserializationOption option)
    {
        Table template = new()
        {
            S = new() { A = 4 }
        };

        Assert.Null(template.OnInitializedContext);
        Assert.Null(template.S.OnInitializedContext);

        Table parsed = template.SerializeAndParse(option);

        Assert.NotNull(parsed.OnInitializedContext);
        Assert.NotNull(parsed.S.OnInitializedContext);

        Assert.Equal(option, parsed.OnInitializedContext.Value.DeserializationOption);
        Assert.Equal(option, parsed.S.OnInitializedContext.Value.DeserializationOption);
    }
}

public partial class Table
{
    public FlatBufferDeserializationContext? OnInitializedContext { get; private set; }

    partial void OnInitialized(FlatBufferDeserializationContext? context)
    {
        this.OnInitializedContext = context;
    }
}

public partial class Struct
{
    public FlatBufferDeserializationContext? OnInitializedContext { get; private set; }

    partial void OnInitialized(FlatBufferDeserializationContext? context)
    {
        this.OnInitializedContext = context;
    }
}


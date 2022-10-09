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

using FlatSharp.CodeGen;
using FlatSharp.TypeModel;

namespace FlatSharpTests;

public static class ContextHelpers
{
    public static ParserCodeGenContext CreateParserContext(FlatBufferSerializerOptions? options = null)
    {
        return new ParserCodeGenContext(
            "a",
            "b",
            "e",
            "c",
            false,
            "d",
            new DefaultMethodNameResolver(),
            options ?? new FlatBufferSerializerOptions(),
            TypeModelContainer.CreateDefault(),
            new Dictionary<ITypeModel, List<TableFieldContext>>());
    }

    public static SerializationCodeGenContext CreateSerializeContext(FlatBufferSerializerOptions? options = null)
    {
        return new SerializationCodeGenContext(
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            false,
            new DefaultMethodNameResolver(),
            TypeModelContainer.CreateDefault(),
            options ?? new FlatBufferSerializerOptions(),
            new Dictionary<ITypeModel, List<TableFieldContext>>());
    }
}

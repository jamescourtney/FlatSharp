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

namespace FlatSharpTests
{
    using FlatSharp;
    using FlatSharp.TypeModel;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class ContextHelpers
    {
        public static ParserCodeGenContext CreateParserContext(FlatBufferSerializerOptions? options = null)
        {
            return new ParserCodeGenContext(
                "a",
                "b",
                "c",
                false,
                "d",
                new ReturnsRandomDictionary(),
                new ReturnsRandomDictionary(),
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
                new ReturnsRandomDictionary(),
                TypeModelContainer.CreateDefault(),
                options ?? new FlatBufferSerializerOptions(),
                new Dictionary<ITypeModel, List<TableFieldContext>>());
        }

        private class ReturnsRandomDictionary : IReadOnlyDictionary<Type, string>
        {
            public string this[Type key] => Guid.NewGuid().ToString();

            public IEnumerable<Type> Keys => new Type[0];

            public IEnumerable<string> Values => new string[0];

            public int Count => 0;

            public bool ContainsKey(Type key)
            {
                return true;
            }

            public IEnumerator<KeyValuePair<Type, string>> GetEnumerator()
            {
                yield break;
            }

            public bool TryGetValue(Type key, out string value)
            {
                value = this[key];
                return false;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                yield break;
            }
        }
    }
}

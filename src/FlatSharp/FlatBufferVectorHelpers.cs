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
 
namespace FlatSharp.TypeModel
{
    using System;

    internal static class FlatBufferVectorHelpers
    {
        public static (string classDef, string className) CreateFlatBufferVectorSubclass(
            Type itemType,
            string inputBufferTypeName,
            string parseFunctionName)
        {
            string className = $"FlatBufferVector_{Guid.NewGuid():n}";

            string classDef = $@"
                public sealed class {className}<{inputBufferTypeName}> : FlatBufferVector<{CSharpHelpers.GetCompilableTypeName(itemType)}, {inputBufferTypeName}>
                    where {inputBufferTypeName} : {nameof(IInputBuffer)}
                {{
                    public {className}(
                        {inputBufferTypeName} memory,
                        int offset,
                        int itemSize) : base(memory, offset, itemSize)
                    {{
                    }}

                    protected override {CSharpHelpers.GetCompilableTypeName(itemType)} ParseItem({inputBufferTypeName} memory, int offset)
                    {{
                        return {parseFunctionName}(memory, offset);
                    }}
                }}
            ";

            return (classDef, className);
        }
    }
}

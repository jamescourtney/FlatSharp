﻿/*
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

using System.Linq;

namespace FlatSharpTests.Compiler;

public class MetadataHelpers
{
    public static readonly string AllAttributes;

    static MetadataHelpers()
    {
        List<string> names = new List<string>
        {
            string.Empty,
            MetadataKeys.SerializerKind,
            MetadataKeys.SortedVector,
            MetadataKeys.SharedString,
            MetadataKeys.DefaultConstructorKind,
            MetadataKeys.VectorKind,
            MetadataKeys.Setter,
            MetadataKeys.ValueStruct,
            MetadataKeys.UnsafeValueStructVector,
            MetadataKeys.MemoryMarshalBehavior,
            MetadataKeys.ForceWrite,
            MetadataKeys.WriteThrough,
            MetadataKeys.RpcInterface,
            MetadataKeys.UnsafeExternal,
            MetadataKeys.LiteralName,
            MetadataKeys.UnsafeUnion,
            MetadataKeys.PartialProperty,
            string.Empty
        };

        AllAttributes = string.Join("\r\n", names.Select(x => $"attribute \"{x}\";"));
    }
}

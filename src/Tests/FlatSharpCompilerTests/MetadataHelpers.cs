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

namespace FlatSharpTests.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FlatSharp.Compiler;

    public class MetadataHelpers
    {
        public static readonly string AllAttributes;

        static MetadataHelpers()
        {
            List<string> names = new List<string>();

            names.Add(string.Empty);
            names.Add(MetadataKeys.SerializerKind);
            names.Add(MetadataKeys.NonVirtualProperty);
            names.Add(MetadataKeys.SortedVector);
            names.Add(MetadataKeys.SharedString);
            names.Add(MetadataKeys.DefaultConstructorKind);
            names.Add(MetadataKeys.VectorKind);
            names.Add(MetadataKeys.Setter);
            names.Add(MetadataKeys.ValueStruct);
            names.Add(MetadataKeys.UnsafeValueStructVector);
            names.Add(MetadataKeys.MemoryMarshalBehavior);
            names.Add(MetadataKeys.ForceWrite);
            names.Add(MetadataKeys.WriteThrough);
            names.Add(MetadataKeys.RpcInterface);
            names.Add(string.Empty);

            AllAttributes = string.Join("\r\n", names.Select(x => $"attribute \"{x}\";"));
        }
    }
}

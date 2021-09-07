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

namespace FlatSharp.Compiler.Schema
{
    using System;
    using FlatSharp.Attributes;

/*
/// New schema language features that are not supported by old code generators.
enum AdvancedFeatures : ulong (bit_flags) {
    AdvancedArrayFeatures,
    AdvancedUnionFeatures,
    OptionalScalars,
    DefaultVectorsAndStrings,
} 
*/
    [FlatBufferEnum(typeof(ulong))]
    [Flags]
    public enum AdvancedFeatures : ulong
    {
        None = 0,

        AdvancedArrayFeatures = 1,
        AdvancedUnionFeatures = 2,
        OptionalScalars = 4,
        DefaultVectorsAndStrings = 8,

        All = AdvancedArrayFeatures 
            | AdvancedUnionFeatures 
            | OptionalScalars 
            | DefaultVectorsAndStrings,
    }
}

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

namespace FlatSharp.Compiler
{
    /// <summary>
    /// Enumerates the values for code write pass. FlatSharp compiler proceeds in several passes, getting
    /// closer to the final output each time. This multi-phase approach is taken to allow the compiler
    /// to only have a minimal understanding of the type system and the relationship between types. Instead,
    /// it uses reflection on previous invocations to fine-tune its approach.
    /// </summary>
    public enum CodeWritingPass
    {
        /// <summary>
        /// Basic definitions of types and properties are written. Output code is reflectable but not functional.
        /// </summary>
        Initialization = 1,

        /// <summary>
        /// Consumes the assembly from the initialization pass and adds the full details of property definitions. Output code has fully-defined FlatSharp
        /// data contracts.
        /// </summary>
        PropertyModeling = 2,

        /// <summary>
        /// Serializers are generated and included in the output.
        /// </summary>
        SerializerAndRpcGeneration = 3,

        LastPass = SerializerAndRpcGeneration,
    }
}

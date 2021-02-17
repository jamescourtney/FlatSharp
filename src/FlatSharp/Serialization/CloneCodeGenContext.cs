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

namespace FlatSharp
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Code gen context for serialization methods.
    /// </summary>
    public class CloneCodeGenContext
    {
        public CloneCodeGenContext(string itemVariableName, IReadOnlyDictionary<Type, string> methodNameMap)
        {
            this.ItemVariableName = itemVariableName;
            this.MethodNameMap = methodNameMap;
        }

        /// <summary>
        /// The variable name of the current value to serialize.
        /// </summary>
        public string ItemVariableName { get; private set; }

        /// <summary>
        /// A mapping of type to clone method name for that type.
        /// </summary>
        public IReadOnlyDictionary<Type, string> MethodNameMap { get; private set; }
    }
}

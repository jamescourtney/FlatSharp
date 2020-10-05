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

namespace FlatSharp
{
    /// <summary>
    /// Defines the result of code generating a method.
    /// </summary>
    public class CodeGeneratedMethod
    {
        /// <summary>
        /// The body of the method.
        /// </summary>
        public string MethodBody { get; set; }

        /// <summary>
        /// A class definition.
        /// </summary>
        public string ClassDefinition { get; set; }

        /// <summary>
        /// Indicates if the method should be marked with aggressive inlining.
        /// </summary>
        public bool IsMethodInline { get; set; }
    }
}

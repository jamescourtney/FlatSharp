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
    /// Defines styles for generating setters.
    /// </summary>
    public enum SetterKind
    {
        /// <summary>
        /// A public setter.
        /// </summary>
        Public = 0,

        /// <summary>
        /// A protected setter.
        /// </summary>
        Protected = 1,

        /// <summary>
        /// A protected internal setter.
        /// </summary>
        ProtectedInternal = 2,

        /// <summary>
        /// No setter.
        /// </summary>
        None = 3,
    }
}

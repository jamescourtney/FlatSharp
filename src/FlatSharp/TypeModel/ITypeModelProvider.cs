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

    /// <summary>
    /// Provides a type model to FlatSharp.
    /// </summary>
    public interface ITypeModelProvider
    {
        /// <summary>
        /// Creates a type model for the given type.
        /// </summary>
        bool TryCreateTypeModel(TypeModelContainer container, Type type, out ITypeModel typeModel);

        /// <summary>
        /// Attempts to resolve the given alias from an FBS schema as a type model element.
        /// </summary>
        bool TryResolveFbsAlias(TypeModelContainer container, string alias, out ITypeModel typeModel);
    }
}

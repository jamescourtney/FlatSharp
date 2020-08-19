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

using System;

namespace FlatSharp
{
    /// <summary>
    /// Settings that can be applied to an <see cref="ISerializer{T}"/> instance.
    /// </summary>
    public class SerializerSettings
    {
        /// <summary>
        /// A factory delegate that produces <see cref="ISharedStringReader"/> instances. The given delegate
        /// must produce a new, unique <see cref="ISharedStringReader"/> each time it is invoked.
        /// </summary>
        public Func<ISharedStringReader> SharedStringReaderFactory
        {
            get;
            set;
        }

        /// <summary>
        /// A factory delegate that produces <see cref="ISharedStringWriter"/> instances. The given delegate
        /// must produce a new, unique <see cref="ISharedStringWriter"/> each time it is invoked.
        /// </summary>
        public Func<ISharedStringWriter> SharedStringWriterFactory
        {
            get;
            set;
        }
    }
}

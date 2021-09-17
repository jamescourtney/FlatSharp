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
    using System.ComponentModel;

    /// <summary>
    /// A contextual containing information about the table field.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class TableFieldContext
    {
        public TableFieldContext(string fullName, bool sharedString, bool writeThrough)
        {
            this.FullName = fullName;
            this.SharedString = sharedString;
            this.WriteThrough = writeThrough;
        }

        /// <summary>
        /// For debug purposes. Contains the full name of the associated table property.
        /// </summary>
        public readonly string FullName;

        /// <summary>
        /// Indicates if this context enables shared strings.
        /// </summary>
        public readonly bool SharedString;

        /// <summary>
        /// Indicates if this field is flagged as writethrough-enabled.
        /// </summary>
        public readonly bool WriteThrough;
    }
}

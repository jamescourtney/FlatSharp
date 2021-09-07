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
    using FlatSharp.Attributes;
    using System.Collections.Generic;

/*
/// File specific information.
/// Symbols declared within a file may be recovered by iterating over all
/// symbols and examining the `declaration_file` field.
table SchemaFile {
  /// Filename, relative to project root.
  filename:string (required, key);
  /// Names of included files, relative to project root.
  included_filenames:[string];
}

*/
    [FlatBufferTable]
    public class SchemaFile
    {
        [FlatBufferItem(0, Required = true, Key = true)]
        public virtual string FileName { get; set; } = string.Empty;

        // Must be a table.
        [FlatBufferItem(1)]
        public virtual IList<string>? IncludedFileNames { get; set; }
    }
}

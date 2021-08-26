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
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    /// <summary>
    /// Common or shared type model properties.
    /// </summary>
    public class OffsetModel
    {
        public static readonly OffsetModel Default = new(4);

        public int OffsetSize;
        public int FileIdentifierSize;
        public bool StrictFileIdentifierSize;

        public OffsetModel(int offsetSize, int fileIdentifierSize, bool strictFileIdentifierSize)
        {
            OffsetSize = offsetSize;
            FileIdentifierSize = fileIdentifierSize;
            StrictFileIdentifierSize = strictFileIdentifierSize;
        }

        public OffsetModel(int offsetSize, int fileIdentifierSize) : this(offsetSize, fileIdentifierSize, fileIdentifierSize == 4)
        {
        }

        public OffsetModel(int offsetSize) : this(offsetSize, offsetSize, offsetSize == 4)
        {
        }

        public OffsetModel(int? offsetSize, int? fileIdentifierSize, bool? strictFileIdentifierSize)
        {
            OffsetSize = offsetSize ?? 4;
            FileIdentifierSize = fileIdentifierSize ?? OffsetSize;
            StrictFileIdentifierSize = strictFileIdentifierSize ?? FileIdentifierSize == 4;
        }
        
        public void ValidateFileIdentifier(ref string? fileIdentifier)
        {
            if (string.IsNullOrEmpty(fileIdentifier)) return;

            if (fileIdentifier.Length != FileIdentifierSize)
            {
                if (StrictFileIdentifierSize)
                {
                    throw new InvalidFlatBufferDefinitionException(
                        $"File identifier '{fileIdentifier}' is invalid. FileIdentifiers must be exactly {FileIdentifierSize} ASCII characters.");
                }
                    
                fileIdentifier = fileIdentifier.Length > FileIdentifierSize
                    ? fileIdentifier.Substring(0, FileIdentifierSize)
                    : fileIdentifier.PadRight(FileIdentifierSize, '\0');
            }

            foreach (var c in fileIdentifier)
            {
                if (c < 128) continue;

                throw new InvalidFlatBufferDefinitionException($"File identifier '{fileIdentifier}' contains non-ASCII characters. Character '{c}' is invalid.");
            }
        }
    }
}

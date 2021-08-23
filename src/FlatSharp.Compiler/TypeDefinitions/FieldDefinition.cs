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
    internal record FieldDefinition
    {
        public FieldDefinition(
            string name,
            string fieldType,
            string? resolvedTypeName)
        {
            this.Name = name;
            this.FbsFieldType = fieldType;
            this.ResolvedTypeName = resolvedTypeName;
        }

        public int Index { get; set; }

        public bool IsIndexSetManually { get; set; }

        public string Name { get; set; }

        public string FbsFieldType { get; set; }

        public bool Deprecated { get; set; }

        public bool SortedVector { get; set; }

        public string? CustomGetter { get; set; }

        public bool IsKey { get; set; }

        public bool? NonVirtual { get; set; }

        public bool? ForceWrite { get; set; }

        public bool? WriteThrough { get; set; }

        public bool IsRequired { get; set; }

        public string? DefaultValue { get; set; }

        public bool IsOptionalScalar { get; set; }

        public VectorType VectorType { get; set; }

        public SetterKind SetterKind { get; set; } = SetterKind.Public;

        public AccessModifier GetterModifier { get; set; } = AccessModifier.Public;

        public bool SharedString { get; set; }

        public bool IsUnsafeStructVector { get; set; }

        /// <summary>
        /// Early-bound resolved type name. May be null if the type is unbound when the field is read.
        /// </summary>
        public string? ResolvedTypeName { get; }
    }
}

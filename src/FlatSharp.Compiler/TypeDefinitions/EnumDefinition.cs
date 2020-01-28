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
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    /// <summary>
    /// Defines an enum.
    /// </summary>
    internal class EnumDefinition : BaseSchemaMember
    {
        private BigInteger nextValue = 1;
        private readonly Dictionary<string, string> nameValuePairs = new Dictionary<string, string>();

        public EnumDefinition(string typeName, string underlyingTypeName, BaseSchemaMember parent)
            : base(typeName, parent)
        {
            this.FbsUnderlyingType = underlyingTypeName;
            this.ClrUnderlyingType = SchemaDefinition.ResolvePrimitiveType(underlyingTypeName);
        }

        protected override bool SupportsChildren => false;

        public string FbsUnderlyingType { get; }

        public string ClrUnderlyingType { get; }

        public IReadOnlyDictionary<string, string> NameValuePairs => this.nameValuePairs;

        public void AddNameValuePair(string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = CodeWriter.GetPrimitiveTypeLiteral(this.ClrUnderlyingType, value, out BigInteger bigInt);

                // C# compiler won't complain about duplicate values.
                if (this.nameValuePairs.Values.Any(x => x == value))
                {
                    ErrorContext.Current?.RegisterError($"Enum '{this.Name}' contains duplicate value '{value}'.");
                }

                this.nameValuePairs[name] = value;
                this.nextValue = bigInt + 1;
            }
            else
            {
                value = this.nextValue.ToString();
                this.nameValuePairs[name] = CodeWriter.GetPrimitiveTypeLiteral(this.ClrUnderlyingType, value, out BigInteger bigInt);
                this.nextValue++;
            }
        }

        protected override void OnWriteCode(CodeWriter writer, IReadOnlyDictionary<string, string> precompiledSerailizers)
        {
            writer.AppendLine($"[FlatBufferEnum(typeof({this.ClrUnderlyingType}))]");
            writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
            writer.AppendLine($"public enum {this.Name} : {this.ClrUnderlyingType}");
            writer.AppendLine($"{{");
            using (writer.IncreaseIndent())
            {
                foreach (var item in this.nameValuePairs)
                {
                    writer.AppendLine($"{item.Key} = {item.Value},");
                }
            }
            writer.AppendLine($"}}");
            writer.AppendLine(string.Empty);
        }
    }
}

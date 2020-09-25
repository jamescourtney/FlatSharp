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
    using FlatSharp.TypeModel;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Numerics;

    /// <summary>
    /// Defines an enum.
    /// </summary>
    internal class EnumDefinition : BaseSchemaMember
    {
        private BigInteger nextValue = 0;
        private readonly Dictionary<string, string> nameValuePairs = new Dictionary<string, string>();

        public EnumDefinition(string typeName, string underlyingTypeName, BaseSchemaMember parent)
            : base(typeName, parent)
        {
            this.FbsUnderlyingType = underlyingTypeName;
            this.UnderlyingType = SchemaDefinition.ResolveBuiltInScalarType(underlyingTypeName);
        }

        protected override bool SupportsChildren => false;

        public string FbsUnderlyingType { get; }

        public ITypeModel UnderlyingType { get; }

        public IReadOnlyDictionary<string, string> NameValuePairs => this.nameValuePairs;

        public void AddNameValuePair(string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                BigInteger bigInt = ParseInteger(value) ?? this.nextValue;

                // If the value got set backwards.
                if (bigInt < this.nextValue && this.nameValuePairs.Count > 0)
                {
                    ErrorContext.Current?.RegisterError($"Enum '{this.Name}' must declare values sorted in ascending order.");
                }

                string standardizedString = this.UnderlyingType.FormatStringAsLiteral(bigInt.ToString());

                // C# compiler won't complain about duplicate values.
                if (this.nameValuePairs.Values.Any(x => x == standardizedString))
                {
                    ErrorContext.Current?.RegisterError($"Enum '{this.Name}' contains duplicate value '{value}'.");
                }

                this.nameValuePairs[name] = standardizedString;
                this.nextValue = bigInt + 1;
            }
            else
            {
                value = this.nextValue.ToString();
                this.nameValuePairs[name] = this.UnderlyingType.FormatStringAsLiteral(value);
                this.nextValue++;
            }
        }

        protected override void OnWriteCode(CodeWriter writer, CodeWritingPass pass, string forFile, IReadOnlyDictionary<string, string> precompiledSerailizers)
        {
            writer.AppendLine($"[FlatBufferEnum(typeof({this.UnderlyingType.ClrType.FullName}))]");
            writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
            writer.AppendLine($"public enum {this.Name} : {this.UnderlyingType.ClrType.FullName}");
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

        protected override string OnGetCopyExpression(string source)
        {
            return source;
        }

        private static BigInteger? ParseInteger(string value)
        {
            if (BigInteger.TryParse(value, NumberStyles.Integer, null, out var bigInt))
            {
                return bigInt;
            }
            else
            {
                // Hex parsing is broken in .NET. Strip off the sign, parse as either long or ulong hex depending on sign, convert to decimal, and then reparse for our given format.
                // Not fast, but works well enough.
                bool negative = value.StartsWith("-");
                if (negative || value.StartsWith("+"))
                {
                    value = value.Substring(1);
                }

                if (value.StartsWith("0x") || value.StartsWith("0X"))
                {
                    value = value.Substring(2);
                    if (BigInteger.TryParse(value, NumberStyles.HexNumber, null, out bigInt))
                    {
                        if (negative)
                        {
                            bigInt *= -1;
                        }

                        return bigInt;
                    }
                }
            }

            ErrorContext.Current?.RegisterError($"Unable to parse enum value '{bigInt}' as an integer.");
            return null;
        }
    }
}

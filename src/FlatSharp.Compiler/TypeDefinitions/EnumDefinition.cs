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
        public EnumDefinition(string typeName, string underlyingTypeName, BaseSchemaMember parent)
            : base(typeName, parent)
        {
            this.FbsUnderlyingType = underlyingTypeName;
        }

        protected override bool SupportsChildren => false;

        public string FbsUnderlyingType { get; }

        public bool IsFlags { get; set; }

        public List<(string name, string? value)> NameValuePairs { get; } = new List<(string, string?)>();

        protected override void OnWriteCode(CodeWriter writer, CompileContext context)
        {
            if (!context.TypeModelContainer.TryResolveFbsAlias(this.FbsUnderlyingType, out var typeModel))
            {
                ErrorContext.Current.RegisterError($"Enum with underlying type '{this.FbsUnderlyingType}' could not be resolved by type model.");
                return;
            }

            writer.AppendLine($"[FlatBufferEnum(typeof({typeModel.ClrType.FullName}))]");
            writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");

            if (this.IsFlags)
            {
                writer.AppendLine("[System.Flags]");
            }

            writer.AppendLine($"public enum {this.Name} : {typeModel.ClrType.FullName}");
            writer.AppendLine($"{{");
            using (writer.IncreaseIndent())
            {
                Dictionary<string, string> standardizedPairs = this.IsFlags 
                    ? this.GetFormattedNameValueBitFlagsPairs(typeModel) 
                    : this.GetFormattedNameValuePairs(typeModel);

                foreach (var item in standardizedPairs)
                {
                    writer.AppendLine($"{item.Key} = {item.Value},");
                }
            }

            writer.AppendLine($"}}");
            writer.AppendLine(string.Empty);
        }

        private Dictionary<string, string> GetFormattedNameValueBitFlagsPairs(ITypeModel model)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            BigInteger nextValue = 1;
            BigInteger allFlags = 0;

            foreach (var kvp in this.NameValuePairs)
            {
                if (!string.IsNullOrEmpty(kvp.value))
                {
                    ErrorContext.Current?.RegisterError($"Enum '{this.Name}' declares the '{MetadataKeys.BitFlags}' attribute. FlatSharp does not support specifying explicit values when used in conjunction with bit flags.");
                    continue;
                }

                if (!model.TryFormatStringAsLiteral(nextValue.ToString(), out string? literal))
                {
                    ErrorContext.Current?.RegisterError($"Could not format value for enum '{this.Name}'. Value = {nextValue}.");
                    continue;
                }

                allFlags |= nextValue;
                nextValue *= 2;

                results[kvp.name] = literal;
            }

            if (model.TryFormatStringAsLiteral("0", out string? zero) &&
                model.TryFormatStringAsLiteral(allFlags.ToString(), out string? all))
            {
                results["None"] = zero;
                results["All"] = all;
            }
            else
            {
                ErrorContext.Current?.RegisterError($"Could not format value for enum '{this.Name}'. Value = {nextValue}.");
            }

            return results;
        }

        private Dictionary<string, string> GetFormattedNameValuePairs(ITypeModel model)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            BigInteger nextValue = 0;

            foreach (var kvp in this.NameValuePairs)
            {
                string key = kvp.name;
                string? value = kvp.value;

                if (results.ContainsKey(key))
                {
                    ErrorContext.Current?.RegisterError($"Enum '{this.Name}' may not have duplicate names. Duplicate = '{key}'");
                    continue;
                }

                if (!string.IsNullOrEmpty(value))
                {
                    BigInteger bigInt = ParseInteger(value) ?? nextValue;

                    // If the value got set backwards.
                    if (bigInt < nextValue && results.Count > 0)
                    {
                        ErrorContext.Current?.RegisterError($"Enum '{this.Name}' must declare values sorted in ascending order.");
                    }

                    if (!model.TryFormatStringAsLiteral(bigInt.ToString(), out string? standardizedString))
                    {
                        ErrorContext.Current?.RegisterError($"Could not format value for enum '{this.Name}'. Value = {bigInt}.");
                        continue;
                    }

                    // C# compiler won't complain about duplicate values.
                    if (results.Values.Any(x => x == standardizedString))
                    {
                        ErrorContext.Current?.RegisterError($"Enum '{this.Name}' contains duplicate value '{value}'.");
                    }

                    results[key] = standardizedString;
                    nextValue = bigInt + 1;
                }
                else
                {
                    value = nextValue.ToString();

                    if (!model.TryFormatStringAsLiteral(value, out string? standardizedString))
                    {
                        ErrorContext.Current?.RegisterError($"Could not format value for enum '{this.Name}'. Value = {value}.");
                        continue;
                    }

                    results[key] = standardizedString;
                    nextValue++;
                }
            }

            return results;
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

            ErrorContext.Current?.RegisterError($"Unable to parse enum value '{value}' as an integer.");
            return null;
        }
    }
}

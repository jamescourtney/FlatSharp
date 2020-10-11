/*
 * Copyright 2018 James Courtney
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
    using FlatSharp.TypeModel;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// A helper to assist with generating property overrides for table and struct readers.
    /// </summary>
    internal class GeneratedProperty
    {
        private readonly FlatBufferSerializerOptions options;
        private readonly PropertyInfo propertyInfo;

        public GeneratedProperty(FlatBufferSerializerOptions options, int index, ItemMemberModel memberModel)
        {
            this.options = options;
            this.propertyInfo = memberModel.PropertyInfo;
            this.MemberModel = memberModel;

            this.ReadValueMethodName = $"__ReadIndex{index}Value";

            if (memberModel.IsVirtual)
            {
                this.HasValueFieldName = options.PropertyCache ? $"__hasIndex{index}" : null;
                this.BackingFieldName = !options.Lazy ? $"__index{index}" : null;
            }
        }

        public string BackingFieldName { get; }

        public string HasValueFieldName { get; }

        public string ReadValueMethodName { get; }

        public ItemMemberModel MemberModel { get; }

        public string ReadValueMethodDefinition { get; set; }

        public string GetterBody
        {
            get
            {
                if (string.IsNullOrEmpty(this.HasValueFieldName))
                {
                    if (string.IsNullOrEmpty(this.BackingFieldName))
                    {
                        return $"return {this.ReadValueMethodName}(this.buffer, this.offset);";
                    }
                    else
                    {
                        return $"return this.{this.BackingFieldName};";
                    }
                }
                else
                {
                    return
$@"
                            if (!this.{this.HasValueFieldName})
                            {{
                                this.{this.BackingFieldName} = {this.ReadValueMethodName}(this.buffer, this.offset);
                                this.{this.HasValueFieldName} = true;
                            }}
                            return this.{this.BackingFieldName};
";
                }
            }
        }

        public string SetterBody
        {
            get
            {
                if (this.options.GenerateMutableObjects)
                {
                    if (string.IsNullOrEmpty(this.HasValueFieldName))
                    {
                        return $"this.{this.BackingFieldName} = value;";
                    }
                    else
                    {
                        return $"this.{this.BackingFieldName} = value; this.{this.HasValueFieldName} = true;";
                    }
                }
                else
                {
                    return $"throw new NotMutableException();";
                }
            }
        }

        public override string ToString()
        {
            List<string> lines = new List<string>();

            if (this.MemberModel.IsVirtual)
            {
                if (!string.IsNullOrEmpty(this.BackingFieldName))
                {
                    lines.Add($"private {CSharpHelpers.GetCompilableTypeName(this.propertyInfo.PropertyType)} {this.BackingFieldName};");
                }

                if (!string.IsNullOrEmpty(this.HasValueFieldName))
                {
                    lines.Add($"private bool {this.HasValueFieldName};");
                }

                var accessModifiers = CSharpHelpers.GetPropertyAccessModifiers(this.propertyInfo);

                lines.Add($@"{accessModifiers.propertyModifier} override {CSharpHelpers.GetCompilableTypeName(this.propertyInfo.PropertyType)} {this.propertyInfo.Name} {{");
                lines.Add($"{accessModifiers.getModifer} get {{ {this.GetterBody} }}");

                MethodInfo methodInfo = this.propertyInfo.SetMethod;
                if (methodInfo != null)
                {
                    lines.Add($"{accessModifiers.setModifier} set {{ {this.SetterBody} }}");
                }

                lines.Add("}");
            }

            lines.Add(this.ReadValueMethodDefinition);

            return string.Join("\r\n", lines);
        }
    }
}

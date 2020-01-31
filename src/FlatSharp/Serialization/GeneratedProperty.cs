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
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    internal class GeneratedProperty
    {
        private readonly FlatBufferSerializerOptions options;
        private readonly PropertyInfo propertyInfo;

        public GeneratedProperty(FlatBufferSerializerOptions options, int index, PropertyInfo propertyInfo)
        {
            this.options = options;
            this.propertyInfo = propertyInfo;
            this.HasValueFieldName = !options.GreedyDeserialize ? $"__hasIndex{index}" : null;
            this.BackingFieldName = $"__index{index}";
            this.ReadValueMethodName = $"__ReadIndex{index}Value";
        }

        public string BackingFieldName { get; }

        public string HasValueFieldName { get; }

        public string ReadValueMethodName { get; }

        public string ReadValueMethodDefinition { get; set; }

        public string GetterBody
        {
            get
            {
                if (string.IsNullOrEmpty(this.HasValueFieldName))
                {
                    return $"return this.{this.BackingFieldName};";
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
            lines.Add($"private {CSharpHelpers.GetCompilableTypeName(this.propertyInfo.PropertyType)} {this.BackingFieldName};");
            if (!string.IsNullOrEmpty(this.HasValueFieldName))
            {
                lines.Add($"private bool {this.HasValueFieldName};");
            }

            lines.Add(this.ReadValueMethodDefinition);

            lines.Add($@"{CSharpHelpers.GetAccessModifier(this.propertyInfo)} override {CSharpHelpers.GetCompilableTypeName(this.propertyInfo.PropertyType)} {this.propertyInfo.Name} {{");
            lines.Add($"get {{ {this.GetterBody} }}");

            MethodInfo methodInfo = this.propertyInfo.GetSetMethod() ?? this.propertyInfo.SetMethod;
            if (methodInfo != null)
            {
                lines.Add($"set {{ {this.SetterBody} }}");
            }

            lines.Add("}");

            return string.Join("\r\n", lines);
        }
    }
}

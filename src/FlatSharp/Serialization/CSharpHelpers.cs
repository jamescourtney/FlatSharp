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
    using FlatSharp.TypeModel;

    /// <summary>
    /// Some C# codegen helpers.
    /// </summary>
    internal static class CSharpHelpers
    {
        internal static bool ConvertProtectedInternalToProtected = true;

        internal static string GetCompilableTypeName(Type t)
        {
            if (string.IsNullOrEmpty(t.FullName))
            {
                throw new InvalidOperationException($"FlatSharp.Unexpected: {t} has null/empty fully name.");
            }

            string name;
            if (t.IsGenericType)
            {
                List<string> parameters = new List<string>();
                foreach (var generic in t.GetGenericArguments())
                {
                    parameters.Add(GetCompilableTypeName(generic));
                }

                name = $"{t.FullName.Split('`')[0]}<{string.Join(", ", parameters)}>";
            }
            else if (t.IsArray)
            {
                name = $"{GetCompilableTypeName(t.GetElementType()!)}[]";
            }
            else
            {
                name = t.FullName;
            }

            name = name.Replace('+', '.');
            return name;
        }

        internal static (string propertyModifier, string getModifer, string setModifier) GetPropertyAccessModifiers(
            PropertyInfo property)
        {
            if (property.GetMethod is null)
            {
                throw new InvalidOperationException($"FlatSharp internal error: Property {property.DeclaringType?.Name}.{property.Name} has null get method.");
            }

            var getTuple = GetAccessModifier(property.GetMethod);

            if (property.SetMethod is null)
            {
                return (getTuple.modifier, string.Empty, string.Empty);
            }

            var setTuple = GetAccessModifier(property.SetMethod);

            if (getTuple.precedence < setTuple.precedence)
            {
                return (getTuple.modifier, string.Empty, setTuple.modifier);
            }
            else if (setTuple.precedence < getTuple.precedence)
            {
                return (setTuple.modifier, getTuple.modifier, string.Empty);
            }

            return (getTuple.modifier, string.Empty, string.Empty);
        }

        internal static (int precedence, string modifier) GetAccessModifier(MethodInfo method)
        {
            if (method.IsPublic)
            {
                return (0, "public");
            }

            if (method.IsFamilyOrAssembly)
            {
                if (ConvertProtectedInternalToProtected)
                {
                    return (2, "protected");
                }

                return (1, "protected internal");
            }

            if (method.IsFamily)
            {
                return (2, "protected");
            }

            throw new InvalidOperationException("Unexpected method visibility: " + method.Name);
        }

        internal static string CreateDeserializeClass(
            string className,
            ITypeModel typeModel,
            IEnumerable<GeneratedProperty> propertyOverrides,
            FlatBufferSerializerOptions options)
        {
            string inputBufferFieldDef = "private readonly TInputBuffer buffer;";
            string offsetFieldDef = "private readonly int offset;";
            string vtableOffsetDef = string.Empty;
            string maxVTableIndexDef = string.Empty;

            List<string> ctorStatements = new List<string>
            {
                "this.buffer = buffer;",
                "this.offset = offset;"
            };

            if (options.GreedyDeserialize)
            {
                inputBufferFieldDef = string.Empty;
                offsetFieldDef = string.Empty;
                ctorStatements.Clear();
            }

            if (typeModel.SchemaType == FlatBufferSchemaType.Table)
            {
                vtableOffsetDef = "private readonly int vtableOffset;";
                maxVTableIndexDef = "private readonly int maxVTableIndex;";
                ctorStatements.Add($"buffer.{nameof(InputBufferExtensions.InitializeVTable)}(offset, out this.vtableOffset, out this.maxVTableIndex);");
            }

            foreach (var property in propertyOverrides)
            {
                if (property.MemberModel.IsVirtual)
                {
                    if (options.GreedyDeserialize)
                    {
                        ctorStatements.Add(
                            $"this.{property.BackingFieldName} = {property.ReadValueMethodName}(buffer, offset);");
                    }
                }
                else
                {
                    ctorStatements.Add(
                        $"base.{property.MemberModel.PropertyInfo.Name} = {property.ReadValueMethodName}(buffer, offset);");
                }
            }

            ConstructorInfo? specialCtor = null;
            foreach (var ctor in typeModel.ClrType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance))
            {
                var @params = ctor.GetParameters();
                if (@params.Length == 1 &&
                    @params[0].ParameterType == typeof(FlatSharpConstructorContext))
                {
                    specialCtor = ctor;
                    break;
                }
            }

            if (specialCtor is not null && 
                !specialCtor.IsPublic &&
                !specialCtor.IsFamily &&
                !specialCtor.IsFamilyOrAssembly)
            {
                throw new InvalidFlatBufferDefinitionException(
                    $"Class '{typeModel.GetCompilableTypeName()}' defines a constructor accepting {nameof(FlatSharpConstructorContext)}, but the constructor is not visible to derived classes.");
            }

            string baseParams = specialCtor is not null ? "CtorContext" : string.Empty;

            return
$@"
                private sealed class {className}<TInputBuffer> : {typeModel.GetCompilableTypeName()} where TInputBuffer : IInputBuffer
                {{
                    private static readonly {nameof(FlatSharpConstructorContext)} CtorContext 
                        = new {nameof(FlatSharpConstructorContext)}({nameof(FlatBufferDeserializationOption)}.{options.DeserializationOption});

                    {inputBufferFieldDef}
                    {offsetFieldDef}
                    {vtableOffsetDef}
                    {maxVTableIndexDef}
        
                    public {className}(TInputBuffer buffer, int offset) : base({baseParams})
                    {{
                        {string.Join("\r\n", ctorStatements)}
                    }}

                    {string.Join("\r\n", propertyOverrides)}
                }}
";
        }
    }
}

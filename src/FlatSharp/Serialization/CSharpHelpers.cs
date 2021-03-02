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
    using System.Diagnostics;
    using System.Reflection;
    using FlatSharp.TypeModel;

    /// <summary>
    /// Some C# codegen helpers.
    /// </summary>
    internal static class CSharpHelpers
    {
        internal static bool ConvertProtectedInternalToProtected = true;

        internal const string GeneratedClassInputBufferFieldName = "__buffer";
        internal const string GeneratedClassOffsetFieldName = "__offset";
        internal const string GeneratedClassMaxVtableIndexFieldName = "__maxVTableIndex";
        internal const string GeneratedClassVTableOffsetFieldName = "__vtableOffset";

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
            FlatBufferSerializerOptions options,
            CreateDeserializeClassContext context)
        {
            const string BufferParameterName = "buffer";
            const string OffsetParameterName = "offset";

            string inputBufferAccessor = "null";

            List<string> ctorStatements = new List<string>();
            List<string> fieldDefinitions = new List<string>();

            if (!string.IsNullOrEmpty(context.InputBufferFieldName))
            {
                ctorStatements.Add($"this.{context.InputBufferFieldName} = {BufferParameterName};");
                fieldDefinitions.Add($"private readonly TInputBuffer {context.InputBufferFieldName};");
                inputBufferAccessor = $"this.{context.InputBufferFieldName}";
            }

            if (!string.IsNullOrEmpty(context.OffsetFieldName))
            {
                ctorStatements.Add($"this.{context.OffsetFieldName} = {OffsetParameterName};");
                fieldDefinitions.Add($"private readonly int {context.OffsetFieldName};");
            }

            if (typeModel.SchemaType == FlatBufferSchemaType.Table)
            {
                ctorStatements.Add($"{BufferParameterName}.{nameof(InputBufferExtensions.InitializeVTable)}({OffsetParameterName}, out var __vtableLocation, out var __vtableMaxIndex);");
            }
            else
            {
                ctorStatements.Add($"int __vtableLocation = 0;");
                ctorStatements.Add($"int __vtableMaxIndex = 0;");
            }

            if (context.VTableFields is not null)
            {
                var (locationFieldName, maxIndexFieldName) = context.VTableFields.Value;
                fieldDefinitions.Add($"private readonly int {locationFieldName};");
                fieldDefinitions.Add($"private readonly int {maxIndexFieldName};");
                ctorStatements.Add($"this.{locationFieldName} = __vtableLocation;");
                ctorStatements.Add($"this.{maxIndexFieldName} = __vtableMaxIndex;");
            }

            foreach (var property in propertyOverrides)
            {
                if (property.MemberModel.IsVirtual)
                {
                    if (options.GreedyDeserialize)
                    {
                        ctorStatements.Add(
                            $"this.{property.BackingFieldName} = {property.ReadValueMethodName}(buffer, offset, __vtableLocation, __vtableMaxIndex);");
                    }
                }
                else
                {
                    ctorStatements.Add(
                        $"base.{property.MemberModel.PropertyInfo.Name} = {property.ReadValueMethodName}(buffer, offset, __vtableLocation, __vtableMaxIndex);");
                }
            }

            ConstructorInfo? specialCtor = null;
            foreach (var ctor in typeModel.ClrType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance))
            {
                var @params = ctor.GetParameters();
                if (@params.Length == 1 &&
                    @params[0].ParameterType == typeof(FlatSharpDeserializationContext))
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
                    $"Class '{typeModel.GetCompilableTypeName()}' defines a constructor accepting {nameof(FlatSharpDeserializationContext)}, but the constructor is not visible to derived classes.");
            }

            string baseParams = specialCtor is not null ? "__CtorContext" : string.Empty;

            return
$@"
                private sealed class {className}<TInputBuffer> 
                    : {typeModel.GetCompilableTypeName()} 
                    , {nameof(IFlatBufferDeserializedObject)}
                    where TInputBuffer : IInputBuffer
                {{
                    private static readonly {nameof(FlatSharpDeserializationContext)} __CtorContext 
                        = new {nameof(FlatSharpDeserializationContext)}({nameof(FlatBufferDeserializationOption)}.{options.DeserializationOption});

                    {string.Join("\r\n", fieldDefinitions)}

                    public {className}(TInputBuffer buffer, int offset) : base({baseParams})
                    {{
                        {string.Join("\r\n", ctorStatements)}
                    }}

                    Type {nameof(IFlatBufferDeserializedObject)}.{nameof(IFlatBufferDeserializedObject.TableOrStructType)} => typeof({typeModel.GetCompilableTypeName()});
                    {nameof(FlatSharpDeserializationContext)} {nameof(IFlatBufferDeserializedObject)}.{nameof(IFlatBufferDeserializedObject.DeserializationContext)} => __CtorContext;
                    {nameof(IInputBuffer)}? {nameof(IFlatBufferDeserializedObject)}.{nameof(IFlatBufferDeserializedObject.InputBuffer)} => {inputBufferAccessor};

                    {string.Join("\r\n", propertyOverrides)}
                }}
";
        }

        public class CreateDeserializeClassContext
        {
            public static CreateDeserializeClassContext GreedyTableOrStruct => new CreateDeserializeClassContext();

            public static CreateDeserializeClassContext NonGreedyStruct => new CreateDeserializeClassContext
            {
                InputBufferFieldName = GeneratedClassInputBufferFieldName,
                OffsetFieldName = GeneratedClassOffsetFieldName,
            };

            public static CreateDeserializeClassContext NonGreedyTable => new CreateDeserializeClassContext
            {
                InputBufferFieldName = GeneratedClassInputBufferFieldName,
                OffsetFieldName = GeneratedClassOffsetFieldName,
                VTableFields = (GeneratedClassVTableOffsetFieldName, GeneratedClassMaxVtableIndexFieldName),
            };

            public string? InputBufferFieldName { get; set; }

            public string? OffsetFieldName { get; set; }

            public (string vtableLocationFieldName, string vtableMaxIndexFieldName)? VTableFields { get; set; }
        }
    }
}

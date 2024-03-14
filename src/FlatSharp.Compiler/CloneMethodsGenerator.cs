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

using FlatSharp.CodeGen;
using FlatSharp.TypeModel;

namespace FlatSharp.Compiler;

internal static class CloneMethodsGenerator
{
    /// <summary>
    /// Returns the fully qualified clone method name.
    /// </summary>
    public static string GenerateCloneMethodsForAssembly(
        CodeWriter writer,
        CompilerOptions options,
        Assembly assembly,
        TypeModelContainer container)
    {
        string @namespace = $"FlatSharp.Compiler.Generated";
        string className = $"CloneHelpers_{Guid.NewGuid():n}";
        string methodName = "Clone";

        string fullyQualifiedMethodName = $"global::{@namespace}.{className}.{methodName}";

        HashSet<Type> seenTypes = new HashSet<Type>();
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsNested)
            {
                continue;
            }

            if (container.TryCreateTypeModel(type, out var typeModel))
            {
                typeModel.TraverseObjectGraph(seenTypes);
            }
        }

        Dictionary<Type, string> methodNameMap = new Dictionary<Type, string>();
        foreach (var seenType in seenTypes)
        {
            methodNameMap[seenType] = fullyQualifiedMethodName;
        }

        writer.AppendLine($"namespace {@namespace}");
        using (writer.WithBlock())
        {
            string visibility = options.FileVisibility ? "file" : "internal";

            writer.AppendLine($"{visibility} static class {className}");
            using (writer.WithBlock())
            {
                foreach (var seenType in seenTypes)
                {
                    if (!container.TryCreateTypeModel(seenType, out ITypeModel? model))
                    {
                        ErrorContext.Current.RegisterError($"Unable to create type model for Type '{seenType.FullName}.'");
                        continue;
                    }

                    GenerateCloneMethod(writer, options, model, methodNameMap);
                }
            }
        }

        return fullyQualifiedMethodName;
    }

    private static void GenerateCloneMethod(
        CodeWriter codeWriter,
        CompilerOptions options,
        ITypeModel typeModel,
        Dictionary<Type, string> methodNameMap)
    {
        string typeName = typeModel.GetGlobalCompilableTypeName();
        CodeGeneratedMethod method = typeModel.CreateCloneMethodBody(new CloneCodeGenContext("item", methodNameMap));

        if (!typeModel.ClrType.IsValueType)
        {
            typeName += "?";

            if (options.NullableWarnings == true)
            {
                codeWriter.AppendLine("[return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(\"item\")]");
            }
        }

        codeWriter.AppendLine(method.GetMethodImplAttribute());
        codeWriter.AppendLine($"public static {typeName} Clone({typeName} item)");
        using (codeWriter.WithBlock())
        {
            codeWriter.AppendLine(method.MethodBody);
        }
    }
}

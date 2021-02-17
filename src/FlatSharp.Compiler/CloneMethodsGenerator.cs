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

namespace FlatSharp.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using FlatSharp.TypeModel;

    internal static class CloneMethodsGenerator
    {
        public static void GenerateCloneMethodsForAssembly(
            CodeWriter writer,
            CompilerOptions options, 
            Assembly assembly, 
            TypeModelContainer container)
        {
            HashSet<Type> seenTypes = new HashSet<Type>();
            foreach (var type in assembly.GetTypes())
            {
                // generate clone method.
                if (container.TryCreateTypeModel(type, out var typeModel))
                {
                    typeModel.TraverseObjectGraph(seenTypes);
                }
            }

            Dictionary<Type, string> methodNameMap = new Dictionary<Type, string>();
            foreach (var seenType in seenTypes)
            {
                methodNameMap[seenType] = "FlatSharp.Generated.CloneHelpers.Clone";
            }

            writer.AppendLine("namespace FlatSharp.Generated");
            using (writer.WithBlock())
            {
                writer.AppendLine("internal static class CloneHelpers");
                using (writer.WithBlock())
                {
                    foreach (var seenType in seenTypes)
                    {
                        if (!container.TryCreateTypeModel(seenType, out ITypeModel model))
                        {
                            ErrorContext.Current.RegisterError($"Unable to create type model for Type '{seenType.FullName}.'");
                            continue;
                        }

                        GenerateCloneMethod(writer, model, methodNameMap);
                    }
                }
            }
        }

        private static void GenerateCloneMethod(
            CodeWriter codeWriter, 
            ITypeModel typeModel, 
            Dictionary<Type, string> methodNameMap)
        {
            string typeName = CSharpHelpers.GetCompilableTypeName(typeModel.ClrType);
            CodeGeneratedMethod method = typeModel.CreateCloneMethodBody(new CloneCodeGenContext("item", methodNameMap));

            if (typeModel.IsNullableReference() == true)
            {
                typeName += "?";
            }

            codeWriter.AppendLine($"public static {typeName} Clone({typeName} item)");
            using (codeWriter.WithBlock())
            {
                codeWriter.AppendLine(method.MethodBody);
            }
        }
    }
}

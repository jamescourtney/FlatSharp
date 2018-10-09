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
    using System.Reflection.Emit;
    using FlatSharp.TypeModel;

    /// <summary>
    /// Generates methods to read array vectors.
    /// </summary>
    internal static class ArrayVectorParserGenerator
    {
        /// <summary>
        /// Registers a method in the given dependency resolver that can deserialize the given vector type model.
        /// </summary>
        public static void Generate(VectorTypeModel vectorModel, ParserGeneratorDependencyResolver dependencyResolver)
        {
            Debug.Assert(vectorModel.IsArray);

            // Arrays can be somewhat fancy. If they are a primitive type and are supported as a Memory{T},
            // then we can actually just use a Memory{T}.ToArray() conversion. If, however, the system is big endian
            // or it's a more complex strucutre, then we need to create a new ListVector{T} to handle the items.
            TypeBuilder typeBuilder = CompilerLock.DynamicModule.CreatePublicStaticType($"ArrayParser_{Guid.NewGuid():n}");
            dependencyResolver.RegisterType(typeBuilder);

            var method = typeBuilder.DefineMethod(
                $"Parse",
                MethodAttributes.Public | MethodAttributes.Static,
                vectorModel.ClrType,
                new[] { typeof(InputBuffer), typeof(int) });

            dependencyResolver.Register(vectorModel, method);
            method.AddAggressiveInlining();

            bool supportsMemory = vectorModel.ItemTypeModel.SchemaType == FlatBufferSchemaType.Scalar;
            supportsMemory &= vectorModel.ItemTypeModel.InlineSize == 1 || BitConverter.IsLittleEndian;

            var il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);

            if (supportsMemory)
            {
                // Memory{T}.ToArray();
                var memoryType = typeof(Memory<>).MakeGenericType(vectorModel.ItemTypeModel.ClrType);
                var memoryLocal = il.DeclareLocal(memoryType);

                MethodInfo memoryMethod = dependencyResolver.GetOrCreateParser(memoryType);
                il.EmitMethodCall(memoryMethod);
                il.EmitStLoc(memoryLocal);
                il.EmitLdLoca(memoryLocal);
                il.EmitMethodCall(ReflectedMethods.Deserialize.Memory_ToArray(vectorModel.ItemTypeModel.ClrType));
            }
            else
            {
                // FlatBufferVector{T}.ToArray();
                var listType = typeof(IList<>).MakeGenericType(vectorModel.ItemTypeModel.ClrType);
                MethodInfo listMethod = dependencyResolver.GetOrCreateParser(listType);

                il.EmitMethodCall(listMethod);
                il.EmitMethodCall(ReflectedMethods.Deserialize.FlatBufferVector_ToArray(vectorModel.ItemTypeModel.ClrType));
            }

            il.Emit(OpCodes.Ret);
        }
    }
}

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
    using System.Diagnostics;
    using System.Reflection;
    using System.Reflection.Emit;
    using FlatSharp.TypeModel;

    /// <summary>
    /// Generates methods to read Memory/ReadOnlyMemory vectors. This class behaves the same in all situations.
    /// </summary>
    internal class MemoryVectorParserGenerator
    {
        /// <summary>
        /// Registers a method on the given dependency resolver to deserialize memory vectors described by the given type model.
        /// </summary>
        public static void Generate(VectorTypeModel vectorModel, ParserGeneratorDependencyResolver dependencyResolver)
        {
            Debug.Assert(vectorModel.IsMemoryVector);
            Debug.Assert(vectorModel.ItemTypeModel.SchemaType == FlatBufferSchemaType.Scalar);
            Debug.Assert(vectorModel.ItemTypeModel.InlineSize == 1 || BitConverter.IsLittleEndian);

            TypeBuilder typeBuilder = CompilerLock.DynamicModule.CreatePublicStaticType($"MemoryParser_{Guid.NewGuid():n}");
            dependencyResolver.RegisterType(typeBuilder);

            var method = typeBuilder.DefineMethod(
                $"Parse",
                MethodAttributes.Public | MethodAttributes.Static,
                vectorModel.ClrType,
                new[] { typeof(InputBuffer), typeof(int) });

            dependencyResolver.Register(vectorModel, method);

            method.AddAggressiveInlining();
            MethodInfo readBlockMethod = GetReadMemoryMethod(vectorModel, vectorModel.ItemTypeModel.ClrType);

            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitInt32Constant(vectorModel.ItemTypeModel.InlineSize);
            il.EmitMethodCall(readBlockMethod);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Gets the appropriate method to read a Memory{T}. Memory{byte} is special since it's fastest and doesn't require any magic conversion.
        /// </summary>
        private static MethodInfo GetReadMemoryMethod(VectorTypeModel vectorModel, Type itemType)
        {
            if (vectorModel.IsReadOnly && itemType == typeof(byte))
            {
                return ReflectedMethods.Deserialize.InputBuffer_ReadReadOnlyByteMemoryBlock;
            }
            else if (itemType == typeof(byte))
            {
                return ReflectedMethods.Deserialize.InputBuffer_ReadByteMemoryBlock;
            }
            else if (vectorModel.IsReadOnly)
            {
                return ReflectedMethods.Deserialize.InputBuffer_ReadReadOnlyMemoryBlock(itemType);
            }
            else
            {
                return ReflectedMethods.Deserialize.InputBuffer_ReadMemoryBlock(itemType);
            }
        }
    }
}

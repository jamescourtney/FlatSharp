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
    using System.Reflection;
    using System.Reflection.Emit;
    using FlatSharp.TypeModel;

    /// <summary>
    /// Generates methods to read IList/IReadOnlyList vectors. This class behaves the same in all situations
    /// </summary>
    internal class ListVectorParserGenerator
    {
        private readonly bool cacheListVectorData;

        public ListVectorParserGenerator(bool cacheListVectorData)
        {
            this.cacheListVectorData = cacheListVectorData;
        }

        /// <summary>
        /// Registers a method on the given dependency resolver to deserialize list vectors described by the given type model.
        /// </summary>
        public void Generate(VectorTypeModel vectorModel, ParserGeneratorDependencyResolver dependencyResolver)
        {
            var itemTypeModel = vectorModel.ItemTypeModel;

            Type baseType;
            
            if (this.cacheListVectorData)
            {
                baseType = typeof(FlatBufferCacheVector<>).MakeGenericType(itemTypeModel.ClrType);
            }
            else
            {
                baseType = typeof(FlatBufferVector<>).MakeGenericType(itemTypeModel.ClrType);
            }

            TypeBuilder listType = CompilerLock.DynamicModule.CreatePublicSealedType(
                $"ListVectorDeserializer_{Guid.NewGuid():n}",
                baseType);

            dependencyResolver.RegisterType(listType);

            var ctor = this.ImplementConstructor(listType, baseType, itemTypeModel.InlineSize);
            var factory = this.ImplementFactory(listType, ctor);
            dependencyResolver.Register(vectorModel, factory);

            this.DefineParseObjectOverride(listType, baseType, vectorModel, dependencyResolver);
        }

        /// <summary>
        /// Generates a .ctor that calls into the base class, which is <see cref="FlatBufferVector{T}"/>. The base class accepts 3 parameters:
        /// - Buffer
        /// - Offset
        /// - Size of each item.
        /// </summary>
        private ConstructorBuilder ImplementConstructor(TypeBuilder typeBuilder, Type baseType, int inlineSize)
        {
            var ctorBuilder = typeBuilder.DefineConstructor(
               MethodAttributes.Public,
               CallingConventions.HasThis,
               new[] { typeof(InputBuffer), typeof(int) });

            var bufferParamBuilder = ctorBuilder.DefineParameter(1, ParameterAttributes.None, "bufferParam");
            var offsetParamBuilder = ctorBuilder.DefineParameter(2, ParameterAttributes.None, "offsetParam");

            var baseCtor = baseType.GetConstructor(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[] { typeof(InputBuffer), typeof(int), typeof(int) },
                new ParameterModifier[0]);

            var ctorIl = ctorBuilder.GetILGenerator();

            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_1);
            ctorIl.Emit(OpCodes.Ldarg_2);
            ctorIl.EmitInt32Constant(inlineSize);
            ctorIl.Emit(OpCodes.Call, baseCtor);
            ctorIl.Emit(OpCodes.Ret);

            return ctorBuilder;
        }

        /// <summary>
        /// Creates a method that returns a new <see cref="FlatBufferVector{T}"/> from the given uoffset_t.
        /// Signature:
        ///     IList{T} CreateNew(Memory{byte} memory, int offset);
        /// </summary>
        private MethodBuilder ImplementFactory(TypeBuilder typeBuilder, ConstructorBuilder ctorBuilder)
        {
            var factory = typeBuilder.DefineMethod(
                "CreateNew",
                MethodAttributes.Static | MethodAttributes.Public,
                typeBuilder,
                new[] { typeof(InputBuffer), typeof(int) });

            factory.AddAggressiveInlining();

            factory.DefineParameter(1, ParameterAttributes.None, "memory");
            factory.DefineParameter(2, ParameterAttributes.None, "offset");

            var il = factory.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitMethodCall(ReflectedMethods.Deserialize.InputBuffer_ReadUOffset);
            il.Emit(OpCodes.Add_Ovf);
            il.Emit(OpCodes.Newobj, ctorBuilder);
            il.Emit(OpCodes.Ret);

            return factory;
        }

        /// <summary>
        /// Overrides the <see cref="FlatBufferVector{T}.ParseItem(Memory{byte}, int)"/> method.
        /// </summary>
        private void DefineParseObjectOverride(
            TypeBuilder typeBuilder,
            Type baseType,
            VectorTypeModel vectorModel,
            ParserGeneratorDependencyResolver resolver)
        {
            MethodBuilder builder = typeBuilder.DefineMethodOverride(
                baseType.GetMethod("ParseItem", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));

            var il = builder.GetILGenerator();

            // arg 0 is "this".
            il.Emit(OpCodes.Ldarg_1); // buffer
            il.Emit(OpCodes.Ldarg_2); // offset
            il.EmitMethodCall(resolver.GetOrCreateParser(vectorModel.ItemTypeModel.ClrType));
            il.Emit(OpCodes.Ret);
        }
    }
}

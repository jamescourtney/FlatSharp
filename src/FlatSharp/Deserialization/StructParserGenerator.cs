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
    /// Generates methods to read structs. When this class generates a parser,
    /// it does so by overriding the appropriate virtual properties on the base class. These overrides
    /// contain hardcoded knowledge about the relative offset of their field within the broader struct.
    /// </summary>
    internal class StructParserGenerator
    {
        private bool implementIDeserializedObject;

        public StructParserGenerator(bool implementIDeserializedObject)
        {
            this.implementIDeserializedObject = implementIDeserializedObject;
        }

        /// <summary>
        /// Generates a subclass of the given struct type model, and registers it as the parser on the given
        /// dependency resolver.
        /// </summary>
        public void Generate(
            StructTypeModel structModel, 
            ParserGeneratorDependencyResolver dependencyResolver)
        {
            #region Example
            /*
             * The gnerated class takes roughly the following shape:
             * public class StructParser : UserDefinedStruct
             * {
             *      private Memory<byte> buffer;
             *      private int offset;
             *      
             *      public StructParser(Memory<byte> buffer, int offset) : base()
             *      {
             *          this.buffer = buffer;
             *          this.offset = offset;
             *      }
             *      
             *      public static UserDefinedStruct CreateNew(Memory<byte> buffer, int offset)
             *      {
             *          // structs are always referred to by their exact position. There is no uoffset to a struct.
             *          return new StructParser(buffer, offset);
             *      }
             * 
             *      public override int Property0
             *      {
             *          get => MemoryHelpers.ReadInt32(this.buffer, checked(this.offset + 0));
             *      }
             *      
             *      public override ushort Property1
             *      {
             *          get => MemoryHelpers.ReadUInt16(this.buffer, checked(this.offset + 4));
             *      }
             * }
             */
            #endregion

            TypeBuilder typeBuilder = CompilerLock.DynamicModule.CreatePublicSealedType(
                $"StructDeserializer_{Guid.NewGuid():n}",
                structModel.ClrType);

            dependencyResolver.RegisterType(typeBuilder);

            var bufferField = typeBuilder.DefineField("buffer", typeof(InputBuffer), FieldAttributes.Private);
            var offsetField = typeBuilder.DefineField("offset", typeof(int), FieldAttributes.Private);

            if (this.implementIDeserializedObject)
            {
                this.ImplementIDeserializedObject(typeBuilder, offsetField);
            }

            var ctor = ImplementConstructor(typeBuilder, structModel.ClrType, bufferField, offsetField);
            var factory = ImplementFactory(typeBuilder, ctor);
            dependencyResolver.Register(structModel, factory);

            foreach (var member in structModel.Members)
            {
                DefinePropertyOverride(typeBuilder, bufferField, offsetField, member, dependencyResolver);
            }
        }

        private static ConstructorBuilder ImplementConstructor(TypeBuilder typeBuilder, Type baseType, FieldBuilder bufferField, FieldBuilder offsetField)
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
                Type.EmptyTypes,
                new ParameterModifier[0]);

            var ctorIl = ctorBuilder.GetILGenerator();

            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Call, baseCtor);

            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_1);
            ctorIl.Emit(OpCodes.Stfld, bufferField);

            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_2);
            ctorIl.Emit(OpCodes.Stfld, offsetField);
            
            ctorIl.Emit(OpCodes.Ret);
            return ctorBuilder;
        }

        /// <summary>
        /// Creates a method that returns a vector at the position indicated by the uoffset_t.
        /// </summary>
        private static MethodBuilder ImplementFactory(TypeBuilder typeBuilder, ConstructorBuilder ctorBuilder)
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
            il.Emit(OpCodes.Newobj, ctorBuilder);
            il.Emit(OpCodes.Ret);

            return factory;
        }

        private static void DefinePropertyOverride(
            TypeBuilder typeBuilder, 
            FieldBuilder bufferField,
            FieldBuilder offsetField,
            StructMemberModel member,
            ParserGeneratorDependencyResolver resolver)
        {
            MethodBuilder getMethodOverride = typeBuilder.DefineMethodOverride(member.PropertyInfo.GetMethod);
            MethodBuilder setMethodOverride = null;

            if (!member.IsReadOnly)
            {
                setMethodOverride = typeBuilder.DefineMethodOverride(member.PropertyInfo.SetMethod);
                var setIl = setMethodOverride.GetILGenerator();

                setIl.Emit(OpCodes.Newobj, typeof(NotMutableException).GetConstructor(new Type[0]));
                setIl.Emit(OpCodes.Throw);
            }

            var reader = resolver.GetOrCreateParser(member.ItemTypeModel.ClrType);

            var propertyInfo = member.PropertyInfo;

            ILGenerator generator = getMethodOverride.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, bufferField);

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, offsetField);
            generator.EmitInt32Constant(member.Offset);
            generator.Emit(OpCodes.Add_Ovf);

            generator.EmitMethodCall(reader);
            generator.Emit(OpCodes.Ret);

            typeBuilder.OverrideProperty(member.PropertyInfo, getMethodOverride, setMethodOverride);
        }

        private void ImplementIDeserializedObject(TypeBuilder typeBuilder, FieldInfo offsetField)
        {
            typeBuilder.AddInterfaceImplementation(typeof(IDeserializedObject));

            MethodBuilder getOffsetBuilder = typeBuilder.DefineMethod(
                "IDeserializedObject.GetOffset",
                MethodAttributes.Private | MethodAttributes.HideBySig |
                MethodAttributes.NewSlot | MethodAttributes.Virtual |
                MethodAttributes.Final,
                typeof(int),
                Type.EmptyTypes);

            var il = getOffsetBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, offsetField);
            il.Emit(OpCodes.Ret);

            // explicit interface implementation.
            typeBuilder.DefineMethodOverride(getOffsetBuilder, typeof(IDeserializedObject).GetMethod("GetOffset"));
        }
    }
}

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
    /// Generates methods to read tables. When this class generates a parser,
    /// it does so by overriding the appropriate virtual properties on the base class. The generated classs contain hardcoded 
    /// knowledge about the indexes of various fields in a table, and can do vtable lookups.
    /// </summary>
    internal class TableParserGenerator
    {
        // Test hook: Implement the IDeserializedObject interface.
        private bool implementIDeserializedObject;

        public TableParserGenerator(bool implementIDeserializedObject)
        {
            this.implementIDeserializedObject = implementIDeserializedObject;
        }

        /// <summary>
        /// Generates a subclass of the given table type model, and registers it as the parser on the given
        /// dependency resolver.
        /// </summary>
        public void Generate(
            TableTypeModel tableModel, 
            ParserGeneratorDependencyResolver dependencyResolver)
        {
            #region Example
            /*
             * The gnerated class takes roughly the following shape:
             * public class TableParser : UserDefinedTable
             * {
             *      private Memory<byte> buffer;
             *      private int offset;
             *      
             *      // These are emitted if caching is enabled.
             *      private byte hasProperty0;
             *      private int property0Value;
             *      
             *      public TableParser(Memory<byte> buffer, int offset) : base()
             *      {
             *          this.buffer = buffer;
             *          this.offset = offset;
             *      }
             *      
             *      public static UserDefinedTable CreateNew(Memory<byte> buffer, int offset)
             *      {
             *          // tables are always referred to by the uoffset to the table, so we need to advance the position here.
             *          return new TableParser(buffer, checked(offset + MemoryHelpers.ReadUOffset(buffer, offset)));
             *      }
             * 
             *      // with caching turned on.
             *      public override int Property0
             *      {
             *          get
             *          {
             *              if (this.hasProperty0 == 0)
             *              {
             *                  this.property0Value = ReadProperty0();
             *                  this.hasPropert0 = 1;
             *              }
             *              
             *              return this.property0Value;
             *          }
             *      }
             *      
             *      // without caching
             *      public override ushort Property1
             *      {
             *          get => this.ReadProperty1();
             *      }
             *      
             *      // Always emitted
             *      private int ReadProperty0()
             *      {
             *          var memory = this.buffer;
             *          
             *          // vtable indirection.
             *          int absoluteTableLocation = MemoryHelpers.GetAbsoluteTableLocation(memory, this.offset, 0); // 0 is the index
             *          if (absoluteTableLocation == 0)
             *          {
             *              return 0; // or default value for this property
             *          }
             *          
             *          return MemoryHelpers.ReadInt32(memory, absoluteTableLocation);
             *      }
             * }
             */
            #endregion

            TypeBuilder typeBuilder = CompilerLock.DynamicModule.CreatePublicSealedType(
                $"TableDeserializer_{Guid.NewGuid():n}",
                tableModel.ClrType);

            dependencyResolver.RegisterType(typeBuilder);

            var bufferField = typeBuilder.DefineField("buffer", typeof(InputBuffer), FieldAttributes.Private);
            var offsetField = typeBuilder.DefineField("offset", typeof(int), FieldAttributes.Private);

            if (this.implementIDeserializedObject)
            {
                this.ImplementIDeserializedObject(typeBuilder, offsetField);
            }

            var ctor = ImplementConstructor(typeBuilder, tableModel.ClrType, bufferField, offsetField);
            var factory = ImplementFactory(typeBuilder, ctor);
            dependencyResolver.Register(tableModel, factory);

            foreach (var member in tableModel.IndexToMemberMap.Values)
            {
                this.DefinePropertyOverride(typeBuilder, bufferField, offsetField, member, dependencyResolver);
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
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Call, ReflectedMethods.Deserialize.InputBuffer_ReadUOffset);
            il.Emit(OpCodes.Add_Ovf);
            il.Emit(OpCodes.Newobj, ctorBuilder);
            il.Emit(OpCodes.Ret);

            return factory;
        }

        private void DefinePropertyOverride(
            TypeBuilder typeBuilder, 
            FieldBuilder bufferField,
            FieldBuilder offsetField,
            TableMemberModel member,
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

            var propertyInfo = member.PropertyInfo;
            MethodInfo readPropertyMethod = ImplementReadPropertyMethod(typeBuilder, bufferField, offsetField, member, resolver);

            if (member.ItemTypeModel is VectorTypeModel vectorModel && vectorModel.IsMemoryVector)
            {
                // It's not safe to cache Memory{T} references internally, since the underlying memory could go away
                // if the object is disposed, etc. This would leave us with a reference to wild memory, which might be mutable. 
                // The better course is to just grab a new Memory{T} instance each time.
                ImplementGetPropertyMethodWithoutCache(typeBuilder, getMethodOverride, member, readPropertyMethod);
            }
            else
            {
                ImplementGetPropertyMethodWithCache(typeBuilder, getMethodOverride, member, readPropertyMethod);
            }

            typeBuilder.OverrideProperty(member.PropertyInfo, getMethodOverride, setMethodOverride);
        }

        private static void ImplementGetPropertyNoCache(MethodBuilder builder, MethodInfo readPropertyMethod)
        {
            var il = builder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, readPropertyMethod);
            il.Emit(OpCodes.Ret);
        }

        private static void ImplementGetPropertyMethodWithoutCache(
            TypeBuilder typeBuilder,
            MethodBuilder method,
            TableMemberModel memberModel,
            MethodInfo readPropertyMethod)
        {
            var il = method.GetILGenerator();

            il.EmitLdArg(0);
            il.EmitMethodCall(readPropertyMethod);
            il.Emit(OpCodes.Ret);
        }

        private static void ImplementGetPropertyMethodWithCache(
            TypeBuilder typeBuilder,
            MethodBuilder method, 
            TableMemberModel memberModel,
            MethodInfo readPropertyMethod)
        {
            // Define two fields: HasValueOfX and ValueOfX.
            FieldInfo cacheValueField = typeBuilder.DefineField(
                $"tablePropertyCache_{memberModel.PropertyInfo.Name}",
                memberModel.PropertyInfo.PropertyType,
                FieldAttributes.Private);

            FieldInfo hasCacheValueField = typeBuilder.DefineField(
                $"tablePropertyHasCacheValue_{memberModel.PropertyInfo.Name}",
                typeof(byte),
                FieldAttributes.Private);

            var il = method.GetILGenerator();

            // if (this.hasCachedValue) { return this.cachedValue; }
            var ifHasCacheLabel = il.DefineLabel();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, hasCacheValueField);
            il.Emit(OpCodes.Brtrue_S, ifHasCacheLabel);

            // this.cacheValueField = this.method();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, readPropertyMethod);
            il.Emit(OpCodes.Stfld, cacheValueField);

            // this.hasCacheValue = true;
            il.Emit(OpCodes.Ldarg_0);
            il.EmitInt32Constant(1);
            il.Emit(OpCodes.Stfld, hasCacheValueField);

            // return this.cacheValue;
            il.MarkLabel(ifHasCacheLabel);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, cacheValueField);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Defines a method that accepts no parameters and reads the given property from the flatbuffer.
        /// </summary>
        private static MethodInfo ImplementReadPropertyMethod(
            TypeBuilder typeBuilder,
            FieldBuilder bufferField,
            FieldBuilder offsetField,
            TableMemberModel member,
            ParserGeneratorDependencyResolver resolver)
        {
            var method = typeBuilder.DefineMethod(
                "ReadProperty_" + member.PropertyInfo.Name + "_" + Guid.NewGuid().ToString("n"),
                MethodAttributes.Private,
                member.ItemTypeModel.ClrType,
                Type.EmptyTypes);

            method.AddAggressiveInlining();
            ILGenerator il = method.GetILGenerator();

            var memoryLocal = il.DeclareLocal(typeof(InputBuffer));
            var offsetLocal = il.DeclareLocal(typeof(int));

            // var memory = this.buffer
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, bufferField);
            il.EmitStLoc(memoryLocal);

            // int absoluteLocation = UnpackUtilities.GetAbsoluteTableLocation(memory, this.offset, index);
            il.EmitLdLoc(memoryLocal);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, offsetField);
            il.EmitInt32Constant(member.Index);
            il.EmitMethodCall(ReflectedMethods.Deserialize.InputBuffer_GetAbsoluteTableFieldLocation);
            il.EmitStLoc(offsetLocal);

            var ifValidOffset = il.DefineLabel();
            il.EmitLdLoc(offsetLocal);
            il.Emit(OpCodes.Brtrue_S, ifValidOffset);
            EmitLoadDefaultValue(il, member);
            il.Emit(OpCodes.Ret);

            // return ReadXXXXX(buffer, absoluteLocation);
            il.MarkLabel(ifValidOffset);
            il.EmitLdLoc(memoryLocal);
            il.EmitLdLoc(offsetLocal);
            il.EmitMethodCall(resolver.GetOrCreateParser(member.ItemTypeModel.ClrType));
            il.Emit(OpCodes.Ret);

            return method;
        }


        private static object GetDefaultValue(TableMemberModel memberModel)
        {
            object defaultValue;
            if (memberModel.HasDefaultValue)
            {
                defaultValue = memberModel.DefaultValue;
                if (defaultValue != null)
                {
                    if (memberModel.PropertyInfo.PropertyType != defaultValue.GetType())
                    {
                        throw new InvalidOperationException("Default value must be of the same type as the property.");
                    }
                }
            }
            else
            {
                if (memberModel.PropertyInfo.PropertyType.IsValueType)
                {
                    defaultValue = Activator.CreateInstance(memberModel.PropertyInfo.PropertyType);
                }
                else
                {
                    defaultValue = null;
                }
            }

            return defaultValue;
        }

        private static void EmitLoadDefaultValue(
            ILGenerator generator,
            TableMemberModel memberModel)
        {
            object defaultValue = GetDefaultValue(memberModel);
            var propertyInfo = memberModel.PropertyInfo;

            if (propertyInfo.PropertyType == typeof(bool))
            {
                if ((bool)defaultValue == true)
                {
                    generator.Emit(OpCodes.Ldc_I4_1);
                }
                else
                {
                    generator.Emit(OpCodes.Ldc_I4_0);
                }
            }
            else if (propertyInfo.PropertyType == typeof(byte) ||
                     propertyInfo.PropertyType == typeof(sbyte) ||
                     propertyInfo.PropertyType == typeof(ushort) ||
                     propertyInfo.PropertyType == typeof(short) ||
                     propertyInfo.PropertyType == typeof(uint) ||
                     propertyInfo.PropertyType == typeof(int))
            {
                generator.EmitInt32Constant(Convert.ToInt32(defaultValue));
            }
            else if (propertyInfo.PropertyType == typeof(ulong) ||
                     propertyInfo.PropertyType == typeof(long))
            {
                generator.Emit(OpCodes.Ldc_I8, Convert.ToInt64(defaultValue));
            }
            else if (propertyInfo.PropertyType == typeof(float))
            {
                generator.Emit(OpCodes.Ldc_R4, (float)defaultValue);
            }
            else if (propertyInfo.PropertyType == typeof(double))
            {
                generator.Emit(OpCodes.Ldc_R8, (double)defaultValue);
            }
            else if (propertyInfo.PropertyType == typeof(string) && defaultValue != null)
            {
                generator.Emit(OpCodes.Ldstr, (string)defaultValue);
            }
            else if (memberModel.ItemTypeModel is VectorTypeModel vectorModel && vectorModel.IsMemoryVector)
            {
                // Memory requires a little special sauce in the form of a new local.
                var tempLocal = generator.DeclareLocal(propertyInfo.PropertyType);

                generator.Emit(OpCodes.Ldloca_S, tempLocal);
                generator.Emit(OpCodes.Initobj, propertyInfo.PropertyType);
                generator.Emit(OpCodes.Ldloc_S, tempLocal);
            }
            else
            {
                generator.Emit(OpCodes.Ldnull);
            }
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

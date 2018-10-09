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
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using FlatSharp.TypeModel;

    /// <summary>
    /// Generates a collection of methods to help serialize the given root type.
    /// Does recursive traversal of the object graph and builds a set of methods to assist with populating vtables and writing values.
    /// 
    /// Eventually, everything must reduce to a built in type of string / scalar, which this will then call out to.
    /// </summary>
    internal class SerializerGenerator
    {
        private readonly Dictionary<Type, MethodBuilder> sizeNeededMethods = new Dictionary<Type, MethodBuilder>();
        private readonly Dictionary<Type, MethodBuilder> writeMethods = new Dictionary<Type, MethodBuilder>();

        public ISerializer<TRoot> Compile<TRoot>()
        {
            TypeBuilder typeBuilder = CompilerLock.DynamicModule.DefineType(
                "PackUtilities_" + Guid.NewGuid().ToString("n"),
                TypeAttributes.Class | TypeAttributes.Public,
                typeof(object),
                new[] { typeof(ISerializer<TRoot>) });

            this.DefineMethods(RuntimeTypeModel.CreateFrom(typeof(TRoot)), typeBuilder);
            this.ImplementMethods();
            this.ImplementInterfaceMethod(typeof(TRoot), typeBuilder, this.writeMethods[typeof(TRoot)]);

            TypeInfo info = typeBuilder.CreateTypeInfo();
            var writeRoot = info.GetMethod(this.writeMethods[typeof(TRoot)].Name, BindingFlags.Public | BindingFlags.Static);

            return (ISerializer<TRoot>)Activator.CreateInstance(info);
        }

        /// <summary>
        /// Recursively crawls through the object graph and looks for methods to define.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dictionary"></param>
        private void DefineMethods(
            RuntimeTypeModel model,
            TypeBuilder builder)
        {
            if (model.SchemaType == FlatBufferSchemaType.Scalar || model.SchemaType == FlatBufferSchemaType.String)
            {
                // Built in; we can call out to those when we need to.
                return;
            }

            if (this.sizeNeededMethods.ContainsKey(model.ClrType))
            {
                // Already done.
                return;
            }

            string nameBase = model.ClrType.FullName.Replace('.', '_');
            if (model is VectorTypeModel vectorModel2)
            {
                if (vectorModel2.IsList)
                {
                    nameBase = "ListVectorOf_" + vectorModel2.ItemTypeModel.ClrType.FullName.Replace('.', '_');
                }
                else
                {
                    nameBase = "MemoryVectorOf_" + vectorModel2.ItemTypeModel.ClrType.FullName.Replace('.', '_');
                }
            }

            var sizeMethod = builder.DefineMethod(
                "GetSizeOf_" + nameBase,
                MethodAttributes.Static | MethodAttributes.Public,
                typeof(int),
                new[] { model.ClrType, model.ClrType, typeof(int).MakeByRefType() });

            var inlineWriteMethod = builder.DefineMethod(
                "WriteInlineValueOf_" + nameBase,
                MethodAttributes.Static | MethodAttributes.Public,
                null,
                new[] { typeof(SpanWriter), typeof(Span<byte>), model.ClrType, typeof(int), typeof(SerializationContext) });

            this.sizeNeededMethods[model.ClrType] = sizeMethod;
            this.writeMethods[model.ClrType] = inlineWriteMethod;

            if (model is TableTypeModel tableModel)
            {
                foreach (var member in tableModel.IndexToMemberMap.Values)
                {
                    this.DefineMethods(member.ItemTypeModel, builder);
                }
            }
            else if (model is StructTypeModel structModel)
            {
                foreach (var member in structModel.Members)
                {
                    this.DefineMethods(member.ItemTypeModel, builder);
                }
            }
            else if (model is VectorTypeModel vectorModel)
            {
                this.DefineMethods(vectorModel.ItemTypeModel, builder);
            }
        }

        private void ImplementInterfaceMethod(
            Type rootType,
            TypeBuilder typeBuilder,
            MethodBuilder writeRoot)
        {
            var rootMethod = typeBuilder.DefineMethod(
                nameof(ISerializer<byte>.Write),
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final,
                null,
                new[] { typeof(SpanWriter), typeof(Span<byte>), rootType, typeof(int), typeof(SerializationContext) });

            rootMethod.SetImplementationFlags(rootMethod.GetMethodImplementationFlags() | MethodImplAttributes.AggressiveInlining);
            var il = rootMethod.GetILGenerator();
            il.EmitLdArg(1);
            il.EmitLdArg(2);
            il.EmitLdArg(3);
            il.EmitLdArg(4);
            il.EmitLdArg(5);
            il.EmitMethodCall(writeRoot);
            il.Emit(OpCodes.Ret);
        }

        private void ImplementMethods()
        {
            foreach (var pair in this.sizeNeededMethods)
            {
                Type type = pair.Key;
                this.ImplementGetSizeMethod(type, pair.Value);
            }

            foreach (var pair in this.writeMethods)
            {
                Type type = pair.Key;
                this.ImplementInlineWriteMethod(type, pair.Value);
            }
        }

        private void ImplementGetSizeMethod(Type type, MethodBuilder getSizeBuilder)
        {
            // parameters: (T value, T defaultValue_notUsed, ref int sizeNeeded)
            var typeModel = RuntimeTypeModel.CreateFrom(type);

            getSizeBuilder.SetImplementationFlags(getSizeBuilder.MethodImplementationFlags | MethodImplAttributes.AggressiveInlining);
            var generator = getSizeBuilder.GetILGenerator();

            var offsetLocal = generator.DeclareLocal(typeof(int));
            var localSizeNeededLocal = generator.DeclareLocal(typeof(int));

            // int offset = 0;
            generator.EmitInt32Constant(0);
            generator.EmitStLoc(offsetLocal);

            // if (item == default), go to return.
            Label gotoReturn = generator.DefineLabel();

            if (typeModel is VectorTypeModel vectorModel)
            {
                if (vectorModel.IsMemoryVector)
                {
                    var clrType = vectorModel.ItemTypeModel.ClrType;
                    PropertyInfo lengthProperty = vectorModel.IsReadOnly ? ReflectedMethods.Serialize.ReadOnlyMemory_LengthProperty(clrType) : ReflectedMethods.Serialize.Memory_LengthProperty(clrType);

                    // memory is a struct, so we only need to check to see if it is empty.
                    generator.Emit(OpCodes.Ldarga_S, 0);
                    generator.EmitMethodCall(lengthProperty.GetGetMethod());
                    generator.Emit(OpCodes.Brfalse_S, gotoReturn);
                }
                else
                {
                    var clrType = vectorModel.ItemTypeModel.ClrType;
                    PropertyInfo lengthProperty = vectorModel.IsReadOnly ? ReflectedMethods.Serialize.IReadOnlyList_CountProperty(clrType) : ReflectedMethods.Serialize.IList_CountProperty(clrType);

                    // for list, we have to check:
                    // if x == null || x.Count == 0
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Brfalse_S, gotoReturn);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.EmitMethodCall(lengthProperty.GetGetMethod());
                    generator.Emit(OpCodes.Brfalse_S, gotoReturn);
                }
            }
            else
            {
                Debug.Assert(!typeModel.ClrType.IsValueType);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Brfalse_S, gotoReturn);
            }

            // int localSizeNeeded = sizeNeeded. Done here to avoid always accessing size needed indirectly.
            generator.Emit(OpCodes.Ldarg_2);
            generator.Emit(OpCodes.Ldind_I4);
            generator.EmitStLoc(localSizeNeededLocal);

            // localSizeNeeded += MemoryHelpers.GetAlignmentError(localSizeNeeded, alignment);
            generator.EmitLdLoc(localSizeNeededLocal);
            generator.EmitLdLoc(localSizeNeededLocal);
            generator.EmitInt32Constant(typeModel.Alignment);
            generator.EmitMethodCall(ReflectedMethods.Serialize.MemoryHelpers_GetAlignmentErrorMethod);
            generator.Emit(OpCodes.Add_Ovf);
            generator.EmitStLoc(localSizeNeededLocal);

            // offset = localSizeNeeded
            generator.EmitLdLoc(localSizeNeededLocal);
            generator.EmitStLoc(offsetLocal);

            // sizeNeeded = localSizeNeeded + typeModel.InlineSize;
            generator.Emit(OpCodes.Ldarg_2);
            generator.EmitLdLoc(localSizeNeededLocal);
            generator.EmitInt32Constant(typeModel.InlineSize);
            generator.Emit(OpCodes.Add_Ovf);
            generator.Emit(OpCodes.Stind_I4);

            generator.MarkLabel(gotoReturn);
            generator.EmitLdLoc(offsetLocal);
            generator.Emit(OpCodes.Ret);
        }

        private void ImplementInlineWriteMethod(Type type, MethodBuilder inlineWriteMethodBuilder)
        {
            var typeModel = RuntimeTypeModel.CreateFrom(type);

            if (typeModel is TableTypeModel tableModel)
            {
                this.ImplementTableInlineWriteMethod(tableModel, inlineWriteMethodBuilder);
            }
            else if (typeModel is StructTypeModel structModel)
            {
                this.ImplementStructInlineWriteMethod(structModel, inlineWriteMethodBuilder);
            }
            else if (typeModel is VectorTypeModel vectorModel)
            {
                if (vectorModel.IsMemoryVector)
                {
                    this.ImplementMemoryVectorInlineWriteMethod(vectorModel, inlineWriteMethodBuilder);
                }
                else
                {
                    // Array implements IList.
                    this.ImplementListVectorInlineWriteMethod(vectorModel, inlineWriteMethodBuilder);
                }
            }
        }

        private void ImplementTableInlineWriteMethod(TableTypeModel tableModel, MethodBuilder inlineWriteMethodBuilder)
        {
            // Params: SpanWriter, Span, item, offset, context
            const int ArgIndex_SpanWriter = 0;
            const int ArgIndex_Span = 1;
            const int ArgIndex_Value = 2;
            const int ArgIndex_Offset = 3;
            const int ArgIndex_Context = 4;

            var generator = inlineWriteMethodBuilder.GetILGenerator();

            List<int> indices = tableModel.IndexToMemberMap.Keys.ToList();
            int maxIndex = indices.Max();

            Dictionary<int, LocalBuilder> offsetLocals = new Dictionary<int, LocalBuilder>();
            Dictionary<int, LocalBuilder> propertyValueLocals = new Dictionary<int, LocalBuilder>();

            LocalBuilder vtableLocal = generator.DeclareLocal(typeof(VTableHelper));
            LocalBuilder sizeNeededLocal = generator.DeclareLocal(typeof(int));
            LocalBuilder vtablePositionLocal = generator.DeclareLocal(typeof(int));
            LocalBuilder tablePositionLocal = generator.DeclareLocal(typeof(int));

            // Declare locals to store values / offsets for each field.
            foreach (var index in indices)
            {
                ItemMemberModel memberInfo = tableModel.IndexToMemberMap[index];
                offsetLocals.Add(index, generator.DeclareLocal(typeof(int)));
                propertyValueLocals.Add(index, generator.DeclareLocal(memberInfo.ItemTypeModel.ClrType));
            }

            // sizeNeeded = 4;
            generator.EmitInt32Constant(sizeof(uint));
            generator.EmitStLoc(sizeNeededLocal);

            // helper = buffer.vtablehelper;
            generator.EmitLdArg(ArgIndex_Context);
            generator.Emit(OpCodes.Ldfld, ReflectedMethods.Serialize.SerializationContext_Vtable);
            generator.EmitStLoc(vtableLocal);

            // helper.StartObject(maxIndex);
            generator.EmitLdLoc(vtableLocal);
            generator.EmitInt32Constant(maxIndex);
            generator.EmitMethodCall(ReflectedMethods.Serialize.VTableHelper_StartObjectMethod);

            // Store the values of the properties in the respective variables.
            // var prop1 = value.Property1;
            // var prop2Value = value.Property2;
            // etc
            foreach (var index in indices)
            {
                ItemMemberModel memberInfo = tableModel.IndexToMemberMap[index];
                LocalBuilder propertyValueLocal = propertyValueLocals[index];

                generator.EmitLdArg(ArgIndex_Value);
                generator.EmitMethodCall(memberInfo.PropertyInfo.GetGetMethod());
                generator.EmitStLoc(propertyValueLocal);
            }

            #region Compute Size of each property and build vtable

            // Compute the size of each property.
            foreach (var index in indices)
            {
                TableMemberModel memberInfo = tableModel.IndexToMemberMap[index];
                LocalBuilder propertyValueLocal = propertyValueLocals[index];
                LocalBuilder offsetLocal = offsetLocals[index];

                // sizeOfIndexN = GetSize(item, defaultValue, ref int sizeNeeded);
                generator.EmitLdLoc(propertyValueLocal);
                #region Emit Default Value
                if (memberInfo.ItemTypeModel.SchemaType == FlatBufferSchemaType.Scalar)
                {
                    var clrType = memberInfo.ItemTypeModel.ClrType;

                    if (clrType == typeof(ulong))
                    {
                        generator.Emit(OpCodes.Ldc_I8, (long)(ulong)(memberInfo.DefaultValue ?? 0UL));
                    }
                    else if (clrType == typeof(long))
                    {
                        generator.Emit(OpCodes.Ldc_I8, (long)(memberInfo.DefaultValue ?? 0L));
                    }
                    else if (clrType == typeof(double))
                    {
                        generator.Emit(OpCodes.Ldc_R8, (double)(memberInfo.DefaultValue ?? 0.0D));
                    }
                    else if (clrType == typeof(float))
                    {
                        generator.Emit(OpCodes.Ldc_R4, (float)(memberInfo.DefaultValue ?? 0.0F));
                    }
                    else if (clrType == typeof(uint))
                    {
                        generator.Emit(OpCodes.Ldc_I4, (int)(uint)(memberInfo.DefaultValue ?? (uint)0));
                    }
                    else
                    {
                        // int32 can hold the rest of the range of values.
                        generator.Emit(OpCodes.Ldc_I4, Convert.ToInt32(memberInfo.DefaultValue ?? 0));
                    }
                }
                else
                {
                    generator.Emit(OpCodes.Ldnull);
                }
                #endregion
                generator.EmitLdLoca(sizeNeededLocal);

                if (!ReflectedMethods.ILSizers.TryGetValue(memberInfo.ItemTypeModel.ClrType, out MethodInfo sizer))
                {
                    sizer = this.sizeNeededMethods[memberInfo.ItemTypeModel.ClrType];
                }

                generator.EmitMethodCall(sizer);
                generator.EmitStLoc(offsetLocal);

                // helper.SetOffset(index, offset);
                generator.EmitLdLoc(vtableLocal);
                generator.EmitInt32Constant(index);
                generator.EmitLdLoc(offsetLocal);
                generator.EmitMethodCall(ReflectedMethods.Serialize.VTableHelper_SetOffsetMethod);
            }

            #endregion

            // vtableStart = helper.EndObject(sizeNeeded);
            generator.EmitLdLoc(vtableLocal);
            generator.EmitLdArg(ArgIndex_Span);
            generator.EmitLdArg(ArgIndex_SpanWriter);
            generator.EmitLdLoc(sizeNeededLocal);
            generator.EmitMethodCall(ReflectedMethods.Serialize.VTableHelper_EndObjectMethod);
            generator.EmitStLoc(vtablePositionLocal);

            // tableStart = context.AllocateSpace(tableSize, alignment);
            generator.EmitLdArg(ArgIndex_Context);
            generator.EmitLdLoc(sizeNeededLocal);
            generator.EmitInt32Constant(tableModel.Alignment);
            generator.EmitMethodCall(ReflectedMethods.Serialize.SerializationContext_AllocateMemoryMethod);
            generator.EmitStLoc(tablePositionLocal);

            // Now we know where the table starts. this means we can update the original
            // 'offset' param with the uoffset to the table start.
            generator.EmitLdArg(ArgIndex_SpanWriter);
            generator.EmitLdArg(ArgIndex_Span);
            generator.EmitLdArg(ArgIndex_Offset);
            generator.EmitLdLoc(tablePositionLocal);
            generator.EmitLdArg(ArgIndex_Context);
            generator.EmitMethodCall(ReflectedMethods.Serialize.SpanWriter_WriteUOffset);

            // Write the soffset to the vtable!
            // spanWriter.WriteInt(
            generator.EmitLdArg(ArgIndex_SpanWriter);
            generator.EmitLdArg(ArgIndex_Span);
            generator.EmitLdLoc(tablePositionLocal);
            generator.EmitLdLoc(vtablePositionLocal);
            generator.Emit(OpCodes.Sub_Ovf);
            generator.EmitLdLoc(tablePositionLocal);
            generator.EmitLdArg(ArgIndex_Context);
            generator.EmitMethodCall(ReflectedMethods.ILWriters[typeof(int)]);

            foreach (var index in indices)
            {
                var nextLabel = generator.DefineLabel();
                ItemMemberModel memberInfo = tableModel.IndexToMemberMap[index];
                LocalBuilder propertyValueLocal = propertyValueLocals[index];
                LocalBuilder localOffsetLocal = offsetLocals[index];

                if (!ReflectedMethods.ILWriters.TryGetValue(memberInfo.ItemTypeModel.ClrType, out MethodInfo writer))
                {
                    writer = this.writeMethods[memberInfo.ItemTypeModel.ClrType];
                }

                // if (offset == 0) go to next property.
                generator.Emit(OpCodes.Ldloc_S, localOffsetLocal);
                generator.Emit(OpCodes.Brfalse_S, nextLabel);

                // Write(Writer, Span, Value, Offset, Context) 
                // (note: the "Write" method may be a static generated method, 
                // or an instance method on SpanWriter. It does not matter since
                // the "writer" argument comes first.
                generator.EmitLdArg(ArgIndex_SpanWriter);            // load writer
                generator.EmitLdArg(ArgIndex_Span);                  // load buffer
                generator.EmitLdLoc(propertyValueLocal);             // load value of the thing.
                generator.EmitLdLoc(localOffsetLocal);               // tableOffset + localOffset
                generator.EmitLdLoc(tablePositionLocal);
                generator.Emit(OpCodes.Add_Ovf);          
                generator.EmitLdArg(ArgIndex_Context);               // load context
                generator.Emit(OpCodes.Call, writer);
                generator.MarkLabel(nextLabel);
            }

            generator.Emit(OpCodes.Ret);
        }

        private void ImplementStructInlineWriteMethod(StructTypeModel structModel, MethodBuilder methodBuilder)
        {
            // Params: SpanWriter, Span, item, offset, context
            const int ArgIndex_SpanWriter = 0;
            const int ArgIndex_Span = 1;
            const int ArgIndex_Value = 2;
            const int ArgIndex_Offset = 3;
            const int ArgIndex_Context = 4;

            methodBuilder.SetImplementationFlags(methodBuilder.MethodImplementationFlags | MethodImplAttributes.AggressiveInlining);
            var generator = methodBuilder.GetILGenerator();

            foreach (var member in structModel.Members)
            {
                if (!ReflectedMethods.ILWriters.TryGetValue(member.ItemTypeModel.ClrType, out MethodInfo writer))
                {
                    writer = this.writeMethods[member.ItemTypeModel.ClrType];
                }

                // load buffer + writer
                generator.EmitLdArg(ArgIndex_SpanWriter);
                generator.EmitLdArg(ArgIndex_Span);

                // load value
                generator.EmitLdArg(ArgIndex_Value);
                generator.EmitMethodCall(member.PropertyInfo.GetGetMethod());

                // load offset of member
                generator.EmitLdArg(ArgIndex_Offset);
                if (member.Offset != 0)
                {
                    generator.EmitInt32Constant(member.Offset);
                    generator.Emit(OpCodes.Add_Ovf);
                }

                // Context
                generator.EmitLdArg(ArgIndex_Context);

                // WriteMethod(writer, span, value, offset, context);
                generator.EmitMethodCall(writer);
            }

            generator.Emit(OpCodes.Ret);
        }

        private void ImplementListVectorInlineWriteMethod(VectorTypeModel vectorModel, MethodBuilder methodBuilder)
        {
            // Params: SpanWriter, Span, item, offset, context
            const int ArgIndex_SpanWriter = 0;
            const int ArgIndex_Span = 1;
            const int ArgIndex_Value = 2;
            const int ArgIndex_Offset = 3;
            const int ArgIndex_Context = 4;

            methodBuilder.SetImplementationFlags(methodBuilder.GetMethodImplementationFlags() | MethodImplAttributes.AggressiveInlining);
            var il = methodBuilder.GetILGenerator();

            var itemCountLocal = il.DeclareLocal(typeof(int));
            var nextOffsetLocal = il.DeclareLocal(typeof(int));
            var loopIndexLocal = il.DeclareLocal(typeof(int));

            Type itemClrType = vectorModel.ItemTypeModel.ClrType;
            PropertyInfo lengthProperty = vectorModel.IsReadOnly ? ReflectedMethods.Serialize.IReadOnlyList_CountProperty(itemClrType) : ReflectedMethods.Serialize.IList_CountProperty(itemClrType);
            PropertyInfo itemProperty = vectorModel.IsReadOnly ? ReflectedMethods.Serialize.IReadOnlyList_ItemProperty(itemClrType) : ReflectedMethods.Serialize.IList_ItemProperty(itemClrType);

            // itemCount = value.Count;
            il.EmitLdArg(ArgIndex_Value);
            il.EmitMethodCall(lengthProperty.GetGetMethod());
            il.EmitStLoc(itemCountLocal);

            // absoluteOffset = context.AllocateVector(item alignment, number of items, size per item)
            il.EmitLdArg(ArgIndex_Context);
            il.EmitInt32Constant(vectorModel.ItemTypeModel.Alignment);
            il.EmitLdLoc(itemCountLocal);
            il.EmitInt32Constant(vectorModel.ItemTypeModel.InlineSize);
            il.EmitMethodCall(ReflectedMethods.Serialize.SerializationContext_AllocateVectorMethod);
            il.EmitStLoc(nextOffsetLocal);

            // Write the UOffset into the span parameter.
            // buffer.WriteUOffset(span, offset, vectorStart, context);
            il.EmitLdArg(ArgIndex_SpanWriter);
            il.EmitLdArg(ArgIndex_Span);
            il.EmitLdArg(ArgIndex_Offset);
            il.EmitLdLoc(nextOffsetLocal);
            il.EmitLdArg(ArgIndex_Context);
            il.EmitMethodCall(ReflectedMethods.Serialize.SpanWriter_WriteUOffset);

            // Write the count into the beginning of the span.
            // buffer.WriteInt(span, count, nextOffset, context);
            il.EmitLdArg(ArgIndex_SpanWriter);
            il.EmitLdArg(ArgIndex_Span);
            il.EmitLdLoc(itemCountLocal);
            il.EmitLdLoc(nextOffsetLocal);
            il.EmitLdArg(ArgIndex_Context);
            il.EmitMethodCall(ReflectedMethods.ILWriters[typeof(int)]);

            // nextOffset += 4;
            il.EmitLdLoc(nextOffsetLocal);
            il.EmitInt32Constant(4);
            il.Emit(OpCodes.Add_Ovf);
            il.EmitStLoc(nextOffsetLocal);

            // loopIndex = 0;
            il.EmitInt32Constant(0);
            il.EmitStLoc(loopIndexLocal);
            var loopStartLabel = il.DefineLabel();
            il.Emit(OpCodes.Br_S, loopStartLabel);

            // Loop body
            var loopBodyStartLabel = il.DefineLabel();
            il.MarkLabel(loopBodyStartLabel);

            // Invoke the right method to write this element.
            if (!ReflectedMethods.ILWriters.TryGetValue(vectorModel.ItemTypeModel.ClrType, out MethodInfo writer))
            {
                writer = this.writeMethods[vectorModel.ItemTypeModel.ClrType];
            }

            // buffer.WriteXXXX(span, value, offset, context)
            il.EmitLdArg(ArgIndex_SpanWriter);
            il.EmitLdArg(ArgIndex_Span);
            il.EmitLdArg(ArgIndex_Value);                   // list[loopIndex]
            il.EmitLdLoc(loopIndexLocal);
            il.EmitMethodCall(itemProperty.GetGetMethod());
            il.EmitLdLoc(nextOffsetLocal);
            il.EmitLdArg(ArgIndex_Context);
            il.EmitMethodCall(writer);

            // nextOffset += sizeOfItem.
            il.EmitLdLoc(nextOffsetLocal);
            il.EmitInt32Constant(vectorModel.ItemTypeModel.InlineSize);
            il.Emit(OpCodes.Add_Ovf);
            il.EmitStLoc(nextOffsetLocal);

            // loopIndex++
            il.EmitLdLoc(loopIndexLocal);
            il.EmitInt32Constant(1);
            il.Emit(OpCodes.Add);
            il.EmitStLoc(loopIndexLocal);

            // if (loopIndex < itemCount) go to top, otherwise return.
            il.MarkLabel(loopStartLabel);
            il.EmitLdLoc(loopIndexLocal);
            il.EmitLdLoc(itemCountLocal);
            il.Emit(OpCodes.Blt_S, loopBodyStartLabel);

            il.Emit(OpCodes.Ret);
        }

        private void ImplementMemoryVectorInlineWriteMethod(VectorTypeModel vectorModel, MethodBuilder methodBuilder)
        {
            // Params: SpanWriter, Span, item, offset, context
            const int ArgIndex_SpanWriter = 0;
            const int ArgIndex_Span = 1;
            const int ArgIndex_Value = 2;
            const int ArgIndex_Offset = 3;
            const int ArgIndex_Context = 4;

            methodBuilder.SetImplementationFlags(methodBuilder.GetMethodImplementationFlags() | MethodImplAttributes.AggressiveInlining);

            Type itemType = vectorModel.ItemTypeModel.ClrType;
            MethodInfo writeMemoryVectorMethod;

            // These methods are special, and accept a subset of what we get here.
            // The signature is: void MemoryHelper.Write(ctx, memory source, uoffset location, absoluteoffset, relativeoffset, alignment, size);
            if (itemType == typeof(byte) && vectorModel.IsReadOnly)
            {
                writeMemoryVectorMethod = ReflectedMethods.Serialize.SpanWriter_WriteReadOnlyByteMemoryBlock;
            }
            else if (itemType == typeof(byte))
            {
                writeMemoryVectorMethod = ReflectedMethods.Serialize.SpanWriter_WriteByteMemoryBlock;
            }
            else if (vectorModel.IsReadOnly)
            {
                writeMemoryVectorMethod = ReflectedMethods.Serialize.SpanWriter_WriteReadOnlyMemoryBlock(itemType);
            }
            else
            {
                writeMemoryVectorMethod = ReflectedMethods.Serialize.SpanWriter_WriteMemoryBlock(itemType);
            }

            var il = methodBuilder.GetILGenerator();

            // writer(spanWriter, span, value, offset, alignment, inlineSize, context);
            il.EmitLdArg(ArgIndex_SpanWriter);
            il.EmitLdArg(ArgIndex_Span);
            il.EmitLdArg(ArgIndex_Value);
            il.EmitLdArg(ArgIndex_Offset);
            il.EmitInt32Constant(vectorModel.ItemTypeModel.Alignment);
            il.EmitInt32Constant(vectorModel.ItemTypeModel.InlineSize);
            il.EmitLdArg(ArgIndex_Context);
            il.EmitMethodCall(writeMemoryVectorMethod);
            il.Emit(OpCodes.Ret);
        }
    }
}

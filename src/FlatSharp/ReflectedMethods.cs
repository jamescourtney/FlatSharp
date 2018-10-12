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

    /// <summary>
    /// Methods invoked from generated IL.
    /// </summary>
    internal static class ReflectedMethods
    {
        private static BindingFlags AllFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        // Built-in readers for certain types.
        internal static IReadOnlyDictionary<Type, MethodInfo> InputBufferReaders = new Dictionary<Type, MethodInfo>
        {
            [typeof(bool)] = GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadBool)),
            [typeof(sbyte)] = GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadSByte)),
            [typeof(byte)] = GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadByte)),
            [typeof(short)] = GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadShort)),
            [typeof(ushort)] = GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadUShort)),
            [typeof(int)] = GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadInt)),
            [typeof(uint)] = GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadUInt)),
            [typeof(long)] = GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadLong)),
            [typeof(ulong)] = GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadULong)),
            [typeof(float)] = GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadFloat)),
            [typeof(double)] = GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadDouble)),
            [typeof(string)] = GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadString)),
        };

        internal static IReadOnlyDictionary<Type, MethodInfo> TableInlineSizeGetters = new Dictionary<Type, MethodInfo>
        {
            [typeof(bool)] = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.ILGetSizeBool)),
            [typeof(byte)] = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.ILGetSizeByte)),
            [typeof(sbyte)] = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.ILGetSizeSByte)),
            [typeof(ushort)] = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.ILGetSizeUInt16)),
            [typeof(short)] = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.ILGetSizeInt16)),
            [typeof(uint)] = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.ILGetSizeUInt32)),
            [typeof(int)] = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.ILGetSizeInt32)),
            [typeof(ulong)] = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.ILGetSizeUInt64)),
            [typeof(long)] = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.ILGetSizeInt64)),
            [typeof(float)] = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.ILGetSizeSingle)),
            [typeof(double)] = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.ILGetSizeDouble)),
            [typeof(string)] = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.ILGetSizeString)),
        };

        internal static IReadOnlyDictionary<Type, MethodInfo> ILWriters = new Dictionary<Type, MethodInfo>
        {
            [typeof(bool)] = GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteBool)),
            [typeof(byte)] = GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteByte)),
            [typeof(sbyte)] = GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteSByte)),
            [typeof(ushort)] = GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteUShort)),
            [typeof(short)] = GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteShort)),
            [typeof(uint)] = GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteUInt)),
            [typeof(int)] = GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteInt)),
            [typeof(ulong)] = GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteULong)),
            [typeof(long)] = GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteLong)),
            [typeof(float)] = GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteFloat)),
            [typeof(double)] = GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteDouble)),
            [typeof(string)] = GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteString)),
        };

        internal static class Serialize
        {
            // Memory, Span, and IList.
            public static PropertyInfo Memory_LengthProperty(Type type) => GetPublicProperty(typeof(Memory<>).MakeGenericType(type), nameof(Memory<byte>.Length));
            public static PropertyInfo ReadOnlyMemory_LengthProperty(Type type) => GetPublicProperty(typeof(ReadOnlyMemory<>).MakeGenericType(type), nameof(ReadOnlyMemory<byte>.Length));

            public static PropertyInfo IList_CountProperty(Type type) => GetPublicProperty(typeof(ICollection<>).MakeGenericType(type), nameof(ICollection<byte>.Count));
            public static PropertyInfo IList_ItemProperty(Type type) => GetPublicProperty(typeof(IList<>).MakeGenericType(type), "Item");
            public static PropertyInfo IReadOnlyList_CountProperty(Type type) => GetPublicProperty(typeof(IReadOnlyCollection<>).MakeGenericType(type), nameof(IReadOnlyCollection<byte>.Count));
            public static PropertyInfo IReadOnlyList_ItemProperty(Type type) => GetPublicProperty(typeof(IReadOnlyList<>).MakeGenericType(type), "Item");

            // SerializationHelpers            
            public static readonly MethodInfo SerializationHelpers_GetAlignmentErrorMethod = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.GetAlignmentError));
            public static readonly MethodInfo SerializationHelpers_GetMaxPaddingethod = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.GetMaxPadding));
            public static readonly MethodInfo SerializationHelpers_GetMaxSizeOfStringMethod = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.GetMaxSize));

            // Serialization context
            public static readonly MethodInfo SerializationContext_AllocateMemoryMethod = GetMethod(typeof(SerializationContext), nameof(SerializationContext.AllocateSpace));
            public static readonly FieldInfo SerializationContext_Vtable = GetField(typeof(SerializationContext), "vtableHelper");
            public static readonly MethodInfo SerializationContext_AllocateVectorMethod = GetMethod(typeof(SerializationContext), nameof(SerializationContext.AllocateVector));

            // SpanWriter
            public static readonly MethodInfo SpanWriter_WriteUOffset = GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteUOffset));
            public static readonly MethodInfo SpanWriter_WriteByteMemoryBlock = GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteByteMemoryBlock));
            public static readonly MethodInfo SpanWriter_WriteReadOnlyByteMemoryBlock = GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteReadOnlyByteMemoryBlock));
            public static MethodInfo SpanWriter_WriteMemoryBlock(Type t) => GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteMemoryBlock)).MakeGenericMethod(t);
            public static MethodInfo SpanWriter_WriteReadOnlyMemoryBlock(Type t) => GetMethod(typeof(SpanWriter), nameof(SpanWriter.WriteReadOnlyMemoryBlock)).MakeGenericMethod(t);

            // VTableHelper
            public static readonly MethodInfo VTableHelper_StartObjectMethod = GetMethod(typeof(VTableHelper), nameof(VTableHelper.StartObject));
            public static readonly MethodInfo VTableHelper_EndObjectMethod = GetMethod(typeof(VTableHelper), nameof(VTableHelper.EndObject));
            public static readonly MethodInfo VTableHelper_SetOffsetMethod = GetMethod(typeof(VTableHelper), nameof(VTableHelper.SetOffset));
        }

        internal static class Deserialize
        {
            public static readonly MethodInfo InputBuffer_ReadUOffset = GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadUOffset));
            public static readonly MethodInfo InputBuffer_GetAbsoluteTableFieldLocation = GetMethod(typeof(InputBuffer), nameof(InputBuffer.GetAbsoluteTableFieldLocation));
            public static readonly MethodInfo InputBuffer_ReadReadOnlyByteMemoryBlock = GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadReadOnlyByteMemoryBlock));
            public static readonly MethodInfo InputBuffer_ReadByteMemoryBlock = GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadByteMemoryBlock));
            public static MethodInfo InputBuffer_ReadReadOnlyMemoryBlock(Type t) => GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadReadOnlyMemoryBlock)).MakeGenericMethod(t);
            public static MethodInfo InputBuffer_ReadMemoryBlock(Type t) => GetMethod(typeof(InputBuffer), nameof(InputBuffer.ReadMemoryBlock)).MakeGenericMethod(t);

            public static MethodInfo Memory_ToArray(Type t) => GetMethod(typeof(Memory<>).MakeGenericType(t), nameof(Memory<byte>.ToArray));
            public static MethodInfo FlatBufferVector_ToArray(Type t) => GetMethod(typeof(FlatBufferVector<>).MakeGenericType(t), nameof(FlatBufferVector<byte>.ToArray));
        }

        private static MethodInfo GetMethod(Type type, string methodName, Type[] parameterTypes = null)
        {
            MethodInfo methodInfo;
            if (parameterTypes == null)
            {
                methodInfo = type.GetMethod(methodName, AllFlags);
            }
            else
            {
                methodInfo = type.GetMethod(methodName, AllFlags, null, parameterTypes, null);
            }

            if (methodInfo == null)
            {
                throw new MissingMethodException($"Can't find method {methodName} on type {type}.");
            }

            return methodInfo;
        }

        private static FieldInfo GetField(Type type, string fieldName)
        {
            FieldInfo fieldInfo = type.GetField(fieldName, AllFlags);
            if (fieldInfo == null)
            {
                throw new MissingMemberException($"Can't find field {fieldName} on type {type}.");
            }

            return fieldInfo;
        }

        private static PropertyInfo GetPublicProperty(Type type, string propertyName)
        {
            PropertyInfo propertyInfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            if (propertyInfo == null)
            {
                throw new MissingMemberException($"Can't find public property {propertyName} on type {type}.");
            }

            return propertyInfo;
        }
    }
}

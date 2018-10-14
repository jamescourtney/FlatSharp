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

        // SerializationHelpers            
        public static readonly MethodInfo SerializationHelpers_GetAlignmentErrorMethod = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.GetAlignmentError));
        public static readonly MethodInfo SerializationHelpers_GetMaxSizeOfStringMethod = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.GetMaxSize));

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

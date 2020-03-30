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

        // SerializationHelpers            
        public static readonly MethodInfo SerializationHelpers_GetAlignmentErrorMethod = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.GetAlignmentError));
        public static readonly MethodInfo SerializationHelpers_GetMaxSizeOfStringMethod = GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.GetMaxSize));

        public static MethodInfo SerializationHelpers_EnsureNonNull(Type t) => GetMethod(typeof(SerializationHelpers), nameof(SerializationHelpers.EnsureNonNull)).MakeGenericMethod(t);

        internal static MethodInfo GetMethod(Type type, string methodName)
        {
            MethodInfo methodInfo = type.GetMethod(methodName, AllFlags);

            if (methodInfo == null)
            {
                throw new MissingMethodException($"Can't find method {methodName} on type {type}.");
            }

            return methodInfo;
        }
    }
}
